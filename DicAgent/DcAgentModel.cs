using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicAgent
{
    public class DcAgentModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Фамилия")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Surname { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Имя")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Отчество")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Secname { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Короткое наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string CodeName { get; set; }

        [VisibleTypes(true)] 
        [DisplayName("Организация")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Org { get; set; }// потом поменять на ссылку на справочник организаций

        public static List<DcAgentModel> _dicClientsModel;
        //private Func<string> birthdate;
        //private DateTime? birthdate1;

        static DcAgentModel()
        {
            _dicClientsModel = new List<DcAgentModel>();
        }

        public DcAgentModel(string id, string surname, string name, string secname, string codeName, string org)
        {
            Id = id;
            Surname = surname;
            Name = name;
            Secname = secname;
            CodeName = codeName;
            Org = org;
        }

        public static DcAgentModel[] GetDcClientsModel
        {
            get
            {
                return DcAgentModel._dicClientsModel.ToArray();
            }
        }

        public static void AddDcClientsModel(DcAgentModel _dcClientsModel)
        {
            DcAgentModel._dicClientsModel.Add(_dcClientsModel);
        }

        public static void ClearDcClientsModel()
        {
            DcAgentModel._dicClientsModel.Clear();
        }
    }
}
