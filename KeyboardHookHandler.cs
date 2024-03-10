using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.System;

namespace tray_yeeter_sharp
{
    internal partial class KeyboardHookHandler
    {
        /// <summary>
        /// Low level keyboard hook.
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static KeyCallbackEvent keydownEvent = delegate { return false; };
        private static KeyCallbackEvent keyupEvent = delegate { return false; };

        public static IntPtr SetupHook(KeyCallbackEvent down, KeyCallbackEvent up)
        {
            keydownEvent = down;
            keyupEvent = up;
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule!;
            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc,
                GetModuleHandle(curModule.ModuleName), 0);
            return _hookID;
        }

        public static void RemoveHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        public delegate bool KeyCallbackEvent(VirtualKey key);

        public delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool res = false;

            if (nCode >= 0)
            {
                if (wParam == WM_KEYDOWN)
                {
                    res = keydownEvent((VirtualKey)Marshal.ReadInt32(lParam));
                }
                if (wParam == WM_KEYUP)
                {
                    res = keyupEvent((VirtualKey)Marshal.ReadInt32(lParam));
                }
            }
            return res ? 1 : CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExA", SetLastError = true)]
        private static partial IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool UnhookWindowsHookEx(IntPtr hhk);

        [LibraryImport("user32.dll", SetLastError = true)]
        private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleA", SetLastError = true, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        private static partial IntPtr GetModuleHandle(string lpModuleName);
    }
}
