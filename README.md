# Tray Yeeter (C#)
A C# tool to yeet your windows.

## Installation
1. Download the [latest release build](https://github.com/Neurs12/tray_yeeter_sharp/releases/).
2. Extract and run `Tray Yeeter.exe`.
3. A notification should pop up, indicates that the process has started successfully.

## Edit hotkeys
- Global hotkeys can't assign to critical hotkeys (Example: `ctrl + alt + del`, `alt + f4`, `win + e`,...)
- All keys are from [virutal key codes](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes).
- To edit a hotkey, open `config.json` in the same folder with `Tray Yeeter.exe`. It look something like this:
```json
{
  "yeet": [ "F1" ],
  "unyeet": [ "F2" ],
  "yeet_all": [ "F11" ],
  "unyeet_all": [ "F12" ]
}
```

- Now, let's change yeet hotkey to `ctrl + 1`:
    1. Specify what Ctrl key do you want. In this example, I'll choose Left Ctrl.
    3. `1` is a key. But we need to specify where is that `1` key. I'll choose the number 1 key (not numpad).
- Apply it to our config:
```json
{
  "yeet": [ "LeftControl", "Number1" ],
  "unyeet": [ "F2" ],
  "yeet_all": [ "F11" ],
  "unyeet_all": [ "F12" ]
}
```

- To see all available keys, go to akeys.txt.

## Start with Windows
1. Create a shortcut to `Tray Yeeter.exe` we've extracted earlier.
2. Press `win + R`, then enter `shell:startup`, this should opens `Startup` folder.
3. Put the shortcut to `Startup` folder.

## Debug
Run the process in a terminal environment.
