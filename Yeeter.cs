using Microsoft.Toolkit.Uwp.Notifications;
using System.Globalization;
using System.Runtime.InteropServices;
using static tray_yeeter_sharp.HotkeysLoad;

namespace tray_yeeter_sharp
{
    public partial class Yeeter : Form
    {
        public Yeeter()
        {
            Load += Yeeter_Load;
            InitializeComponent();
        }

        private void Yeeter_Load(object? sender, EventArgs e)
        {
            notifyIcon1.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Reload", null, ReloadEvent);
            notifyIcon1.ContextMenuStrip.Items.Add("Quit", null, QuitEvent);
            RegisterHotkeys(HotkeyEvent);
        }

        private static void ReloadEvent(object? sender, EventArgs e)
        {
            RegisterHotkeys(HotkeyEvent);
        }

        private static void QuitEvent(object? sender, EventArgs e)
        {
            if (yeetedWindows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Some yeeted windows are still... yeeted. Do you want to unyeet it? (This program can't recover yeeted windows from previous session)",
                    "Tray Yeeter",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    foreach (IntPtr hWnd in yeetedWindows)
                    {
                        try { ShowWindow(hWnd, SW_SHOW); } catch { }
                    }
                }
                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            new ToastContentBuilder()
                 .AddText("Tray Yeeter has stopped ╯︿╰")
                 .Show();
            Environment.Exit(0);
        }

        private static void HotkeyEvent(int Id)
        {
            if (Id == 0)
            {
                IntPtr hWnd = GetForegroundWindow();
                try { ShowWindow(hWnd, SW_HIDE); } catch { }
                yeetedWindows.Add(hWnd);
            }
            if (Id == 1)
            {
                if (yeetedWindows.Count > 0)
                {
                    try { ShowWindow(yeetedWindows[^1], SW_SHOW); } catch { }
                    yeetedWindows.RemoveAt(yeetedWindows.Count - 1);
                }
            }
            if (Id == 2)
            {
                EnumWindows(new EnumWindowsProc(EnumWindowCallback), IntPtr.Zero);
                yeetedWindows.AddRange(enumWindowsResult);

                foreach (IntPtr hWnd in enumWindowsResult)
                {
                    try { ShowWindow(hWnd, SW_HIDE); } catch { }
                }

                enumWindowsResult.Clear();
            }
            if (Id == 3)
            {
                foreach (IntPtr hWnd in yeetedWindows)
                {
                    try { ShowWindow(hWnd, SW_SHOW); } catch { }
                }

                yeetedWindows.Clear();
            }
        }

        private static bool EnumWindowCallback(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                enumWindowsResult.Add(hWnd);
            }
            return true;
        }

        private static List<IntPtr> yeetedWindows = [];
        private static List<IntPtr> enumWindowsResult = [];

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [LibraryImport("user32.dll")]
        private static partial IntPtr GetForegroundWindow();

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool IsWindowVisible(IntPtr hWnd);
    }
}
