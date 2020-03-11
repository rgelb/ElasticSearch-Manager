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
using AutoMapper;
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
            Task,
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
            SetupInfractructure();
            PopulateConnections();
            InitializeTextEditor();
            InitializeUserSettings();            
            InitilizeMiscUI();    
            DisplayContentControl(ContentType.Grid);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.LastConnection = cboConnections.Text;
            Properties.Settings.Default.SplitterDistance = appSplitContainer.SplitterDistance;
            Properties.Settings.Default.WindowState = this.WindowState;
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

            this.WindowState = Properties.Settings.Default.WindowState;
            appSplitContainer.SplitterDistance = Properties.Settings.Default.SplitterDistance;
        }

        private void SetupInfractructure() {

            Mapper.Initialize(cfg => {
                _ = cfg.CreateMap<CatAliasesRecord, ElasticAlias>();
                _ = cfg.CreateMap<CatIndicesRecord, ElasticIndex>();
                _ = cfg.CreateMap<CatIndicesRecordExtended, ElasticIndex>();
                _ = cfg.CreateMap<TaskState, ElasticTask>();
            });

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

                List<ElasticIndex> indexList = ElasticIndex.ToList(access.IndexListExtended());
                //List<ElasticIndex> indexList = ElasticIndex.ToList(access.IndexList());
                RenderIndexes(indexList);

                List<ElasticAlias> aliasList = ElasticAlias.ToList(access.AliasList());
                RenderAliases(aliasList);

                // apply aliases to indexes
                ApplyAliasesToIndexes(indexList, aliasList);

                // add a Tasks node
                var parentNode = treeEntities.Nodes.Add("Tasks");

            }
        }

        private void ApplyAliasesToIndexes(List<ElasticIndex> indexList, List<ElasticAlias> aliasList) {
            foreach (var alias in aliasList) {
                // look for the index
                var index = indexList.SingleOrDefault(i => i.Index == alias.Index);
                if (index != null) {
                    index.Alias = alias.Alias;
                }
            }
        }

        private ElasticAccess GetElasticAccess() {
            var connString = (ConnectionStringSettings) cboConnections.SelectedItem;
            return new ElasticAccess(connString.ConnectionString);
        }        

        private void ClearTree() {
            treeEntities.Nodes.Clear();
        }

        private void RenderIndexes(List<ElasticIndex> indexList) {
            var parentNode = treeEntities.Nodes.Add("Indexes");

            var records = indexList.OrderBy(r => r.Index).ToList();
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

        private void RenderAliases(List<ElasticAlias> aliasList) {
            var parentNode = treeEntities.Nodes.Add("Aliases");
            
            var aliases = aliasList.OrderBy(r => r.Alias).ToList();
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
                    // list of indexes or aliases or tasks
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
                        var indexStats = access.IndexStats(indexRecord.Index);
                        var indexDescription = access.IndexDescription(indexRecord.Index);
                        PopulateIndexDescription(indexDescription, indexStats);
                    }

                    break;
                case EntityType.Alias:
                    var aliasRecord = (ElasticAlias) treeEntities.SelectedNode.Tag;
                    break;
                case EntityType.Unknown:
                    MessageBox.Show("Select either Indexes or Aliases on the left");
                    break;
                case EntityType.NotSelected:
                    return;
            }            
        }

        private void PopulateIndexDescription(ElasticAccess.IndexDefinition indexDef, IIndicesStatsResponse indexStats) {
            string description = string.Empty;

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

                const string CREATED_ON = "index.creation_date";
                if (indexDef.Settings.ContainsKey(CREATED_ON)) {
                    double milliseconds;
                    if (double.TryParse(indexDef.Settings[CREATED_ON].ToString(), out milliseconds)) {
                        description += "Created On: " + Utilities.UnixTimeStampToDateTime(milliseconds).ToString() + Environment.NewLine;
                    }
                }
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

            // stats
            description += Environment.NewLine + "Statistics" + Environment.NewLine;
            if (indexStats?.Stats?.Total?.Store != null) {                
                description += "Size: " + Utilities.SizeSuffix( (long) indexStats.Stats.Total.Store.SizeInBytes) + Environment.NewLine;
            }


            if (indexStats?.Stats?.Total?.Documents != null) {
                description += "Documents: " + $"{indexStats.Stats.Total.Documents.Count:N0}" + Environment.NewLine;
                description += "Deleted: " + $"{indexStats.Stats.Total.Documents.Deleted:N0}" + Environment.NewLine;
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
                case EntityType.Task:
                    PopulateTaskGrid();
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
            var indexes = (List<ElasticIndex>) node.Tag;

            string filter = txtToolbarSearch.Text;
            List<ElasticIndex> filtered = string.IsNullOrWhiteSpace(filter) ? indexes : indexes.Where(idx => idx.Index.Contains(filter)).ToList();
                
            var source = new SortableBindingList<ElasticIndex>(filtered);
            grdEntities.DataSource = source;

            // set order of columns
            grdEntities.Columns["Alias"].DisplayIndex = 0;
            grdEntities.Columns["Index"].DisplayIndex = 1;
            grdEntities.Columns["DocsCount"].DisplayIndex = 2;
            grdEntities.Columns["DocsDeleted"].DisplayIndex = 3;
            grdEntities.Columns["Primary"].DisplayIndex = 4;
            grdEntities.Columns["Replica"].DisplayIndex = 5;
            grdEntities.Columns["SegmentCount"].DisplayIndex = 6;
            grdEntities.Columns["PrimaryStoreSize"].DisplayIndex = 7;
            grdEntities.Columns["StoreSize"].DisplayIndex = 8;
            grdEntities.Columns["Health"].DisplayIndex = 9;
            grdEntities.Columns["Status"].DisplayIndex = 10;
            grdEntities.Columns["TotalMemory"].DisplayIndex = 11;
        }

        private void PopulateAliasGrid() {
            TreeNode node = treeEntities.SelectedNode;
            var aliases = (List<ElasticAlias>) node.Tag;

            string filter = txtToolbarSearch.Text;
            List<ElasticAlias> filtered = string.IsNullOrWhiteSpace(filter) ? aliases : aliases.Where(idx => idx.Alias.Contains(filter)).ToList();
                
            var source = new SortableBindingList<ElasticAlias>(filtered);
            grdEntities.DataSource = source;
        }

        private void PopulateTaskGrid()
        {
            using (var access = GetElasticAccess())
            {
                var tasks = ElasticTask.ToList(access.TaskList());

                string filter = txtToolbarSearch.Text;
                List<ElasticTask> filtered = string.IsNullOrWhiteSpace(filter) ? tasks : tasks.Where(idx => idx.Action.Contains(filter)).ToList();

                var source = new SortableBindingList<ElasticTask>(filtered);
                grdEntities.DataSource = source;

                grdEntities.Columns["RunningTimeInNanoSeconds"].Visible = false;
                grdEntities.Columns["StartTimeInMilliseconds"].Visible = false;
                grdEntities.Columns["Description"].Visible = false;
                grdEntities.Columns["Headers"].Visible = false;
                grdEntities.Columns["Status"].Visible = false;
                grdEntities.Columns["Node"].Visible = false;
                //grdEntities.Columns["RunningTimeInNanoSeconds"].Visible = false;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e) {
            ElasticAccess access = GetElasticAccess();
            EntityType selectedType = GetEntityType();

            if (selectedType == EntityType.Unknown) {
                MessageBox.Show("Select either Aliases and Indexes in the tree");
                return;
            }

            if (selectedType == EntityType.Task) {
                MessageBox.Show("Deleting a task is not working right now...  check back later");
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
                            var entityAlias = (ElasticAlias)row.DataBoundItem;
                            access.DeleteAlias(entityAlias);
                            break;
                    }
                }

                // recreate the grid
                RefreshSidebar();
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
                        var entityAlias = (ElasticAlias)row.DataBoundItem;
                        itemsToBeDeleted.Add(entityAlias.Alias);
                        break;
                }
            }

            var indexesPointedToByAlias = new List<string>();
            if (selectedType == EntityType.Index) {
                // check to see if any alias points to it.
                TreeNode foundNode = treeEntities.Nodes.FindByFullPath("Aliases");
                foreach (TreeNode aliasNode in foundNode.Nodes) {
                    var alias = (ElasticAlias)aliasNode.Tag;

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
                } else if (treeEntities.SelectedNode.Text == "Tasks") { 
                    entityType = EntityType.Task;
                }
            } else if (treeEntities.SelectedNode.Level == 1) {
                object o = treeEntities.SelectedNode.Tag;
                if (o is CatIndicesRecord) {
                    return EntityType.Index;
                } else if (o is ElasticAlias) {
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

        private void grdEntities_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            // add a tooltip for indexes that include a Ticks based creation timestamp
            // example: school_637034229424498988
            if (grdEntities.Columns.Contains("Index") && e.ColumnIndex == grdEntities.Columns["Index"].Index) {
                DataGridViewCell cell = grdEntities.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string text = (string) cell.Value;

                int pos = text.LastIndexOf('_');
                if (pos > 0) {
                    string ticks = text.Substring(pos + 1);
                    if (ticks.Length == 18 && long.TryParse(ticks, out long ticksValue)) {
                        cell.ToolTipText = new DateTime(ticksValue).ToString();
                    }
                }

            }

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
