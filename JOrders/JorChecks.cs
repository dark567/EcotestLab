using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace JOrders
{
    public partial class JorChecks : Form
    {
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public JorChecks()
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

            JrOrdersMainModel.ClearJrOrdersModel();
            JrOrdersChildModel.ClearJrOrdersModel();

            getJrOrdersModel(DateTimePickerFrom.Value, DateTimePickerTo.Value);

            SortableBindingList<JrOrdersMainModel> data = new SortableBindingList<JrOrdersMainModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview
            SortableBindingList<JrOrdersChildModel> dataChild = new SortableBindingList<JrOrdersChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (JrOrdersMainModel s in JrOrdersMainModel.GetJrOrdersModel)
            {
                data.Add(new JrOrdersMainModel(id: s.Id, numCheck: s.NumCheck, subdivision: s.Subdivision, client: s.Client, agent: s.Agent, dataCheck: s?.DataCheck.ToString(), isFiscal: s.IsFiscal));
            }

            foreach (JrOrdersChildModel s in JrOrdersChildModel.GetJrOrdersChildModel)
            {
                dataChild.Add(new JrOrdersChildModel(id: s.Id, analiz: s.Analiz, edIzm: s.EdIzm, count: s.Count, price: Convert.ToDecimal(s.Price), priceOut: Convert.ToDecimal(s.PriceOut), datePlan: s?.DatePlan.ToString(), dateDone: s?.DateDone.ToString()));
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
                SelectSQL = new FbCommand("select first 1000 S.ID, S.DATE_TIME, S.NUM, S.SUBDIVISION_ID, DS.NAME as SUB, S.CLIENT_ID," +
                                        "trim(coalesce(C.SURNAME || ' ', '') || coalesce(C.NAME || ' ', '') || coalesce(C.SECNAME || ' ', '')) as CLIENT," +
                                        "S.MANAGER_ID, M.CODE_NAME as MANAGER, S.AGENT_ID," +
                                        "trim(coalesce(A.SURNAME || ' ', '') || coalesce(A.NAME || ' ', '') || coalesce(A.SECNAME || ' ', '')) as AGENT," +
                                        "S.PAYER_ORG_ID, O.CODE_NAME as ORG, S.DESCR, S.DESCR_PREVIEW, S.IS_FISCAL, S.AGREEMENT_DOC, S.SUM_BASE, S.SUM_," +
                                        "S.PAYED_SUM, C.SEX as SEX_NAME, S.DIAGNOSIS_ID, DIA.NAME as DIAGNOSIS, S.MENSTRPHASE_ID, C.SEX as SEX_ID_ID," +
                                        "S.PREG_WEEK_FROM, S.PREG_WEEK_TO, S.LAST_MENSTR_DAY, S.CYCLE_LENGTH, S.DISCONT_DESCR, S.DISCONT_DESCR_PREVIEW," +
                                        "ADDD.CODE_NAME as ADD_EMPLOYEE, S.FISCAL_PRINT_TIME, S.TYPE_DONE_FOR_COLOR as COLOR, " +
                                        "S.TYPE_EMAIL_STATUS_FOR_COLOR as COLOR_EMAIL_SEND, S.TYPE_DONE_FOR_COLOR, S.TYPE_EMAIL_STATUS_FOR_COLOR " +
                                        "/*ADDCOLUMNS*/ " +
                                        "from JOR_CHECKS S " +
                                        "left join DIC_SUBDIVISIONS DS on DS.ID = S.SUBDIVISION_ID " +
                                        "left join DIC_CLIENTS C on C.ID = S.CLIENT_ID " +
                                        "left join DIC_EMPLOYEE M on M.ID = S.MANAGER_ID " +
                                        "left join DIC_EMPLOYEE ADDD on ADDD.ID = S.ADD_EMPLOYEE_ID " +
                                        "left join DIC_CLIENTS A on A.ID = S.AGENT_ID " +
                                        "left join DIC_ORG O on O.ID = S.PAYER_ORG_ID " +
                                        "left join DIC_DIAGNOSIS DIA on DIA.ID = S.DIAGNOSIS_ID " +
                                        "where 1 = 1 " +
                                        " /*BEGINWHERECONDITIONS*/ " +
                                        //  " and S.SUBDIVISION_ID = :SUBDIVISION_ID " +
                                        " and S.DATE_TIME < cast('" + to.ToString("dd.MM.yyyy") + "' as date) + 1 " +
                                        " and S.DATE_TIME >= cast('" + from.ToString("dd.MM.yyyy") + "' as date) " +
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
                    JrOrdersMainModel.AddJrOrdersModel(new JrOrdersMainModel(id: reader?.GetString(0), numCheck: reader?.GetString(2), subdivision: reader?.GetString(4), client: reader?.GetString(6), agent: reader.GetString(10), dataCheck: reader?.GetString(1), isFiscal: reader.GetBoolean(15)));
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
            SelectSQL = new FbCommand("select first 1000 S.ID, S.HD_ID, S.GOODS_ID, G.NAME as GOOD, G.CODE, S.UNIT_ID, U.NAME as UNIT, S.BULB_NUM_ID, S.BULB_NUM_CODE, " +
                                    " S.CNT, S.PRICE_BASE, S.PRICE, S.SUM_BASE, S.SUM_, S.SUBDIVISION_EXEC_ID, S.ORG_EXEC_ID, S.PLAN_DATE_DONE, " +
                                    " S.DATE_DONE, S.DONE_EMPLOYEE_ID, E.CODE_NAME, S.IS_URGENT, S.COMPLEX_ID, S.IS_REFUSE, S.DESCR, S.DESCR_PREVIEW, " +
                                    " S.SUM_OUT, " +
                                    " case " +
                                    " when BT.COLOR is null then 16777215 " +
                                    " else BT.COLOR " +
                                    " end as COLOR, " +
                                    " S.MANIPULATION_DATE_TIME, EM.CODE_NAME as MANIPULATION_EXECUTOR, LNUM.NUM_TEXT as LAB_NUM, " +
                                    " S.DIC_NO_OPPORT_TO_RES_ID, N.NAME as DIC_NO_OPP_TO_RES, S.BLANK_ID, BM.VAL_IDX as BIOMATERIAL,  " +
                                    " S.GOODS_GRP_NAME as GRP_C " +
                                    " /*ADDCOLUMNS*/ " +
                                    " from JOR_CHECKS_DT S " +
                                    " left join DIC_GOODS G on G.ID = S.GOODS_ID " +
                                    " left join DIC_UNITS U on U.ID = S.UNIT_ID " +
                                    " left join DIC_SUBDIVISIONS DS on DS.ID = S.SUBDIVISION_EXEC_ID " +
                                    " left join DIC_ORG O on O.ID = S.ORG_EXEC_ID " +
                                    " left join DIC_EMPLOYEE E on E.ID = S.DONE_EMPLOYEE_ID " +
                                    " left join DIC_EMPLOYEE EM on EM.ID = S.MANIPULATION_EMPLOYEE_ID " +
                                    " left join DIC_BULB_TYPES BT on BT.ID = S.BULB_NUM_ID " +
                                    " left join JOR_CHECKS_DT_LABNUM LNUM on LNUM.HD_ID = S.ID " +
                                    " left join DIC_NO_OPPORT_TO_RES N on N.ID = S.DIC_NO_OPPORT_TO_RES_ID " +
                                    " left join DIC_DICS BM on BM.ID = S.MATERIAL_ID " +
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
                    JrOrdersChildModel.AddJrOrdersModel(new JrOrdersChildModel(id: readerChild?.GetString(0), count: readerChild?.GetString(9), edIzm: readerChild?.GetString(6), analiz: readerChild?.GetString(3), price: readerChild.GetDecimal(10), priceOut: readerChild.GetDecimal(12), datePlan: readerChild?.GetString(16), dateDone: readerChild?.GetString(17)));
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

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
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

                JrOrdersChildModel.ClearJrOrdersModel();

                getJrOrdersChildModel(y);

                SortableBindingList<JrOrdersChildModel> dataChild = new SortableBindingList<JrOrdersChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

                foreach (JrOrdersChildModel s in JrOrdersChildModel.GetJrOrdersChildModel)
                {
                    dataChild.Add(new JrOrdersChildModel(id: s.Id, analiz: s.Analiz, edIzm: s.EdIzm, count: s.Count, price: Convert.ToDecimal(s.Price), priceOut: Convert.ToDecimal(s.PriceOut), datePlan: s?.DatePlan.ToString(), dateDone: s?.DateDone.ToString()));
                }

                dataGridView2.DataSource = dataChild;
            }
        }


        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                EditOrder edOr = new EditOrder();
                edOr.Id = y;
                edOr.ShowDialog();
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            AddrowsToDataGrid();
        }

        #region export exel
        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "JorChecks.xls";
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

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;

            dataGridView1.RowHeadersWidth = 70;
            dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // MessageBox.Show(e.RowIndex.ToString());
            // MessageBox.Show(dataGridView1.CurrentCell.RowIndex.ToString());

           // var senderGrid = (DataGridView)sender;

            if (e.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                dataGridView2.Rows.Clear();

                dataGridView2.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
                dataGridView2.AllowUserToResizeColumns = true;

                JrOrdersChildModel.ClearJrOrdersModel();

                getJrOrdersChildModel(y);

                SortableBindingList<JrOrdersChildModel> dataChild = new SortableBindingList<JrOrdersChildModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

                foreach (JrOrdersChildModel s in JrOrdersChildModel.GetJrOrdersChildModel)
                {
                    dataChild.Add(new JrOrdersChildModel(id: s.Id, analiz: s.Analiz, edIzm: s.EdIzm, count: s.Count, price: Convert.ToDecimal(s.Price), priceOut: Convert.ToDecimal(s.PriceOut), datePlan: s?.DatePlan.ToString(), dateDone: s?.DateDone.ToString()));
                }

                dataGridView2.DataSource = dataChild;
            }
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(dataGridView1.CurrentCell?.RowIndex.ToString());

            // var senderGrid = (DataGridView)sender;
            if (dataGridView1.CurrentCell?.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                PrintReport printForm = new PrintReport();

                printForm.Id = y;
                printForm.Param = "FJOR_CHECKS";
                printForm.ShowDialog();
            }
            else
            {
                PrintReport printForm = new PrintReport();
                printForm.Param = "FJOR_CHECKS";
                printForm.ShowDialog();
            }
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

        private void DataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView2.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dataGridView2.Rows[index].HeaderCell.Value = indexStr;

            dataGridView2.RowHeadersWidth = 50;
            dataGridView2.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void DataGridView2_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
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

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {

            EditOrder editForm = new EditOrder();
            editForm.ShowDialog();
        }

        FormWindowState LastWindowState = FormWindowState.Minimized;
        private void JorChecks_Resize(object sender, EventArgs e)
        {
            // When window state changes
            //if (WindowState != LastWindowState)
            //{
            //    LastWindowState = WindowState;


            //    if (WindowState == FormWindowState.Maximized)
            //    {
            //        this.ShowIcon = false;
            //       // this.ShowInTaskbar = false;
            //    }
            //    if (WindowState == FormWindowState.Normal)
            //    {
            //       // this.ShowInTaskbar = true;
            //        this.ShowIcon = true;
            //    }
            //}
        }
    }
}
