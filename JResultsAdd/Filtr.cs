using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JResultsAdd
{
    public partial class Filtr : Form
    {
        public Filtr()
        {
            InitializeComponent();
        }

        private void Filtr_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = int.Parse(FiltrModel.FiltrTest);
        }

        private void Filtr_FormClosing(object sender, FormClosingEventArgs e)
        {
            FiltrModel.FiltrTest = comboBox1.SelectedIndex.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
