using DG;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DClients
{
    public partial class dicClients : Form
    {
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public dicClients()
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
                    //db_puth.Value = path_db;

                    //path_db = manager.GetPrivateString("workstation", "Key");
                    // Key.Value = path_db;
                    //MessageBox.Show("бд - " + path_db, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    File.AppendAllText(Application.StartupPath + @"\Applog.log", "путь к db:" + path_db + "\n");
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


            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            DcClientsModel.ClearDcClientsModel();
            getNomDicGoodsID_();

            SortableBindingList<DcClientsModel> data = new SortableBindingList<DcClientsModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcClientsModel s in DcClientsModel.GetDcClientsModel)
            {
                data.Add(new DcClientsModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
            }
            
            dataGridView1.DataSource = data;
 
     
        }

        public static void getNomDicGoodsID_(string SURNAME = null)
        {

            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "UTF8"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = path_db; //путь к файлу базы данных
                                       // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection


            fb.Open();
            FbCommand SelectSQL;
            if (SURNAME == null || SURNAME == "") SelectSQL = new FbCommand("SELECT first 100 Id, SURNAME, Name, SECNAME, SEX, BIRTH_DATE, EMAIL FROM dic_clients ORDER BY SURNAME", fb);
            else SelectSQL = new FbCommand("SELECT first 1000 Id, SURNAME, Name, SECNAME, SEX, BIRTH_DATE, EMAIL FROM dic_clients where UPPER(CODE_NAME) LIKE UPPER(@param) ORDER BY SURNAME", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", "%" + SURNAME + "%");
           // FbParameter nameParam = new FbParameter("@param", SURNAME + "%");
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    DcClientsModel.AddDcClientsModel(new DcClientsModel(id: reader?.GetString(0), surname: reader?.GetString(1), name: reader?.GetString(2), secname: reader?.GetString(3), sex: reader.GetString(4), birthdate: reader?.GetString(5), email: reader?.GetString(6)));
                }
            }
            catch (Exception) 
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
        }

        //private static string MyConvert(string dateTime)
        //{
        //    if (dateTime == null) return null;
        //    return (DateTime.ParseExact(dateTime, "yyyy-MM-dd",
        //                               CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd");
        //}

        private void DicClients_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void Button1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button1.ClientRectangle,
            SystemColors.ControlLightLight, 1, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 1, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 3, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 3, ButtonBorderStyle.Outset);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            DcClientsModel.ClearDcClientsModel();
            getNomDicGoodsID_(secnameFIO.Text);

            BindingList<DcClientsModel> data = new BindingList<DcClientsModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcClientsModel s in DcClientsModel.GetDcClientsModel)
            {
                data.Add(new DcClientsModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
            }

            dataGridView1.DataSource = data;
        }

        private void SecnameFIO_KeyUp(object sender, KeyEventArgs e)
        {
            if (/*e.KeyCode == Keys.Enter ||*/ e.KeyCode == Keys.F2)
            {
                Button1_Click(sender, e);
            }
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
           
                // Get the property object based on the DataPropertyName of the column
                var property = typeof(DcClientsModel).GetProperty(e.Column.DataPropertyName);
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
    }
}
