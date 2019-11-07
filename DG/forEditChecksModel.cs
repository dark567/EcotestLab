using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG
{
    public class forEditChecksModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Id { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Название")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Name { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Группа id")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string CategoriaId { get; set; }

        [VisibleTypes(false)]
        [DisplayName("Группа")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Categoria { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Код")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Code { get; set; }

        [VisibleTypes(true)]
        [DisplayName("Цена")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        public string Price { get; set; }


        public static List<forEditChecksModel> _jrTestModel;

        static forEditChecksModel()
        {
            _jrTestModel = new List<forEditChecksModel>();
        }

        public forEditChecksModel(string id, string name, string categoriaId, string categoria, string code, string price)
        {
            Id = id;
            Name = name;
            CategoriaId = categoriaId;
            Categoria = categoria;
            Code = code;
            Price = price;
        }

        public static forEditChecksModel[] GetjrTestModel
        {
            get
            {
                return forEditChecksModel._jrTestModel.ToArray();
            }
        }

        public static void AddjrTestModel(forEditChecksModel jrTestModel)
        {
            forEditChecksModel._jrTestModel.Add(jrTestModel);
        }

        public static void ClearjrTestModel()
        {
            forEditChecksModel._jrTestModel.Clear();
        }
    }
}
