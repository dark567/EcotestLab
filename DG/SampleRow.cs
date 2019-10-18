using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG
{
    public class SampleRow
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string id { get; set; }

        [DisplayName("Код")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string code { get; set; }

        [DisplayName("Наименование")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
       // [TypesIService(false)]
        public string name { get; set; } //обязательно нужно использовать get конструкцию

        [DisplayName("Единица измерения")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string edIzm { get; set; }

        [DisplayName("Услуга")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(true)] //todo
        public bool isService { get; set; }

        [DisplayName("Цена реализации")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public string price { get; set; }

        [DisplayName("В продаже")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)] //todo
        public bool isSale { get; set; }

        public string Hidden = ""; //Данное свойство не будет отображаться как колонка

        public SampleRow(string id, string code, string name, bool _isService, string price, bool _isSale)
        {
            this.id = id;
            this.code = code;
            this.name = name;
            this.isService = _isService;
            this.price = price;
            this.isSale = _isSale;
        }
    }
}
