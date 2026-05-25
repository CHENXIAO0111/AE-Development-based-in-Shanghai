namespace Route_of_Shanghai
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem tsmi_Query_ClearResult;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip_File = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_ExportPlanPath = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_SaveMapView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_View = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_View_FullExtent = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_View_RefreshMap = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_View_ToggleBuildingHeritage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_View_ToggleRoadNetwork = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_MapTool = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_MapTool_ZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_MapTool_ZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_MapTool_Pan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip_BuildingHeritageQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Query = new System.Windows.Forms.ToolStripMenuItem();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbo_EndPoint = new System.Windows.Forms.ComboBox();
            this.cbo_WayPoint = new System.Windows.Forms.ComboBox();
            this.cbo_StartPoint = new System.Windows.Forms.ComboBox();
            this.btn_ClearPath = new System.Windows.Forms.Button();
            this.btn_PlanPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            tsmi_Query_ClearResult = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsmi_Query_ClearResult
            // 
            tsmi_Query_ClearResult.Name = "tsmi_Query_ClearResult";
            tsmi_Query_ClearResult.Size = new System.Drawing.Size(200, 30);
            tsmi_Query_ClearResult.Text = "清空查询结果";
            tsmi_Query_ClearResult.Click += new System.EventHandler(this.tsmi_Query_ClearResult_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStrip_File,
            this.menuStrip_View,
            this.menuStrip_MapTool,
            this.menuStrip_BuildingHeritageQuery});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1200, 32);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuStrip_File
            // 
            this.menuStrip_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_ExportPlanPath,
            this.tsmi_SaveMapView,
            this.tsmi_Exit});
            this.menuStrip_File.Name = "menuStrip_File";
            this.menuStrip_File.Size = new System.Drawing.Size(58, 28);
            this.menuStrip_File.Text = "文件";
            // 
            // tsmi_ExportPlanPath
            // 
            this.tsmi_ExportPlanPath.Name = "tsmi_ExportPlanPath";
            this.tsmi_ExportPlanPath.Size = new System.Drawing.Size(200, 30);
            this.tsmi_ExportPlanPath.Text = "导出规划路径";
            // 
            // tsmi_SaveMapView
            // 
            this.tsmi_SaveMapView.Name = "tsmi_SaveMapView";
            this.tsmi_SaveMapView.Size = new System.Drawing.Size(200, 30);
            this.tsmi_SaveMapView.Text = "保存地图视图";
            // 
            // tsmi_Exit
            // 
            this.tsmi_Exit.Name = "tsmi_Exit";
            this.tsmi_Exit.Size = new System.Drawing.Size(200, 30);
            this.tsmi_Exit.Text = "退出";
            // 
            // menuStrip_View
            // 
            this.menuStrip_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_View_FullExtent,
            this.tsmi_View_RefreshMap,
            this.tsmi_View_ToggleBuildingHeritage,
            this.tsmi_View_ToggleRoadNetwork});
            this.menuStrip_View.Name = "menuStrip_View";
            this.menuStrip_View.Size = new System.Drawing.Size(58, 28);
            this.menuStrip_View.Text = "视图";
            // 
            // tsmi_View_FullExtent
            // 
            this.tsmi_View_FullExtent.Name = "tsmi_View_FullExtent";
            this.tsmi_View_FullExtent.Size = new System.Drawing.Size(316, 30);
            this.tsmi_View_FullExtent.Text = "全图显示";
            // 
            // tsmi_View_RefreshMap
            // 
            this.tsmi_View_RefreshMap.Name = "tsmi_View_RefreshMap";
            this.tsmi_View_RefreshMap.Size = new System.Drawing.Size(316, 30);
            this.tsmi_View_RefreshMap.Text = "刷新地图";
            // 
            // tsmi_View_ToggleBuildingHeritage
            // 
            this.tsmi_View_ToggleBuildingHeritage.Name = "tsmi_View_ToggleBuildingHeritage";
            this.tsmi_View_ToggleBuildingHeritage.Size = new System.Drawing.Size(316, 30);
            this.tsmi_View_ToggleBuildingHeritage.Text = "显示/隐藏优秀建筑或文物点";
            this.tsmi_View_ToggleBuildingHeritage.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // tsmi_View_ToggleRoadNetwork
            // 
            this.tsmi_View_ToggleRoadNetwork.Name = "tsmi_View_ToggleRoadNetwork";
            this.tsmi_View_ToggleRoadNetwork.Size = new System.Drawing.Size(316, 30);
            this.tsmi_View_ToggleRoadNetwork.Text = "显示/隐藏路网";
            // 
            // menuStrip_MapTool
            // 
            this.menuStrip_MapTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_MapTool_ZoomIn,
            this.tsmi_MapTool_ZoomOut,
            this.tsmi_MapTool_Pan});
            this.menuStrip_MapTool.Name = "menuStrip_MapTool";
            this.menuStrip_MapTool.Size = new System.Drawing.Size(94, 28);
            this.menuStrip_MapTool.Text = "地图工具";
            // 
            // tsmi_MapTool_ZoomIn
            // 
            this.tsmi_MapTool_ZoomIn.Name = "tsmi_MapTool_ZoomIn";
            this.tsmi_MapTool_ZoomIn.Size = new System.Drawing.Size(164, 30);
            this.tsmi_MapTool_ZoomIn.Text = "居中放大";
            this.tsmi_MapTool_ZoomIn.Click += new System.EventHandler(this.tsmi_MapTool_ZoomIn_Click);
            // 
            // tsmi_MapTool_ZoomOut
            // 
            this.tsmi_MapTool_ZoomOut.Name = "tsmi_MapTool_ZoomOut";
            this.tsmi_MapTool_ZoomOut.Size = new System.Drawing.Size(164, 30);
            this.tsmi_MapTool_ZoomOut.Text = "居中缩小";
            this.tsmi_MapTool_ZoomOut.Click += new System.EventHandler(this.tsmi_MapTool_ZoomOut_Click);
            // 
            // tsmi_MapTool_Pan
            // 
            this.tsmi_MapTool_Pan.Name = "tsmi_MapTool_Pan";
            this.tsmi_MapTool_Pan.Size = new System.Drawing.Size(164, 30);
            this.tsmi_MapTool_Pan.Text = "平移";
            this.tsmi_MapTool_Pan.Click += new System.EventHandler(this.tsmi_MapTool_Pan_Click);
            // 
            // menuStrip_BuildingHeritageQuery
            // 
            this.menuStrip_BuildingHeritageQuery.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Query,
            tsmi_Query_ClearResult});
            this.menuStrip_BuildingHeritageQuery.Name = "menuStrip_BuildingHeritageQuery";
            this.menuStrip_BuildingHeritageQuery.Size = new System.Drawing.Size(166, 28);
            this.menuStrip_BuildingHeritageQuery.Text = "建筑或文物点查询";
            // 
            // tsmi_Query
            // 
            this.tsmi_Query.Name = "tsmi_Query";
            this.tsmi_Query.Size = new System.Drawing.Size(200, 30);
            this.tsmi_Query.Text = "进入查询";
            this.tsmi_Query.Click += new System.EventHandler(this.tsmi_Query_Click);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 32);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1200, 28);
            this.axToolbarControl1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.axLicenseControl1);
            this.splitContainer1.Panel2.Controls.Add(this.axMapControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 611);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.axTOCControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(280, 611);
            this.splitContainer2.SplitterDistance = 124;
            this.splitContainer2.TabIndex = 0;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(0, 0);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(280, 124);
            this.axTOCControl1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbo_EndPoint);
            this.groupBox1.Controls.Add(this.cbo_WayPoint);
            this.groupBox1.Controls.Add(this.cbo_StartPoint);
            this.groupBox1.Controls.Add(this.btn_ClearPath);
            this.groupBox1.Controls.Add(this.btn_PlanPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(280, 483);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "路径规划";
            // 
            // cbo_EndPoint
            // 
            this.cbo_EndPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_EndPoint.FormattingEnabled = true;
            this.cbo_EndPoint.Location = new System.Drawing.Point(20, 190);
            this.cbo_EndPoint.Name = "cbo_EndPoint";
            this.cbo_EndPoint.Size = new System.Drawing.Size(240, 26);
            this.cbo_EndPoint.TabIndex = 9;
            // 
            // cbo_WayPoint
            // 
            this.cbo_WayPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_WayPoint.FormattingEnabled = true;
            this.cbo_WayPoint.Location = new System.Drawing.Point(20, 120);
            this.cbo_WayPoint.Name = "cbo_WayPoint";
            this.cbo_WayPoint.Size = new System.Drawing.Size(240, 26);
            this.cbo_WayPoint.TabIndex = 8;
            // 
            // cbo_StartPoint
            // 
            this.cbo_StartPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_StartPoint.FormattingEnabled = true;
            this.cbo_StartPoint.Location = new System.Drawing.Point(20, 55);
            this.cbo_StartPoint.Name = "cbo_StartPoint";
            this.cbo_StartPoint.Size = new System.Drawing.Size(240, 26);
            this.cbo_StartPoint.TabIndex = 2;
            // 
            // btn_ClearPath
            // 
            this.btn_ClearPath.BackColor = System.Drawing.Color.LightGray;
            this.btn_ClearPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ClearPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btn_ClearPath.Location = new System.Drawing.Point(20, 320);
            this.btn_ClearPath.Name = "btn_ClearPath";
            this.btn_ClearPath.Size = new System.Drawing.Size(240, 40);
            this.btn_ClearPath.TabIndex = 7;
            this.btn_ClearPath.Text = "清空规划路径";
            this.btn_ClearPath.UseVisualStyleBackColor = false;
            // 
            // btn_PlanPath
            // 
            this.btn_PlanPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btn_PlanPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PlanPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btn_PlanPath.ForeColor = System.Drawing.Color.White;
            this.btn_PlanPath.Location = new System.Drawing.Point(20, 265);
            this.btn_PlanPath.Name = "btn_PlanPath";
            this.btn_PlanPath.Size = new System.Drawing.Size(240, 45);
            this.btn_PlanPath.TabIndex = 6;
            this.btn_PlanPath.Text = "开始规划";
            this.btn_PlanPath.UseVisualStyleBackColor = false;
            this.btn_PlanPath.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(17, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "终点";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(17, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "途经点(可选)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(17, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "起点选择";
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(50, 7);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(0, 0);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(916, 611);
            this.axMapControl1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 671);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1200, 29);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(82, 24);
            this.toolStripStatusLabel1.Text = "准备就绪";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 700);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上海市历史遗产路径规划系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.ToolStripMenuItem menuStrip_File;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ExportPlanPath;
        private System.Windows.Forms.ToolStripMenuItem tsmi_SaveMapView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Exit;
        private System.Windows.Forms.ToolStripMenuItem menuStrip_View;
        private System.Windows.Forms.ToolStripMenuItem tsmi_View_FullExtent;
        private System.Windows.Forms.ToolStripMenuItem tsmi_View_RefreshMap;
        private System.Windows.Forms.ToolStripMenuItem tsmi_View_ToggleBuildingHeritage;
        private System.Windows.Forms.ToolStripMenuItem tsmi_View_ToggleRoadNetwork;
        private System.Windows.Forms.Button btn_PlanPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem menuStrip_MapTool;
        private System.Windows.Forms.ToolStripMenuItem tsmi_MapTool_ZoomIn;
        private System.Windows.Forms.ToolStripMenuItem tsmi_MapTool_ZoomOut;
        private System.Windows.Forms.ToolStripMenuItem tsmi_MapTool_Pan;
        private System.Windows.Forms.ToolStripMenuItem menuStrip_BuildingHeritageQuery;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Query;
        private System.Windows.Forms.Button btn_ClearPath;
        private System.Windows.Forms.ComboBox cbo_EndPoint;
        private System.Windows.Forms.ComboBox cbo_WayPoint;
        private System.Windows.Forms.ComboBox cbo_StartPoint;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

