using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Diagnostics;
using Windows.System;

namespace tray_yeeter_sharp
{
    internal partial class HotkeysLoad
    {
        private static readonly List<IntPtr> yeetedWindows = [];
        private static readonly List<IntPtr> enumWindowsResult = [];

        private static readonly List<int> modifierKeys = [
            (int)VirtualKey.LeftControl,
            (int)VirtualKey.RightControl,
            (int)VirtualKey.Control,
            (int)VirtualKey.LeftShift,
            (int)VirtualKey.RightShift,
            (int)VirtualKey.Shift,
            (int)VirtualKey.LeftMenu,
            (int)VirtualKey.RightMenu,
            (int)VirtualKey.Menu,
            (int)VirtualKey.LeftWindows,
            (int)VirtualKey.RightWindows];

        private static readonly HashSet<int> currentKeys = [];
        private static readonly List<int> assignedHotkeys = [];
        private static HotkeyEvent pointedEvent = delegate { };

        public delegate void HotkeyEvent(int Id);

        public static void RegisterHotkeys(HotkeyEvent hotkeyEvent)
        {
            pointedEvent = hotkeyEvent;
            KeyboardHookHandler.RemoveHook();
            ConfigParse();
            KeyboardHookHandler.SetupHook(DownKeyEvent, UpKeyEvent);
        }

        private static bool DownKeyEvent(int key)
        {
            int index = assignedHotkeys.IndexOf(key);
            if (index != -1 && !currentKeys.Intersect(modifierKeys).Any())
            {
                pointedEvent(index);
                return true;
            }

            currentKeys.Add(key);

            int combined = currentKeys.Sum();
            index = assignedHotkeys.IndexOf(combined);

            if (index != -1)
            {
                pointedEvent(index);
                return true;
            }

            return false;
        }

        private static bool UpKeyEvent(int key)
        {
            currentKeys.Remove(key);
            return false;
        }

        private static void ConfigParse()
        {
            JSchema configSchema = JSchema.Parse(@"{
                'type': 'object',
                'properties': {
                    'yeet': {
                        'type': 'array',
                        'items': {
                            'type': 'string'
                        }
                    },
                    'unyeet': {
                        'type': 'array',
                        'items': {
                            'type': 'string'
                        }
                    },
                    'yeet_all': {
                        'type': 'array',
                        'items': {
                            'type': 'string'
                        }
                    },
                    'unyeet_all': {
                        'type': 'array',
                        'items': {
                            'type': 'string'
                        }
                    },
                },
                'required': ['yeet', 'unyeet', 'yeet_all', 'unyeet_all']
            }");

            JObject config = JObject.Parse(File.ReadAllText("config.json"));

            if (!config.IsValid(configSchema))
            {
                new ToastContentBuilder()
                 .AddText("Invalid config ╯︿╰")
                 .AddText("Please check config.json and Github page to correct it.")
                 .Show();

                return;
            }

            try
            {
                InitHotkey("yeet", config);
                InitHotkey("unyeet", config);
                InitHotkey("yeet_all", config);
                InitHotkey("unyeet_all", config);
            }
            catch
            {
                new ToastContentBuilder()
                    .AddText("Tray Yeeter can't register your hotkeys (┬┬﹏┬┬)")
                    .AddText("Check your config.json for spelling errors then reload. If nothing is wrong and TY still can't register, please open an issue on Github.")
                    .Show();

                return;
            }

            new ToastContentBuilder()
                .AddText("Tray Yeeter has settled in (。・ω・。)")
                .AddText("Yeet current focusing window: " + string.Join("+", config["yeet"]!.Select(v => (string?)v))
                + "\nUnyeet latest yeeted window: " + string.Join("+", config["unyeet"]!.Select(v => (string?)v))
                + "\nYeet everything: " + string.Join("+", config["yeet_all"]!.Select(v => (string?)v))
                + "\nUnyeet everything: " + string.Join("+", config["unyeet_all"]!.Select(v => (string?)v)))
                .Show();
        }
        private static void InitHotkey(string name, JObject config)
        {
            int hotkey = 0;
            foreach (string? key in config[name]!.Select(v => (string?)v))
            {
                hotkey += (int)Enum.Parse<VirtualKey>(key!);
            }

            assignedHotkeys.Add(hotkey);
        }
    }
}
