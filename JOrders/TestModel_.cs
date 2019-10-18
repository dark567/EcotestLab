using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOrders
{
    public class TestModel_
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static List<TestModel_> _jrTestModel;

        static TestModel_()
        {
            _jrTestModel = new List<TestModel_>();
        }

        public static TestModel_[] GetjrTestModel
        {
            get
            {
                return TestModel_._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(TestModel_ jrTestModel)
        {
            TestModel_._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            TestModel_._jrTestModel.Clear();
        }
    }
}
