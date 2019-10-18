using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG
{
    public class forEditChecksModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID___")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Catgoria { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Code { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string EdIzm { get; set; }


        public static List<forEditChecksModel> _jrTestModel;

        static forEditChecksModel()
        {
            _jrTestModel = new List<forEditChecksModel>();
        }

        public forEditChecksModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static forEditChecksModel[] GetjrTestModel
        {
            get
            {
                return forEditChecksModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forEditChecksModel jrTestModel)
        {
            forEditChecksModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forEditChecksModel._jrTestModel.Clear();
        }
    }
}
