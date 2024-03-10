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
            bool checkProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly()!.Location)).Length > 1;

            if (checkProcess)
            {
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Yeeter());
        }
    }
}