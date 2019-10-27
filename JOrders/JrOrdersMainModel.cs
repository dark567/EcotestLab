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

        [VisibleTypes(true)]
        [DisplayName("Дата")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DataCheck { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Номер заказа")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string NumCheck { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Подразделение")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Subdivision { get; set; } //обязательно нужно использовать get конструкцию

        [VisibleTypes(true)]
        [DisplayName("Клиент")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Client { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Агент")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Agent { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Менеджер")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Manager { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Фискальная операция")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public bool IsFiscal { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Сумма по прайсу")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string SUM_BASE { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Сумма реализации")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string SUM_Realiz { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Оплачено")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PAYED_SUM { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Выполнение")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public bool IsDone { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Печать фискального чека")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DataPrintFiscal { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Отправка по почте")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public bool IsPrintRecent { get; set; }

        //public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<JrOrdersMainModel> _jrOrdersModel;

        static JrOrdersMainModel()
        {
            _jrOrdersModel = new List<JrOrdersMainModel>();
        }

        public JrOrdersMainModel(string id, string numCheck, string subdivision, string client, string agent, string manager, bool isFiscal, string sum_Base, string sum_Realiz, string pAYED_SUM, bool isDone, string dataPrintFiscal, bool isPrintRecent, string dataCheck = "")
        {
            Id = id;
            NumCheck = numCheck;
            Subdivision = subdivision;
            Client = client;
            Agent = agent;
            Manager = manager;
            DataCheck = dataCheck;
            IsFiscal = isFiscal;
            SUM_BASE = String.Format("{0:0.00}", sum_Base);
            SUM_Realiz = String.Format("{0:0.00}", sum_Realiz);
            PAYED_SUM = String.Format("{0:0.00}", pAYED_SUM);
            IsDone = isDone;
            DataPrintFiscal= dataPrintFiscal;
            IsPrintRecent = isPrintRecent;
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
