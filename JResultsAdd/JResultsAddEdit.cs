using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JResultsAdd
{
    public partial class JResultsAddEdit : Form
    {
        public string Id = "n/a";
       // public string Id = "f7e38f5b0fd24330be3b4d5a8bfffabf";
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;

        public JResultsAddEdit()
        {
            InitializeComponent();
        }

        private void JResultsAddEdit_Load(object sender, EventArgs e)
        {
            textBox1.Text = $"ID:{Id}";

            //dateTimePicker1.Format = DateTimePickerFormat.Custom;
            //dateTimePicker1.CustomFormat = " ";
            //dateTimePicker2.Format = DateTimePickerFormat.Custom;
            //dateTimePicker2.CustomFormat = " ";

            //dateTimePicker1.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            //dateTimePicker2.CustomFormat = "dd-MM-yyyy HH:mm:ss";

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

            getJrOrdersModel(Id);


            foreach (JResultsAddEditModel s in JResultsAddEditModel.GetJrOrdersModel)
            {
                // data.Add(new JResultsAddEditModel(id: s.Id, dateChecks: s?.DateChecks.ToString(), numChecks: s.NumChecks, name: s.Name, surname: s.Surname, sex: s.Sex, email: s.Email));
                textBox1.Text = s.DataDoc;
                textBox2.Text = s.NomerDoc;
                textBox3.Text = s.ShkProb;
                textBox4.Text = s.TypeAnaliz;
                textBox5.Text = s.GrpAnaliz;
                textBox6.Text = s.CodeName;
                //textBox15.Text = s.PLAN_DATE_DONE;
                //textBox13.Text = s.DATE_DONE;

                if (s.PLAN_DATE_DONE != null && s.PLAN_DATE_DONE != "")
                {
                   // dateTimePicker1.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                    DateTime PLAN_DATE_DONE = DateTime.ParseExact(s.PLAN_DATE_DONE, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                   // if (PLAN_DATE_DONE != null) dateTimePicker1.Value = PLAN_DATE_DONE;
                    textBox12.Text = PLAN_DATE_DONE.ToString();
                }
                if (s.DATE_DONE != null && s.DATE_DONE != "")
                {
                    //dateTimePicker2.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                    DateTime DATE_DONE = DateTime.ParseExact(s.DATE_DONE, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                   // if (DATE_DONE != null) dateTimePicker1.Value = DATE_DONE;
                    textBox14.Text = DATE_DONE.ToString();
                }

                //if (s.PLAN_DATE_DONE != null || s.PLAN_DATE_DONE != "") dateTimePicker1.Value = DateTime.ParseExact(s.PLAN_DATE_DONE, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                //if (s.DATE_DONE != null || s.DATE_DONE != "") dateTimePicker2.Value = DateTime.ParseExact(s.DATE_DONE, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);


            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            foreach (JResultsAddEditModel s in JResultsAddEditModel.GetJrOrdersModel)
            {
                //if (dateTimePicker1.Value == DateTime.Parse("01.01.1753") && s.DATE_DONE != null)
                //{
                //    MessageBox.Show("null");
                //    delJrCheckEditModel(Id);
                //}
                //else MessageBox.Show($"not null {dateTimePicker1.Value}");
                if (s.DATE_DONE != "" && textBox14.Text == "")
                {
                    delJrCheckEditModel(Id);
                }
                
            }
            Close();
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
            //FbCommand SelectSQL = new FbCommand("delete from JOR_CHECKS where id = cast(@param as ID)", fb);
            FbCommand SelectSQL = new FbCommand("update JOR_CHECKS_DT set date_done = null where id = cast(@param as ID)", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", value: param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            try
            {
                SelectSQL.ExecuteNonQuery();
                MessageBox.Show("update dateDone");
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


        public static void getJrOrdersModel(string param = null)
        {
            // param = "f7e38f5b0fd24330be3b4d5a8bfffabf";

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
                                         " where D.ID = cast(@param as ID)", fb);

            //add one IN parameter                     
            FbParameter nameParam = new FbParameter("@param", param);
            // добавляем параметр к команде
            SelectSQL.Parameters.Add(nameParam);

            FbTransaction fbt = fb.BeginTransaction();
            SelectSQL.Transaction = fbt;
            FbDataReader reader = SelectSQL.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    JResultsAddEditModel.AddJrOrdersModel(new JResultsAddEditModel(id: reader.GetString(0), dataDoc: reader.GetString(9), nomerDoc: reader.GetString(3), shkProb: reader.GetString(4), typeAnaliz: reader.GetString(6), grpAnaliz: reader.GetString(8), codeName: reader.GetString(11), pLAN_DATE_DONE: reader.GetString(21), dATE_DONE: reader.GetString(20)));
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

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           // dateTimePicker1.CustomFormat = "dd-MM-yyyy HH:mm:ss";
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
           // dateTimePicker2.CustomFormat = "dd-MM-yyyy HH:mm:ss";
        }
    }
}
