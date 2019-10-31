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

namespace DicOrg
{
    public partial class DicOrgForm : Form
    {


        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public string Param = "";

        public delegate void MyLabelClickedHandler(forAddDicOrgModel testModel);
        public event MyLabelClickedHandler MyLabelClicked;

        public DicOrgForm()
        {
            InitializeComponent();
        }

        private void MacButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DicOrgForm_Load(object sender, EventArgs e)
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

            DcOrgModel.ClearDcClientsModel();

            getNomDicGoodsID(Param);

            SortableBindingList<DcOrgModel> data = new SortableBindingList<DcOrgModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcOrgModel s in DcOrgModel.GetDcClientsModel)
            {
                data.Add(new DcOrgModel(id: s.Id, shortName: s.ShortName, name: s.Name, egrrpou: s.Egrrpou));

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
                SelectSQL = new FbCommand("select first 1000 S.ID, S.CODE_NAME, S.NAME, S.EGRPOU, S.PHONE, S.FAX, S.ADDRESS," +
                    " S.BANK_ACC, S.BANK_MFO, S.BANK_NAME, S.EMAIL, " +
                    "S.WWW, S.DESCR, S.DESCR_PREVIEW, S.IS_FISCAL, S.LOGIN, S.PASS, S.EMPLOYEE_ID, E.CODE_NAME as EMPL, " +
                    "S.TYPE_PRICE_ID, TP.NAME as TYPE_PRICE, S.TYPE_BONUSES_ID, TB.NAME as TYPE_BON, S.BONUS_PRC, S.CREDIT_LIMIT, " +
                    "S.SYTE_SYNC_NEEDED, S.LOGO, S.LICENSE, S.TYPE_ORG_ID, cast(DD.VAL as NAME) as TYPE_ORG, S.CASH_PAY_TYPE_ID, " +
                    "CP.NAME as CASH_PAY_TYPE, S.CITY_ID, CI.VAL_IDX as CITY, S.ADD_MARK_1 " +
                    /*ADDCOLUMNS*/
                    "from DIC_ORG S " +
                    "left join DIC_EMPLOYEE E on S.EMPLOYEE_ID = E.ID " +
                    "left join DIC_TYPE_PRICES TP on S.TYPE_PRICE_ID = TP.ID " +
                    "left join DIC_TYPE_BONUSES TB on S.TYPE_BONUSES_ID = TP.ID " +
                    "left join DIC_DICS DD on S.TYPE_ORG_ID = DD.ID " +
                    "left join DIC_CASH_PAYS_TYPES CP on CP.ID = S.CASH_PAY_TYPE_ID " +
                    "left join DIC_DICS CI on S.CITY_ID = CI.ID", fb);
            else SelectSQL = new FbCommand("select first 1000 S.ID, S.CODE_NAME, S.NAME, S.EGRPOU, S.PHONE, S.FAX, S.ADDRESS," +
                    " S.BANK_ACC, S.BANK_MFO, S.BANK_NAME, S.EMAIL, " +
                    "S.WWW, S.DESCR, S.DESCR_PREVIEW, S.IS_FISCAL, S.LOGIN, S.PASS, S.EMPLOYEE_ID, E.CODE_NAME as EMPL, " +
                    "S.TYPE_PRICE_ID, TP.NAME as TYPE_PRICE, S.TYPE_BONUSES_ID, TB.NAME as TYPE_BON, S.BONUS_PRC, S.CREDIT_LIMIT, " +
                    "S.SYTE_SYNC_NEEDED, S.LOGO, S.LICENSE, S.TYPE_ORG_ID, cast(DD.VAL as NAME) as TYPE_ORG, S.CASH_PAY_TYPE_ID, " +
                    "CP.NAME as CASH_PAY_TYPE, S.CITY_ID, CI.VAL_IDX as CITY, S.ADD_MARK_1 " +
                    /*ADDCOLUMNS*/
                    "from DIC_ORG S " +
                    "left join DIC_EMPLOYEE E on S.EMPLOYEE_ID = E.ID " +
                    "left join DIC_TYPE_PRICES TP on S.TYPE_PRICE_ID = TP.ID " +
                    "left join DIC_TYPE_BONUSES TB on S.TYPE_BONUSES_ID = TP.ID " +
                    "left join DIC_DICS DD on S.TYPE_ORG_ID = DD.ID " +
                    "left join DIC_CASH_PAYS_TYPES CP on CP.ID = S.CASH_PAY_TYPE_ID " +
                    "left join DIC_DICS CI on S.CITY_ID = CI.ID " +
                    "where UPPER(S.NAME) LIKE UPPER(@param)", fb);

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
                    DcOrgModel.AddDcClientsModel(new DcOrgModel(id: reader?.GetString(0), shortName: reader.GetString(1), name: reader.GetString(2), egrrpou: reader.GetString(3)));
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

            DcOrgModel.ClearDcClientsModel();

            getNomDicGoodsID(secnameFIO.Text);

            BindingList<DcOrgModel> data = new BindingList<DcOrgModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcOrgModel s in DcOrgModel.GetDcClientsModel)
            {
                data.Add(new DcOrgModel(id: s.Id, shortName: s.ShortName, name: s.Name, egrrpou: s.Egrrpou));
            }

            dataGridView1.DataSource = data;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1?.CurrentCell?.RowIndex != null)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                //string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                 string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();

                forAddDicOrgModel.AddjrTestModel(new forAddDicOrgModel(id: id, codeName: codename));
            }

            foreach (forAddDicOrgModel s in forAddDicOrgModel.GetjrTestModel)
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
                //string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();

                forAddDicOrgModel.AddjrTestModel(new forAddDicOrgModel(id: id, codeName: codename));
            }

            foreach (forAddDicOrgModel s in forAddDicOrgModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Get the property object based on the DataPropertyName of the column
            var property = typeof(DcOrgModel).GetProperty(e.Column.DataPropertyName);
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
