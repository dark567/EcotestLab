using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicEmployee
{
   public class forAddDicEmployeeModel
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


        public static List<forAddDicEmployeeModel> _jrTestModel;

        static forAddDicEmployeeModel()
        {
            _jrTestModel = new List<forAddDicEmployeeModel>();
        }

        public forAddDicEmployeeModel(string id, string name, string codeName)
        {
            Id = id;
            Name = name;
            CodeName = codeName;
        }

        public static forAddDicEmployeeModel[] GetjrTestModel
        {
            get
            {
                return forAddDicEmployeeModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forAddDicEmployeeModel jrTestModel)
        {
            forAddDicEmployeeModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forAddDicEmployeeModel._jrTestModel.Clear();
        }
    }
}
