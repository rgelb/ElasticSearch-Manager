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
            Unknown,
            NotSelected
        }

        private enum ContentType {
            Grid,
            TextEditor,
            SplitTextEditor
        }

        private TextEditor textEditor;
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
            InitializeTextEditor();
            InitilizeMiscUI();    
            DisplayContentControl(ContentType.Grid);
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


        private void InitializeTextEditor() {
            var txtContent = new Scintilla();
            appSplitContainer.Panel2.Controls.Add(txtContent);
            txtContent.Dock = DockStyle.None;

            textEditor = new TextEditor(txtContent);
        }

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
            PopulateConnectionEntities();
        }

        private void PopulateConnectionEntities() {
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
                node.Tag = record;

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
                node.Tag = alias;
            }
        }

        private void treeIndexes_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeNode node = treeEntities.SelectedNode;

            try {
                Cursor.Current = Cursors.WaitCursor;

                if (node.Level == 0) {
                    // list of indexes or aliases
                    PopulateGrid();
                    DisplayContentControl(ContentType.Grid);
                }
                else if (node.Level == 1) {
                    // index/alias info
                    PopulateEntityInformation();
                    DisplayContentControl(ContentType.TextEditor);
                }
            }
            finally {
                Cursor.Current = Cursors.Default;
            }
        }

        private void PopulateEntityInformation() {
            switch (GetEntityType()) {
                case EntityType.Index:
                    var indexRecord = (CatIndicesRecord) treeEntities.SelectedNode.Tag;

                    using (var access = GetElasticAccess()) {
                        var indexDescription = access.IndexDescription(indexRecord.Index);
                        PopulateIndexDescription(indexDescription);
                    }

                    break;
                case EntityType.Alias:
                    var aliasRecord = (CatAliasesRecord) treeEntities.SelectedNode.Tag;
                    break;
                case EntityType.Unknown:
                    MessageBox.Show("Select either Indexes or Aliases on the left");
                    break;
                case EntityType.NotSelected:
                    return;
            }            
        }

        private void PopulateIndexDescription(ElasticAccess.IndexDefinition indexDef) {
            string description = string.Empty;


            //if (indexDef.Index.Indices.Count == 0) {
            //    textEditor.Editor.Text = "Index not found";
            //    return;
            //}

            //var index = indexDef.Indices.FirstOrDefault();
            //var indexState = index.Value;


            // get names and aliases
            if (indexDef.Index != null) {
                var index = indexDef.Index.Indices.FirstOrDefault();
                var indexState = index.Value;

                description = index.Key + Environment.NewLine + Environment.NewLine;

                if (indexState.Aliases.Count > 0)
                    description += "Aliased by: " + indexState.Aliases.First().Key.Name + Environment.NewLine + Environment.NewLine;

            }

            // settings
            if (indexDef.Settings != null) {
                description += "Replicas: " + indexDef.Settings.NumberOfReplicas + Environment.NewLine;
                description += "Shards: " + indexDef.Settings.NumberOfShards + Environment.NewLine;
            }

            // fields
            string fields = string.Empty;
            if (indexDef.Mappings != null) {

                foreach (var field in indexDef.Mappings.Values.First().Properties) {
                    fields += $"{field.Value.Name.Name}, {field.Value.Type}{Environment.NewLine}";
                    Debug.WriteLine(field.Value.Name);
                }

                description += Environment.NewLine + "Fields" + Environment.NewLine;
                description += fields + Environment.NewLine;
            }

            textEditor.Editor.Text = description;
        }

        private void PopulateGrid() {

            grdEntities.AutoGenerateColumns = true;
            grdEntities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
                case EntityType.NotSelected:
                    return;
            }
        }

        private void DisplayContentControl(ContentType contentType) {

            // hide other things
            textEditor.Editor.Dock = DockStyle.None;
            textEditor.Editor.Visible = false;
            
            grdEntities.Dock = DockStyle.None;
            grdEntities.Visible = false;

            switch (contentType) {
                case ContentType.Grid:
                    grdEntities.Dock = DockStyle.Fill;
                    grdEntities.Visible = true;
                    break;
                case ContentType.TextEditor:
                    textEditor.Editor.Dock = DockStyle.Fill;
                    textEditor.Editor.Visible = true;
                    break;
                case ContentType.SplitTextEditor:
                    break;
                default:
                    MessageBox.Show($"Invalid content type: {contentType}");
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

            DialogResult dialogResult = AskPermissionToDelete(selectedType);

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

        private DialogResult AskPermissionToDelete(EntityType selectedType) {
            // get a preview of what's about to be deleted
            var itemsToBeDeleted = new List<string>();
            bool aliasPointsToIndexWarn = false;
            foreach (DataGridViewRow row in grdEntities.SelectedRows) {
                switch (selectedType) {
                    case EntityType.Index:
                        var entityIndex = (CatIndicesRecord)row.DataBoundItem;
                        itemsToBeDeleted.Add(entityIndex.Index);
                        break;
                    case EntityType.Alias:
                        var entityAlias = (CatAliasesRecord)row.DataBoundItem;
                        itemsToBeDeleted.Add(entityAlias.Alias);
                        break;
                }
            }

            var indexesPointedToByAlias = new List<string>();
            if (selectedType == EntityType.Index) {
                // check to see if any alias points to it.
                TreeNode foundNode = treeEntities.Nodes.FindByFullPath("Aliases");
                foreach (TreeNode aliasNode in foundNode.Nodes) {
                    var alias = (CatAliasesRecord)aliasNode.Tag;

                    if (itemsToBeDeleted.Contains(alias.Index)) {
                        indexesPointedToByAlias.Add(alias.Index);
                        aliasPointsToIndexWarn = true;
                    }
                }
            }

            string msg = $"{grdEntities.SelectedRows.Count} {selectedType.ToString()} are to be deleted!!! Are you sure?  There is no going back!";

            foreach(var item in itemsToBeDeleted) {
                msg += Environment.NewLine + "\t" + item;
            }

            if (aliasPointsToIndexWarn) {
                msg += Environment.NewLine + Environment.NewLine;
                if (indexesPointedToByAlias.Count == 1) {
                    msg += $"In addition, index {indexesPointedToByAlias[0]} is pointed to by an alias";
                } else {
                    msg += $"In addition, indexes {string.Join(", ", indexesPointedToByAlias)} are pointed to by an aliases";
                }
            }

            var dialogResult = MessageBox.Show(msg, "Destructive Action Ahead", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return dialogResult;
        }

        private EntityType GetEntityType() {
            // returns Index or Alias regardless of whether it's 1st level entry or 2nd level entry
            EntityType entityType = EntityType.Unknown;

            if (treeEntities.SelectedNode == null) {
                return EntityType.NotSelected;
            }

            if (treeEntities.SelectedNode.Level == 0) {
                if (treeEntities.SelectedNode.Text == "Indexes") {
                    entityType = EntityType.Index;
                } else if (treeEntities.SelectedNode.Text == "Aliases") {
                    entityType = EntityType.Alias;
                }
            } else if (treeEntities.SelectedNode.Level == 1) {
                object o = treeEntities.SelectedNode.Tag;
                if (o is CatIndicesRecord) {
                    return EntityType.Index;
                } else if (o is CatAliasesRecord) {
                    return EntityType.Alias;
                }
            }



            return entityType;
        }



        private void btnToolbarSearch_Click(object sender, EventArgs e) {
            PopulateGrid();
        }

        private void btnRefresh_Click(object sender, EventArgs e) {
            RefreshSidebar();
        }

        private void RefreshSidebar() {
            try {
                Cursor.Current = Cursors.WaitCursor;

                if (cboConnections.SelectedIndex > -1 && treeEntities.SelectedNode != null) {

                    // save the current node of the tree
                    var nodePath = treeEntities.SelectedNode.FullPath;

                    PopulateConnectionEntities();

                    // set the tree to the same node
                    TreeNode foundNode = treeEntities.Nodes.FindByFullPath(nodePath);

                    if (foundNode != null) {
                        treeEntities.SelectedNode = foundNode;
                    }
                }
            }
            finally {
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
            }

        }
    }
}
