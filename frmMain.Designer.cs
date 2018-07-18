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
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtToolbarSearch = new System.Windows.Forms.ToolStripTextBox();
            this.btnToolbarSearch = new System.Windows.Forms.ToolStripButton();
            this.lbl = new System.Windows.Forms.ToolStripLabel();
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1820, 872);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1820, 935);
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
            this.appStatusBar.Size = new System.Drawing.Size(1820, 30);
            this.appStatusBar.TabIndex = 0;
            // 
            // lblAppStatus
            // 
            this.lblAppStatus.Name = "lblAppStatus";
            this.lblAppStatus.Size = new System.Drawing.Size(41, 25);
            this.lblAppStatus.Text = "Idle";
            // 
            // appSplitContainer
            // 
            this.appSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.appSplitContainer.Name = "appSplitContainer";
            // 
            // appSplitContainer.Panel1
            // 
            this.appSplitContainer.Panel1.Controls.Add(this.treeEntities);
            // 
            // appSplitContainer.Panel2
            // 
            this.appSplitContainer.Panel2.Controls.Add(this.grdEntities);
            this.appSplitContainer.Size = new System.Drawing.Size(1820, 872);
            this.appSplitContainer.SplitterDistance = 496;
            this.appSplitContainer.TabIndex = 0;
            // 
            // treeEntities
            // 
            this.treeEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeEntities.HideSelection = false;
            this.treeEntities.Location = new System.Drawing.Point(0, 0);
            this.treeEntities.Name = "treeEntities";
            this.treeEntities.Size = new System.Drawing.Size(496, 872);
            this.treeEntities.TabIndex = 0;
            this.treeEntities.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeIndexes_AfterSelect);
            // 
            // grdEntities
            // 
            this.grdEntities.AllowUserToAddRows = false;
            this.grdEntities.AllowUserToOrderColumns = true;
            this.grdEntities.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEntities.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdEntities.Location = new System.Drawing.Point(0, 0);
            this.grdEntities.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grdEntities.Name = "grdEntities";
            this.grdEntities.ReadOnly = true;
            this.grdEntities.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdEntities.Size = new System.Drawing.Size(1320, 872);
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
            this.lbl});
            this.appToolbar.Location = new System.Drawing.Point(3, 0);
            this.appToolbar.Name = "appToolbar";
            this.appToolbar.Size = new System.Drawing.Size(615, 33);
            this.appToolbar.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(110, 30);
            this.toolStripLabel1.Text = "Connections";
            // 
            // cboConnections
            // 
            this.cboConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConnections.Name = "cboConnections";
            this.cboConnections.Size = new System.Drawing.Size(121, 33);
            this.cboConnections.SelectedIndexChanged += new System.EventHandler(this.cboConnections_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::ElasticSearchManager.Properties.Resources.if_58_Cross_Circle_Remove_Delete_1864217;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(102, 30);
            this.btnDelete.Text = "Delete...";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(50, 30);
            this.toolStripLabel2.Text = "Filter";
            // 
            // txtToolbarSearch
            // 
            this.txtToolbarSearch.Name = "txtToolbarSearch";
            this.txtToolbarSearch.Size = new System.Drawing.Size(130, 33);
            this.txtToolbarSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToolbarSearch_KeyDown);
            // 
            // btnToolbarSearch
            // 
            this.btnToolbarSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolbarSearch.Image = global::ElasticSearchManager.Properties.Resources.if_icon_111_search_314689;
            this.btnToolbarSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolbarSearch.Name = "btnToolbarSearch";
            this.btnToolbarSearch.Size = new System.Drawing.Size(28, 30);
            this.btnToolbarSearch.ToolTipText = "Search Layouts";
            this.btnToolbarSearch.Click += new System.EventHandler(this.btnToolbarSearch_Click);
            // 
            // lbl
            // 
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(0, 30);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1820, 935);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}

