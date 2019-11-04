using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JOrders
{
    class JrOrderDetailsModel
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
        [DisplayName("Подразделение id")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string SubdivisionId { get; set; } //обязательно нужно использовать get конструкцию

        [VisibleTypes(true)]
        [DisplayName("Подразделение name")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string SubdivisionName { get; set; } //обязательно нужно использовать get конструкцию

        [VisibleTypes(true)]
        [DisplayName("Клиент id")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string ClientId { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Клиент name")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string ClientName { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Менеджер id")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string ManagerId { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Менеджер name")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string ManagerName { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Агент Id")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string AgentId { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Агент name")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string AgentName { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Плательщик id")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string OrgId { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Плательщик name")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string OrgName { get; set; }

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
        [DisplayName("Сумма оплаты")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PAYED_SUM { get; set; }


        [VisibleTypes(true)]
        [DisplayName("Описание")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Descr { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Фискальный номер")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string FiscalNum { get; set; }


        public static List<JrOrderDetailsModel> _jrOrdersModel;

        static JrOrderDetailsModel()
        {
            _jrOrdersModel = new List<JrOrderDetailsModel>();
        }

        public JrOrderDetailsModel(string id, bool isFiscal = false , decimal sum_Base = 0, decimal sum_Realiz = 0, decimal pAYED_SUM = 0, string dataCheck = "", string numCheck = "", string subdivisionId = "", string subdivisionName = "", string clientId = "", string clientName = "", string agentId = "", string agentName = "", string managerId = "", string managerName = "", string orgId = "", string orgName = "", string descr = "", string fiscalNum = "")
        {
            Id = id;
            NumCheck = numCheck;
            SubdivisionId = subdivisionId;
            SubdivisionName = subdivisionName;
            ClientId = clientId;
            ClientName = clientName;
            AgentId = agentId;
            AgentName = agentName;
            ManagerId = managerId;
            ManagerName = managerName;
            DataCheck = dataCheck;
            IsFiscal = isFiscal;
            OrgId = orgId;
            OrgName = orgName;
            SUM_BASE = String.Format("{0:0.00}", sum_Base);
            SUM_Realiz = String.Format("{0:0.00}", sum_Realiz);
            PAYED_SUM = String.Format("{0:0.00}", pAYED_SUM);
            Descr = descr;
            FiscalNum = fiscalNum;
        }

        public static JrOrderDetailsModel[] GetJrOrdersModel
        {
            get
            {
                return JrOrderDetailsModel._jrOrdersModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrOrderDetailsModel jrOrdersModel)
        {
            JrOrderDetailsModel._jrOrdersModel.Add(jrOrdersModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrOrderDetailsModel._jrOrdersModel.Clear();
        }
    }
}
