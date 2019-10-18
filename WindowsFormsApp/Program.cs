using System;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
    static class Login
    {
        public static string Value { get; set; }
    }

    static class Login_e
    {
        public static string Value { get; set; }
    }

    static class db_puth
    {
        public static string Value { get; set; }
    }

    static class Key
    {
        public static string Value { get; set; }
    }
}
