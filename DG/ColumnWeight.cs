using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DG
{
    public class ColumnWeight : Attribute
    {
        public int Weight { get; set; }

        public ColumnWeight(int weight)
        {
            Weight = weight;
        }
    }
}
