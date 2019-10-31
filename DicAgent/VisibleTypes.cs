using System;
using System.Windows.Forms;

namespace DicAgent
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