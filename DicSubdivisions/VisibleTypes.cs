using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicSubdivisions
{
    public class VisibleTypes : Attribute
    {
        public bool typesVisible { get; set; }

        public VisibleTypes(bool _typesVisible)
        {
            typesVisible = _typesVisible;
        }
    }
}
