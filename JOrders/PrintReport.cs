using FastReport;
using FastReport.Data;
using FastReport.Utils;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JOrders
{
    public partial class PrintReport : Form
    {
        public string IdCh = "n/a";
        public string Id = "n/a";

        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;
        public string Param = "";

        public PrintReport()
        {
            InitializeComponent();

            Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
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

            AddrowsToDataGrid();
        }

        private void AddrowsToDataGrid()
        {
            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            JrChecksPrintRepModel.ClearjrChecksPrintRepModel();

            getJrOrdersModel(Param);

            SortableBindingList<JrChecksPrintRepModel> data = new SortableBindingList<JrChecksPrintRepModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview


            foreach (JrChecksPrintRepModel s in JrChecksPrintRepModel.GetjrChecksPrintRep)
            {
                data.Add(new JrChecksPrintRepModel(id: s.Id, name: s.Name, body: s.Body));
            }

            dataGridView1.DataSource = data;
        }

        public static void getJrOrdersModel(string param)
        {

            //param = "FJOR_CHECKS";
            //MessageBox.Show(string.Format("Вы выбрали период с {0} до {1}", from.ToLongDateString(), to.ToLongDateString()), "Информация");
            //File.AppendAllText(Application.StartupPath + @"\Event.log", string.Format("Вы выбрали период с {0} до {1}", from.ToLongDateString(), to.ToLongDateString()) + "\n");

            FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            fb_con.Charset = "UTF8"; //используемая кодировка
            fb_con.UserID = "SYSDBA"; //логин
            fb_con.Password = "masterkey"; //пароль
            fb_con.Database = path_db; //путь к файлу базы данных
                                       // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection


            fb.Open();
            FbCommand SelectSQL = new FbCommand("select first 1000 R.ID, " +
                                        " case " +
                                        " when I.VAL is null then R.NAME " +
                                        " else I.VAL " +
                                        " end as NAME, " +
                                        " R.BODY" +
                                        // "--, A.PRINTER_NAME, R.IS_AUTO_PRINT " +
                                        " from REP_REPORTS R " +
                                        " left join LNG_INTERFACE I on I.LNG_ID = cast('1' as ID) and I.KEY_VAL = upper(R.NAME) " +
                                        //" left join CFG_AUTOPRINTER_SELECT A on A.REPORT_ID = R.ID and A.WS_ID = cast(:WS_ID as NAME) " +
                                        " where upper(GRP_NAME) containing ',' || upper(cast(@param as NAME_LARGE)) || ',' and " +
                                        " R.IS_VISIBLE = 1 " +
                                        " /*BEGINWHERECONDITIONS*/ " +
                                        " /*ENDWHERECONDITIONS*/ " +
                                        " order by NAME", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();


            // string ext = "";//!!!
            // string tempFile = Path.Combine(Application.StartupPath + @"\frx\"); //!!!

            try
            {
                while (reader.Read())
                {
                    JrChecksPrintRepModel.AddJrChecksPrintRepModel(new JrChecksPrintRepModel(id: reader?.GetString(0), name: reader.GetString(1), body: reader.GetString(2)));

                    //ext = reader.GetString(2);
                    //MessageBox.Show("ext: {0}", byte[])reader["Body");
                    //  File.WriteAllBytes(tempFile + reader.GetString(1)+".fr3", (byte[])reader["Body"]); //todo !!!

                    ////ext = rdr.GetString(2);
                    //using (MemoryStream ms = new MemoryStream((byte[])reader["Body"]))
                    //{
                    //    picBox.Image = Image.FromStream(ms);
                    //}
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
                fb.Close();
            }
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception caught in process: {0}", ex);
                MessageBox.Show("Exception caught in process: {0}", ex.Message);
                return false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PrintReport_Load(object sender, EventArgs e)
        {
            label1.Text = $"ID:{Param}";

        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Get the property object based on the DataPropertyName of the column
            var property = typeof(JrChecksPrintRepModel).GetProperty(e.Column.DataPropertyName);
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
        }

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
            {
                this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;
                // this.dataGridView1.Rows[index].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            //dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 50;
            dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

        }

        /// <summary>
        /// печать
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            //переделать  под hd_id

            if (dataGridView1.SelectedRows.Count > 0) // make sure user select at least 1 row 
            {
               // string jobId = dataGridView1.SelectedRows[0].Cells[0].Value + string.Empty;
                string nameRep = dataGridView1.SelectedRows[0].Cells[1].Value + string.Empty;

                Report report = new Report();
                // report.Load(Application.StartupPath + @"\frx\testКвитанция заказа.frx");
                report.Load(Application.StartupPath + @"\frx\test" + nameRep + ".frx");
                //report.SetParameterValue("ID", IdCh);

             //   if (nameRep == "Этикетка на заказ 3x2") report.SetParameterValue("ID", Id);
             //  if (nameRep == "Квитанция заказа") report.SetParameterValue("ID", IdCh);

                report.SetParameterValue("ID", IdCh);

             //   MessageBox.Show(Id);
              ///  MessageBox.Show(IdCh);

                report.Prepare();
                report.Show();
            }


            //// перенести в отчет
            //FbConnection fb = GetConnection();

            //string getId = "select id from jor_checks_dt where hd_id = cast(@param as ID)";
            //FbCommand SelectID = new FbCommand(getId, fb);

            ////add one IN parameter                     
            //FbParameter nameParam = new FbParameter("@param", value: IdCh);
            //// добавляем параметр к команде
            //SelectID.Parameters.Add(nameParam);

            //FbTransaction fbt = fb.BeginTransaction();
            //SelectID.Transaction = fbt;
            //FbDataReader reader = SelectID.ExecuteReader();

            //try
            //{
            //    while (reader.Read())
            //    {
            //        MessageBox.Show(reader.GetString(0));
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("error" + ex.Message);
            //    // fbt.Rollback();
            //}
            //finally
            //{
            //    fbt.Commit();
            //    reader.Close();
            //    SelectID.Dispose();
            //    fb.Close();
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

        /// <summary>
        ///  редактор
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // make sure user select at least 1 row 
            {
                string jobId = dataGridView1.SelectedRows[0].Cells[0].Value + string.Empty;
                string nameRep = dataGridView1.SelectedRows[0].Cells[1].Value + string.Empty;

                Report report = new Report();
                // report.Load(Application.StartupPath + @"\frx\testКвитанция заказа.frx");
                report.Load(Application.StartupPath + @"\frx\" + nameRep + ".frx");

                if (nameRep == "Этикетка на заказ 3x2") report.SetParameterValue("ID", Id);
                if (nameRep == "Квитанция заказа") report.SetParameterValue("ID", IdCh);
                report.Design();

            }
            //FastReport.Report report = new FastReport.Report();
            //report.Design();



        }

        public static string getJrOrdersModel_(string param = "")
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
            FbCommand SelectSQL = new FbCommand("select first 1 R.ID, " +
                                        " case " +
                                        " when I.VAL is null then R.NAME " +
                                        " else I.VAL " +
                                        " end as NAME, " +
                                        " R.BODY" +
                                        // "--, A.PRINTER_NAME, R.IS_AUTO_PRINT " +
                                        " from REP_REPORTS R " +
                                        " left join LNG_INTERFACE I on I.LNG_ID = cast('1' as ID) and I.KEY_VAL = upper(R.NAME) " +
                                        //" left join CFG_AUTOPRINTER_SELECT A on A.REPORT_ID = R.ID and A.WS_ID = cast(:WS_ID as NAME) " +
                                        " where upper(GRP_NAME) containing ',' || upper(cast('FJOR_CHECKS' as NAME_LARGE)) || ',' and " +
                                        " R.IS_VISIBLE = 1 " +
                                        " and R.ID = cast(@param as ID) " +
                                        " order by NAME", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();


            string ext = "";//!!!
            string tempFile = Path.Combine(Application.StartupPath + @"\frx\"); //!!!
            string putchFrx = null;
            try
            {
                if (!reader.HasRows) MessageBox.Show("getJrOrdersModel_ rows null");
                while (reader.Read())
                {
                    JrChecksPrintRepModel.AddJrChecksPrintRepModel(new JrChecksPrintRepModel(id: reader?.GetString(0), name: reader.GetString(1), body: reader.GetString(2)));

                    ext = reader.GetString(2);
                    //MessageBox.Show("ext: {0}", byte[])reader["Body");
                    putchFrx = tempFile + reader.GetString(1) + ".frx";
                    File.WriteAllBytes(putchFrx, (byte[])reader["Body"]); //todo !!!

                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectSQL.Dispose();
                fb.Close();

            }

            return putchFrx;
        }

    }
}
