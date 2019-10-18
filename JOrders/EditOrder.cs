using DClients;
using DG;
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

    public partial class EditOrder : Form
    {
        public string Id = "n/a";
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;
        public string param = "";

        public EditOrder()
        {

            InitializeComponent();
        }

        private void EditOrder_Load(object sender, EventArgs e)
        {

            textBox1.Text = $"ID:{Id}";

           
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

            getJrCheckEditModel(Id);

            SortableBindingList<JrEditCheckModel> data = new SortableBindingList<JrEditCheckModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview


            foreach (JrEditCheckModel s in JrEditCheckModel.GetJrEditCheckModel)
            {
                data.Add(new JrEditCheckModel(id: s.Id, dateChecks: s?.DateChecks.ToString(), numChecks: s.NumChecks, name: s.Name, surname: s.Surname, sex: s.Sex, email: s.Email));
            }

            dataGridView1.DataSource = data;
        }

        public static void getJrCheckEditModel(string param = "")
        {
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
            FbCommand SelectSQL = new FbCommand("select first 1000 S.ID, S.HD_ID, S.GOODS_ID, G.NAME as GOOD," +
                                        "G.CODE, S.UNIT_ID, U.NAME as UNIT, S.BULB_NUM_ID, S.BULB_NUM_CODE, " +
                                        "S.CNT, S.PRICE_BASE, S.PRICE, S.SUM_BASE, S.SUM_, S.SUBDIVISION_EXEC_ID, " +
                                        "S.ORG_EXEC_ID, S.PLAN_DATE_DONE, " +
                                        "S.DATE_DONE, S.DONE_EMPLOYEE_ID, E.CODE_NAME, S.IS_URGENT, S.COMPLEX_ID, " +
                                        "S.IS_REFUSE, S.DESCR, S.DESCR_PREVIEW, " +
                                        "S.SUM_OUT, case when BT.COLOR is null then 16777215 else BT.COLOR end as COLOR, " +
                                        "S.MANIPULATION_DATE_TIME, EM.CODE_NAME as MANIPULATION_EXECUTOR, LNUM.NUM_TEXT as LAB_NUM, " +
                                        "S.DIC_NO_OPPORT_TO_RES_ID, N.NAME as DIC_NO_OPP_TO_RES, S.BLANK_ID," +
                                        "BM.VAL_IDX as BIOMATERIAL, S.MATERIAL_ID, " +
                                        "S.GOODS_GRP_NAME as GRP_C " +
                                        "from JOR_CHECKS_DT S " +
                                        "left join DIC_GOODS G on G.ID = S.GOODS_ID " +
                                        "left join DIC_UNITS U on U.ID = S.UNIT_ID " +
                                        "left join DIC_SUBDIVISIONS DS on DS.ID = S.SUBDIVISION_EXEC_ID " +
                                        "left join DIC_ORG O on O.ID = S.ORG_EXEC_ID " +
                                        "left join DIC_EMPLOYEE E on E.ID = S.DONE_EMPLOYEE_ID " +
                                        "left join DIC_EMPLOYEE EM on EM.ID = S.MANIPULATION_EMPLOYEE_ID " +
                                        "left join DIC_BULB_TYPES BT on BT.ID = S.BULB_NUM_ID " +
                                        "left join JOR_CHECKS_DT_LABNUM LNUM on LNUM.HD_ID = S.ID " +
                                        "left join DIC_NO_OPPORT_TO_RES N on N.ID = S.DIC_NO_OPPORT_TO_RES_ID " +
                                        "left join DIC_DICS BM on BM.ID = S.MATERIAL_ID " +
                                        "where 1 = 1 " +
                                        "and S.HD_ID = cast(@param as ID) " +
                                        "order by cast(substring(S.GOODS_NAME from 1 for 4) || right(S.GOODS_ID, 2) as CHAR_IDX)", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    JrEditCheckModel.AddJrOrdersModel(new JrEditCheckModel(id: reader?.GetString(0), surname: reader.GetString(1), name: reader.GetString(2), numChecks: reader.GetString(2), sex: reader.GetString(2), dateChecks: reader.GetString(2), email: reader.GetString(2)));
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

        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            int index = e.RowIndex;
            string indexStr = (index + 1).ToString();
            object header = this.dataGridView1.Rows[index].HeaderCell.Value;
            if (header == null || !header.Equals(indexStr))
                this.dataGridView1.Rows[index].HeaderCell.Value = indexStr;

            dataGridView1.RowHeadersWidth = 50;
            dataGridView1.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {

            // Get the property object based on the DataPropertyName of the column
            var property = typeof(JrEditCheckModel).GetProperty(e.Column.DataPropertyName);
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

        #region export exel
        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = "EditOrder.xls";
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

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            // DicGoods DicGoodsForm = new DicGoods();
            //// DicGoodsForm.Owner = this;
            // DicGoodsForm.ShowDialog();

            DicGoods f2 = new DicGoods();
            f2.MyLabelClicked += new DicGoods.MyLabelClickedHandler(DicGoods_DicGoodsClicked);
            f2.ShowDialog();
        }

        void DicGoods_DicGoodsClicked(forEditChecksModel testModel)
        {
            // textBox6.Text = text;


            SortableBindingList<forEditChecksModel> data = new SortableBindingList<forEditChecksModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //More code here
                data.Add(new forEditChecksModel(id: row.Cells[0].Value.ToString(), name: row.Cells[1].Value.ToString()/*, secname: row.Cells[1].Value.ToString(), sex: row.Cells[1].Value.ToString(), birthdate: row.Cells[1].Value.ToString(), email: row.Cells[1].Value.ToString()*/));
            }

            data.Add(new forEditChecksModel(id: testModel.Id, name: testModel.Name/*, secname: s.Surname, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email*/));

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = data;
        }

        void DicClientsAdd(forAddDicClientsModel testModel)
        {
            textBox1.Text = testModel.CodeName.ToString();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
            DialogResult dialogResult = MessageBox.Show("Предупреждение", "Удалить заказ?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                delJrCheckEditModel(Id);
                Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        public static void delJrCheckEditModel(string param = "")
        {
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
            FbCommand SelectSQL = new FbCommand("delete from JOR_CHECKS where id = cast(@param as ID)", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            try
            {
                SelectSQL.ExecuteNonQuery();
                MessageBox.Show("Del");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" +ex.Message);
            }
            finally
            {
                fbt.Commit();
                SelectSQL.Dispose();
                fb.Close();
            }

            //FbConnection conn = new FbConnection(fb_con.ToString());
            //FbTransaction fbt_ = conn.BeginTransaction();
            //try
            //{
            //    using (FbCommand cmd = new FbCommand("delete from JOR_CHECKS where id = cast(@param as ID)", conn))
            //    {
            //        //add one IN parameter                     
            //        FbParameter nameParam_ = new FbParameter("@param", value: param);
            //        // добавляем параметр к команде
            //        cmd.Parameters.Add(nameParam_);
            //        cmd.Transaction = fbt_;
            //        cmd.ExecuteNonQuery();
            //    }
            //}
            //finally
            //{
            //    fbt_.Commit();
            //    //SelectSQL.Dispose();
            //    conn.Close();
            //}
        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(dataGridView1.CurrentCell?.RowIndex.ToString());

            // var senderGrid = (DataGridView)sender;
            if (dataGridView1.CurrentCell?.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                PrintReport printForm = new PrintReport();
                printForm.Id = y;
                printForm.Param = "FJOR_CHECKS_EDITOR";
                printForm.ShowDialog();
            }
            else
            {
                PrintReport printForm = new PrintReport();
                printForm.Param = "FJOR_CHECKS_EDITOR";
                printForm.ShowDialog();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (/*e.KeyCode == Keys.Enter ||*/ e.KeyCode == Keys.F2)
            {
                Button1_Click(sender, e);
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            dicClients f2 = new dicClients();
            f2.MyLabelClicked += new dicClients.MyLabelClickedHandler(DicClientsAdd);
            f2.ShowDialog();
        }
    }
}
