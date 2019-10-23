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

        [DisplayName("Номер")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Nomer { get; set; }

        [DisplayName("Клиент")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string CodeName { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Ш/к пробирки")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string ShkProb { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Анализ")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Goods { get; set; }

        [DisplayName("Подразделение исполнитель")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string PodrIsp { get; set; }

        [DisplayName("Подразделение рецепции")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PodrRec { get; set; }

        [DisplayName("Плановая дата выполнения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DatePlan { get; set; }

        [DisplayName("Дата выполнения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DateDone { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<JrResultsChildModel> _jrOrdersChildModel;

        static JrResultsChildModel()
        {
            _jrOrdersChildModel = new List<JrResultsChildModel>();
        }

        public JrResultsChildModel(string id, string nomer, string codeName, string shkProb, string goods, string podrIsp, string podrRec, string datePlan, string dateDone)
        {
            Id = id;
            Nomer = nomer;
            CodeName = codeName;
            ShkProb = shkProb;
            Goods = goods;
            PodrIsp = podrIsp;
            PodrRec = podrRec;
            DatePlan = datePlan;
            DateDone = dateDone;
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
