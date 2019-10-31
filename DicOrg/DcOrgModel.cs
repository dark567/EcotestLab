using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicOrg
{
   public class DcOrgModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Короткое наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string ShortName { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ЕГРПОУ")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Egrrpou { get; set; }

        public static List<DcOrgModel> _dicClientsModel;
        //private Func<string> birthdate;
        //private DateTime? birthdate1;

        static DcOrgModel()
        {
            _dicClientsModel = new List<DcOrgModel>();
        }

        public DcOrgModel(string id, string shortName, string name, string egrrpou)
        {
            Id = id;
            ShortName = shortName;
            Name = name;
            Egrrpou = egrrpou;
        }

        public static DcOrgModel[] GetDcClientsModel
        {
            get
            {
                return DcOrgModel._dicClientsModel.ToArray();
            }
        }

        public static void AddDcClientsModel(DcOrgModel _dcClientsModel)
        {
            DcOrgModel._dicClientsModel.Add(_dcClientsModel);
        }

        public static void ClearDcClientsModel()
        {
            DcOrgModel._dicClientsModel.Clear();
        }
    }
}
