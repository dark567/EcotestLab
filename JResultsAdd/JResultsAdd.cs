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

namespace JResultsAdd
{
    public partial class jResultsAdd : Form
    {
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public jResultsAdd()
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
            dataGridView2.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView2.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView2.AllowUserToResizeColumns = true;

            JrResultsMainModel.ClearJrOrdersModel();
            JrResultsChildModel.ClearJrOrdersModel();

            getJrOrdersModel(dateTimePickerFrom.Value, dateTimePickerTo.Value);

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
                SelectSQL = new FbCommand("select first 1000 D.ID as ID, D.IS_URGENT, D.IS_REFUSE, D.CHECK_NUM as NUM, D.BULB_NUM_CODE as BULB_CODE, D.GOODS_ID, " +
                                         " D.GOODS_NAME as GOODS, D.GOODS_GRP_ID as GRP_ID, D.GOODS_GRP_NAME as GRP_, D.CHECK_DATE as DATE_TIME, " +
                                         " D.CHECK_CLIENT_CODE_NAME as CLIENT, D.CHECK_CLIENT_ID as CLIENT_CODE, " +
                                         " D.CHECK_SUBDIVISION_NAME as SUBDIVISION_RECEPT, D.CHECK_SUBDIVISION_ID as SUBDIVISION_RECEPT_ID, " +
                                         " D.CHECK_PAYER_ORG_CODE_NAME as PAYER_ORG, D.SUBDIVISION_EXEC_NAME as SUBDIVISION_EXEC, D.SUBDIVISION_EXEC_ID, " +
                                         " D.ORG_EXEC_CODE_NAME as ORG_EXEC, D.ORG_EXEC_ID, D.PLAN_DATE_DONE, D.DATE_DONE, D.DATE_DONE_PREV, " +
                                         " D.DONE_EMPLOYEE_ID, D.DONE_EMPLOYEE_CODE_NAME as DONE_EMPLOYEE, D.CHECK_EMPLOYEE_ID, " +
                                         " D.CHECK_EMPLOYEE_CODE_NAME as CHECK_EMPLOYEE, CL.SEX as SEX_ID, CL.SEX as SEX_NAME, " +
                                         " (select R_YEAR from GET_DATE_DIFF(CL.BIRTH_DATE, current_date)) as AGE, DI.CODE_NAME as DIAGNOSIS, DD.VAL_IDX as MENSTPHASE, " +
                                         " C.PREG_WEEK_FROM, C.PREG_WEEK_TO, C.DESCR_PREVIEW as CHECK_DESCR, D.DESCR as DT_DESCR, " +
                                         " D.DESCR_PREVIEW as DT_DESCR_PREVIEW, D.RESULT_TEXT_PREVIEW as RESULT_TEXT, D.GOODS_ORDER as ORDER_, " +
                                         " D.CHECK_AGENT_CODE_NAME as AGENT, AGO.CODE_NAME as AGENT_ORG, C.AGENT_ID, D.HD_ID as JOR_CHECKS_ID, " +
                                         " D.MANIPULATION_DATE_TIME as MANIPULATION_DATE, D.MANIPULATION_EMPLOYEE_CODE_NAME as MANIPULATION_EXECUTOR, " +
                                         " case when VIS.CNT_ > 1 then 1 " +
                                         " else 0 end as IS_WAS_BEFORE, " +
                                         " LNUM.NUM_TEXT as NUM_TEXT_ADD, D.DIC_NO_OPPORT_TO_RES_ID, D.DIC_NO_OPPORT_TO_RES_NAME as NO_OPPORT_TO_RES_NAME, " +
                                         " D.MATERIAL_ID, BM.VAL_IDX as BIOMATERIAL, D.LAB_PROCESS_ID, LP.NUM as LAB_PROCESS, M.VAL_IDX as METHODIC, " +
                                         " D.IS_REMARKED_DATE_TIME, D.BLANK_ID as DIC_BLANK_ID " +
                                         " , (select jb.print_time from JOR_BLANKS JB " +
                                         " where d.date_done is not null " +
                                         " and D.HD_ID = JB.JOR_CHECK_ID " +
                                         " and D.BLANK_ID = JB.BLANK_ID) as PRINT_BLANK_DATE " +
                                         " from JOR_CHECKS_DT D " +
                                         " left join JOR_CHECKS C on C.ID = D.HD_ID " +
                                         " left join DIC_CLIENTS CL on CL.ID = C.CLIENT_ID " +
                                         " left join ACC_CNT_CLIENTS_VISITS VIS on VIS.CLIENT_ID = C.CLIENT_ID " +
                                         " left join DIC_DIAGNOSIS DI on DI.ID = C.DIAGNOSIS_ID " +
                                         " left join DIC_DICS DD on DD.ID = C.MENSTRPHASE_ID " +
                                         " left join DIC_CLIENTS AG on AG.ID = C.AGENT_ID " +
                                         " left join DIC_ORG AGO on AGO.ID = AG.ORG_ID " +
                                         " left join JOR_CHECKS_DT_LABNUM LNUM on LNUM.HD_ID = D.ID " +
                                         " left join DIC_DICS BM on BM.ID = D.MATERIAL_ID " +
                                         " left join JOR_LAB_PROCESS LP on LP.ID = D.LAB_PROCESS_ID " +
                                         " left join JOR_CHECKS_DT_METHODIC DTM on DTM.CHECK_DT_ID = D.ID " +
                                         " left join DIC_DICS M on M.ID = DTM.METHODIC_ID " +
                                         " where D.BULB_NUM_ID is not null and D.IS_COMPLEX = 0 " +
                                         " /*BEGINWHERECONDITIONS*/ " +
                                         " and D.IS_REFUSE = 0 " +
                                         " and D.DATE_DONE is null " +
                                         " and D.CHECK_DATE < cast('" + to.ToString("dd.MM.yyyy") + "' as date) + 1 " +
                                         " and D.CHECK_DATE >= cast('" + from.ToString("dd.MM.yyyy") + "' as date) " +
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
            SelectSQL = new FbCommand("select first 1000 S.ID, S.HD_ID, S.NORMS_DT_ID, S.NAME, S.UNIT_ID, U.NAME as UNIT, S.NORM_TEXT as NORM_TEXT_BLOB, " +
                                    " S.NORM_TEXT_PREWIEV as NORM_TEXT, S.RESULT, S.RESULT_TEXT, S.RESULT_BLOB, S.RESULT_BLOB_PREVIEW, " +
                                    " S.IS_OUT_OF_NORM, DT.ORDER_, S.HIGH, S.LOW, S.VHIGH, S.VLOW, S.DILUTION, S.CODE_NAME, S.IS_FILLED " +
                                    " ,(s.code_name) as CODE " +
                                    " from JOR_RESULTS_DT S " +
                                    " left join DIC_UNITS U on U.ID = S.UNIT_ID " +
                                    " left join DIC_GOODS_LAB_NORMS_DT DT on DT.ID = S.NORMS_DT_ID " +
                                    " where 1 = 1 " +
                                    " /*BEGINWHERECONDITIONS*/ " +
                                    " and S.HD_ID = cast(@param as ID) " +
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

        private void DateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value > dateTimePickerTo.Value) dateTimePickerTo.Value = dateTimePickerFrom.Value;
        }

        private void DateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value > dateTimePickerTo.Value) dateTimePickerFrom.Value = dateTimePickerTo.Value;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddrowsToDataGrid();
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

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var senderGrid = (DataGridView)sender;

                string y = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                JResultsAddEdit edOr = new JResultsAddEdit();
                edOr.Id = y;
                edOr.ShowDialog();
            }
        }

        #region export exel
        private void ToolStripButton7_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "JorResultsAdd.xls";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Copy DataGridView results to clipboard
                CopyAlltoClipboard();

                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Microsoft.Office.Interop.Excel.Application();

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

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlexcel);

                // Clear Clipboard and DataGridView selection
                Clipboard.Clear();
                dataGridView1.ClearSelection();

                // Open the newly saved excel file
                if (File.Exists(sfd.FileName))
                    System.Diagnostics.Process.Start(sfd.FileName);
            }
        }
        private void CopyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void ReleaseObject(object obj)
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

        private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // MessageBox.Show(e.RowIndex.ToString());
            // MessageBox.Show(dataGridView1.CurrentCell.RowIndex.ToString());

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

        private void ToolStripButton8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell?.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                PrintReport printForm = new PrintReport();

                printForm.Id = y;
                printForm.Param = "FJOR_LAB_MANAGER";
                printForm.ShowDialog();
            }
            else
            {
                PrintReport printForm = new PrintReport();
                printForm.Param = "FJOR_LAB_MANAGER";
                printForm.ShowDialog();
            }
        }
    }


}
