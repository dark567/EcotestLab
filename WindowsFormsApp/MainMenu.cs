using DClients;
using DG;
using JOrders;
using JResultsAdd;
using System;
using System.Windows.Forms;
using JResults;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using RbKey;
using System.Linq;

namespace WindowsFormsApp
{
    public partial class MainMenu : Form
    {
        public string path_db;
        public string user;
        public string pass;

        FbConnection fb;

        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";

        public MainMenu()
        {
            InitializeComponent();
            // Load += new EventHandler(Form1_Load);
        }

        /// <summary>
        /// Check Lic
        /// </summary>
        /// <returns></returns>
        private bool CheckLic()
        {
            if (ApplicLogic.CalculateMD5Hash(ApplicLogic.GetProcessorIdAndGetOSSerialNumberID()) != Key.Value)
                return false;
            else
                return true;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            #region write ini
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
                    // MessageBox.Show("бд - " + db_puth.Value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // MessageBox.Show("Key.Value - " + Key.Value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    File.AppendAllText(Application.StartupPath + @"\AppLog.log", "путь к db:" + path_db + "\n");
                    //Записать значение по ключу age в секции main
                    // manager.WritePrivateString("main", "age", "21");

                    if (!CheckLic())
                    {
                        Application.Exit();

                        //KeyLic keyLic = new KeyLic(); // потом включить или нет)
                        //keyLic.Param = ApplicLogic.CalculateMD5Hash(ApplicLogic.GetProcessorIdAndGetOSSerialNumberID());
                        //keyLic.ShowDialog();

                    }
                    // else MessageBox.Show(Key.Value);
                }
                else MessageBox.Show("File set.ini not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ini не прочтен" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            var dialog = new LoginForm();
            //dialog.UserName = "";
            //dialog.Password = "";

            if ((dialog.ShowDialog() == DialogResult.OK))
            {
                //MessageBox.Show("Не корректный логин или пароль " + dialog.UserName + '/' + dialog.Password, "", MessageBoxButtons.OK, MessageBoxIcon.Error);


                ApplicationContext dbContext = new ApplicationContext();

                try
                {
                    //string s = dbContext.Database.Connection.ConnectionString;
                    //var builder = new FbConnectionStringBuilder(s);
                    //builder.UserID = dialog.UserName;
                    //builder.Password = dialog.Password;

                    //dbContext.Database.Connection.ConnectionString = builder.ConnectionString;

                    //// пробуем подключится
                    // dbContext.Database.Connection.Open();

                    // формируем connection string для последующего соединения с нашей базой данных
                    FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
                    fb_con.Charset = "UTF8"; //используемая кодировка
                    fb_con.UserID = "SYSDBA"; //логин
                    fb_con.Password = "masterkey"; //пароль
                    fb_con.Database = db_puth.Value; //путь к файлу базы данных
                    fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
                    fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

                    DataSet dataset = new DataSet();

                    fb.Open();

                    FbCommand fbcommand = fb.CreateCommand();
                    fbcommand.CommandType = CommandType.Text;
                    fbcommand.Connection = fb;

                    fbcommand.CommandText = @"SELECT login, pass FROM sec_users WHERE Login='" + dialog.UserName + "' AND Pass='" + LibrarySec.CalculateMD5Hash(dialog.Password) + "';";

                    FbDataAdapter adaptor = new FbDataAdapter(fbcommand.CommandText, fbcommand.Connection);
                    adaptor.Fill(dataset, "0");
                    int count = dataset.Tables[0].Rows.Count;

                    FbDatabaseInfo fb_inf = new FbDatabaseInfo(fb); //информация о БД
                    //typelabel.Text = "connect Info: " + fb_inf.ServerClass + "; " + fb_inf.ServerVersion + $"; Rows:{count}";
                    //statusStrip1.Items.Add(typelabel);

                    if (count <= 0)
                    {
                        MessageBox.Show("Неправильный логин или пароль"/* + dialog.UserName + '/' + dialog.Password*/, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //MessageBox.Show(fbcommand.CommandText, "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Application.Exit(); //!!!!!!!!!!!
                    }

                    //if (!CheckLic())
                    //{
                    //    MessageBox.Show(Key.Value + ":" + LibrarySec.CalculateMD5Hash(LibrarySec.GetProcessorIdAndGetOSSerialNumberID()), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                }
                catch (Exception ex)
                {
                    // отображаем ошибку
                    MessageBox.Show(ex.Message, "Error");
                    Application.Exit();
                }

            }
            else Application.Exit();
        }


        #region event
        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            About();
        }
        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About();
        }
        private void НоменклатураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<DicGoods>().Count() < 1)
                DicGoods();
        }
        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<DicGoods>().Count() < 1)
                DicGoods();
        }
        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<JorChecks>().Count() < 1)
                JorChecks();

        }
        private void ЗаказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<JorChecks>().Count() < 1)
                JorChecks();
        }
        private void КлиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<dicClients>().Count() < 1)
                dicClients();
        }
        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<dicClients>().Count() < 1)
                dicClients();
        }
        private void ЗаполнениеРезультатовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Application.OpenForms.OfType<jResultsAdd>().Count() == 1)
            //    Application.OpenForms.OfType<jResultsAdd>().First().Close();

            if (Application.OpenForms.OfType<jResultsAdd>().Count() < 1)
                jResultsAdd();

        }
        private void ToolStripButton2_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<jResultsAdd>().Count() < 1)
                jResultsAdd();
        }
        private void РезультатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<jResults>().Count() < 1)
                jResults();
        }
        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<jResults>().Count() < 1)
                jResults();
        }
        private void jResults()
        {
            jResults newMDIChild = new jResults();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
        private void jResultsAdd()
        {
            jResultsAdd newMDIChild = new jResultsAdd();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
        private void dicClients()
        {
            dicClients newMDIChild = new dicClients();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
        private void JorChecks()
        {
            JorChecks newMDIChild = new JorChecks();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
        private void DicGoods()
        {
            //DicGoods dicGoods = new DicGoods();
            //dicGoods.ShowDialog(this);

            DicGoods newMDIChild = new DicGoods();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            // Display the new form.
            newMDIChild.Show();
        }
        private void About()
        {
            About formAbout = new About();
            formAbout.ShowDialog(this);
        }
        #endregion
    }
}


/*
 если поставить одинаковый icon  у ролительской формы и child То интересный эффект. 
 расширяется toolStrip родителя
     
     */
