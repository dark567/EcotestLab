using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JResultsAdd
{
    public class JrResultsChildModel
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

        [DisplayName("Пол")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Sex { get; set; }

        [DisplayName("Дата рождения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Birthdate { get; set; }

        [DisplayName("Почта")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Email { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<JrResultsChildModel> _jrOrdersChildModel;

        static JrResultsChildModel()
        {
            _jrOrdersChildModel = new List<JrResultsChildModel>();
        }

        public JrResultsChildModel(string id, string surname, string name, string secname, string sex, string birthdate = "", string email = null)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Secname = secname;
            Sex = sex;
            Birthdate = birthdate;
            Email = email;
        }

        public static JrResultsChildModel[] GetJrOrdersChildModel
        {
            get
            {
                return JrResultsChildModel._jrOrdersChildModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrResultsChildModel jrOrdersChildModel)
        {
            JrResultsChildModel._jrOrdersChildModel.Add(jrOrdersChildModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrResultsChildModel._jrOrdersChildModel.Clear();
        }
    }
}
