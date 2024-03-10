using System.Diagnostics;

namespace tray_yeeter_sharp
{
    internal static partial class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Yeeter());
        }
    }
}