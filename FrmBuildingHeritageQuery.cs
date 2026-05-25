using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Runtime.InteropServices;

namespace Route_of_Shanghai
{
    public partial class FrmBuildingHeritageQuery : Form
    {
        public AxMapControl MainMap { get; set; }

        public event EventHandler ClearPathPlanning;

        // 用于缓存实际的字段索引
        private int _nameFieldIndex = -1;
        private int _typeFieldIndex = -1;
        private int _regionFieldIndex = -1;

        // 用于存储查询结果对应的要素
        private Dictionary<string, IFeature> _featureCache = new Dictionary<string, IFeature>();

        public FrmBuildingHeritageQuery()
        {
            InitializeComponent();
        }

        private void FrmBuildingHeritageQuery_Load(object sender, EventArgs e)
        {
            // 加载区域选项
            cbo_QueryRegion.Items.AddRange(new string[]
            {
                "黄浦区", "徐汇区", "长宁区", "静安区", "普陀区",
                "虹口区", "杨浦区", "闵行区", "宝山区", "嘉定区",
                "浦东新区", "松江区", "金山区", "青浦区", "奉贤区", "崇明区"
            });

            // 动态加载类别选项
            LoadUniqueTypes();

            // 优雅的现代 UI 样式美化
            StyleQueryControls();
        }

        #region 各个按钮的点击事件

        // 按钮1：【联合查询】(必须同时满足填写的多个条件)
        private void button1_Click(object sender, EventArgs e)
        {
            string name = txt_QueryName.Text.Trim();
            string type = GetComboValue(cbo_QueryType);
            string region = GetComboValue(cbo_QueryRegion);

            // 联合查询模式，最后一个参数传入 true
            PerformSearch(name, type, region, true);
        }

        // 按钮2：【清除按钮】
        private void button2_Click(object sender, EventArgs e)
        {
            txt_QueryName.Clear();
            cbo_QueryType.SelectedIndex = -1;
            cbo_QueryRegion.SelectedIndex = -1;
            cbo_QueryType.Text = "";
            cbo_QueryRegion.Text = "";
            dataGridView1.DataSource = null;

            // 清空查询时，自动清除高亮选择，缩放到全图并刷新地图
            if (MainMap != null)
            {
                MainMap.Map.ClearSelection();
                MainMap.Extent = MainMap.FullExtent;
                MainMap.Refresh();
            }
        }

        // 按钮3：【按名称查询】
        private void button3_Click(object sender, EventArgs e)
        {
            string name = txt_QueryName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入要查询的名称！", "提示");
                return;
            }

            // 【核心防错】：自动清除其他框的干扰项，确保是纯粹的单项查询
            cbo_QueryType.SelectedIndex = -1;
            cbo_QueryType.Text = "";
            cbo_QueryRegion.SelectedIndex = -1;
            cbo_QueryRegion.Text = "";

            PerformSearch(name, null, null, false);
        }

        // 按钮4：【按类别查询】
        private void button4_Click_1(object sender, EventArgs e)
        {
            string type = GetComboValue(cbo_QueryType);
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("请选择或输入要查询的类别！", "提示");
                return;
            }

            // 【核心防错】：自动清除其他框的干扰项，确保是纯粹的单项查询
            txt_QueryName.Clear();
            cbo_QueryRegion.SelectedIndex = -1;
            cbo_QueryRegion.Text = "";

            PerformSearch(null, type, null, false);
        }

        // 按钮5：【按区域查询】
        private void button5_Click_1(object sender, EventArgs e)
        {
            string region = GetComboValue(cbo_QueryRegion);
            if (string.IsNullOrEmpty(region))
            {
                MessageBox.Show("请选择或输入要查询的区域！", "提示");
                return;
            }

            // 【核心防错】：自动清除其他框的干扰项，确保是纯粹的单项查询
            txt_QueryName.Clear();
            cbo_QueryType.SelectedIndex = -1;
            cbo_QueryType.Text = "";

            PerformSearch(null, null, region, false);
        }

        #endregion

        #region 核心查询逻辑（提取出的公共方法）

        /// <summary>
        /// 核心查询引擎
        /// </summary>
        /// <param name="queryName">要查询的名称</param>
        /// <param name="queryType">要查询的类别</param>
        /// <param name="queryRegion">要查询的区域</param>
        /// <param name="isCombined">是否为联合查询(AND逻辑)</param>
        private void PerformSearch(string queryName, string queryType, string queryRegion, bool isCombined)
        {
            // 1. 空值校验
            if (string.IsNullOrEmpty(queryName) && string.IsNullOrEmpty(queryType) && string.IsNullOrEmpty(queryRegion))
            {
                MessageBox.Show("请至少输入或选择一个查询条件！", "提示");
                return;
            }

            try
            {
                // 清空之前的缓存
                _featureCache.Clear();

                // 2. 获取图层与字段绑定
                IFeatureLayer pLayer = GetHeritageLayer();
                if (pLayer == null)
                {
                    MessageBox.Show("未在地图中找到名为 'heritages_cn' 的图层！", "错误");
                    return;
                }

                IFeatureClass pFeatureClass = pLayer.FeatureClass;
                if (!BindFields(pFeatureClass.Fields)) return; // 绑定失败则终止

                // 3. 准备数据表
                DataTable dt = new DataTable();
                dt.Columns.Add("名称");
                dt.Columns.Add("类别");
                dt.Columns.Add("区域");

                // 4. 游标遍历检索
                IFeatureCursor pCursor = pFeatureClass.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();

                int resultIndex = 0;
                while (pFeature != null)
                {
                    string curName = GetValue(pFeature, _nameFieldIndex);
                    string curType = GetValue(pFeature, _typeFieldIndex);
                    string curRegion = GetValue(pFeature, _regionFieldIndex);

                    bool match = false;

                    if (isCombined)
                    {
                        // 【联合查询模式】：必须同时满足所有非空的条件
                        match = true;
                        if (!string.IsNullOrEmpty(queryName) && !curName.Contains(queryName)) match = false;
                        if (!string.IsNullOrEmpty(queryType) && !curType.Contains(queryType)) match = false;
                        if (!string.IsNullOrEmpty(queryRegion) && !curRegion.Contains(queryRegion)) match = false;
                    }
                    else
                    {
                        // 【独立查询模式】：只匹配当前传入的单项条件
                        if (!string.IsNullOrEmpty(queryName) && curName.Contains(queryName)) match = true;
                        if (!string.IsNullOrEmpty(queryType) && curType.Contains(queryType)) match = true;
                        if (!string.IsNullOrEmpty(queryRegion) && curRegion.Contains(queryRegion)) match = true;
                    }

                    // 5. 如果匹配成功，加入结果表
                    if (match)
                    {
                        DataRow row = dt.NewRow();
                        // 过滤清除名称前面的多余序号和数字
                        string cleanName = Regex.Replace(curName, @"^[\d\s\.,、-]+", "");
                        row["名称"] = cleanName;
                        row["类别"] = curType;
                        row["区域"] = curRegion;
                        dt.Rows.Add(row);

                        // 缓存要素，使用索引作为键
                        _featureCache[resultIndex.ToString()] = pFeature;
                        resultIndex++;
                    }

                    pFeature = pCursor.NextFeature();
                }

                Marshal.ReleaseComObject(pCursor);
                dataGridView1.DataSource = dt;

                // 6. 如果查空，给出精确反馈，极大简化诊断调试
                if (dt.Rows.Count == 0)
                {
                    string debugCondition = isCombined ?
                        $"联合条件 -> 名称:[{queryName}] 类别:[{queryType}] 区域:[{queryRegion}]" :
                        $"单项条件 -> [{queryName}{queryType}{queryRegion}]";

                    MessageBox.Show($"未找到符合条件的记录！\n\n后台实际检索内容为：\n{debugCondition}", "查询结果");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询过程中发生错误：\n" + ex.Message, "异常");
            }
        }

        // DataGridView 双击定位事件
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || MainMap == null)
                {
                    return;
                }

                // a. 获取选中行的名称
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string targetName = row.Cells["名称"].Value.ToString();

                // 获取图层
                IFeatureLayer pLayer = GetHeritageLayer();
                if (pLayer == null)
                {
                    MessageBox.Show("未在地图中找到名为 'heritages_cn' 的图层！", "错误");
                    return;
                }

                IFeatureClass pFeatureClass = pLayer.FeatureClass;
                if (!BindFields(pFeatureClass.Fields)) return;

                // b. 通过名称重新查询要素，而不是使用缓存
                IQueryFilter pQueryFilter = new QueryFilter();
                IField nameField = pFeatureClass.Fields.get_Field(_nameFieldIndex);
                pQueryFilter.WhereClause = string.Format("{0} LIKE '%{1}%'", nameField.Name, targetName.Replace("'", "''"));

                IFeatureCursor pCursor = pFeatureClass.Search(pQueryFilter, false);
                IFeature pFeature = pCursor.NextFeature();

                if (pFeature == null || pFeature.Shape == null)
                {
                    Marshal.ReleaseComObject(pCursor);
                    MessageBox.Show("未找到该要素或要素几何信息无效！", "提示");
                    return;
                }

                // 清除路径规划结果
                ClearPathPlanning?.Invoke(this, EventArgs.Empty);

                // 选中要素
                MainMap.Map.ClearSelection();
                MainMap.Map.SelectFeature(pLayer as ILayer, pFeature);

                // 移动到要素位置
                IGeometry geometry = pFeature.ShapeCopy;
                if (geometry != null)
                {
                    // 获取要素类自身的空间参考 (Shapefile为 WGS84)
                    IGeoDataset geoDataset = pFeatureClass as IGeoDataset;
                    if (geoDataset != null && geoDataset.SpatialReference != null)
                    {
                        geometry.SpatialReference = geoDataset.SpatialReference;
                    }

                    // 投影转换至当前地图的坐标系 (网络路网坐标系)
                    if (MainMap.Map.SpatialReference != null)
                    {
                        geometry.Project(MainMap.Map.SpatialReference);
                    }

                    if (geometry.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        IPoint point = geometry as IPoint;
                        if (point != null)
                        {
                            // 1. 居中并放大到该点区域 (设置视角大小为 500m * 500m，适合单个历史建筑物与街区展示)
                            IEnvelope env = MainMap.Extent;
                            env.Width = 500;
                            env.Height = 500;
                            env.CenterAt(point);
                            MainMap.Extent = env;

                            // 2. 在地图上添加该点的名称标注 (Text Element)
                            ITextSymbol textSymbol = new TextSymbol() as ITextSymbol;
                            textSymbol.Size = 12;

                            IRgbColor textColor = new RgbColor() as IRgbColor;
                            textColor.Red = 180;
                            textColor.Green = 0;
                            textColor.Blue = 0; // 绛红色字体，优雅古典
                            textSymbol.Color = textColor;

                            // 通过 IMask 接口实现白色背景光晕 (Halo)，彻底解决 ITextSymbol 不包含 HaloSize 的编译问题
                            IMask mask = textSymbol as IMask;
                            if (mask != null)
                            {
                                mask.MaskStyle = esriMaskStyle.esriMSHalo;
                                mask.MaskSize = 2.0; // 光晕尺寸

                                // 创建用于光晕颜色的填充符号
                                ISimpleFillSymbol fillSymbol = new SimpleFillSymbol() as ISimpleFillSymbol;
                                IRgbColor haloColor = new RgbColor() as IRgbColor;
                                haloColor.Red = 255;
                                haloColor.Green = 255;
                                haloColor.Blue = 255; // 白色光晕背景
                                fillSymbol.Color = haloColor;
                                fillSymbol.Outline = null; // 光晕本身不需要外边框线
                                
                                mask.MaskSymbol = fillSymbol as IFillSymbol;
                            }

                            ITextElement textElement = new TextElement() as ITextElement;
                            textElement.Text = targetName;
                            textElement.Symbol = textSymbol;

                            IElement element = textElement as IElement;
                            IPoint labelPoint = new ESRI.ArcGIS.Geometry.Point();
                            // 上移 20 米避开选定点的高亮闪烁符号
                            labelPoint.PutCoords(point.X, point.Y + 20);
                            element.Geometry = labelPoint;

                            // 添加到地图的临时图形图层中
                            IGraphicsContainer gc = MainMap.Map as IGraphicsContainer;
                            gc.AddElement(element, 0);
                        }
                    }
                    else
                    {
                        IEnvelope envelope = geometry.Envelope;
                        // 对于线或面，适当缓冲扩大 1.5 倍
                        envelope.Expand(1.5, 1.5, true);
                        MainMap.Extent = envelope;
                    }
                }

                MainMap.Refresh();

                Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                MessageBox.Show("定位过程中发生错误：\n" + ex.Message, "异常");
            }
        }

        #endregion

        #region 辅助工具方法

        // 安全获取下拉框文本值（完美兼容选择项和手动打字输入）
        private string GetComboValue(ComboBox cbo)
        {
            if (cbo.SelectedItem != null && !string.IsNullOrWhiteSpace(cbo.SelectedItem.ToString()))
            {
                return cbo.SelectedItem.ToString().Trim();
            }
            return cbo.Text.Trim();
        }

        private void LoadUniqueTypes()
        {
            try
            {
                IFeatureLayer pLayer = GetHeritageLayer();
                if (pLayer == null) return;

                IFeatureClass pFeatureClass = pLayer.FeatureClass;
                int typeIndex = FindRealFieldIndex(pFeatureClass.Fields, "类别", "类型", "type", "class");
                if (typeIndex == -1) return;

                HashSet<string> uniqueTypes = new HashSet<string>();
                IFeatureCursor pCursor = pFeatureClass.Search(null, true);
                IFeature pFeature = pCursor.NextFeature();

                while (pFeature != null)
                {
                    string curType = GetValue(pFeature, typeIndex);
                    if (!string.IsNullOrWhiteSpace(curType))
                    {
                        uniqueTypes.Add(curType);
                    }
                    pFeature = pCursor.NextFeature();
                }
                Marshal.ReleaseComObject(pCursor);

                cbo_QueryType.Items.Clear();
                foreach (string typeName in uniqueTypes)
                {
                    cbo_QueryType.Items.Add(typeName);
                }
            }
            catch { }
        }

        private bool BindFields(IFields fields)
        {
            _nameFieldIndex = FindRealFieldIndex(fields, "名称", "name");
            _typeFieldIndex = FindRealFieldIndex(fields, "类别", "类型", "type", "class");
            _regionFieldIndex = FindRealFieldIndex(fields, "区域", "区县", "所属区", "region", "district");

            if (_nameFieldIndex == -1 && _typeFieldIndex == -1 && _regionFieldIndex == -1)
            {
                MessageBox.Show("无法在图层中匹配到核心字段，请检查属性表是否存在乱码。", "字段异常");
                return false;
            }
            return true;
        }

        private int FindRealFieldIndex(IFields fields, params string[] possibleNames)
        {
            for (int i = 0; i < fields.FieldCount; i++)
            {
                IField field = fields.get_Field(i);
                string fieldName = field.Name.Trim().ToLower();
                string aliasName = field.AliasName.Trim().ToLower();

                foreach (string keyword in possibleNames)
                {
                    if (fieldName.Contains(keyword.ToLower()) || aliasName.Contains(keyword.ToLower()))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private string GetValue(IFeature pFeature, int index)
        {
            if (index < 0) return "";
            object val = pFeature.get_Value(index);
            return (val == DBNull.Value || val == null) ? "" : val.ToString().Trim();
        }

        private IFeatureLayer GetHeritageLayer()
        {
            if (MainMap == null) return null;
            for (int i = 0; i < MainMap.LayerCount; i++)
            {
                ILayer layer = MainMap.get_Layer(i);
                if (layer.Name.Contains("heritages_cn") && layer is IFeatureLayer)
                {
                    return layer as IFeatureLayer;
                }
            }
            return null;
        }

        private void StyleQueryControls()
        {
            try
            {
                // 1. 窗口及容器整体配色 (温暖沙白底色与金棕色标题)
                this.BackColor = Color.FromArgb(250, 248, 245);
                groupBox1.ForeColor = Color.FromArgb(92, 53, 26);
                groupBox2.ForeColor = Color.FromArgb(92, 53, 26);

                // 2. 表格美化 (DataGridView) - 打造扁平化金棕色高级质感
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.BorderStyle = BorderStyle.None;
                dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridView1.GridColor = Color.FromArgb(238, 232, 222); // 暖沙色细网格线
                
                // 表格头部
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(92, 53, 26); // 金棕色表头
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(245, 235, 220); // 暖麦色字
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                
                // 表格单元格及选择样式
                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(64, 40, 16); // 暗古铜色字体
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(160, 110, 60); // 暖古铜金色选中高亮
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
                
                // 奇数行背景微调 (Alternating Colors)
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(253, 251, 247); // 温暖象牙白
                
                // 行表头隐藏及行高自适应
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.RowTemplate.Height = 32;

                // 3. 按钮美化 (FlatStyle)
                // 联合查询主按钮
                btn_Search.FlatStyle = FlatStyle.Flat;
                btn_Search.FlatAppearance.BorderSize = 0;
                btn_Search.BackColor = Color.FromArgb(120, 80, 38); // 暖金棕主色
                btn_Search.ForeColor = Color.White;
                btn_Search.Cursor = Cursors.Hand;

                // 清空条件按钮
                btn_ClearCondition.FlatStyle = FlatStyle.Flat;
                btn_ClearCondition.FlatAppearance.BorderSize = 1;
                btn_ClearCondition.FlatAppearance.BorderColor = Color.FromArgb(197, 160, 89); // 青铜色边框
                btn_ClearCondition.BackColor = Color.White;
                btn_ClearCondition.ForeColor = Color.FromArgb(120, 80, 38);
                btn_ClearCondition.Cursor = Cursors.Hand;

                // 单项检索按钮（button3, button4, button5）
                Color sideBtnColor = Color.FromArgb(170, 130, 85); // 暖铜金色
                
                button3.FlatStyle = FlatStyle.Flat;
                button3.FlatAppearance.BorderSize = 0;
                button3.BackColor = sideBtnColor;
                button3.ForeColor = Color.White;
                button3.Cursor = Cursors.Hand;

                button4.FlatStyle = FlatStyle.Flat;
                button4.FlatAppearance.BorderSize = 0;
                button4.BackColor = sideBtnColor;
                button4.ForeColor = Color.White;
                button4.Cursor = Cursors.Hand;

                button5.FlatStyle = FlatStyle.Flat;
                button5.FlatAppearance.BorderSize = 0;
                button5.BackColor = sideBtnColor;
                button5.ForeColor = Color.White;
                button5.Cursor = Cursors.Hand;

                // 4. 下拉框及输入框细节微调
                cbo_QueryRegion.FlatStyle = FlatStyle.Flat;
                cbo_QueryType.FlatStyle = FlatStyle.Flat;
            }
            catch
            {
                // 容错处理
            }
        }

        #endregion
    }
}