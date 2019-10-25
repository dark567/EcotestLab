using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JResultsAdd
{
    public class JrResultsChildModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Id { get; set; }

        [DisplayName("Наименование")]
        [ColumnWeight(5)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [DisplayName("Единица измерения")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string EdIzm { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Текст нормы")]
        [ColumnWeight(15)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        // [TypesIService(false)]
        public string TextNorm { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Результат")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Rezult { get; set; }

        [DisplayName("Описание")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public string Descr { get; set; }

        [DisplayName("Результат число")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string RezultNum { get; set; }

        [DisplayName("За пределами нормы")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string ZaNorm { get; set; }

        [DisplayName("Порядок сортировки")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string Sort { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public static List<JrResultsChildModel> _jrOrdersChildModel;

        static JrResultsChildModel()
        {
            _jrOrdersChildModel = new List<JrResultsChildModel>();
        }

        public JrResultsChildModel(string id, string name, string edIzm, string textNorm, string rezult, string descr, string rezultNum, string zaNorm, string sort)
        {
            Id = id;
            Name = name;
            EdIzm = edIzm;
            TextNorm = textNorm;
            Rezult = rezult;
            Descr = descr;
            RezultNum = rezultNum;
            ZaNorm = zaNorm;
            Sort = sort;
        }

        public static JrResultsChildModel[] GetJrOrdersChildModel
        {
            get
            {
                return JrResultsChildModel._jrOrdersChildModel.ToArray();
            }
        }

        public static void AddJrOrdersModel(JrResultsChildModel jrOrdersChildModel)
        {
            JrResultsChildModel._jrOrdersChildModel.Add(jrOrdersChildModel);
        }

        public static void ClearJrOrdersModel()
        {
            JrResultsChildModel._jrOrdersChildModel.Clear();
        }
    }
}
