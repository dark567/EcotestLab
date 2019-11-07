using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace DG
{
    public partial class DicGoods : Form
    {
        public int i = 0;

        public delegate void MyLabelClickedHandler(forEditChecksModel testModel);
        public event MyLabelClickedHandler MyLabelClicked;

        static class Login
        {
            public static string Value { get; set; }
        }

        static class Login_e
        {
            public static string Value { get; set; }
        }

        static class db_puth
        {
            public static string Value { get; set; }
        }

        static class Key
        {
            public static string Value { get; set; }
        }

        public string path_db;
        public string user;
        public string pass;
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";

        public DicGoods()
        {
            InitializeComponent();

            Load += new EventHandler(Form1_Load);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            #region read ini
            try
            {
                if (File.Exists(fileIniPath))
                {
                    //Создание объекта, для работы с файлом
                    INIManager manager = new INIManager(fileIniPath);
                    //Получить значение по ключу name из секции main
                    path_db = manager.GetPrivateString("connection", "db");
                    db_puth.Value = path_db;

                 //   path_db = manager.GetPrivateString("workstation", "Key");
                   // Key.Value = path_db;
                    //MessageBox.Show("бд - " + db_puth.Value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    File.AppendAllText(Application.StartupPath + @"\new_file.txt", "путь к db:" + path_db + "\n");
                    //Записать значение по ключу age в секции main
                    // manager.WritePrivateString("main", "age", "21");
                }
                else MessageBox.Show("File set.ini not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("File set.ini don't read" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            TreeNode rootNode = new TreeNode() { Name = "0", Text = "Номенклатура", ImageIndex = 0 };
            treeView1.Nodes.Add(rootNode);

            TreeNode autoNode00 = treeView1.Nodes["0"];
            autoNode00.ImageIndex = 0;

            //создаём и добавляем дочерний узел
            autoNode00.Nodes.Add("29513", getNomNameFromID("29513"));

            TreeNode autoNode01 = rootNode.Nodes["29513"];
            autoNode01.ImageIndex = 0;

            TreeNode autoNode02;

            TreeNode autoNode03;


            //autoNode01.Nodes.Add("29530", getNomNameFromID("29530"));
            foreach (string s in getNomID_dic_goods_grp("29513"))
            {
                autoNode01.Nodes.Add(s, getNomNameFromID(s));
                autoNode02 = autoNode01.Nodes[s];
                autoNode02.ImageIndex = 0;
                foreach (string s02 in getNomID_dic_goods_grp(s))
                {
                    autoNode02.Nodes.Add(s02, getNomNameFromID(s02));
                    autoNode03 = autoNode02.Nodes[s02];
                    autoNode03.ImageIndex = 0;
                    foreach (string s03 in getNomID_dic_goods_grp(s02))
                    {
                        autoNode03.Nodes.Add(s03, getNomNameFromID(s03));
                    }
                }
                // autoNode01.Nodes.Add("29513", getNomNameFromID("29513"));
                // testDirectoryModel.AddDicGoods(new testDirectoryModel(s, "0", getNomNameFromID(s), i));
                //testDirectoryModel.AddDicGoods(new testDirectoryModel("1", "1", "test", i));
            }

            // RegistryKey key = Registry.CurrentUser;

            ////заполняем первый раз коллекцию
            //foreach (string s in getNomID("0"))
            //{
            //    testDirectoryModel.AddDicGoods(new testDirectoryModel(s, "0", getNomNameFromID(s), i));
            //    //testDirectoryModel.AddDicGoods(new testDirectoryModel("1", "1", "test", i));
            //}


            //foreach (testDirectoryModel s in testDirectoryModel.GetDicGoods.Where(n => n.NumDepth == i).Where(n => n.ParentID == "0"/* || n.ParentID == "1"*/))
            //{
            //    //TreeNode drive = new TreeNode($"({s.ParentID}):" + s.FullName + $"({s.ID}):{s.NumDepth}") { Name = s.ParentID };
            //    //treeView1.Nodes.Add(drive);

            //    TreeNode tn = new TreeNode($"({s.ParentID}:{s.ParentID}):" + s.FullName + $"({s.ID}):{s.NumDepth}") { Name = s.ParentID };

            //    BuildChildNodes(tn);
            //    treeView1.Nodes.Add(tn);
            //    //GetGoods(drive, s);
            //}

            

        }
        private FbConnection GetConnection()
        {
            string connectionString =
                "User=SYSDBA;" +
                "Password=masterkey;" +
                @"Database=" + path_db + ";" +
                "Charset=UTF8;" +
                "Pooling=true;" +
                "ServerType=0;";

            FbConnection conn = new FbConnection(connectionString.ToString());

            conn.Open();

            return conn;
        }

        public ArrayList getNomID_dic_goods_grp(string ID)
        {
            ArrayList Nom = new ArrayList(); ;

            FbConnection fb = GetConnection();

            FbCommand SelectSQL = new FbCommand("SELECT id FROM dic_goods_grp where PARENT_ID = @cust_no and id <> '0' ORDER BY name", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@cust_no", ID);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Nom.Add(reader.GetString(0));
                }
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
                fb.Close();
            }

            return Nom;
        }

        public string getNomNameFromID(string ID)
        {
            string Nom = "n/a";

            FbConnection fb = GetConnection();

            try
            {
                FbCommand SelectSQL = new FbCommand("SELECT Name FROM dic_goods_grp where ID = @cust_no ORDER BY name", fb);
                //add one IN parameter                     
                FbParameter nameParam = new FbParameter("@cust_no", ID);
                // добавляем параметр к команде
                SelectSQL.Parameters.Add(nameParam);


                FbTransaction fbt = fb.BeginTransaction();
                SelectSQL.Transaction = fbt;
                FbDataReader reader = SelectSQL.ExecuteReader();
                // SelectSQL.Parameters["cust_no"].Value = reader["0"];

                try
                {
                    while (reader.Read()) { Nom = reader.GetString(0); }
                }
                finally
                {
                    fbt.Commit();
                    reader.Close();
                    SelectSQL.Dispose();
                    fb.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }

            return Nom;
        }


        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Get the property object based on the DataPropertyName of the column
            var property = typeof(SampleRow).GetProperty(e.Column.DataPropertyName);
            // Get the ColumnWeight attribute from the property if it exists
            var weightAttribute = (ColumnWeight)property?.GetCustomAttribute(typeof(ColumnWeight));
            if (weightAttribute != null)
            {
                // Finally, set the FillWeight of the column to our defined weight in the attribute
                e.Column.FillWeight = weightAttribute.Weight;
            }

            var weightAttributeDisp = (DisplayNameAttribute)property?.GetCustomAttribute(typeof(DisplayNameAttribute));
            if (weightAttributeDisp != null)
            {
                // Finally, set the FillWeight of the column to our defined weight in the attribute
                e.Column.HeaderText = weightAttributeDisp.DisplayName;
            }

            var autoSize = (AutoSizeMode)property?.GetCustomAttribute(typeof(AutoSizeMode));
            if (autoSize != null)
            {
                // Finally, set the FillWeight of the column to our defined weight in the attribute
                e.Column.AutoSizeMode = (DataGridViewAutoSizeColumnMode)autoSize.SizeMode;
            }

            var visibleTypes = (VisibleTypes)property?.GetCustomAttribute(typeof(VisibleTypes));
            if (visibleTypes != null)
            {
                // Finally, set the FillWeight of the column to our defined weight in the attribute
                e.Column.Visible = visibleTypes.typesVisible;
            }

            //var TypeIsService = (TypesIServiceAttribute)property?.GetCustomAttribute(typeof(TypesIServiceAttribute));
            //if (TypeIsService != null)
            //{
            //    // Finally, set the FillWeight of the column to our defined weight in the attribute
            //    if (TypeIsService.typesIServiceAttribute)
            //    {
            //        AddOutOfOfficeColumn();
            //    }
            //}
            // base.DataGridView1_ColumnAdded(sender, e);

        }


        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;
        }
        private void TreeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            foreach (var childNode in e.Node.Nodes.Cast<TreeNode>())
            {
                if (childNode.Nodes.Count == 0)
                {
                    BuildChildNodes(childNode);
                }
            }
        }

        private void BuildChildNodes(TreeNode tn)
        {

            string keyTest = (string)tn.Name;
            // MessageBox.Show(keyTest.ToString());


            /* foreach (testDirectoryModel item in testDirectoryModel.GetDicGoods.Where(n => n.NumDepth == i).Where(n => n.ParentID == keyTest || n.ParentID == ""))*/
            //заполнение массива
            foreach (testDirectoryModel item in testDirectoryModel.GetDicGoods.Where(n => n.NumDepth == i && n.ParentID == keyTest))
            {
                foreach (string s in getNomID_dic_goods_grp(item.ID))
                {
                    testDirectoryModel.AddDicGoods(new testDirectoryModel(s, item.ID, getNomNameFromID(s), i + 1));
                }
            }

            i++;


            foreach (testDirectoryModel s in testDirectoryModel.GetDicGoods.Where(n => (n.NumDepth == i)))
            {
                TreeNode child = new TreeNode($"({keyTest}:{s.ParentID}:{s.ID}):" + s.FullName + $":{s.NumDepth}") { Name = s.ParentID };
                tn.Nodes.Add(child);
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            dataGridView1.Rows.Clear();
            // dataGridView1.Columns.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            TreeNode selectedNode = e.Node;
            DicGoodsModel.ClearDicGoodsModel();
            getNomDicGoodsID_(selectedNode.Name);
            //MessageBox.Show(selectedNode.Name);

            SortableBindingList<SampleRow> data = new SortableBindingList<SampleRow>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DicGoodsModel s in DicGoodsModel.GetDicGoodsModel)
            {
                data.Add(new SampleRow(id: s.ID, grpId: s.GrpId, grpName: s.GrpName, code: s.Code, name: s.Name, _isService: s.IsService, price: s.Price.ToString("F2"), _isSale: s.IsSale));
            }

            dataGridView1.DataSource = data;

            //нумерация
            int index = 0;
            object header;
            string indexStr = (index + 1).ToString();
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                header = this.dataGridView1.Rows[index].HeaderCell.Value;
                if (header == null || !header.Equals(indexStr))
                    this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;
                indexStr = (index++).ToString();
            }

        }
        public void getNomDicGoodsID_(string ID)
        {

            FbConnection fb = GetConnection();

            FbCommand SelectSQL = new FbCommand(" SELECT dg.id, dg.grp_id, dgg.name, dg.code, dg.name, dg.IS_SERVICE, dg.PRICE_OUT, dg.IS_ACTIVE " +
                "FROM dic_goods dg " +
                "left join dic_goods_grp dgg on dgg.id = dg.grp_id where dg.GRP_ID = @cust_no ORDER BY dg.name", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@cust_no", ID);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    DicGoodsModel.AddDicGoodsModel(new DicGoodsModel(id: reader?.GetString(0), grpId: reader?.GetString(1), grpName: reader?.GetString(2), code: reader?.GetString(3), name: reader?.GetString(4), isService: reader?.GetString(5) == "1" ? true : false, price: reader.GetDouble(6), isSale: reader?.GetString(7) == "1" ? true : false));
                }
            }
            catch (Exception) //selection=="+"? (x+y) : (x-y);
            {
                //dataGridView1.Rows.Clear();
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
                fb.Close();
            }
            // return dgm;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                string name = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                // MessageBox.Show($"{y}");

                EditDicGoods myform = new EditDicGoods();
                myform.X = id;
                myform.ShowDialog();
            }
        }

        private void DataGridView1_KeyUp(object sender, KeyEventArgs e)
        {

            if (/*e.KeyCode == Keys.Enter ||*/ e.KeyCode == Keys.Space)
            {
                if (dataGridView1?.CurrentCell?.RowIndex != null)
                {
                    string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                    string categoriaid = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                    string categoria = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2]?.Value.ToString();
                    string code = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3]?.Value.ToString();
                    string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4]?.Value.ToString();
                    string price = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[7]?.Value.ToString();

                    forEditChecksModel.AddjrTestModel(new forEditChecksModel(id: id, name: name, categoriaId: categoriaid, categoria: categoria, code: code, price: price));

                    if (String.IsNullOrEmpty(richTextBox1.Text))
                        richTextBox1.AppendText(name);
                    else
                        richTextBox1.AppendText(Environment.NewLine + name);

                    richTextBox1.ScrollToCaret();
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            foreach (forEditChecksModel s in forEditChecksModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }
    }
}
