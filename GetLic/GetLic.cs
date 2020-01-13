using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetLic
{
    public partial class GetLic : Form
    {
        public GetLic()
        {
            InitializeComponent();
        }

        private void KeyLic_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = Logic.CalculateMD5Hash(Logic.GetProcessorIdAndGetOSSerialNumberID());
        }
    }
}
