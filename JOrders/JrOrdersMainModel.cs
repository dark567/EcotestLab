using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace JOrders
{
    public class JrOrdersMainModel
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
        public string DataCheck { get; set; }

        [DisplayName("Номер заказа")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string NumCheck { get; set; }

        [DisplayName("Подразделение")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Subdivision { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Клиент")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Client { get; set; }

        [DisplayName("Агент")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Agent { get; set; }

        [DisplayName("Фискальная операция")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public bool IsFiscal { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<JrOrdersMainModel> _jrOrdersModel;

        static JrOrdersMainModel()
        {
            _jrOrdersModel = new List<JrOrdersMainModel>();
        }

        public JrOrdersMainModel(string id, string numCheck, string subdivision, string client, string agent, bool isFiscal, string dataCheck = "")
        {
            Id = id;
            NumCheck = numCheck;
            Subdivision = subdivision;
            Client = client;
            Agent = agent;
            DataCheck = dataCheck;
            IsFiscal = isFiscal;
        }

        public static JrOrdersMainModel[] GetJrOrdersModel
        {
            get
            {
                return JrOrdersMainModel._jrOrdersModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrOrdersMainModel jrOrdersModel)
        {
            JrOrdersMainModel._jrOrdersModel.Add(jrOrdersModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrOrdersMainModel._jrOrdersModel.Clear();
        }
    }
}
