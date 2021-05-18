using System;
using System.Windows.Forms;
using System.Web.Script.Serialization;
namespace ProSwapperSplashScreen
{
    static class Program
    {
        public static JavaScriptSerializer js = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SplashUI());
        }
    }
}
