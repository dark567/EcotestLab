using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JOrders
{
    public class JrChecksPrintRepModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [VisibleTypes(false)]
        [DisplayName("body")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string Body { get; set; } //обязательно нужно использовать get конструкцию

        public static List<JrChecksPrintRepModel> _jrChecksPrintRepModel;

        static JrChecksPrintRepModel()
        {
            _jrChecksPrintRepModel = new List<JrChecksPrintRepModel>();
        }

        public JrChecksPrintRepModel(string id, string name, string body)
        {
            Id = id;
            Name = name;
            Body = body;
        }

        public static JrChecksPrintRepModel[] GetjrChecksPrintRep
        {
            get
            {
                return JrChecksPrintRepModel._jrChecksPrintRepModel.ToArray();
            }
        }

        public static void AddJrChecksPrintRepModel(JrChecksPrintRepModel jrChecksPrintRep)
        {
            JrChecksPrintRepModel._jrChecksPrintRepModel.Add(jrChecksPrintRep);
        }

        public static void ClearjrChecksPrintRepModel()
        {
            JrChecksPrintRepModel._jrChecksPrintRepModel.Clear();
        }
    }
}
