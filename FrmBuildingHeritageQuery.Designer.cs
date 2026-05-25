namespace Route_of_Shanghai
{
    partial class FrmBuildingHeritageQuery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_QueryName = new System.Windows.Forms.Label();
            this.lbl_QueryRegion = new System.Windows.Forms.Label();
            this.lbl_QueryType = new System.Windows.Forms.Label();
            this.txt_QueryName = new System.Windows.Forms.TextBox();
            this.cbo_QueryRegion = new System.Windows.Forms.ComboBox();
            this.cbo_QueryType = new System.Windows.Forms.ComboBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.btn_ClearCondition = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_QueryName
            // 
            this.lbl_QueryName.AutoSize = true;
            this.lbl_QueryName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_QueryName.Location = new System.Drawing.Point(20, 25);
            this.lbl_QueryName.Name = "lbl_QueryName";
            this.lbl_QueryName.Size = new System.Drawing.Size(80, 20);
            this.lbl_QueryName.TabIndex = 0;
            this.lbl_QueryName.Text = "名称查询";
            // 
            // lbl_QueryRegion
            // 
            this.lbl_QueryRegion.AutoSize = true;
            this.lbl_QueryRegion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_QueryRegion.Location = new System.Drawing.Point(20, 90);
            this.lbl_QueryRegion.Name = "lbl_QueryRegion";
            this.lbl_QueryRegion.Size = new System.Drawing.Size(80, 20);
            this.lbl_QueryRegion.TabIndex = 1;
            this.lbl_QueryRegion.Text = "区域筛选";
            // 
            // lbl_QueryType
            // 
            this.lbl_QueryType.AutoSize = true;
            this.lbl_QueryType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_QueryType.Location = new System.Drawing.Point(20, 155);
            this.lbl_QueryType.Name = "lbl_QueryType";
            this.lbl_QueryType.Size = new System.Drawing.Size(80, 20);
            this.lbl_QueryType.TabIndex = 2;
            this.lbl_QueryType.Text = "类型筛选";
            // 
            // txt_QueryName
            // 
            this.txt_QueryName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txt_QueryName.Location = new System.Drawing.Point(20, 48);
            this.txt_QueryName.Name = "txt_QueryName";
            this.txt_QueryName.Size = new System.Drawing.Size(240, 27);
            this.txt_QueryName.TabIndex = 3;
            // 
            // cbo_QueryRegion
            // 
            this.cbo_QueryRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_QueryRegion.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cbo_QueryRegion.FormattingEnabled = true;
            this.cbo_QueryRegion.Location = new System.Drawing.Point(20, 113);
            this.cbo_QueryRegion.Name = "cbo_QueryRegion";
            this.cbo_QueryRegion.Size = new System.Drawing.Size(240, 28);
            this.cbo_QueryRegion.TabIndex = 5;
            // 
            // cbo_QueryType
            // 
            this.cbo_QueryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_QueryType.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.cbo_QueryType.FormattingEnabled = true;
            this.cbo_QueryType.Location = new System.Drawing.Point(20, 178);
            this.cbo_QueryType.Name = "cbo_QueryType";
            this.cbo_QueryType.Size = new System.Drawing.Size(240, 28);
            this.cbo_QueryType.TabIndex = 6;
            // 
            // btn_Search
            // 
            this.btn_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Search.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(20, 240);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(240, 45);
            this.btn_Search.TabIndex = 7;
            this.btn_Search.Text = "联合查询";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_ClearCondition
            // 
            this.btn_ClearCondition.BackColor = System.Drawing.Color.LightGray;
            this.btn_ClearCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ClearCondition.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btn_ClearCondition.Location = new System.Drawing.Point(20, 295);
            this.btn_ClearCondition.Name = "btn_ClearCondition";
            this.btn_ClearCondition.Size = new System.Drawing.Size(240, 40);
            this.btn_ClearCondition.TabIndex = 8;
            this.btn_ClearCondition.Text = "清空条件";
            this.btn_ClearCondition.UseVisualStyleBackColor = false;
            this.btn_ClearCondition.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(10, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(680, 490);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(20, 350);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(240, 40);
            this.button3.TabIndex = 10;
            this.button3.Text = "按名称查询";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(20, 440);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(240, 40);
            this.button4.TabIndex = 11;
            this.button4.Text = "按类别查询";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(20, 395);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(240, 40);
            this.button5.TabIndex = 12;
            this.button5.Text = "按区域查询";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbl_QueryName);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.txt_QueryName);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.lbl_QueryRegion);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.cbo_QueryRegion);
            this.groupBox1.Controls.Add(this.lbl_QueryType);
            this.groupBox1.Controls.Add(this.btn_ClearCondition);
            this.groupBox1.Controls.Add(this.cbo_QueryType);
            this.groupBox1.Controls.Add(this.btn_Search);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(280, 530);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.groupBox2.Location = new System.Drawing.Point(280, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(700, 530);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询结果 (双击行定位)";
            // 
            // FrmBuildingHeritageQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 530);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmBuildingHeritageQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "优秀建筑/文物点查询";
            this.Load += new System.EventHandler(this.FrmBuildingHeritageQuery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_QueryName;
        private System.Windows.Forms.Label lbl_QueryRegion;
        private System.Windows.Forms.Label lbl_QueryType;
        private System.Windows.Forms.TextBox txt_QueryName;
        private System.Windows.Forms.ComboBox cbo_QueryRegion;
        private System.Windows.Forms.ComboBox cbo_QueryType;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_ClearCondition;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}
