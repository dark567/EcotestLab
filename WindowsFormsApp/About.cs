using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            VersionAssembly.Text = typeof(MainMenu).Assembly.GetName().Version.ToString();
        }
    }
}
