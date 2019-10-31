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

namespace DicSubdivisions
{
    public partial class DicSubdivisionsForm : Form
    {

        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public string Param = "";

        public delegate void MyLabelClickedHandler(forAddDicSubDivModel testModel);
        public event MyLabelClickedHandler MyLabelClicked;

        public DicSubdivisionsForm()
        {
            InitializeComponent();
        }

        private void DicSubdivisionsForm_Load(object sender, EventArgs e)
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

            DcSubdivModel.ClearDcClientsModel();

            getNomDicGoodsID(Param);

            SortableBindingList<DcSubdivModel> data = new SortableBindingList<DcSubdivModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcSubdivModel s in DcSubdivModel.GetDcClientsModel)
            {
                data.Add(new DcSubdivModel(id: s.Id, name: s.Name, orgRecep: s.OrgRecep, orgFin: s.OrgFin, orgScl: s.OrgScl, orgLab:s.OrgLab));

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
            if (SURNAME == null || SURNAME == "") SelectSQL = new FbCommand("select ID, NAME, IS_RECEPT_SUBDIVISION, IS_CASH_SUBDIVISION, IS_GOOD_SUBDIVISION, IS_LAB_SUBDIVISION from dic_subdivisions ORDER BY NAME", fb);
            else SelectSQL = new FbCommand("select ID, NAME, IS_RECEPT_SUBDIVISION, IS_CASH_SUBDIVISION, IS_GOOD_SUBDIVISION, IS_LAB_SUBDIVISION from dic_subdivisions where UPPER(NAME) LIKE UPPER(@param)", fb);
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
                    DcSubdivModel.AddDcClientsModel(new DcSubdivModel(id: reader?.GetString(0), name: reader.GetString(1), orgRecep: reader.GetBoolean(2), orgFin: reader.GetBoolean(3), orgScl: reader.GetBoolean(4), orgLab: reader.GetBoolean(4)));
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

            DcSubdivModel.ClearDcClientsModel();

            getNomDicGoodsID(secnameFIO.Text);


            BindingList<DcSubdivModel> data = new BindingList<DcSubdivModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcSubdivModel s in DcSubdivModel.GetDcClientsModel)
            {
                data.Add(new DcSubdivModel(id: s.Id, name: s.Name, orgRecep: s.OrgRecep, orgFin: s.OrgFin, orgScl: s.OrgScl, orgLab: s.OrgLab));
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

                forAddDicSubDivModel.AddjrTestModel(new forAddDicSubDivModel(id: id, name: name));
            }

            foreach (forAddDicSubDivModel s in forAddDicSubDivModel.GetjrTestModel)
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

                forAddDicSubDivModel.AddjrTestModel(new forAddDicSubDivModel(id: id, name: name));
            }

            foreach (forAddDicSubDivModel s in forAddDicSubDivModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            // Get the property object based on the DataPropertyName of the column
            var property = typeof(DcSubdivModel).GetProperty(e.Column.DataPropertyName);
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
    }
}
