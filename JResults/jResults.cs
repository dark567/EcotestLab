using FirebirdSql.Data.FirebirdClient;
using JOrders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace JResults
{
    public partial class jResults : Form
    {
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public jResults()
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

        private void Button1_Click(object sender, EventArgs e)
        {
            AddrowsToDataGrid();
        }

        private void AddrowsToDataGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView2.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView2.AllowUserToResizeColumns = true;

            JrResultsMainModel.ClearJrOrdersModel();
            JrResultsChildModel.ClearJrOrdersModel();

            getJrOrdersModel(DateTimePickerFrom.Value, DateTimePickerTo.Value);

            SortableBindingList<JrResultsMainModel> data = new SortableBindingList<JrResultsMainModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview
            SortableBindingList<JrResultsChildModel> dataChild = new SortableBindingList<JrResultsChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (JrResultsMainModel s in JrResultsMainModel.GetJrOrdersModel)
            {
                data.Add(new JrResultsMainModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
            }

            foreach (JrResultsChildModel s in JrResultsChildModel.GetJrOrdersChildModel)
            {
                dataChild.Add(new JrResultsChildModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
            }

            dataGridView1.DataSource = data;
            dataGridView2.DataSource = dataChild;
        }

        public static void getJrOrdersModel(DateTime from, DateTime to, string param = null)
        {
            string _param = "5c79547510ab484fa0f4dbc72ccdb74e";

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
            FbCommand SelectSQL;
            if (param == null || param == "")
                SelectSQL = new FbCommand("select first 1000 C.ID, B.ID, B.JOR_CHECK_ID, B.BLANK_ID, C.DATE_TIME, C.NUM, C.SUBDIVISION_ID," +
                                        " DS.NAME as RECEPT_SUB, C.CLIENT_ID, " +
                                        " CL.CODE_NAME as CLIENT, BL.NAME as BLANK_NAME, C.PAYER_ORG_ID, O.CODE_NAME as PAYER_ORG, " +
                                        " C.DESCR_PREVIEW as DESCR, C.AGENT_ID, A.CODE_NAME as AGENT, '' as TYPE_DONE, B.CNT_NOT_DONE_EXISTS, " +
                                        " B.CNT_DONE_EXISTS, B.PRINT_TIME, '' as COLOR " +
                                        " /*ADDCOLUMNS*/ " +
                                        " from JOR_CHECKS C " +
                                        " inner join JOR_BLANKS B on C.ID = B.JOR_CHECK_ID " +
                                        " inner join DIC_BLANKS BL on BL.ID = B.BLANK_ID " +
                                        " left join DIC_SUBDIVISIONS DS on DS.ID = C.SUBDIVISION_ID " +
                                        " left join DIC_CLIENTS CL on CL.ID = C.CLIENT_ID " +
                                        " left join DIC_CLIENTS A on A.ID = C.AGENT_ID " +
                                        " left join DIC_ORG O on O.ID = C.PAYER_ORG_ID " +
                                        " where 1 = 1 " +
                                        " /*BEGINWHERECONDITIONS*/ " +
                                         " and C.DATE_TIME < cast('" + to.ToString("dd.MM.yyyy") + "' as date) + 1 " +
                                         " and C.DATE_TIME >= cast('" + from.ToString("dd.MM.yyyy") + "'as date) " +
                                        " /*ENDWHERECONDITIONS*/", fb);
            else
                SelectSQL = new FbCommand("SELECT first 1000 Id, SURNAME, Name, SECNAME, SEX, BIRTH_DATE, EMAIL FROM dic_clients where UPPER(CODE_NAME) LIKE UPPER(@param) ORDER BY SURNAME", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", "%" + param + "%");
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    JrResultsMainModel.AddJrOrdersModel(new JrResultsMainModel(id: reader?.GetString(0), surname: reader?.GetString(1), name: reader?.GetString(2), secname: reader?.GetString(3), sex: reader.GetString(4), birthdate: reader?.GetString(5), email: reader?.GetString(6)));
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

        public static void getJrOrdersChildModel(string Hd_ID = null)
        {
            string _param = Hd_ID;

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
            FbCommand SelectSQL;
            //child
            SelectSQL = new FbCommand("select first 1000 S.ID, S.NAME as PARAM, S.UNIT_ID, S.UNIT_NAME as UNIT, " +
                                    "S.NORM_TEXT_PREWIEV as NORM_TEXT, S.RESULT, S.RESULT_TEXT, " +
                                    " S.RESULT_BLOB_PREVIEW, S.IS_OUT_OF_NORM, S.ORDER_, D.DATE_DONE, G.NAME as ANALIZ " +
                                    " /*ADDCOLUMNS*/ " +
                                    " from JOR_CHECKS_DT D " +
                                    " left join JOR_RESULTS_DT S on D.ID = S.HD_ID " +
                                    " left join DIC_GOODS G on G.ID = D.GOODS_ID " +
                                    " where 1 = 1 and D.IS_COMPLEX = 0 " +
                                    " /*BEGINWHERECONDITIONS*/ " +
                                    " and D.HD_ID = cast(@param as ID) " +
                                    //" and D.BLANK_ID = cast(:BLANK_ID as ID) " +
                                    " /*ENDWHERECONDITIONS*/", fb);
            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: _param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader readerChild = SelectSQL.ExecuteReader();

            try
            {
                while (readerChild.Read())
                {
                    JrResultsChildModel.AddJrOrdersModel(new JrResultsChildModel(id: readerChild?.GetString(0), surname: readerChild?.GetString(1), name: readerChild?.GetString(2), secname: readerChild?.GetString(3), sex: readerChild.GetString(4), birthdate: readerChild?.GetString(5), email: readerChild?.GetString(6)));
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                fbt.Commit();
                readerChild.Close();
                SelectSQL.Dispose();
                fb.Close();
            }

        }

        #region export exel
        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "JorResults.xls";
         
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Copy DataGridView results to clipboard
                copyAlltoClipboard();

                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Excel.Application();

                xlexcel.DisplayAlerts = false; // Without this you will get two confirm overwrite prompts
                Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // Format column D as text before pasting results, this was required for my data
                Excel.Range rng = xlWorkSheet.get_Range("D:D").Cells;
                rng.NumberFormat = "@";

                // Paste clipboard results to worksheet range
                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                // For some reason column A is always blank in the worksheet. ¯\_(ツ)_/¯
                // Delete blank column A and select cell A1
                Excel.Range delRng = xlWorkSheet.get_Range("A:A").Cells;
                delRng.Delete(Type.Missing);
                xlWorkSheet.get_Range("A1").Select();

                // Save the excel file under the captured location from the SaveFileDialog
                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlexcel.DisplayAlerts = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlexcel.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlexcel);

                // Clear Clipboard and DataGridView selection
                Clipboard.Clear();
                dataGridView1.ClearSelection();

                // Open the newly saved excel file
                if (File.Exists(sfd.FileName))
                    System.Diagnostics.Process.Start(sfd.FileName);
            }
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

        private void DateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (DateTimePickerFrom.Value > DateTimePickerTo.Value) DateTimePickerTo.Value = DateTimePickerFrom.Value;
        }

        private void DateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            if (DateTimePickerFrom.Value > DateTimePickerTo.Value) DateTimePickerFrom.Value = DateTimePickerTo.Value;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                dataGridView2.Rows.Clear();

                dataGridView2.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
                dataGridView2.AllowUserToResizeColumns = true;

                JrResultsChildModel.ClearJrOrdersModel();

                getJrOrdersChildModel(y);

                SortableBindingList<JrResultsChildModel> dataChild = new SortableBindingList<JrResultsChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

                foreach (JrResultsChildModel s in JrResultsChildModel.GetJrOrdersChildModel)
                {
                    dataChild.Add(new JrResultsChildModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
                }

                dataGridView2.DataSource = dataChild;
            }
        }

        private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                dataGridView2.Rows.Clear();

                dataGridView2.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
                dataGridView2.AllowUserToResizeColumns = true;

                JrResultsChildModel.ClearJrOrdersModel();

                getJrOrdersChildModel(y);

                SortableBindingList<JrResultsChildModel> dataChild = new SortableBindingList<JrResultsChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

                foreach (JrResultsChildModel s in JrResultsChildModel.GetJrOrdersChildModel)
                {
                    dataChild.Add(new JrResultsChildModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
                }

                dataGridView2.DataSource = dataChild;
            }
        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell?.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                PrintReport printForm = new PrintReport();

                printForm.Id = y;
                printForm.Param = "FJOR_RESULTS";
                printForm.ShowDialog();
            }
            else
            {
                PrintReport printForm = new PrintReport();
                printForm.Param = "FJOR_RESULTS";
                printForm.ShowDialog();
            }
        }
    }

}
