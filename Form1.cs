using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.NetworkAnalysis;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;

namespace Route_of_Shanghai
{
    public partial class Form1 : Form
    {
        // =========================
        // 网络相关
        // =========================
        private IGeometricNetwork mGeometricNetwork;
        private IPointCollection mPointCollection;
        private IPointToEID mPointToEID;
        private IEnumNetEID mEnumNetEID_Junctions;
        private IEnumNetEID mEnumNetEID_Edges;
        private double mdblPathCost;

        // =========================
        // 坐标系
        // =========================
        private ISpatialReference m_wgs84SR;
        private ISpatialReference m_networkSR;

        // =========================
        // 遗产点
        // =========================
        private List<IPoint> heritagePoints =
            new List<IPoint>();

        // =========================
        // 当前路径
        // =========================
        private IPolyline mCurrentPathLine = null;

        // =========================
        // 路径结果悬浮窗
        // =========================
        private Panel pnlPathResult;
        private Label lblPathResult;
        private System.Windows.Forms.Timer pathResultTimer;

        // =========================
        // 外挂式查询窗口与页眉
        // =========================
        private FrmBuildingHeritageQuery queryFormInstance = null;
        private Panel pnlHeader = null;

        public Form1()
        {
            InitializeComponent();

            m_wgs84SR =
                CreateWGS84SpatialReference();

            // 初始化路径结果悬浮窗
            InitializePathResultPanel();

            // =========================
            // 手动绑定事件
            // =========================
            this.Shown += Form1_Shown;

            tsmi_Exit.Click += tsmi_Exit_Click;

            tsmi_View_FullExtent.Click +=
                tsmi_View_FullExtent_Click;

            tsmi_View_RefreshMap.Click +=
                tsmi_View_RefreshMap_Click;

            tsmi_View_ToggleRoadNetwork.Click +=
                tsmi_View_ToggleRoadNetwork_Click;

            tsmi_MapTool_ZoomIn.Click +=
                tsmi_MapTool_ZoomIn_Click;

            tsmi_MapTool_ZoomOut.Click +=
                tsmi_MapTool_ZoomOut_Click;

            tsmi_MapTool_Pan.Click +=
                tsmi_MapTool_Pan_Click;

            btn_ClearPath.Click +=
                btn_ClearPath_Click;

            tsmi_ExportPlanPath.Click +=
                tsmi_ExportPlanPath_Click;

            tsmi_SaveMapView.Click +=
                tsmi_SaveMapView_Click;

            // 注释掉遗产点选中时的自动定位
            // cbo_StartPoint.SelectedIndexChanged += cbo_Heritage_SelectedIndexChanged;
            // cbo_EndPoint.SelectedIndexChanged += cbo_Heritage_SelectedIndexChanged;
            // cbo_WayPoint.SelectedIndexChanged += cbo_Heritage_SelectedIndexChanged;

            // 绑定外挂式侧边栏位置与尺寸随动同步事件
            this.LocationChanged += (s, e) => SyncQueryFormPosition();
            this.SizeChanged += (s, e) => SyncQueryFormPosition();
            
            // 初始化行驶模式下拉框默认选中
            this.cbo_TravelMode.SelectedIndex = 0;
        }

        #region 投影

        private void UpdatePathResultPanelLocation()
        {
            if (pnlPathResult == null || axMapControl1 == null) return;
            try
            {
                // 将 MapControl 右上角坐标转换到 Form 客户区坐标，彻底解决多层容器嵌套导致的定位偏差 Bug
                System.Drawing.Point mapTopRightInScreen = axMapControl1.PointToScreen(
                    new System.Drawing.Point(axMapControl1.Width - pnlPathResult.Width - 15, 15)
                );
                System.Drawing.Point locationInForm = this.PointToClient(mapTopRightInScreen);
                pnlPathResult.Location = locationInForm;
            }
            catch
            {
                // 异常保护
            }
        }

        private void InitializePathResultPanel()
        {
            pnlPathResult = new Panel();
            pnlPathResult.Width = 200; // 缩小宽度，更加精致
            pnlPathResult.Height = 62;  // 增高以容纳两行文字
            pnlPathResult.BackColor = Color.FromArgb(220, 45, 45, 45); // 深灰色半透明背景
            pnlPathResult.Visible = false;
            pnlPathResult.BorderStyle = BorderStyle.None; // 扁平化无边框设计
            pnlPathResult.Padding = new Padding(5);

            lblPathResult = new Label();
            lblPathResult.AutoSize = false;
            lblPathResult.Dock = DockStyle.Fill;
            lblPathResult.TextAlign = ContentAlignment.MiddleCenter;
            lblPathResult.Font = new Font("微软雅黑", 10F, FontStyle.Bold); // 稍微缩小字体以适应紧凑窗口
            lblPathResult.ForeColor = Color.White;
            lblPathResult.Text = "";

            pnlPathResult.Controls.Add(lblPathResult);

            pathResultTimer = new System.Windows.Forms.Timer();
            pathResultTimer.Interval = 15000; // 延长展示时间至 15 秒，给用户充足的阅读和记录时间
            pathResultTimer.Tick += PathResultTimer_Tick;

            this.Controls.Add(pnlPathResult);
            pnlPathResult.BringToFront();

            // 订阅大小改变与窗口大小改变事件，动态调整悬浮窗位置，保持与地图右上角对齐
            axMapControl1.Resize += (s, e) => UpdatePathResultPanelLocation();
            this.SizeChanged += (s, e) => UpdatePathResultPanelLocation();
        }

        private void PathResultTimer_Tick(object sender, EventArgs e)
        {
            pnlPathResult.Visible = false;
            pathResultTimer.Stop();
        }

        private void ShowPathResult(string message)
        {
            // 防御性检查：确保悬浮窗已创建，若未创建则立即初始化
            if (pnlPathResult == null || lblPathResult == null)
            {
                InitializePathResultPanel();
            }
            lblPathResult.Text = message;
            pnlPathResult.Visible = true;
            UpdatePathResultPanelLocation(); // 显示时即时重新定位
            pnlPathResult.BringToFront();
            pathResultTimer?.Stop();
            pathResultTimer?.Start();
        }

        private void ClearPathPlanningResults()
        {
            cbo_StartPoint.SelectedIndex = -1;
            cbo_EndPoint.SelectedIndex = -1;
            cbo_WayPoint.SelectedIndex = -1;

            axMapControl1.ActiveView
                .GraphicsContainer
                .DeleteAllElements();

            axMapControl1.Refresh();

            mCurrentPathLine = null;

            // 清空路径时，同步隐藏悬浮窗
            if (pnlPathResult != null)
            {
                pnlPathResult.Visible = false;
                pathResultTimer.Stop();
            }
        }

        private ISpatialReference CreateWGS84SpatialReference()
        {
            ISpatialReferenceFactory srFactory =
                new SpatialReferenceEnvironment();

            return srFactory.CreateGeographicCoordinateSystem(
                (int)esriSRGeoCSType.esriSRGeoCS_WGS1984
            );
        }

        private IPoint ProjectPoint(IPoint srcPoint)
        {
            if (m_networkSR == null ||
                m_wgs84SR == null)
            {
                return srcPoint;
            }

            IGeometry geo =
                srcPoint as IGeometry;

            geo.SpatialReference =
                m_wgs84SR;

            geo.Project(m_networkSR);

            return geo as IPoint;
        }

        #endregion

        #region 窗体加载

        private void Form1_Load(
            object sender,
            EventArgs e)
        {
            try
            {
                // 动态优化初始窗口大小为经典 16:10 宽屏比例 1024x800，进一步压缩宽度以适配更小屏幕
            this.ClientSize = new Size(1024, 800);
                // =========================================================
                // 【核心修改】：通过执行程序所在目录（bin\Debug）动态计算项目根目录
                // =========================================================
                string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // 向上追溯两级目录 (bin -> Debug 的上一层)，精确切入到与 bin、obj 同级的文件夹中
                string relativeDataFolder = System.IO.Path.GetFullPath(
                    System.IO.Path.Combine(exeDirectory, @"..\..\")
                );

                // 基于计算出的相对路径，组装 MDB 数据库路径
                string mdbPath = System.IO.Path.Combine(relativeDataFolder, "network_peo.mdb");

                // 检验数据库文件物理存在状态，如意外遗失进行明确阻断提示
                if (!File.Exists(mdbPath))
                {
                    throw new FileNotFoundException(
                        "无法在相对路径中定位几何网络数据库文件，请检查 network_peo.mdb 是否已放入项目根目录下！",
                        mdbPath
                    );
                }

                // =========================
                // 打开工作空间
                // =========================
                IWorkspaceFactory pWF =
                    new AccessWorkspaceFactory();

                IWorkspace pWorkspace =
                    pWF.OpenFromFile(mdbPath, 0);

                // =========================
                // 获取 FeatureDataset
                // =========================
                IEnumDataset datasets =
                    pWorkspace.get_Datasets(
                        esriDatasetType.esriDTFeatureDataset
                    );

                datasets.Reset();

                IFeatureDataset pFD = null;
                INetworkCollection nc = null;

                IDataset ds;

                while ((ds = datasets.Next()) != null)
                {
                    IFeatureDataset fd =
                        ds as IFeatureDataset;

                    INetworkCollection ncTemp =
                        fd as INetworkCollection;

                    if (ncTemp != null &&
                        ncTemp.GeometricNetworkCount > 0)
                    {
                        pFD = fd;
                        nc = ncTemp;
                        break;
                    }
                }

                if (pFD == null)
                {
                    throw new Exception(
                        "未找到几何网络数据集"
                    );
                }

                // =========================
                // 获取几何网络
                // =========================
                mGeometricNetwork =
                    nc.get_GeometricNetwork(0);

                // =========================
                // 坐标系
                // =========================
                IGeoDataset geoDs =
                    pFD as IGeoDataset;

                m_networkSR =
                    geoDs.SpatialReference;

                // =========================
                // 添加网络图层
                // 不显示 Junction 点图层
                // =========================
                IFeatureClassContainer fcc =
                    mGeometricNetwork
                    as IFeatureClassContainer;

                for (int i = 0; i < fcc.ClassCount; i++)
                {
                    IFeatureClass fc =
                        fcc.get_Class(i);

                    // 不显示点图层
                    if (fc.ShapeType ==
                        esriGeometryType.esriGeometryPoint)
                    {
                        continue;
                    }

                    IFeatureLayer fl =
                        new FeatureLayer();

                    fl.FeatureClass = fc;
                    fl.Name = fc.AliasName;

                    axMapControl1.AddLayer(fl);
                }

                // =========================================================
                // 【核心修改】：传入刚刚动态算出来的平级项目根目录加载 Shapefile
                // =========================================================
                LoadShapefileLayer(relativeDataFolder);

                // =========================
                // TOC绑定
                // =========================
                axTOCControl1.SetBuddyControl(
                    axMapControl1
                );

                // =========================
                // 初始化 PointToEID
                // =========================
                IEnvelope env =
                    axMapControl1.FullExtent;

                double tol =
                    Math.Max(env.Width, env.Height)
                    * 0.05;

                mPointToEID =
                    new PointToEID();

                mPointToEID.SourceMap =
                    axMapControl1.Map;

                mPointToEID.GeometricNetwork =
                    mGeometricNetwork;

                mPointToEID.SnapTolerance =
                    tol;

                // =========================
                // 工具栏
                // =========================
                axToolbarControl1.SetBuddyControl(
                    axMapControl1
                );

                axToolbarControl1.AddItem(
                    "esriControls.ControlsMapPanTool"
                );

                axToolbarControl1.AddItem(
                    "esriControls.ControlsMapZoomInTool"
                );

                axToolbarControl1.AddItem(
                    "esriControls.ControlsMapZoomOutTool"
                );

                // =========================
                // 加载名称
                // =========================
                LoadHeritageNames();

                // =========================
                // 刷新
                // =========================
                axMapControl1.Extent =
                    axMapControl1.FullExtent;

                axMapControl1.Refresh();

                // 优雅的现代 UI 样式美化
                StyleForm1Controls();

                // 优化侧边栏布局，自适应防裁剪与DPI高分屏缩放
                OptimizeSidebarLayout();

                // 初始化浮动式高精度地图工具栏，移除旧版丑陋的白底工具条
                InitializeFloatingMapTools();

                // 初始化金棕色系统页眉 (图标 + 标题 + 随动 Dock)
                InitializeFormHeader();

                axTOCControl1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "加载失败：\r\n\r\n" +
                    ex.Message
                );
            }
        }

        private void Form1_Shown(
            object sender,
            EventArgs e)
        {
            try
            {
                axMapControl1.Extent =
                    axMapControl1.FullExtent;

                axMapControl1.Refresh();
            }
            catch
            {

            }
        }

        #endregion

        #region 加载 SHP

        private void LoadShapefileLayer(
            string baseDir)
        {
            string shpPath =
                System.IO.Path.Combine(
                    baseDir,
                    "heritages_cn.shp"
                );

            if (!File.Exists(shpPath))
            {
                throw new FileNotFoundException(
                    "无法在相对路径中定位遗产点文件，请检查 heritages_cn.shp 是否已放入项目根目录下！",
                    shpPath
                );
            }

            string shpDir =
                System.IO.Path.GetDirectoryName(
                    shpPath
                );

            string shpName =
                System.IO.Path.GetFileNameWithoutExtension(
                    shpPath
                );

            IWorkspaceFactory shpWsf =
                new ShapefileWorkspaceFactory();

            IFeatureWorkspace fwShp =
                shpWsf.OpenFromFile(shpDir, 0)
                as IFeatureWorkspace;

            IFeatureClass fcPoint =
                fwShp.OpenFeatureClass(shpName);

            IFeatureLayer lyrPoint =
                new FeatureLayer();

            lyrPoint.FeatureClass =
                fcPoint;

            lyrPoint.Name =
                shpName;

            axMapControl1.AddLayer(lyrPoint);
        }

        #endregion

        #region 加载遗产点名称

        private void LoadHeritageNames()
        {
            try
            {
                cbo_StartPoint.Items.Clear();
                cbo_EndPoint.Items.Clear();
                cbo_WayPoint.Items.Clear();

                heritagePoints.Clear();

                IFeatureLayer pFeatureLayer =
                    null;

                for (int i = 0;
                    i < axMapControl1.LayerCount;
                    i++)
                {
                    ILayer pLayer =
                        axMapControl1.get_Layer(i);

                    if (pLayer is IFeatureLayer)
                    {
                        IFeatureLayer fl =
                            pLayer as IFeatureLayer;

                        if (fl.FeatureClass != null)
                        {
                            if (fl.FeatureClass.ShapeType ==
                                esriGeometryType.esriGeometryPoint)
                            {
                                pFeatureLayer = fl;
                                break;
                            }
                        }
                    }
                }

                if (pFeatureLayer == null)
                {
                    throw new Exception(
                        "未找到遗产点图层"
                    );
                }

                IFeatureClass pFeatureClass =
                    pFeatureLayer.FeatureClass;

                int nameIndex =
                    pFeatureClass.Fields.FindField("名称");

                if (nameIndex < 0)
                {
                    throw new Exception(
                        "未找到字段：名称"
                    );
                }

                IFeatureCursor pCursor =
                    pFeatureClass.Search(
                        null,
                        true
                    );

                IFeature pFeature =
                    pCursor.NextFeature();

                while (pFeature != null)
                {
                    IPoint rawPoint =
                        pFeature.ShapeCopy as IPoint;

                    if (rawPoint != null)
                    {
                        IPoint projPoint =
                            ProjectPoint(rawPoint);

                        object val =
                            pFeature.get_Value(nameIndex);

                        string name = "";

                        if (val != null &&
                            val != DBNull.Value)
                        {
                            name =
                                val.ToString().Trim();
                        }

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            heritagePoints.Add(projPoint);

                            cbo_StartPoint.Items.Add(name);
                            cbo_EndPoint.Items.Add(name);
                            cbo_WayPoint.Items.Add(name);
                        }
                    }

                    pFeature =
                        pCursor.NextFeature();
                }

                Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "读取遗产点失败：\r\n\r\n" +
                    ex.Message
                );
            }
        }

        #endregion

        #region 求解路径

        // 计算指定权重下的路径费用并返回
        private double SolvePath(string weightName)
        {
            if (mPointToEID == null)
            {
                throw new Exception("未初始化");
            }

            ITraceFlowSolverGEN pSolver =
                new TraceFlowSolver()
                as ITraceFlowSolverGEN;

            INetSolver pNetSolver =
                pSolver as INetSolver;

            INetwork pNet =
                mGeometricNetwork.Network;

            pNetSolver.SourceNetwork =
                pNet;

            int count =
                mPointCollection.PointCount;

            if (count < 2)
            {
                throw new Exception("至少2个点");
            }

            IEdgeFlag[] flags =
                new IEdgeFlag[count];

            for (int i = 0; i < count; i++)
            {
                IPoint pt =
                    mPointCollection.get_Point(i);

                int eid;
                IPoint foundPt;
                double percent;

                mPointToEID.GetNearestEdge(
                    pt,
                    out eid,
                    out foundPt,
                    out percent
                );

                if (eid <= 0)
                {
                    throw new Exception(
                        $"第{i + 1}个点不在路网"
                    );
                }

                INetElements elements =
                    pNet as INetElements;

                int ucid, uid, usid;

                elements.QueryIDs(
                    eid,
                    esriElementType.esriETEdge,
                    out ucid,
                    out uid,
                    out usid
                );

                IEdgeFlag flag =
                    new EdgeFlag();

                ((INetFlag)flag).UserClassID =
                    ucid;

                ((INetFlag)flag).UserID =
                    uid;

                ((INetFlag)flag).UserSubID =
                    usid;

                flags[i] = flag;
            }

            pSolver.PutEdgeOrigins(ref flags);

            INetSchema schema =
                pNet as INetSchema;

            INetSolverWeights w =
                pSolver as INetSolverWeights;

            try
            {
                INetWeight weight =
                    schema.get_WeightByName(
                        weightName
                    );

                w.FromToEdgeWeight = weight;
                w.ToFromEdgeWeight = weight;
            }
            catch
            {

            }

            object[] res = new object[count - 1];

            pSolver.FindPath(
                esriFlowMethod.esriFMConnected,
                esriShortestPathObjFn.esriSPObjFnMinSum,
                out mEnumNetEID_Junctions,
                out mEnumNetEID_Edges,
                count - 1,
                ref res
            );

            double pathCost = 0;
            for (int i = 0; i < count - 1; i++)
            {
                pathCost += (double)res[i];
            }
            return pathCost;
        }

        #endregion

        #region EID 转 Polyline

        private IPolyline PathToPolyline()
        {
            if (mEnumNetEID_Edges == null)
            {
                return null;
            }

            IPolyline pLineRet = new Polyline() as IPolyline;

            IGeometryCollection geoColl =
                pLineRet as IGeometryCollection;

            IEIDHelper helper =
                new EIDHelper();

            helper.GeometricNetwork =
                mGeometricNetwork;

            helper.OutputSpatialReference =
                axMapControl1.Map.SpatialReference;

            helper.ReturnGeometries =
                true;

            IEnumEIDInfo enumInfo =
                helper.CreateEnumEIDInfo(
                    mEnumNetEID_Edges
                );

            enumInfo.Reset();

            IEIDInfo info;

            while ((info = enumInfo.Next()) != null)
            {
                if (info.Geometry != null)
                {
                    geoColl.AddGeometryCollection(
                        info.Geometry as IGeometryCollection
                    );
                }
            }

            return pLineRet;
        }

        #endregion

        #region 绘制路径

        private void DrawPath(IPolyline line)
        {
            axMapControl1.ActiveView.GraphicsContainer.DeleteAllElements();

            IRgbColor color =
                new RgbColor();

            color.Red = 255;
            color.Green = 0;
            color.Blue = 0;

            ICartographicLineSymbol lineSymbol =
                new CartographicLineSymbol();

            lineSymbol.Width = 4;
            lineSymbol.Color =
                color;

            ILineElement lineElement = new LineElement() as ILineElement;

            IElement element =
                lineElement as IElement;

            element.Geometry =
                line;

            lineElement.Symbol =
                lineSymbol;

            IGraphicsContainer gc =
                axMapControl1.Map as IGraphicsContainer;

            gc.AddElement(element, 0);

            axMapControl1.ActiveView.Refresh();
        }

        private void AddPointLabel(IPoint point, string name, string prefix)
        {
            if (point == null || string.IsNullOrWhiteSpace(name)) return;
            try
            {
                // 创建高可读性的外挂标注
                ITextSymbol textSymbol = new TextSymbol() as ITextSymbol;
                textSymbol.Size = 10;

                IRgbColor textColor = new RgbColor() as IRgbColor;
                textColor.Red = 45;
                textColor.Green = 45;
                textColor.Blue = 48; // 深灰黑色，易于辨识
                textSymbol.Color = textColor;

                // 使用 IMask 接口实现白色背景光晕 (Halo)
                IMask mask = textSymbol as IMask;
                if (mask != null)
                {
                    mask.MaskStyle = esriMaskStyle.esriMSHalo;
                    mask.MaskSize = 2.0;

                    ISimpleFillSymbol fillSymbol = new SimpleFillSymbol() as ISimpleFillSymbol;
                    IRgbColor haloColor = new RgbColor() as IRgbColor;
                    haloColor.Red = 255;
                    haloColor.Green = 255;
                    haloColor.Blue = 255; // 白色光晕背景
                    fillSymbol.Color = haloColor;
                    fillSymbol.Outline = null; // 不要外包框边线
                    
                    mask.MaskSymbol = fillSymbol as IFillSymbol;
                }

                ITextElement textElement = new TextElement() as ITextElement;
                textElement.Text = prefix + ": " + name;
                textElement.Symbol = textSymbol;

                IElement element = textElement as IElement;
                IPoint labelPoint = new ESRI.ArcGIS.Geometry.Point();
                // 向上偏移 20 米，避开起点、终点等高亮定位闪烁图标的重叠
                labelPoint.PutCoords(point.X, point.Y + 20);
                element.Geometry = labelPoint;

                IGraphicsContainer gc = axMapControl1.Map as IGraphicsContainer;
                gc.AddElement(element, 0);
            }
            catch
            {
                // 异常保护
            }
        }

        #endregion

        #region 鼠标选点

        private void axMapControl1_OnMouseDown(
            object sender,
            IMapControlEvents2_OnMouseDownEvent e)
        {
            IPoint ipt =
                new ESRI.ArcGIS.Geometry.Point();

            ipt.PutCoords(
                e.mapX,
                e.mapY
            );

            if (mPointCollection == null)
            {
                mPointCollection =
                    new Multipoint()
                    as IPointCollection;
            }

            object miss =
                Type.Missing;

            mPointCollection.AddPoint(
                ipt,
                ref miss,
                ref miss
            );
        }

        #endregion

        #region 双击路径分析

        private void axMapControl1_OnDoubleClick(
            object sender,
            IMapControlEvents2_OnDoubleClickEvent e)
        {
            try
            {
                if (mGeometricNetwork == null)
                {
                    throw new Exception(
                        "网络未加载"
                    );
                }

                if (mPointCollection == null ||
                    mPointCollection.PointCount < 2)
                {
                    throw new Exception(
                        "至少点2个点"
                    );
                }

                double length = SolvePath("Length");

                IPolyline line = PathToPolyline();

                if (line == null)
                {
                    throw new Exception(
                        "无法生成路径"
                    );
                }

                mCurrentPathLine =
                    line;

                DrawPath(line);

                // 规划后缩放到路径，并适当处理比例尺
                IEnvelope pathEnvelope = line.Envelope;
                IEnvelope mapExtent = axMapControl1.Extent;
                
                // 扩展一定的缓冲距离，避免只显示一部分
                double bufferScale = Math.Max(mapExtent.Width, mapExtent.Height) * 0.1;
                pathEnvelope.Expand(bufferScale, bufferScale, false);
                
                // 确保扩展后的 envelope 在地图空间范围内是有效的
                if (pathEnvelope.Width > 0 && pathEnvelope.Height > 0)
                {
                    axMapControl1.Extent = pathEnvelope;
                }
                
                axMapControl1.Refresh();

                double kmLength = length / 1000;
                ShowPathResult($"路径长度：{kmLength:F2} km");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "失败：" +
                    ex.Message
                );
            }
            finally
            {
                mPointCollection = null;
            }
        }

        #endregion

        #region 按钮路径规划

        private void button1_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cbo_StartPoint.SelectedIndex < 0 ||
                    cbo_EndPoint.SelectedIndex < 0)
                {
                    throw new Exception("请选择起点终点");
                }

                // Determine travel mode and set speed (meters per second)
                string travelMode = "车行";
                if (cbo_TravelMode != null && cbo_TravelMode.SelectedItem != null)
                    travelMode = cbo_TravelMode.SelectedItem.ToString();
                // Car: 15 km/h, Pedestrian: 5 km/h
                double speedMps = travelMode == "车行" ? 40.0/3.6 : 5.0/3.6; // speed in meters/second

                // 构建点集合
                mPointCollection = new Multipoint() as IPointCollection;
                object miss = Type.Missing;
                mPointCollection.AddPoint(heritagePoints[cbo_StartPoint.SelectedIndex], ref miss, ref miss);
                if (cbo_WayPoint.SelectedIndex >= 0)
                    mPointCollection.AddPoint(heritagePoints[cbo_WayPoint.SelectedIndex], ref miss, ref miss);
                mPointCollection.AddPoint(heritagePoints[cbo_EndPoint.SelectedIndex], ref miss, ref miss);

                // 先获取长度
                double length = SolvePath("Length");
                // 再获取时间（根据行驶模式）
                double time = speedMps > 0 ? length / speedMps : 0; // estimated time in seconds

                IPolyline line = PathToPolyline();
                if (line == null)
                    throw new Exception("无法生成路径");

                mCurrentPathLine = line;
                DrawPath(line);

                // 标注起止点
                AddPointLabel(heritagePoints[cbo_StartPoint.SelectedIndex], cbo_StartPoint.Text, "起点");
                if (cbo_WayPoint.SelectedIndex >= 0)
                    AddPointLabel(heritagePoints[cbo_WayPoint.SelectedIndex], cbo_WayPoint.Text, "途经点");
                AddPointLabel(heritagePoints[cbo_EndPoint.SelectedIndex], cbo_EndPoint.Text, "终点");

                // 缩放到路径
                IEnvelope pathEnvelope = line.Envelope;
                IEnvelope mapExtent = axMapControl1.Extent;
                double bufferScale = Math.Max(mapExtent.Width, mapExtent.Height) * 0.1;
                pathEnvelope.Expand(bufferScale, bufferScale, false);
                if (pathEnvelope.Width > 0 && pathEnvelope.Height > 0)
                    axMapControl1.Extent = pathEnvelope;
                axMapControl1.Refresh();

                double kmLength = length / 1000;
                TimeSpan ts = TimeSpan.FromSeconds(time);
                string timeStr = $"{ts.Hours}:{ts.Minutes:D2}:{ts.Seconds:D2}";
                ShowPathResult($"路径长度：{kmLength:F2} km\r\n预计时间：{timeStr}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("失败：" + ex.Message);
            }
            finally
            {
                mPointCollection = null;
            }
        }

        #endregion

        #region 清空路径

        private void btn_ClearPath_Click(
            object sender,
            EventArgs e)
        {
            ClearPathPlanningResults();
        }

        #endregion

        #region 图层控制

        private void ToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            for (int i = 0;
                i < axMapControl1.LayerCount;
                i++)
            {
                ILayer lyr =
                    axMapControl1.get_Layer(i);

                if (lyr.Name == "heritages_cn")
                {
                    lyr.Visible =
                        !lyr.Visible;

                    break;
                }
            }

            axMapControl1.Refresh();
        }

        private void tsmi_View_ToggleRoadNetwork_Click(
            object sender,
            EventArgs e)
        {
            for (int i = 0;
                i < axMapControl1.LayerCount;
                i++)
            {
                ILayer lyr =
                    axMapControl1.get_Layer(i);

                if (lyr is IFeatureLayer)
                {
                    IFeatureLayer fl =
                        lyr as IFeatureLayer;

                    if (fl.FeatureClass != null)
                    {
                        if (fl.FeatureClass.ShapeType ==
                            esriGeometryType.esriGeometryPolyline)
                        {
                            lyr.Visible =
                                !lyr.Visible;
                        }
                    }
                }
            }

            axMapControl1.Refresh();
        }

        #endregion

        #region 地图工具

        private void tsmi_MapTool_ZoomIn_Click(object sender, EventArgs e)
        {
            // Centered zoom in: shrink extent size by half around its center
            IEnvelope env = axMapControl1.Extent;
            double factor = 0.5; // Zoom in factor (less than 1 shrinks the view)
            env.Expand(factor, factor, true);
            axMapControl1.Extent = env;
            axMapControl1.Refresh();
        }

        private void tsmi_MapTool_ZoomOut_Click(object sender, EventArgs e)
        {
            // Centered zoom out: expand extent size by factor 2 around its center
            IEnvelope env = axMapControl1.Extent;
            double factor = 2.0; // Zoom out factor (greater than 1 expands the view)
            env.Expand(factor, factor, true);
            axMapControl1.Extent = env;
            axMapControl1.Refresh();
        }

        private void tsmi_MapTool_Pan_Click(
            object sender,
            EventArgs e)
        {
            // 设置当前工具为手势平移工具，彻底激活平移拖拽响应
            ICommand cmd = new ControlsMapPanTool();
            cmd.OnCreate(axMapControl1.Object);
            cmd.OnClick();
            axMapControl1.CurrentTool = cmd as ITool;
        }

        #endregion

        #region 地图视图

        private void tsmi_View_FullExtent_Click(
            object sender,
            EventArgs e)
        {
            axMapControl1.Extent =
                axMapControl1.FullExtent;

            axMapControl1.Refresh();
        }

        private void tsmi_View_RefreshMap_Click(
            object sender,
            EventArgs e)
        {
            axMapControl1.Refresh();
        }

        #endregion

        #region 查询

        private void SyncQueryFormPosition()
        {
            if (queryFormInstance != null && !queryFormInstance.IsDisposed && queryFormInstance.Visible)
            {
                // 将查询窗口的位置贴在主窗口的外侧右边，并且高度与主窗口对齐，形成挂载体验
                queryFormInstance.Location = new System.Drawing.Point(this.Right, this.Top);
                queryFormInstance.Height = this.Height;
            }
        }

        private void tsmi_Query_Click(
            object sender,
            EventArgs e)
        {
            if (queryFormInstance == null || queryFormInstance.IsDisposed)
            {
                queryFormInstance = new FrmBuildingHeritageQuery();
                queryFormInstance.MainMap = this.axMapControl1;
                queryFormInstance.ClearPathPlanning += QueryForm_ClearPathPlanning;
                queryFormInstance.FormClosed += (s, ev) => {
                    queryFormInstance = null;
                    tsmi_Query.Checked = false;
                };

                // 设置外侧挂载窗口的定位与高度对齐
                queryFormInstance.StartPosition = FormStartPosition.Manual;
                queryFormInstance.Location = new System.Drawing.Point(this.Right, this.Top);
                queryFormInstance.Height = this.Height;
                queryFormInstance.Show(); // 直接在外侧展示，不加载到主窗口内部
            }
            else
            {
                // 切换外挂窗口的显示与隐藏状态，实现极佳的外侧挂载式体验
                if (queryFormInstance.Visible)
                {
                    queryFormInstance.Hide();
                }
                else
                {
                    queryFormInstance.Location = new System.Drawing.Point(this.Right, this.Top);
                    queryFormInstance.Height = this.Height;
                    queryFormInstance.Show();
                }
            }
            tsmi_Query.Checked = queryFormInstance.Visible;
        }

        private void QueryForm_ClearPathPlanning(object sender, EventArgs e)
        {
            ClearPathPlanningResults();
        }

        private void tsmi_Query_ClearResult_Click(
            object sender,
            EventArgs e)
        {
            // 清除当前选择集
            axMapControl1.Map.ClearSelection();

            // 缩放到全图范围，并刷新地图显示
            axMapControl1.Extent = axMapControl1.FullExtent;
            axMapControl1.Refresh();
        }

        #endregion

        #region ComboBox 选中定位

        private void cbo_Heritage_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                ComboBox cbo = sender as ComboBox;
                if (cbo == null || cbo.SelectedIndex < 0)
                {
                    return;
                }

                // 获取选中的点
                IPoint point = heritagePoints[cbo.SelectedIndex];
                if (point == null)
                {
                    return;
                }

                // 设置地图范围并定位到该点
                IEnvelope envelope = axMapControl1.Extent;
                double scale = Math.Min(envelope.Width, envelope.Height) * 0.2;
                
                envelope.CenterAt(point);
                envelope.Expand(scale, scale, false);

                axMapControl1.Extent = envelope;
                axMapControl1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "定位失败：\r\n\r\n" +
                    ex.Message);
            }
        }

        #endregion

        #region 导出路径

        private void tsmi_ExportPlanPath_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (mCurrentPathLine == null)
                {
                    MessageBox.Show(
                        "没有可导出的路径，请先规划路径");
                    return;
                }

                SaveFileDialog sfd =
                    new SaveFileDialog();

                sfd.Filter =
                    "Shapefile|*.shp";

                sfd.Title =
                    "导出路径";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string shpPath = sfd.FileName;
                string shpDir = System.IO.Path.GetDirectoryName(shpPath);
                string shpName = System.IO.Path.GetFileNameWithoutExtension(shpPath);

                // 创建 Shapefile 工作空间
                IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = shpWorkspaceFactory.OpenFromFile(shpDir, 0) as IFeatureWorkspace;

                // 创建字段集合
                IFields fields = new Fields();
                IFieldsEdit fieldsEdit = fields as IFieldsEdit;

                // 创建 Shape 字段
                IField shapeField = new Field();
                IFieldEdit shapeFieldEdit = shapeField as IFieldEdit;
                shapeFieldEdit.Name_2 = "Shape";
                shapeFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

                // 设置几何定义
                IGeometryDef geometryDef = new GeometryDef();
                IGeometryDefEdit geometryDefEdit = geometryDef as IGeometryDefEdit;
                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
                geometryDefEdit.SpatialReference_2 = axMapControl1.Map.SpatialReference;
                shapeFieldEdit.GeometryDef_2 = geometryDef;

                fieldsEdit.AddField(shapeField);

                // 创建 ID 字段
                IField idField = new Field();
                IFieldEdit idFieldEdit = idField as IFieldEdit;
                idFieldEdit.Name_2 = "ID";
                idFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                fieldsEdit.AddField(idField);

                // 创建长度字段
                IField lengthField = new Field();
                IFieldEdit lengthFieldEdit = lengthField as IFieldEdit;
                lengthFieldEdit.Name_2 = "Length";
                lengthFieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                fieldsEdit.AddField(lengthField);

                // 创建要素类
                IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(
                    shpName,
                    fields,
                    null,
                    null,
                    esriFeatureType.esriFTSimple,
                    "Shape",
                    "");

                // 插入要素
                IFeature feature = featureClass.CreateFeature();
                feature.Shape = mCurrentPathLine;
                feature.set_Value(featureClass.Fields.FindField("ID"), 1);
                feature.set_Value(featureClass.Fields.FindField("Length"), mdblPathCost);
                feature.Store();

                double kmLength = mdblPathCost / 1000;
                MessageBox.Show(
                    "路径导出成功！\r\n" +
                    "路径长度：" + kmLength.ToString("F2") + " km");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "导出失败：\r\n\r\n" +
                    ex.Message);
            }
        }

        #endregion

        #region 保存地图

        private void tsmi_SaveMapView_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                SaveFileDialog sfd =
                    new SaveFileDialog();

                sfd.Filter =
                    "JPEG图片|*.jpg";

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                IExport export = new ExportJPEG() as IExport;

                export.ExportFileName =
                    sfd.FileName;

                export.Resolution = 96;

                tagRECT exportRECT;
                exportRECT.left = 0;
                exportRECT.top = 0;
                exportRECT.right =
                    axMapControl1.Width;
                exportRECT.bottom =
                    axMapControl1.Height;

                IEnvelope envelope = new Envelope() as IEnvelope;

                envelope.PutCoords(
                    exportRECT.left,
                    exportRECT.top,
                    exportRECT.right,
                    exportRECT.bottom
                );

                export.PixelBounds =
                    envelope;

                int hDC =
                    export.StartExporting();

                axMapControl1.ActiveView.Output(
                    hDC,
                    96,
                    ref exportRECT,
                    null,
                    null
                );

                export.FinishExporting();

                export.Cleanup();

                MessageBox.Show(
                    "地图保存成功"
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message
                );
            }
        }

        #endregion

        #region 退出

        private void tsmi_Exit_Click(
            object sender,
            EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeFormHeader()
        {
            try
            {
                pnlHeader = new Panel();
                pnlHeader.Height = 65;
                pnlHeader.Dock = DockStyle.Top;
                pnlHeader.BackColor = Color.FromArgb(92, 53, 26); // 富贵金棕色底色
                pnlHeader.Padding = new Padding(10, 0, 10, 0);

                // 1. 图标 (Unicode 古典建筑纪念碑符号 🏛，金色)
                Label lblIcon = new Label();
                lblIcon.Text = "🏛";
                lblIcon.Font = new Font("Segoe UI Symbol", 24F, FontStyle.Regular);
                lblIcon.ForeColor = Color.FromArgb(218, 165, 32); // 金黄色 Gold
                lblIcon.AutoSize = true;
                lblIcon.Location = new System.Drawing.Point(15, 8);
                pnlHeader.Controls.Add(lblIcon);

                // 2. 主标题
                Label lblTitle = new Label();
                lblTitle.Text = "上海市优秀建筑与不可移动文物路径规划系统";
                lblTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
                lblTitle.ForeColor = Color.FromArgb(248, 242, 230); // 暖白/麦色
                lblTitle.AutoSize = true;
                lblTitle.Location = new System.Drawing.Point(65, 8);
                pnlHeader.Controls.Add(lblTitle);

                // 3. 副标题
                Label lblSubtitle = new Label();
                lblSubtitle.Text = "SHANGHAI CULTURAL HERITAGE & HISTORICAL ARCHITECTURE PATH SYSTEM";
                lblSubtitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                lblSubtitle.ForeColor = Color.FromArgb(197, 160, 89); // 青铜/暗金色
                lblSubtitle.AutoSize = true;
                lblSubtitle.Location = new System.Drawing.Point(67, 36);
                pnlHeader.Controls.Add(lblSubtitle);

                // 4. 底部金棕色装饰细线 (3px 高度，增加立体层次感)
                Panel pnlBorder = new Panel();
                pnlBorder.Height = 3;
                pnlBorder.Dock = DockStyle.Bottom;
                pnlBorder.BackColor = Color.FromArgb(197, 160, 89); // 金属铜金色
                pnlHeader.Controls.Add(pnlBorder);

                // 将 Header 加入主窗体 Controls
                this.Controls.Add(pnlHeader);

                // 使用 SuspendLayout/ResumeLayout 确保重新布局过程平滑且不闪烁，并彻底解决 Z-order 重叠遮挡 Bug
                this.SuspendLayout();
                
                // 在 WinForms 停靠布局中，ChildIndex 越小（越接近 0），停靠优先级越低（越往内侧/后处理）
                // 停靠为 Fill 的 splitContainer1 必须拥有最小索引 0（最后处理，填充剩余中心区域）
                // 顶层停靠控件依次拥有更大索引，从内往外排列：
                if (splitContainer1 != null)
                {
                    this.Controls.SetChildIndex(splitContainer1, 0);
                }
                if (axToolbarControl1 != null)
                {
                    this.Controls.SetChildIndex(axToolbarControl1, 1);
                }
                if (menuStrip1 != null)
                {
                    this.Controls.SetChildIndex(menuStrip1, 2);
                }
                this.Controls.SetChildIndex(pnlHeader, 3);
                if (statusStrip1 != null)
                {
                    this.Controls.SetChildIndex(statusStrip1, 4);
                }

                this.ResumeLayout(true);
                this.PerformLayout();
            }
            catch
            {
            }
        }

        private void StyleForm1Controls()
        {
            try
            {
                // 1. 整体底色美化
                this.BackColor = Color.FromArgb(250, 248, 245);
                groupBox1.ForeColor = Color.FromArgb(92, 53, 26);

                // 2. 菜单栏和工具栏现代感扁平化
                menuStrip1.BackColor = Color.FromArgb(70, 38, 16);
                statusStrip1.BackColor = Color.FromArgb(240, 240, 242);

                foreach (ToolStripItem item in menuStrip1.Items)
                {
                    item.ForeColor = Color.FromArgb(240, 230, 215);
                }

                // 3. 规划按钮美化 (金棕色)
                btn_PlanPath.FlatStyle = FlatStyle.Flat;
                btn_PlanPath.FlatAppearance.BorderSize = 0;
                btn_PlanPath.BackColor = Color.FromArgb(120, 80, 38);
                btn_PlanPath.ForeColor = Color.White;
                btn_PlanPath.Cursor = Cursors.Hand;

                // 4. 清空规划按钮美化 (白底金边)
                btn_ClearPath.FlatStyle = FlatStyle.Flat;
                btn_ClearPath.FlatAppearance.BorderSize = 1;
                btn_ClearPath.FlatAppearance.BorderColor = Color.FromArgb(197, 160, 89);
                btn_ClearPath.BackColor = Color.White;
                btn_ClearPath.ForeColor = Color.FromArgb(120, 80, 38);
                btn_ClearPath.Cursor = Cursors.Hand;

                // 5. 下拉列表样式
                cbo_StartPoint.FlatStyle = FlatStyle.Flat;
                cbo_WayPoint.FlatStyle = FlatStyle.Flat;
                cbo_EndPoint.FlatStyle = FlatStyle.Flat;
                cbo_TravelMode.FlatStyle = FlatStyle.Flat;
            }
            catch
            {
                // 防御性容错
            }
        }

        private void OptimizeSidebarLayout()
        {
            try
            {
                // 1. 动态设定 Splitter 比例，防止 TOCControl 过高压迫路径规划面板，解决高DPI缩放裁剪Bug
                splitContainer2.SplitterDistance = (int)(splitContainer2.Height * 0.28);

                // 2. 将 GroupBox 转化为无边框现代卡片式 Panel 样式，移除老旧的 3D GroupBox 边框
                groupBox1.FlatStyle = FlatStyle.Flat;
                groupBox1.Paint += (s, e) =>
                {
                    // 仅绘制顶部一条淡淡的金属铜装饰线，比粗糙的 GroupBox 外框高级 100 倍
                    using (Pen p = new Pen(Color.FromArgb(220, 210, 195), 1f))
                    {
                        e.Graphics.DrawLine(p, 10, 5, groupBox1.Width - 10, 5);
                    }
                };

                // 3. 紧凑排布所有子控件，彻底解决 high-DPI 缩放裁剪 Bug
                Color labelColor = Color.FromArgb(120, 80, 38);
                Font labelFont = new Font("微软雅黑", 9F, FontStyle.Bold);
                Font comboFont = new Font("微软雅黑", 9.5F, FontStyle.Regular);

                // 起点
                label1.Font = labelFont;
                label1.ForeColor = labelColor;
                label1.Location = new System.Drawing.Point(15, 20);
                cbo_StartPoint.Font = comboFont;
                cbo_StartPoint.Location = new System.Drawing.Point(15, 45);
                cbo_StartPoint.Width = groupBox1.Width - 30;

                // 途经点
                label2.Font = labelFont;
                label2.ForeColor = labelColor;
                label2.Location = new System.Drawing.Point(15, 85);
                cbo_WayPoint.Font = comboFont;
                cbo_WayPoint.Location = new System.Drawing.Point(15, 110);
                cbo_WayPoint.Width = groupBox1.Width - 30;

                // 终点
                label3.Font = labelFont;
                label3.ForeColor = labelColor;
                label3.Location = new System.Drawing.Point(15, 150);
                cbo_EndPoint.Font = comboFont;
                cbo_EndPoint.Location = new System.Drawing.Point(15, 175);
                cbo_EndPoint.Width = groupBox1.Width - 30;

                // 行驶模式
                labelTravelMode.Font = labelFont;
                labelTravelMode.ForeColor = labelColor;
                labelTravelMode.Location = new System.Drawing.Point(15, 215);
                cbo_TravelMode.Font = comboFont;
                cbo_TravelMode.Location = new System.Drawing.Point(15, 240);
                cbo_TravelMode.Width = groupBox1.Width - 30;

                // 开始规划按钮 (扁平圆角感金棕色)
                btn_PlanPath.Location = new System.Drawing.Point(15, 280);
                btn_PlanPath.Width = groupBox1.Width - 30;
                btn_PlanPath.Height = 40;
                btn_PlanPath.Font = new Font("微软雅黑", 9.5F, FontStyle.Bold);

                // 清空路径按钮 (扁平白底金边)
                btn_ClearPath.Location = new System.Drawing.Point(15, 330);
                btn_ClearPath.Width = groupBox1.Width - 30;
                btn_ClearPath.Height = 35;
                btn_ClearPath.Font = new Font("微软雅黑", 9F, FontStyle.Regular);

                // 4. 下拉框背景色与边框美化
                cbo_StartPoint.BackColor = Color.White;
                cbo_WayPoint.BackColor = Color.White;
                cbo_EndPoint.BackColor = Color.White;
                cbo_TravelMode.BackColor = Color.White;

                // 5. 绑定容器大小改变事件，自适应宽度
                groupBox1.SizeChanged += (s, e) =>
                {
                    cbo_StartPoint.Width = groupBox1.Width - 30;
                    cbo_WayPoint.Width = groupBox1.Width - 30;
                    cbo_EndPoint.Width = groupBox1.Width - 30;
                    cbo_TravelMode.Width = groupBox1.Width - 30;
                    btn_PlanPath.Width = groupBox1.Width - 30;
                    btn_ClearPath.Width = groupBox1.Width - 30;
                };

                // 6. 绑定 splitContainer2 大小改变事件，动态调整以防止折叠
                splitContainer2.SizeChanged += (s, e) =>
                {
                    splitContainer2.SplitterDistance = (int)(splitContainer2.Height * 0.28);
                };
            }
            catch { }
        }

        private void InitializeFloatingMapTools()
        {
            try
            {
                // 1. 将原有的 ActiveX 工具条完全隐藏，腾出宝贵的纵向空间并让 UI 扁平纯净
                if (axToolbarControl1 != null)
                {
                    axToolbarControl1.Visible = false;
                }

                // 2. 创建现代感极强的浮动式 GIS 地图导航工具条 (Panel)
                Panel pnlMapTools = new Panel();
                pnlMapTools.Width = 42;
                pnlMapTools.Height = 210;
                pnlMapTools.BackColor = Color.FromArgb(235, 92, 53, 26); // 高级半透明金棕色底色
                pnlMapTools.BorderStyle = BorderStyle.None;

                // 绘制极细金属铜色边框线
                pnlMapTools.Paint += (s, e) =>
                {
                    using (Pen p = new Pen(Color.FromArgb(197, 160, 89), 1.5f))
                    {
                        e.Graphics.DrawRectangle(p, 0, 0, pnlMapTools.Width - 1, pnlMapTools.Height - 1);
                    }
                };

                // 3. 配置现代化高精细度扁平导航按钮群
                string[] tooltips = { "放大 (Zoom In)", "缩小 (Zoom Out)", "平移 (Pan Map)", "全图显示 (Full Extent)", "刷新地图 (Refresh View)" };
                string[] icons = { "➕", "➖", "✋", "🗺️", "🔄" };
                EventHandler[] handlers = {
                    tsmi_MapTool_ZoomIn_Click,
                    tsmi_MapTool_ZoomOut_Click,
                    tsmi_MapTool_Pan_Click,
                    tsmi_View_FullExtent_Click,
                    tsmi_View_RefreshMap_Click
                };

                for (int i = 0; i < icons.Length; i++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(34, 34);
                    btn.Location = new System.Drawing.Point(4, 6 + i * 40);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 197, 160, 89); // 优雅的微亮金属铜色
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(120, 197, 160, 89);
                    btn.Text = icons[i];
                    btn.Font = new Font("Segoe UI Symbol", 11F, FontStyle.Bold);
                    btn.ForeColor = Color.FromArgb(248, 242, 230); // 暖白麦色
                    btn.Cursor = Cursors.Hand;

                    ToolTip tt = new ToolTip();
                    tt.SetToolTip(btn, tooltips[i]);

                    int index = i;
                    btn.Click += handlers[index];

                    pnlMapTools.Controls.Add(btn);
                }

                // 4. 将浮动工具条加入窗体控件树并置于最顶层
                this.Controls.Add(pnlMapTools);
                pnlMapTools.BringToFront();

                // 5. 声明位置随动同步函数，使其始终挂在地图控件左上角 (15, 15) 像素处
                Action syncPosition = () =>
                {
                    if (axMapControl1 == null || pnlMapTools == null) return;
                    try
                    {
                        System.Drawing.Point mapTopLeftInScreen = axMapControl1.PointToScreen(new System.Drawing.Point(15, 15));
                        System.Drawing.Point locationInForm = this.PointToClient(mapTopLeftInScreen);
                        pnlMapTools.Location = locationInForm;
                        pnlMapTools.BringToFront();
                    }
                    catch { }
                };

                // 订阅各类大小变化事件
                axMapControl1.Resize += (s, e) => syncPosition();
                this.SizeChanged += (s, e) => syncPosition();
                this.Shown += (s, e) => syncPosition();

                // 用微型定时器做初始加载延迟，确保地图完全加载后再计算坐标
                System.Windows.Forms.Timer delayTimer = new System.Windows.Forms.Timer();
                delayTimer.Interval = 200;
                delayTimer.Tick += (s, e) => {
                    syncPosition();
                    delayTimer.Stop();
                    delayTimer.Dispose();
                };
                delayTimer.Start();
            }
            catch { }
        }

        #endregion
    }
}