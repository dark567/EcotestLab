﻿//using DG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DClients
{
    public class DcClientsModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [DisplayName("Фамилия")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Secname { get; set; }

        [DisplayName("Имя")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Name { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Отчество")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Surname { get; set; }

        [DisplayName("CodeName")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string CodeName { get; set; }

        [DisplayName("Пол")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Sex { get; set; }

        [DisplayName("Дата рождения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Birthdate {
            get;
            //{
            //    if (Birthdate == null)
            //        Birthdate = "";
            //    else if (Birthdate != null)
            //        Birthdate = Birthdate.Remove(3);
            //    return Birthdate;
            //}
            set;
        }

        [DisplayName("Почта")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Email { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<DcClientsModel> _dicClientsModel;
        private Func<string> birthdate;
        private DateTime? birthdate1;

        static DcClientsModel()
        {
            _dicClientsModel = new List<DcClientsModel>();
        }



        public DcClientsModel(string id, string surname, string name, string secname, string codeName, string sex, string birthdate = "", string email = null)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Secname = secname;
            CodeName = codeName;
            Sex = sex;
            Birthdate = birthdate = birthdate == "" ? "": (DateTime.Parse(birthdate).ToShortDateString()).ToString();
            Email = email;
        }

        public static DcClientsModel[] GetDcClientsModel
        {
            get
            {
                return DcClientsModel._dicClientsModel.ToArray();
            }
        }

        public static void AddDcClientsModel(DcClientsModel _dcClientsModel)
        {
            DcClientsModel._dicClientsModel.Add(_dcClientsModel);
        }

        public static void ClearDcClientsModel()
        {
            DcClientsModel._dicClientsModel.Clear();
        }
    }
}
