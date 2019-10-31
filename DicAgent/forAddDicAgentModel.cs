using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicAgent
{
    public class forAddDicAgentModel
    {
        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(32)] //todo
                           // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("CodeName")]
        [ColumnWeight(10)] //todo
                           // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string CodeName { get; set; }

        //[VisibleTypes(true)]
        //[DisplayName("CodeName")]
        //[ColumnWeight(10)] //todo
        // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //public string CodeName { get; set; }


        public static List<forAddDicAgentModel> _jrTestModel;

        static forAddDicAgentModel()
        {
            _jrTestModel = new List<forAddDicAgentModel>();
        }

        public forAddDicAgentModel(string id, string codeName)
        {
            Id = id;
            CodeName = codeName;
        }

        public static forAddDicAgentModel[] GetjrTestModel
        {
            get
            {
                return forAddDicAgentModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forAddDicAgentModel jrTestModel)
        {
            forAddDicAgentModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forAddDicAgentModel._jrTestModel.Clear();
        }
    }
}
