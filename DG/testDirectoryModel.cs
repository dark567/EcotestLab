using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG
{
    public class testDirectoryModel
    {
        public string ID;
        public string ParentID;
        public string FullName;
        public int NumDepth;

        public static List<testDirectoryModel> _testDirectoryModel;

        static testDirectoryModel()
        {
            _testDirectoryModel = new List<testDirectoryModel>();
        }

        public testDirectoryModel(string _ID, string _parentID, string _fullName, int _numDepth)
        {
            this.ID = _ID;
            this.ParentID = _parentID;
            this.FullName = _fullName;
            this.NumDepth = _numDepth;
        }


        public static testDirectoryModel[] GetDicGoods
        {
            get
            {
                return _testDirectoryModel.ToArray();
            }
        }

        public static void AddDicGoods(testDirectoryModel testDirectoryModel)
        {
            _testDirectoryModel.Add(testDirectoryModel);
        }
    }
}
