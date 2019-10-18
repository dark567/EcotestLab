using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DClients
{
    public class forAddDicClientsModel
    {
        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(32)] //todo
       // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Name")]
        [ColumnWeight(10)] //todo
       // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("CodeName")]
        [ColumnWeight(10)] //todo
       // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string CodeName { get; set; }


        public static List<forAddDicClientsModel> _jrTestModel;

        static forAddDicClientsModel()
        {
            _jrTestModel = new List<forAddDicClientsModel>();
        }

        public forAddDicClientsModel(string id, string name, string codeName)
        {
            Id = id;
            Name = name;
            CodeName = codeName;
        }

        public static forAddDicClientsModel[] GetjrTestModel
        {
            get
            {
                return forAddDicClientsModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forAddDicClientsModel jrTestModel)
        {
            forAddDicClientsModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forAddDicClientsModel._jrTestModel.Clear();
        }
    }
}
