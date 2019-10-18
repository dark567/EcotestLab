using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JOrders
{
    public class JrEditCheckModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [DisplayName("Дата")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DateChecks { get; set; }

        [DisplayName("Номер")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string NumChecks { get; set; }

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



        [DisplayName("Почта")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Email { get; set; }



        public static List<JrEditCheckModel> _jrEditCheckModel;

        static JrEditCheckModel()
        {
            _jrEditCheckModel = new List<JrEditCheckModel>();
        }

        public JrEditCheckModel(string id, string surname, string name, string numChecks, string sex, string dateChecks = "", string email = null)
        {
            Id = id;
            Surname = surname;
            Name = name;
            NumChecks = numChecks;
            Sex = sex;
            DateChecks = dateChecks;
            Email = email;
        }

        public static JrEditCheckModel[] GetJrEditCheckModel
        {
            get
            {
                return JrEditCheckModel._jrEditCheckModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrEditCheckModel jrEditCheckModel)
        {
            JrEditCheckModel._jrEditCheckModel.Add(jrEditCheckModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrEditCheckModel._jrEditCheckModel.Clear();
        }
    }
}
