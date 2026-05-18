using do_an_1.Frm;

namespace do_an_1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Utils.DatabaseConnection.TestConnection();
            Application.Run(new frm_login());

        }
    }
}