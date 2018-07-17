using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ElasticSearchManager.Common;
using ElasticSearchManager.DataAccess;
using Nest;
using ScintillaNET;
using BindingSource = System.Windows.Forms.BindingSource;

namespace ElasticSearchManager {
    public partial class frmMain : Form {

        #region Variables
        private enum EntityType {
            Index,
            Alias,
            Unknown
        }
        #endregion

        #region Constructors

        public frmMain() {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void frmMain_Load(object sender, EventArgs e) {
            PopulateConnections();
            InitializeUserSettings();
            InitilizeMiscUI();            
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.LastConnection = cboConnections.Text;
            Properties.Settings.Default.SplitterDistance = appSplitContainer.SplitterDistance;
            Properties.Settings.Default.Save();
        }

        private void txtToolbarSearch_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                PopulateGrid();
            }
        }

        #endregion

        #region Private Methods

        private void InitilizeMiscUI() {
            NativeMethods.SetToolstripTextBoxPlaceHolder(txtToolbarSearch, "Filter Entities");            
        }

        private void InitializeUserSettings() {
            if (Properties.Settings.Default.LastConnection.Length > 0) {
                cboConnections.Text = Properties.Settings.Default.LastConnection;
            }

            appSplitContainer.SplitterDistance = Properties.Settings.Default.SplitterDistance;
        }

        private void PopulateConnections()
        {
            cboConnections.ComboBox.ValueMember = "ConnectionString";
            cboConnections.ComboBox.DisplayMember = "Name";

            foreach (ConnectionStringSettings entry in ConfigurationManager.ConnectionStrings) {
                if (entry.Name == "LocalSqlServer") continue;                
                cboConnections.Items.Add(entry);
            }
        }

        #endregion

        private void cboConnections_SelectedIndexChanged(object sender, EventArgs e) {

            using (var access = GetElasticAccess()) {
                ClearTree();

                ICatResponse<CatIndicesRecord> indexList = access.IndexList();
                RenderIndexes(indexList);

                ICatResponse<CatAliasesRecord> aliasList = access.AliasList();
                RenderAliases(aliasList);
            }
        }

        private ElasticAccess GetElasticAccess() {
            var connString = (ConnectionStringSettings) cboConnections.SelectedItem;
            return new ElasticAccess(connString.ConnectionString);
        }        

        private void ClearTree() {
            treeEntities.Nodes.Clear();
        }

        private void RenderIndexes(ICatResponse<CatIndicesRecord> indexList) {
            var parentNode = treeEntities.Nodes.Add("Indexes");

            var records = indexList.Records.OrderBy(r => r.Index).ToList();
            parentNode.Tag = records;

            foreach (var record in records) {
                var node = parentNode.Nodes.Add(record.Index);
                node.Text = $"{record.Index} ({record.DocsCount})";

                if (record.Health != "green") {
                    node.ForeColor = Color.FromName(record.Health);
                }
            }
        }

        private void RenderAliases(ICatResponse<CatAliasesRecord> aliasList) {
            var parentNode = treeEntities.Nodes.Add("Aliases");
            
            var aliases = aliasList.Records.OrderBy(r => r.Alias).ToList();
            parentNode.Tag = aliases;

            foreach (var alias in aliases) {
                var node = parentNode.Nodes.Add(alias.Alias);
                node.Text = $"{alias.Alias} ({alias.Index})";
            }
        }

        private void treeIndexes_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeNode node = treeEntities.SelectedNode;

            grdEntities.AutoGenerateColumns = true;
            grdEntities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            PopulateGrid();
            
        }

        private void PopulateGrid() {
            switch (GetEntityType()) {
                case EntityType.Index:
                    PopulateIndexGrid();
                    break;
                case EntityType.Alias:
                    PopulateAliasGrid();
                    break;
                case EntityType.Unknown:
                    MessageBox.Show("Select either Indexes or Aliases on the left");
                    break;
            }
        }

        private void PopulateIndexGrid() {
            TreeNode node = treeEntities.SelectedNode;
            var indexes = (List<CatIndicesRecord>) node.Tag;

            string filter = txtToolbarSearch.Text;
            List<CatIndicesRecord> filtered = string.IsNullOrWhiteSpace(filter) ? indexes : indexes.Where(idx => idx.Index.Contains(filter)).ToList();
                
            var source = new SortableBindingList<CatIndicesRecord>(filtered);
            grdEntities.DataSource = source;
        }

        private void PopulateAliasGrid() {
            TreeNode node = treeEntities.SelectedNode;
            var aliases = (List<CatAliasesRecord>) node.Tag;

            string filter = txtToolbarSearch.Text;
            List<CatAliasesRecord> filtered = string.IsNullOrWhiteSpace(filter) ? aliases : aliases.Where(idx => idx.Alias.Contains(filter)).ToList();
                
            var source = new SortableBindingList<CatAliasesRecord>(filtered);
            grdEntities.DataSource = source;
        }


        private void btnDelete_Click(object sender, EventArgs e) {
            ElasticAccess access = GetElasticAccess();
            EntityType selectedType = GetEntityType();

            if (selectedType == EntityType.Unknown) {
                MessageBox.Show("Select either Aliases and Indexes in the tree");
                return;
            }

            var dialogResult = MessageBox.Show($"Deleting {grdEntities.SelectedRows.Count} {selectedType.ToString()}!!! Are you sure?  There is no going back!", "Destructive Action Ahead",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes) {

                foreach (DataGridViewRow row in grdEntities.SelectedRows) {

                    switch (selectedType) {
                        case EntityType.Index:
                        var entityIndex = (CatIndicesRecord)row.DataBoundItem;
                        access.DeleteIndex(entityIndex);
                        break;
                        case EntityType.Alias:
                        var entityAlias = (CatAliasesRecord)row.DataBoundItem;
                        access.DeleteAlias(entityAlias);
                        break;
                    }
                }

                // recreate the grid
                treeIndexes_AfterSelect(sender, null);
            }
        }

        private EntityType GetEntityType() {
            EntityType entityType = EntityType.Unknown;

            if (treeEntities.SelectedNode.Text == "Indexes") {
                entityType = EntityType.Index;
            } else if (treeEntities.SelectedNode.Text == "Aliases") {
                entityType = EntityType.Alias;
            }

            return entityType;
        }

        private void btnToolbarSearch_Click(object sender, EventArgs e) {
            PopulateGrid();
        }
    }
}
