using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class KeyLic : Form
    {
        public string Param = "";

        public KeyLic()
        {
            InitializeComponent();
        }

        private void KeyLic_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = Param;
        }
    }
}
