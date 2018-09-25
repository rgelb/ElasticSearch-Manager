namespace ElasticSearchManager {
    partial class frmMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.appStatusBar = new System.Windows.Forms.StatusStrip();
            this.lblAppStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.appSplitContainer = new System.Windows.Forms.SplitContainer();
            this.treeEntities = new System.Windows.Forms.TreeView();
            this.grdEntities = new System.Windows.Forms.DataGridView();
            this.appToolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cboConnections = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtToolbarSearch = new System.Windows.Forms.ToolStripTextBox();
            this.lbl = new System.Windows.Forms.ToolStripLabel();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnToolbarSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.appStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appSplitContainer)).BeginInit();
            this.appSplitContainer.Panel1.SuspendLayout();
            this.appSplitContainer.Panel2.SuspendLayout();
            this.appSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEntities)).BeginInit();
            this.appToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.appStatusBar);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.appSplitContainer);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1213, 555);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1213, 608);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.appToolbar);
            // 
            // appStatusBar
            // 
            this.appStatusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.appStatusBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.appStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAppStatus});
            this.appStatusBar.Location = new System.Drawing.Point(0, 0);
            this.appStatusBar.Name = "appStatusBar";
            this.appStatusBar.Size = new System.Drawing.Size(1213, 22);
            this.appStatusBar.TabIndex = 0;
            // 
            // lblAppStatus
            // 
            this.lblAppStatus.Name = "lblAppStatus";
            this.lblAppStatus.Size = new System.Drawing.Size(26, 17);
            this.lblAppStatus.Text = "Idle";
            // 
            // appSplitContainer
            // 
            this.appSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.appSplitContainer.Margin = new System.Windows.Forms.Padding(2);
            this.appSplitContainer.Name = "appSplitContainer";
            // 
            // appSplitContainer.Panel1
            // 
            this.appSplitContainer.Panel1.Controls.Add(this.treeEntities);
            // 
            // appSplitContainer.Panel2
            // 
            this.appSplitContainer.Panel2.Controls.Add(this.grdEntities);
            this.appSplitContainer.Size = new System.Drawing.Size(1213, 555);
            this.appSplitContainer.SplitterDistance = 330;
            this.appSplitContainer.SplitterWidth = 3;
            this.appSplitContainer.TabIndex = 0;
            // 
            // treeEntities
            // 
            this.treeEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeEntities.HideSelection = false;
            this.treeEntities.Location = new System.Drawing.Point(0, 0);
            this.treeEntities.Margin = new System.Windows.Forms.Padding(2);
            this.treeEntities.Name = "treeEntities";
            this.treeEntities.Size = new System.Drawing.Size(330, 555);
            this.treeEntities.TabIndex = 0;
            this.treeEntities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeIndexes_AfterSelect);
            // 
            // grdEntities
            // 
            this.grdEntities.AllowUserToAddRows = false;
            this.grdEntities.AllowUserToOrderColumns = true;
            this.grdEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEntities.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdEntities.Location = new System.Drawing.Point(43, 51);
            this.grdEntities.Name = "grdEntities";
            this.grdEntities.ReadOnly = true;
            this.grdEntities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdEntities.Size = new System.Drawing.Size(352, 176);
            this.grdEntities.TabIndex = 0;
            // 
            // appToolbar
            // 
            this.appToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.appToolbar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.appToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cboConnections,
            this.toolStripSeparator2,
            this.btnDelete,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.txtToolbarSearch,
            this.btnToolbarSearch,
            this.lbl,
            this.toolStripSeparator3,
            this.btnRefresh});
            this.appToolbar.Location = new System.Drawing.Point(3, 0);
            this.appToolbar.Name = "appToolbar";
            this.appToolbar.Size = new System.Drawing.Size(602, 31);
            this.appToolbar.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(74, 28);
            this.toolStripLabel1.Text = "Connections";
            // 
            // cboConnections
            // 
            this.cboConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConnections.Name = "cboConnections";
            this.cboConnections.Size = new System.Drawing.Size(121, 31);
            this.cboConnections.SelectedIndexChanged += new System.EventHandler(this.cboConnections_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(33, 28);
            this.toolStripLabel2.Text = "Filter";
            // 
            // txtToolbarSearch
            // 
            this.txtToolbarSearch.Name = "txtToolbarSearch";
            this.txtToolbarSearch.Size = new System.Drawing.Size(130, 31);
            this.txtToolbarSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToolbarSearch_KeyDown);
            // 
            // lbl
            // 
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(0, 28);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ElasticSearchManager.Properties.Resources.if_58_Cross_Circle_Remove_Delete_1864217;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 28);
            this.btnDelete.Text = "Delete...";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnToolbarSearch
            // 
            this.btnToolbarSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolbarSearch.Image = global::ElasticSearchManager.Properties.Resources.if_icon_111_search_314689;
            this.btnToolbarSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolbarSearch.Name = "btnToolbarSearch";
            this.btnToolbarSearch.Size = new System.Drawing.Size(28, 28);
            this.btnToolbarSearch.ToolTipText = "Search Layouts";
            this.btnToolbarSearch.Click += new System.EventHandler(this.btnToolbarSearch_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::ElasticSearchManager.Properties.Resources.Refresh1;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(74, 28);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 608);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.Text = "Elastic Search Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.appStatusBar.ResumeLayout(false);
            this.appStatusBar.PerformLayout();
            this.appSplitContainer.Panel1.ResumeLayout(false);
            this.appSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.appSplitContainer)).EndInit();
            this.appSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEntities)).EndInit();
            this.appToolbar.ResumeLayout(false);
            this.appToolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip appStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblAppStatus;
        private System.Windows.Forms.ToolStrip appToolbar;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cboConnections;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.SplitContainer appSplitContainer;
        private System.Windows.Forms.TreeView treeEntities;
        private System.Windows.Forms.ToolStripTextBox txtToolbarSearch;
        private System.Windows.Forms.ToolStripButton btnToolbarSearch;
        private System.Windows.Forms.ToolStripLabel lbl;
        private System.Windows.Forms.DataGridView grdEntities;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnRefresh;
    }
}

