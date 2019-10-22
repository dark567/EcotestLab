using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JResultsAdd
{
    public class JResultsAddEditModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [DisplayName("Дата")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string DataDoc { get; set; }

        [DisplayName("Номер")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string NomerDoc { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Ш/к пробирки")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string ShkProb { get; set; }

        [DisplayName("TypeAnaliz")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string TypeAnaliz { get; set; }

        [DisplayName("GrpAnaliz")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string GrpAnaliz { get; set; }

        [DisplayName("CodeName")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string CodeName { get; set; }

        [DisplayName("PLAN_DATE_DONE")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string PLAN_DATE_DONE { get; set; }

        [DisplayName("DATE_DONE")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string DATE_DONE { get; set; }

        public static List<JResultsAddEditModel> _jrOrdersModel;

        static JResultsAddEditModel()
        {
            _jrOrdersModel = new List<JResultsAddEditModel>();
        }

        public JResultsAddEditModel(string id, string dataDoc, string nomerDoc, string shkProb, string typeAnaliz, string grpAnaliz, string codeName, string pLAN_DATE_DONE, string dATE_DONE)
        {
            Id = id;
            DataDoc = dataDoc;
            NomerDoc = nomerDoc;
            ShkProb = shkProb;
            TypeAnaliz = typeAnaliz;
            GrpAnaliz = grpAnaliz;
            CodeName = codeName;
            PLAN_DATE_DONE = pLAN_DATE_DONE;
            DATE_DONE = dATE_DONE;
        }

        public static JResultsAddEditModel[] GetJrOrdersModel
        {
            get
            {
                return JResultsAddEditModel._jrOrdersModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JResultsAddEditModel jrOrdersModel)
        {
            JResultsAddEditModel._jrOrdersModel.Add(jrOrdersModel);
        }

        public static void ClearJrOrdersModel()
        {
            JResultsAddEditModel._jrOrdersModel.Clear();
        }
    }
}
