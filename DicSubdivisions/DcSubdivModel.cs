using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicSubdivisions
{
    public class DcSubdivModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Подразделение рецепции")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public bool OrgRecep { get; set; } //обязательно нужно использовать get конструкцию

        [VisibleTypes(true)]
        [DisplayName("Финансовое подразделение")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public bool OrgFin { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Подразделение склада")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public bool OrgScl{ get; set; }

        [VisibleTypes(true)]
        [DisplayName("Подразделение лаборатории")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public bool OrgLab { get; set; }

        //[DisplayName("Дата рождения")]
        //[ColumnWeight(10)] //todo
        //[AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        ////[TypesIService(false)] //todo
        //public string Birthdate
        //{
        //    get;
        //    //{
        //    //    if (Birthdate == null)
        //    //        Birthdate = "";
        //    //    else if (Birthdate != null)
        //    //        Birthdate = Birthdate.Remove(3);
        //    //    return Birthdate;
        //    //}
        //    set;
        //}

        //[DisplayName("Почта")]
        //[ColumnWeight(10)] //todo
        //[AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        ////[TypesIService(false)] //todo
        // public string Email { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<DcSubdivModel> _dicClientsModel;
        //private Func<string> birthdate;
        //private DateTime? birthdate1;

        static DcSubdivModel()
        {
            _dicClientsModel = new List<DcSubdivModel>();
        }



        public DcSubdivModel(string id, string name, bool orgRecep, bool orgFin, bool orgScl, bool orgLab)
        {
            Id = id;
            Name = name;
            OrgRecep = orgRecep;
            OrgFin = orgFin;
            OrgScl = orgScl;
            OrgLab = orgLab;
            //Birthdate = birthdate = birthdate == "" ? "" : (DateTime.Parse(birthdate).ToShortDateString()).ToString();
            //Email = email;
        }

        public static DcSubdivModel[] GetDcClientsModel
        {
            get
            {
                return DcSubdivModel._dicClientsModel.ToArray();
            }
        }

        public static void AddDcClientsModel(DcSubdivModel _dcClientsModel)
        {
            DcSubdivModel._dicClientsModel.Add(_dcClientsModel);
        }

        public static void ClearDcClientsModel()
        {
            DcSubdivModel._dicClientsModel.Clear();
        }
    }
}
