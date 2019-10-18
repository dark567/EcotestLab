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
    public partial class JResultsAddEdit : Form
    {
        public string Id = "n/a";
        public JResultsAddEdit()
        {
            InitializeComponent();
        }

        private void JResultsAddEdit_Load(object sender, EventArgs e)
        {
            textBox1.Text = $"ID:{Id}";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
