using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DG
{
    public class DicGoodsModel
    {
        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string ID { get; set; }

        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string GrpId { get; set; }

        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string GrpName { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Code { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public string Name { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public bool IsService { get; set; }

        [VisibleTypes(true)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public double Price { get; set; }

        [VisibleTypes(false)]
        [DisplayName("ID")]
        [ColumnWeight(10)] //todo
        [AutoSizeMode(DataGridViewAutoSizeColumnMode.AllCells)]
        //[TypesIService(false)]
        public bool IsSale { get; set; }

        public static List<DicGoodsModel> _dicGoodsModel;
        static DicGoodsModel()
        {
            _dicGoodsModel = new List<DicGoodsModel>();
        }

        public DicGoodsModel(string id, string grpId, string grpName, string code, string name, bool isService, bool isSale, double price = 0.00)
        {
            this.ID = id;
            this.GrpId = grpId;
            this.GrpName = grpName;
            this.Code = code;
            this.Name = name;
            this.IsService = isService;
            this.Price = price;
            this.IsSale = isSale;
        }

        public static DicGoodsModel[] GetDicGoodsModel
        {
            get
            {
                return DicGoodsModel._dicGoodsModel.ToArray();
            }
        }

        public static void AddDicGoodsModel(DicGoodsModel _dicGoodsModel)
        {
            DicGoodsModel._dicGoodsModel.Add(_dicGoodsModel);
        }

        public static void ClearDicGoodsModel()
        {
            DicGoodsModel._dicGoodsModel.Clear();
        }

    }
}
