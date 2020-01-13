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

        [VisibleTypes(true)]
        [DisplayName("Анализзз")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Goods { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Категория")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Categor { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Код")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Code { get; set; } 

        [VisibleTypes(true)]
        [DisplayName("Единица измерения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Unit { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Количество")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Count { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Цена базовая")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PriceBaz { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Цена")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Price { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Цена реализации")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PriceReal { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Исполнитель")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Employee { get; set; }

        public static List<JrEditCheckModel> _jrEditCheckModel;

        static JrEditCheckModel()
        {
            _jrEditCheckModel = new List<JrEditCheckModel>();
        }

        public JrEditCheckModel(string id, string goods, string categor, string code, string unit, string count, decimal priceBaz, decimal price, decimal priceReal, string employee)
        {
            Id = id;
            Goods = goods;
            Categor =categor;
            Code = code;
            Unit = unit;
            Count = count;
            PriceBaz = String.Format("{0:0.00}", priceBaz);
            Price = String.Format("{0:0.00}", price);
            PriceReal = String.Format("{0:0.00}", priceReal);
            Employee = employee;
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
