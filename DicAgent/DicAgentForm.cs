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

namespace DicAgent
{
    public partial class DicAgentForm : Form
    {

        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public string Param = "";

        public delegate void MyLabelClickedHandler(forAddDicAgentModel testModel);
        public event MyLabelClickedHandler MyLabelClicked;

        public DicAgentForm()
        {
            InitializeComponent();
        }

        private void DicAgentForm_Load(object sender, EventArgs e)
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

            DcAgentModel.ClearDcClientsModel();

            getNomDicGoodsID(Param);

            SortableBindingList<DcAgentModel> data = new SortableBindingList<DcAgentModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcAgentModel s in DcAgentModel.GetDcClientsModel)
            {
                data.Add(new DcAgentModel(id: s.Id, surname: s.Surname, name: s.Name, secname: s.Secname, codeName: s.CodeName, org: s.Org));

                secnameFIO.Text = Param;
            }
            dataGridView1.DataSource = data;
        }

        public static void getNomDicGoodsID(string SURNAME = null)
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
            if (SURNAME == null || SURNAME == "")
                SelectSQL = new FbCommand("select first 1000 S.ID, S.SURNAME, S.NAME, S.SECNAME, S.CODE_NAME, S.SEX, S.SEX as SEX_NAME, " +
                    "S.CELLPHONE, S.EMAIL, S.DESCR, S.DESCR_PREVIEW, S.IS_SEND_EMAIL, S.LOGIN, " +
                    "S.PASS, S.TYPE_BONUSES_ID, " +
                    "B.NAME as TYPE_BONUS, S.BONUS_PRC, S.DEPARTMENT_ID, D.VAL_IDX as DEPART, " +
                    " S.JOB_TITLE_ID, J.NAME as JOB_TITLE, " +
                    "S.MANAGER_ID, E.CODE_NAME as MANAGER, S.MANAGER_PRC, " +
                    " S.ORG_ID, O.CODE_NAME as ORG " +
                    /*ADDCOLUMNS*/
                    "from DIC_CLIENTS S " +
                    "left join DIC_DICS D on D.ID = S.DEPARTMENT_ID " +
                    "left join DIC_TYPE_PRICES TP on TP.ID = S.TYPE_PRICE_ID " +
                    "left join DIC_JOB_TITLES J on J.ID = S.JOB_TITLE_ID " +
                    "left join DIC_EMPLOYEE E on E.ID = S.MANAGER_ID " +
                    "left join DIC_TYPE_BONUSES B on B.ID = S.TYPE_BONUSES_ID " +
                    "left join DIC_ORG O on O.ID = S.ORG_ID " +
                    "where S.ORG_ID is not null", fb);
            else SelectSQL = new FbCommand("select first 1000 S.ID, S.SURNAME, S.NAME, S.SECNAME, S.CODE_NAME, S.SEX, S.SEX as SEX_NAME, " +
                    "S.CELLPHONE, S.EMAIL, S.DESCR, S.DESCR_PREVIEW, S.IS_SEND_EMAIL, S.LOGIN, " +
                    "S.PASS, S.TYPE_BONUSES_ID, " +
                    "B.NAME as TYPE_BONUS, S.BONUS_PRC, S.DEPARTMENT_ID, D.VAL_IDX as DEPART, " +
                    " S.JOB_TITLE_ID, J.NAME as JOB_TITLE, " +
                    "S.MANAGER_ID, E.CODE_NAME as MANAGER, S.MANAGER_PRC, " +
                    " S.ORG_ID, O.CODE_NAME as ORG " +
                    /*ADDCOLUMNS*/
                    "from DIC_CLIENTS S " +
                    "left join DIC_DICS D on D.ID = S.DEPARTMENT_ID " +
                    "left join DIC_TYPE_PRICES TP on TP.ID = S.TYPE_PRICE_ID " +
                    "left join DIC_JOB_TITLES J on J.ID = S.JOB_TITLE_ID " +
                    "left join DIC_EMPLOYEE E on E.ID = S.MANAGER_ID " +
                    "left join DIC_TYPE_BONUSES B on B.ID = S.TYPE_BONUSES_ID " +
                    "left join DIC_ORG O on O.ID = S.ORG_ID " +
                    "where S.ORG_ID is not null and UPPER(S.NAME) LIKE UPPER(@param)", fb);

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
                    DcAgentModel.AddDcClientsModel(new DcAgentModel(id: reader?.GetString(0), surname: reader.GetString(1), name: reader.GetString(2), secname: reader.GetString(3), codeName: reader.GetString(4), org: reader.GetString(25)));
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

        private void Button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            DcAgentModel.ClearDcClientsModel();

            getNomDicGoodsID(secnameFIO.Text);


            BindingList<DcAgentModel> data = new BindingList<DcAgentModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcAgentModel s in DcAgentModel.GetDcClientsModel)
            {
                data.Add(new DcAgentModel(id: s.Id, surname: s.Surname, name: s.Name, secname: s.Secname, codeName: s.CodeName, org: s.Org));
            }

            dataGridView1.DataSource = data;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1?.CurrentCell?.RowIndex != null)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                // string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4]?.Value.ToString();

                forAddDicAgentModel.AddjrTestModel(new forAddDicAgentModel(id: id, codeName: name));
            }

            foreach (forAddDicAgentModel s in forAddDicAgentModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }

        private void MacButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1?.CurrentCell?.RowIndex != null)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                //string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4]?.Value.ToString();

                forAddDicAgentModel.AddjrTestModel(new forAddDicAgentModel(id: id, codeName: name));
            }

            foreach (forAddDicAgentModel s in forAddDicAgentModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Get the property object based on the DataPropertyName of the column
            var property = typeof(DcAgentModel).GetProperty(e.Column.DataPropertyName);
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
    }
}
