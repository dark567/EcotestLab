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

                    path_db = manager.GetPrivateString("workstation", "Key");
                    Key.Value = path_db;
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
        public static ArrayList getNomID_dic_goods_grp(string ID)
        {
            ArrayList Nom = new ArrayList(); ;

            // Описание: ExecuteScalar — получение единственного значения. Firebird, InterBase .Net провайдер (c#)
            //string connString = "User=SYSDBA;" +
            //                    "Password=masterkey;" +
            //                    "Charset = UTF8;" +
            //                    "Database=127.0.0.1:terra;" +
            //                    "DataSource=localhost;" +
            //                    "Port=3050;";

            // формируем connection string для последующего соединения с нашей базой данных
            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "UTF8"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = db_puth.Value; //путь к файлу базы данных
                                             // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection


            // FbConnection fb = new FbConnection(connString);

            fb.Open();
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

        public static string getNomNameFromID(string ID)
        {
            string Nom = "n/a";

            // Описание: ExecuteScalar — получение единственного значения. Firebird, InterBase .Net провайдер (c#)
            //string connString = "User=SYSDBA;" +
            //                    "Password=masterkey;" +
            //                    "Charset = UTF8;" +
            //                    "Database=127.0.0.1:terra;" +
            //                    "DataSource=localhost;" +
            //                    "Port=3050;";
            //FbConnection fb = new FbConnection(connString);

            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "UTF8"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = db_puth.Value; //путь к файлу базы данных
                                             // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection


            try
            {
                fb.Open();
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
                data.Add(new SampleRow(id: s.ID, code: s.Code, name: s.Name, _isService: s.IsService, price: s.Price.ToString("F2"), _isSale: s.IsSale));
            }

            dataGridView1.DataSource = data;

            //// set autosize mode
            // dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            // dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // //dataGridView1.Columns[0].Width = 250;
            // //dataGridView1.Columns[1].Width = 250;
            // //dataGridView1.Columns[2].Width = 250;

            // //datagrid has calculated it widths so we can store them
            // for (int i = 0; i <= dataGridView1.Columns.Count - 1; i++)
            // {
            //     //store autosized widths
            //     int colw = dataGridView1.Columns[i].Width;
            //     //remove autosizing
            //     dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //     //set width to calculated by autosize
            //     dataGridView1.Columns[i].Width = colw;
            // }

            //dataGridView1.AutoGenerateColumns = false;

            ////create the column programatically
            //DataGridViewCell cell = new DataGridViewTextBoxCell();
            //DataGridViewTextBoxColumn colFileName = new DataGridViewTextBoxColumn()
            //{
            //    CellTemplate = cell,
            //    Name = "code",
            //    HeaderText = "File Name",
            //    DataPropertyName = "Value" // Tell the column which property of FileName it should use
            //};

            //dataGridView1.Columns.Add(colFileName);


            /*
            TreeNode selectedNode = e.Node;

            DicGoodsModel.ClearDicGoodsModel();
            getNomDicGoodsID_(selectedNode.Name);

            foreach (DicGoodsModel s in DicGoodsModel.GetDicGoodsModel)
            {
                dataGridView1.Rows.Add();
                dataGridView1["code", dataGridView1.Rows.Count - 1].Value = s.ID;
                dataGridView1["name", dataGridView1.Rows.Count - 1].Value = s.Name;
                dataGridView1["price", dataGridView1.Rows.Count - 1].Value = s.Price.ToString("F2");
            }
            */
            //for (int i = 0; i < 100; ++i)
            //{
            //    //Добавляем строку, указывая значения колонок поочереди слева направо
            //    dataGridView1.Rows.Add("Пример 1, Товар " + i, i * 1000, i);
            //}

            //for (int i = 0; i < 5; ++i)
            //{
            //   // Добавляем строку, указывая значения каждой ячейки по имени(можно использовать индекс 0, 1, 2 вместо имен)
            //    dataGridView1.Rows.Add();
            //    dataGridView1["code", dataGridView1.Rows.Count - 1].Value = "Пример 2, Товар xxx" + i;
            //    dataGridView1["name", dataGridView1.Rows.Count - 1].Value = i * 1000;
            //    dataGridView1["price", dataGridView1.Rows.Count - 1].Value = i;
            //}

            ////А теперь простой пройдемся циклом по всем ячейкам
            //for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            //{
            //    for (int j = 0; j < dataGridView1.Columns.Count; ++j)
            //    {
            //        //Значения ячеек хрянятся в типе object
            //        //это позволяет хранить любые данные в таблице
            //        object o = dataGridView1[j, i].Value;
            //    }
            //}

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
        public static void getNomDicGoodsID_(string ID)
        {

            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "UTF8"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = db_puth.Value; //путь к файлу базы данных
                                             // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

            fb.Open();
            FbCommand SelectSQL = new FbCommand("SELECT id, code, name, IS_SERVICE, PRICE_OUT, IS_ACTIVE FROM dic_goods where GRP_ID = @cust_no ORDER BY name", fb);
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
                    DicGoodsModel.AddDicGoodsModel(new DicGoodsModel(id: reader?.GetString(0), code: reader?.GetString(1), name: reader?.GetString(2), isService: reader?.GetString(3) == "1" ? true : false, price: reader.GetDouble(4), isSale: reader?.GetString(5) == "1" ? true : false));
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
                //var senderGrid = (DataGridView)sender;

                if (dataGridView1?.CurrentCell?.RowIndex != null)
                {
                    string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                    string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2]?.Value.ToString();

                    forEditChecksModel.AddjrTestModel(new forEditChecksModel(id: id, name: name));

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
