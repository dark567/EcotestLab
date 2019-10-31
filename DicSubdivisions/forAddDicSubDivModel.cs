using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicSubdivisions
{
    public class forAddDicSubDivModel
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

        //[VisibleTypes(true)]
        //[DisplayName("CodeName")]
        //[ColumnWeight(10)] //todo
        //                   // [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //public string CodeName { get; set; }


        public static List<forAddDicSubDivModel> _jrTestModel;

        static forAddDicSubDivModel()
        {
            _jrTestModel = new List<forAddDicSubDivModel>();
        }

        public forAddDicSubDivModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public static forAddDicSubDivModel[] GetjrTestModel
        {
            get
            {
                return forAddDicSubDivModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forAddDicSubDivModel jrTestModel)
        {
            forAddDicSubDivModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forAddDicSubDivModel._jrTestModel.Clear();
        }
    }
}
