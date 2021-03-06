﻿using DG;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DClients
{
    public partial class dicClients : Form
    {
        public readonly string fileIniPath = Application.StartupPath + @"\set.ini";
        public static string path_db;
        public string user;
        public string pass;

        public string Param = "";

        public delegate void MyLabelClickedHandler(forAddDicClientsModel testModel);
        public event MyLabelClickedHandler MyLabelClicked;

        public dicClients()
        {
            InitializeComponent();
            Load += new EventHandler(Form1_Load);
        }

        void Form1_Load(object sender, EventArgs e)
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

            DcClientsModel.ClearDcClientsModel();
            getNomDicGoodsID(Param);

            SortableBindingList<DcClientsModel> data = new SortableBindingList<DcClientsModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcClientsModel s in DcClientsModel.GetDcClientsModel)
            {
                data.Add(new DcClientsModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, codeName: s.CodeName, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));

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
            if (SURNAME == null || SURNAME == "") SelectSQL = new FbCommand("SELECT first 100 Id, SURNAME, Name, SECNAME, code_name, SEX, BIRTH_DATE, EMAIL FROM dic_clients ORDER BY SURNAME", fb);
            else SelectSQL = new FbCommand("SELECT Id, SURNAME, Name, SECNAME, code_name, SEX, BIRTH_DATE, EMAIL FROM dic_clients where UPPER(CODE_NAME) LIKE UPPER(@param)", fb);
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
                    DcClientsModel.AddDcClientsModel(new DcClientsModel(id: reader?.GetString(0), surname: reader?.GetString(1), name: reader?.GetString(2), secname: reader?.GetString(3), codeName: reader?.GetString(4), sex: reader.GetString(5), birthdate: reader?.GetString(6), email: reader?.GetString(7)));
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


        private void DicClients_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void Button1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button1.ClientRectangle,
            SystemColors.ControlLightLight, 1, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 1, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 3, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 3, ButtonBorderStyle.Outset);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            dataGridView1.AllowUserToAddRows = false; //запрешаем пользователю самому добавлять строки
            dataGridView1.AllowUserToResizeColumns = true;

            DcClientsModel.ClearDcClientsModel();
            getNomDicGoodsID(secnameFIO.Text);

            BindingList<DcClientsModel> data = new BindingList<DcClientsModel>(); //Специальный список List с вызовом события обновления внутреннего состояния, необходимого для автообновления datagridview

            foreach (DcClientsModel s in DcClientsModel.GetDcClientsModel)
            {
                data.Add(new DcClientsModel(id: s.Id, surname: s.Secname, name: s.Name, secname: s.Surname, codeName: s.CodeName, sex: s.Sex, birthdate: s?.Birthdate.ToString(), email: s.Email));
            }

            dataGridView1.DataSource = data;
        }

        private void SecnameFIO_KeyUp(object sender, KeyEventArgs e)
        {
            if (/*e.KeyCode == Keys.Enter ||*/ e.KeyCode == Keys.F2)
            {
                Button1_Click(sender, e);
            }
        }

        private void DataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
           
                // Get the property object based on the DataPropertyName of the column
                var property = typeof(DcClientsModel).GetProperty(e.Column.DataPropertyName);
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

                //var TypeIsService = (TypesIServiceAttribute)property?.GetCustomAttribute(typeof(TypesIServiceAttribute));
                //if (TypeIsService != null)
                //{
                //    // Finally, set the FillWeight of the column to our defined weight in the attribute
                //    if (TypeIsService.typesIServiceAttribute)
                //    {
                //        AddOutOfOfficeColumn();
                //    }
                //}
                // base.DataGridView1_ColumnAdded(sender, e);

            
        }

        private void MacButton1_Click(object sender, EventArgs e)
        {

            if (dataGridView1?.CurrentCell?.RowIndex != null)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4]?.Value.ToString();

                forAddDicClientsModel.AddjrTestModel(new forAddDicClientsModel(id: id, name: name, codeName: codename));
            }

            foreach (forAddDicClientsModel s in forAddDicClientsModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
        }

        private void DataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            //if (dataGridView1?.CurrentCell?.RowIndex != null)
            //{
            //    //string output = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2]?.Value.ToString();

            //    string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
            //    string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
            //    string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2]?.Value.ToString();
            //    //string y = dataGridView1.CurrentCell.RowIndex.ToString();

            //    //richTextBox1.Text = y;
            //    //richTextBox1.AppendText("\r\n" + y);
            //    //richTextBox1.ScrollToCaret();

            //    //TestModel testModel = new TestModel(id, name);

            //    forAddDicClientsModel.AddjrTestModel(new forAddDicClientsModel(id: id, name: name, codeName: codename));

              

            //    // MyLabelClicked?.Invoke(testModel);
            //}
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1?.CurrentCell?.RowIndex != null)
            {
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0]?.Value.ToString();
                string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1]?.Value.ToString();
                string codename = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4]?.Value.ToString();

                forAddDicClientsModel.AddjrTestModel(new forAddDicClientsModel(id: id, name: name, codeName: codename));
            }

            foreach (forAddDicClientsModel s in forAddDicClientsModel.GetjrTestModel)
            {
                MyLabelClicked?.Invoke(s);
            }

            Close();
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
