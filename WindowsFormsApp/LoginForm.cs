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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get
            {
                return edtUserName.Text;
            }
            set
            {
                edtUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return edtPassword.Text;
            }
            set
            {
                edtPassword.Text = value;
            }
        }

        /// <summary>
        /// реакция на Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        private void EdtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }
    }
}
