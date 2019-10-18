using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JOrders
{
    public class JrOrdersChildModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [DisplayName("Анализ")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Analiz { get; set; }

        [DisplayName("Единица измерений")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string EdIzm { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Количество")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Count { get; set; }

        [DisplayName("Цена")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Price { get; set; }

        [DisplayName("Цена реализации")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string PriceOut { get; set; }

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

        public static List<JrOrdersChildModel> _jrOrdersChildModel;

        static JrOrdersChildModel()
        {
            _jrOrdersChildModel = new List<JrOrdersChildModel>();
        }

        public JrOrdersChildModel(string id, string analiz, string edIzm, string count, decimal price, decimal priceOut, string datePlan = "", string dateDone = "")
        {
            Id = id;
            Analiz = analiz;
            EdIzm = edIzm;
            Count = count;
            Price = String.Format("{0:0.00}", price);
            PriceOut = String.Format("{0:0.00}", priceOut);
            DatePlan = datePlan == "" ? "" : (DateTime.Parse(datePlan).ToShortDateString()).ToString();
            DateDone = dateDone == "" ? "" : (DateTime.Parse(dateDone).ToShortDateString()).ToString();
        }

        public static JrOrdersChildModel[] GetJrOrdersChildModel
        {
            get
            {
                return JrOrdersChildModel._jrOrdersChildModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrOrdersChildModel jrOrdersChildModel)
        {
            JrOrdersChildModel._jrOrdersChildModel.Add(jrOrdersChildModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrOrdersChildModel._jrOrdersChildModel.Clear();
        }
    }
}
