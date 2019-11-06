using DClients;
using DG;
using DicAgent;
using DicEmployee;
using DicOrg;
using DicSubdivisions;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace JOrders
{

    public partial class EditOrder : Form
    {
        public string Id = null;
        public string param = null;

        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;




        public EditOrder()
        {
            InitializeComponent();
        }

        private void EditOrder_Load(object sender, EventArgs e)
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


          //  textBox1.Text = $"ID:{Id}"; //client

            this.ActiveControl = textBox1;

            if (Id == null) AddFirstItems();

            AddTitle();

            AddrowsToDataGrid();
        }

        private void AddTitle()
        {
            getOrderDetailsModel(Id);


            foreach (JrOrderDetailsModel s in JrOrderDetailsModel.GetJrOrdersModel)
            {
                // data.Add(new JResultsAddEditModel(id: s.Id, dateChecks: s?.DateChecks.ToString(), numChecks: s.NumChecks, name: s.Name, surname: s.Surname, sex: s.Sex, email: s.Email));
                textBox1.Text = s.ClientName;

               // DateTime DataCheck = DateTime.ParseExact(s.DataCheck, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime DataCheck = DateTime.Parse(s.DataCheck);
                if (DataCheck != null) dateTimePicker1.Value = DataCheck;

                textBox3.Text = s.NumCheck;

                textBox4.Text = s.AgentName;
                textBox5.Text = s.OrgName;
                textBox10.Text = s.SubdivisionName;
                textBox11.Text = s.ManagerName;
                richTextBox1.Text = s.Descr;
                checkBox1.Checked = s.IsFiscal;
                textBox7.Text = s.SUM_Realiz;
                textBox8.Text = s.PAYED_SUM;
                //textBox15.Text = s.PLAN_DATE_DONE;
                //textBox13.Text = s.DATE_DONE;






            }
        }

        private void AddFirstItems()
        {
            DicClientsModelFormMain.idManager = "a223fbe2510a4a1a828486ae6275c793";
            DicClientsModelFormMain.NameManager = "Admin";

            DicClientsModelFormMain.idSubdivision = "1";
            DicClientsModelFormMain.NameSubdivision = "Лаборатория-регистратура";

            textBox3.Text = CreateNum();
            textBox10.Text = DicClientsModelFormMain.NameSubdivision;
            textBox11.Text = DicClientsModelFormMain.NameManager;

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
                data.Add(new JrEditCheckModel(id: s.Id, goods: s.Goods, categor: s.Categor, code: s.Code, unit: s.Unit, count: s.Count, priceBaz: Convert.ToDecimal(s.PriceBaz), price: Convert.ToDecimal(s.Price), priceReal: Convert.ToDecimal(s.PriceReal), employee: s.Employee));
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
                    JrEditCheckModel.AddJrOrdersModel(new JrEditCheckModel(id: reader.GetString(0), goods: reader.GetString(3), categor: reader.GetString(35), code: reader.GetString(4), unit: reader.GetString(6), count: reader.GetString(9), priceBaz: reader.GetDecimal(10), price: reader.GetDecimal(11), priceReal: reader.GetDecimal(13), employee: reader.GetString(19)));
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

        public void getOrderDetailsModel(string id = "")
        {

            FbConnection fb = GetConnection();
            FbCommand SelectSQL = new FbCommand("select first 1 jc.id, jc.date_time, jc.NUM, jc.SUBDIVISION_ID," +
                "jcd.check_subdivision_name, jc.CLIENT_ID, jcd.check_client_code_name, jc.MANAGER_ID," +
                "de.code_name, jc.AGENT_ID, jcd.CHECK_AGENT_CODE_NAME, jc.PAYER_ORG_ID, jcd.check_payer_org_code_name, jc.DESCR/*13*/," +
                "jc.SUM_BASE, jc.SUM_, jc.PAYED_SUM, jc.FISCAL_NUM, jc.is_fiscal " +
                "from jor_checks jc " +
                "join jor_checks_dt jcd on jcd.hd_id = jc.id " + 
                "join dic_employee de on de.id = jc.MANAGER_ID " +
                "where jc.id = cast(@paramId as ID)", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@paramId", value: id);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    JrOrderDetailsModel.AddJrOrdersModel(new JrOrderDetailsModel(id: reader.GetString(0), dataCheck: reader.GetString(1), numCheck: reader.GetString(2), subdivisionId: reader.GetString(3), subdivisionName: reader.GetString(4), clientId: reader.GetString(5), clientName: reader.GetString(6), managerId: reader.GetString(7), managerName: reader.GetString(8), agentId: reader?.GetString(9), agentName: reader?.GetString(10), orgId: reader?.GetString(11), orgName: reader?.GetString(12), descr: reader.GetString(13), sum_Base: reader.GetDecimal(14), sum_Realiz: reader.GetDecimal(15), pAYED_SUM: reader.GetDecimal(16), isFiscal: reader.GetBoolean(18), fiscalNum: reader?.GetString(17)));
                }
                //while (reader.Read())
                //{
                //    JrOrderDetailsModel.AddJrOrdersModel(new JrOrderDetailsModel(id: reader.GetString(0), dataCheck: reader.GetString(1), numCheck: reader.GetString(2), subdivisionId: reader.GetString(3), subdivisionName: reader.GetString(4), isFiscal: reader.GetBoolean(18), sum_Base: reader.GetDecimal(14), sum_Realiz: reader.GetDecimal(15), pAYED_SUM: reader.GetDecimal(16)));
                //}
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

        public void delJrCheckEditModel(string param = "")
        {
            //MessageBox.Show(string.Format("Вы выбрали период с {0} до {1}", from.ToLongDateString(), to.ToLongDateString()), "Информация");
            //File.AppendAllText(Application.StartupPath + @"\Event.log", string.Format("Вы выбрали период с {0} до {1}", from.ToLongDateString(), to.ToLongDateString()) + "\n");

            FbConnection fb = GetConnection();

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
                MessageBox.Show("error" + ex.Message);
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

            Print();
            
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void Print()
        {
            if (dataGridView1.CurrentCell?.RowIndex >= 0)
            {
                string y = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                PrintReport printForm = new PrintReport();
                printForm.IdCh = Id;
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


        private void Button3_Click(object sender, EventArgs e)
        {
            //FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
            //fb_con.Charset = "UTF8"; //используемая кодировка
            //fb_con.UserID = "SYSDBA"; //логин
            //fb_con.Password = "masterkey"; //пароль
            //fb_con.Database = path_db; //путь к файлу базы данных
            //                           // fb_con.Database = "127.0.0.1:terra"; //путь к файлу базы данных
            //fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))
            //FbConnection fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

            //

            string IdCheck = CreateID();

            if (IdCheck != null) WriteTitle(IdCheck);

            //
            if (IdCheck != null) WriteSpecification(IdCheck);
            Close();
        }

        private string CreateID()
        {
            string id = null;
            FbConnection fb = GetConnection();
            // fb.Open();

            string getId = "select first 1 U.UUID from GET_HEX_UUID U";
            FbCommand SelectID = new FbCommand(getId, fb);

            FbTransaction fbt = fb.BeginTransaction();
            SelectID.Transaction = fbt;
            FbDataReader reader = SelectID.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    id = reader.GetString(0);
                }
                return id;
            }
            catch (Exception ex)
            {
                 MessageBox.Show("error" + ex.Message);
                // fbt.Rollback();
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectID.Dispose();
                fb.Close();
            }
            return null;
        }

        private string CreateNum()
        {
            string num = null;
            FbConnection fb = GetConnection();
            // fb.Open();

            string getNum = "select gen_id(NUM_JOR_CHECKS, 0) from CFG_GLOBAL_OPTIONS";
            FbCommand SelectID = new FbCommand(getNum, fb);

            FbTransaction fbt = fb.BeginTransaction();
            SelectID.Transaction = fbt;
            FbDataReader reader = SelectID.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    num = reader.GetString(0);
                }
                return num;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
                // fbt.Rollback();
            }
            finally
            {
                fbt.Commit();
                reader.Close();
                SelectID.Dispose();
                fb.Close();
            }
            return null;
        }


        private void WriteTitle(string idCheck)
        {
            FbConnection fb = GetConnection();
            // fb.Open();

            #region Title
            string insertCmdStrTitle = "INSERT INTO JOR_CHECKS (ID,/*1*/ DATE_TIME,/*2*/NUM,/*3*/ADD_EMPLOYEE_ID,/*4*/SUBDIVISION_ID,/*5*/CLIENT_ID,/*6*/" +
                           "MANAGER_ID,/*7*/AGENT_ID,/*8*/PAYER_ORG_ID,/*9*/DESCR, /*10*/DESCR_PREVIEW,/*11*/IS_FISCAL,/*12*/AGREEMENT_DOC,/*13*/" +
                           "SUM_BASE, SUM_,/*14*/DISCONT_DESCR,/*15*/DISCONT_DESCR_PREVIEW,/*16*/DIAGNOSIS_ID,/*17*/MENSTRPHASE_ID," +
                           "/*18*/PREG_WEEK_FROM,/*19*/PREG_WEEK_TO,/*20*/LAST_MENSTR_DAY,/*21*/CYCLE_LENGTH,/*22*/AUTO_PRINT_DATE,/*23*/FISCAL_PRINT_TIME," +
                           "/*24*/DISCONTS_CARD,/*25*/MANUAL_DISC_TYPE,/*26*/MANUAL_TYPE_PRICE_ID,/*27*/MANUAL_SUM_PRC_IDX,/*28*/MANUAL_SUM_PRC_VALUE," +
                           "/*29*/TYPE_DONE_FOR_COLOR,/*30*/TYPE_EMAIL_STATUS_FOR_COLOR,/*31*/PAYED_SUM, /*32*/FISCAL_NUM/*33*/) " +
                           " VALUES(@idcheck,@date,@num,NULL,@idsubdiv,@idclient,@idemployee, NULL,NULL," +
                           "NULL,NULL,0,NULL,170,170,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,0,0,170,NULL);";

            FbCommand SelectSQLTiTle = new FbCommand(insertCmdStrTitle, fb);

            // SelectSQL.Parameters.Add("@num", FbDbType.Numeric);
            SelectSQLTiTle.Parameters.Add("@idcheck", FbDbType.Text);
            SelectSQLTiTle.Parameters.Add("@num", FbDbType.Text);
            SelectSQLTiTle.Parameters.Add("@date", FbDbType.TimeStamp);
            SelectSQLTiTle.Parameters.Add("@idclient", FbDbType.Text);
            SelectSQLTiTle.Parameters.Add("@idsubdiv", FbDbType.Text);
            SelectSQLTiTle.Parameters.Add("@idemployee", FbDbType.Text);

            SelectSQLTiTle.Parameters[0].Value = idCheck;
            SelectSQLTiTle.Parameters[1].Value = textBox3.Text == null ? "n/a" : textBox3.Text;
            SelectSQLTiTle.Parameters[2].Value = dateTimePicker1.Value;
            SelectSQLTiTle.Parameters[3].Value = DicClientsModelFormMain.idClients;
            SelectSQLTiTle.Parameters[4].Value = DicClientsModelFormMain.idSubdivision; //DicClientsModelFormMain
            SelectSQLTiTle.Parameters[5].Value = DicClientsModelFormMain.idManager; //DicClientsModelFormMain
            //SelectSQL.Parameters[2].Direction = ParameterDirection.ReturnValue;

            FbTransaction fbtTitle = fb.BeginTransaction();
            SelectSQLTiTle.Transaction = fbtTitle;

            #endregion

            try
            {
                int resultT = SelectSQLTiTle.ExecuteNonQuery();
                MessageBox.Show($"Add {resultT}");

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    ////More code here
                    //data.Add(new forEditChecksModel(id: row.Cells[0].Value.ToString(), name: row.Cells[1].Value.ToString()/*, secname: row.Cells[1].Value.ToString(), sex: row.Cells[1].Value.ToString(), birthdate: row.Cells[1].Value.ToString(), email: row.Cells[1].Value.ToString()*/));
                    MessageBox.Show($"Add {row.Cells[0].Value.ToString()}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
                fbtTitle.Rollback();
            }
            finally
            {
                fbtTitle.Commit();
                SelectSQLTiTle.Dispose();
                fb.Close();
            }
        }


        private void WriteSpecification(string idCheck)
        {
            FbConnection fb = GetConnection();

            #region Spec
            string insertCmdStrSpecific = "INSERT INTO JOR_CHECKS_DT (ID, HD_ID, CHECK_DATE, CHECK_NUM, CHECK_CLIENT_ID, CHECK_CLIENT_CODE_NAME, " +
                   "CHECK_SUBDIVISION_ID, CHECK_SUBDIVISION_NAME, CHECK_AGENT_ID, CHECK_AGENT_CODE_NAME, CHECK_PAYER_ORG_ID, CHECK_PAYER_ORG_CODE_NAME," +
                   "GOODS_ID, GOODS_NAME, GOODS_GRP_ID, GOODS_GRP_NAME, GOODS_ORDER, UNIT_ID, MATERIAL_ID, BARCODE_GEN_ID, BULB_NUM_ID, BLANK_ID," +
                   "BULB_NUM_CODE, IS_EAN8, IS_REMARKED_DATE_TIME, REMARKED_EMPLOYEE_ID, REMARKER_EMPLOYEE_CODE_NAME, CNT, PRICE_BASE, PRICE, " +
                   "SUM_BASE, SUM_, SUBDIVISION_EXEC_ID, " +
                   "SUBDIVISION_EXEC_NAME, ORG_EXEC_ID, ORG_EXEC_CODE_NAME," +
                    "PLAN_DATE_DONE, DATE_DONE, DATE_DONE_PREV, DONE_EMPLOYEE_ID, DONE_EMPLOYEE_CODE_NAME, CHECK_EMPLOYEE_ID, CHECK_EMPLOYEE_CODE_NAME, " +
                    "IS_URGENT, COMPLEX_ID," +
                    "IS_COMPLEX, MANIPULATION_ID, MANIPULATION_DATE_TIME, MANIPULATION_EMPLOYEE_ID, MANIPULATION_EMPLOYEE_CODE_NAME," +
                    "IS_MANIPULATION," +
                    "IS_REFUSE, REFUSE_PRINT_TIME," +
                    "DESCR, DESCR_PREVIEW, NEW_BULB_CODE, DATE_SEND, SUM_OUT, IMPORT_LAB_ID, DIC_NO_OPPORT_TO_RES_ID, DIC_NO_OPPORT_TO_RES_NAME," +
                    "DATE_ADD, RESULT_TEXT_PREVIEW," +
                    "LAB_PROCESS_ID, LAB_PROCESS_DATE_ADD, LAB_PROCESS_NUM, AUTO_PRINT_DATE)" +
                    "VALUES((select U.UUID from GET_HEX_UUID U), @idcheck," +
                    "@date, @num, @idclient, 'Верюхалова З. И.', @idsubdiv, 'Лаборатория - На дому'," +
                    "NULL, NULL, @idemployee, NULL, '29419', 'КАК+Тромбоциты', '200', 'Общеклинические исследования крови', NULL, NULL, NULL," +
                    "898, NULL, 'c84a047e46f640b598657ee8fa106d38', NULL, 0, NULL, NULL, NULL, 1, 125, 125, 125, 125, NULL, NULL, NULL, NULL," +
                    "NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL," +
                    "NULL, NULL, NULL, '9-JUL-2019 06:18:22', NULL, NULL, NULL, NULL, NULL); ";

            FbCommand SelectSQLSpecific = new FbCommand(insertCmdStrSpecific, fb);

            // SelectSQL.Parameters.Add("@num", FbDbType.Numeric);
            SelectSQLSpecific.Parameters.Add("@idcheck", FbDbType.Text);
            SelectSQLSpecific.Parameters.Add("@num", FbDbType.Text);
            SelectSQLSpecific.Parameters.Add("@date", FbDbType.TimeStamp);
            SelectSQLSpecific.Parameters.Add("@idclient", FbDbType.Text);
            SelectSQLSpecific.Parameters.Add("@idsubdiv", FbDbType.Text);
            SelectSQLSpecific.Parameters.Add("@idemployee", FbDbType.Text);

            SelectSQLSpecific.Parameters[0].Value = idCheck;
            SelectSQLSpecific.Parameters[1].Value = textBox3.Text == null ? "n/a" : textBox3.Text;
            SelectSQLSpecific.Parameters[2].Value = dateTimePicker1.Value;
            SelectSQLSpecific.Parameters[3].Value = DicClientsModelFormMain.idClients;
            SelectSQLSpecific.Parameters[4].Value = DicClientsModelFormMain.idSubdivision; //DicClientsModelFormMain
            SelectSQLSpecific.Parameters[5].Value = DicClientsModelFormMain.idManager; //DicClientsModelFormMain
                                                                                       //SelectSQL.Parameters[2].Direction = ParameterDirection.ReturnValue;

            FbTransaction fbtSpecif = fb.BeginTransaction();
            SelectSQLSpecific.Transaction = fbtSpecif;

            #endregion
            try
            {
                int resultT = SelectSQLSpecific.ExecuteNonQuery();
                //  int resultS = SelectSQLSpecific.ExecuteNonQuery();

                MessageBox.Show($"Add {resultT}");
                // MessageBox.Show($"Add {resultS}");
                //
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    ////More code here
                    //data.Add(new forEditChecksModel(id: row.Cells[0].Value.ToString(), name: row.Cells[1].Value.ToString()/*, secname: row.Cells[1].Value.ToString(), sex: row.Cells[1].Value.ToString(), birthdate: row.Cells[1].Value.ToString(), email: row.Cells[1].Value.ToString()*/));
                    MessageBox.Show($"Add {row.Cells[0].Value.ToString()}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error" + ex.Message);
                fbtSpecif.Rollback();
                //  fbtSpecif.Rollback();
            }
            finally
            {
                fbtSpecif.Commit();
                // fbtSpecif.Commit();
                SelectSQLSpecific.Dispose();
                //  SelectSQLSpecific.Dispose();
                fb.Close();


            }
        }



        void DicClientsAdd(forAddDicClientsModel testModel)
        {
            textBox1.Text = testModel.CodeName.ToString();
            DicClientsModelFormMain.idClients = testModel.Id;
            DicClientsModelFormMain.codeName = testModel.CodeName;
        }

        void DicEmployeeуAdd(forAddDicEmployeeModel testModel)
        {
            textBox11.Text = testModel.CodeName.ToString();
            DicClientsModelFormMain.idManager = testModel.Id;
            DicClientsModelFormMain.NameManager = testModel.CodeName;
        }

        void DicSubdivAdd(forAddDicSubDivModel testModel)
        {
            textBox10.Text = testModel.Name.ToString();
            DicClientsModelFormMain.idSubdivision = testModel.Id;
            DicClientsModelFormMain.NameSubdivision = testModel.Name;
        }

        void DicAgentAdd(forAddDicAgentModel testModel)
        {
            textBox4.Text = testModel.CodeName.ToString();
            DicClientsModelFormMain.idAgent = testModel.Id;
            DicClientsModelFormMain.NameAgent = testModel.CodeName;
        }

        void DicOrgAdd(forAddDicOrgModel testModel)
        {
            textBox5.Text = testModel.CodeName.ToString();
            DicClientsModelFormMain.idOrg = testModel.Id;
            DicClientsModelFormMain.NameOrg = testModel.CodeName;
        }


        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (/*e.KeyCode == Keys.Enter ||*/ e.KeyCode == Keys.F2)
            {
                dicClients f2 = new dicClients();
                f2.MyLabelClicked += new dicClients.MyLabelClickedHandler(DicClientsAdd);
                f2.Param = textBox1.Text;
                f2.ShowDialog();
            }
        }

        /// <summary>
        /// клиенты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button10_Click(object sender, EventArgs e)
        {
            dicClients f2 = new dicClients();
            f2.MyLabelClicked += new dicClients.MyLabelClickedHandler(DicClientsAdd);
            f2.ShowDialog();
        }

        /// <summary>
        /// агент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button13_Click(object sender, EventArgs e)
        {
            DicAgentForm f2 = new DicAgentForm();
            f2.MyLabelClicked += new DicAgentForm.MyLabelClickedHandler(DicAgentAdd);
            f2.ShowDialog();
        }

        /// <summary>
        /// менеджер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button19_Click(object sender, EventArgs e)
        {
            DicEmployee.DicEmployee dEm = new DicEmployee.DicEmployee();
            dEm.MyLabelClicked += new DicEmployee.DicEmployee.MyLabelClickedHandler(DicEmployeeуAdd);
            dEm.ShowDialog();
        }

        /// <summary>
        /// Subdiv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button17_Click(object sender, EventArgs e)
        {
            DicSubdivisionsForm dEm = new DicSubdivisionsForm();
            dEm.MyLabelClicked += new DicSubdivisionsForm.MyLabelClickedHandler(DicSubdivAdd);
            dEm.ShowDialog();
        }

        /// <summary>
        /// Org
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button15_Click(object sender, EventArgs e)
        {
            DicOrgForm dEm = new DicOrgForm();
            dEm.MyLabelClicked += new DicOrgForm.MyLabelClickedHandler(DicOrgAdd);
            dEm.ShowDialog();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            textBox10.Text = "";
        }

        private void Button18_Click(object sender, EventArgs e)
        {
            textBox11.Text = "";
        }

       
    }
}
