using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicEmployee
{
    public class DcEmployeeModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Фамилия")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Secname { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Имя")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Name { get; set; } //обязательно нужно использовать get конструкцию

        [VisibleTypes(true)]
        [DisplayName("Отчество")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Surname { get; set; }

        [VisibleTypes(false)]
        [DisplayName("CodeName")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string CodeName { get; set; }

        //[DisplayName("Пол")]
        //[ColumnWeight(10)] //todo
        //[AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        ////[TypesIService(true)] //todo
        //public string Sex { get; set; }

        //[DisplayName("Дата рождения")]
        //[ColumnWeight(10)] //todo
        //[AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        ////[TypesIService(false)] //todo
        //public string Birthdate
        //{
        //    get;
        //    //{
        //    //    if (Birthdate == null)
        //    //        Birthdate = "";
        //    //    else if (Birthdate != null)
        //    //        Birthdate = Birthdate.Remove(3);
        //    //    return Birthdate;
        //    //}
        //    set;
        //}

        //[DisplayName("Почта")]
        //[ColumnWeight(10)] //todo
        //[AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        ////[TypesIService(false)] //todo
       // public string Email { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<DcEmployeeModel> _dicClientsModel;
        //private Func<string> birthdate;
        //private DateTime? birthdate1;

        static DcEmployeeModel()
        {
            _dicClientsModel = new List<DcEmployeeModel>();
        }



        public DcEmployeeModel(string id, string surname, string name, string secname, string codeName)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Secname = secname;
            CodeName = codeName;
            //Sex = sex;
            //Birthdate = birthdate = birthdate == "" ? "" : (DateTime.Parse(birthdate).ToShortDateString()).ToString();
            //Email = email;
        }

        public static DcEmployeeModel[] GetDcClientsModel
        {
            get
            {
                return DcEmployeeModel._dicClientsModel.ToArray();
            }
        }

        public static void AddDcClientsModel(DcEmployeeModel _dcClientsModel)
        {
            DcEmployeeModel._dicClientsModel.Add(_dcClientsModel);
        }

        public static void ClearDcClientsModel()
        {
            DcEmployeeModel._dicClientsModel.Clear();
        }
    }
}
