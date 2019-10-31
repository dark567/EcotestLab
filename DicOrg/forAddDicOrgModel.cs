using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicOrg
{
    public class forAddDicOrgModel
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


        public static List<forAddDicOrgModel> _jrTestModel;

        static forAddDicOrgModel()
        {
            _jrTestModel = new List<forAddDicOrgModel>();
        }

        public forAddDicOrgModel(string id, string codeName)
        {
            Id = id;
            CodeName = codeName;
        }

        public static forAddDicOrgModel[] GetjrTestModel
        {
            get
            {
                return forAddDicOrgModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forAddDicOrgModel jrTestModel)
        {
            forAddDicOrgModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forAddDicOrgModel._jrTestModel.Clear();
        }
    }
}
