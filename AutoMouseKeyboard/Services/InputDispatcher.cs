#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;

namespace AutoMouseKeyboard.Services
{
    public class InputDispatcher
    {
    private static readonly Dictionary<string, Keys> KeyAliases = new Dictionary<string, Keys>(StringComparer.OrdinalIgnoreCase)
    {
        ["CTRL"] = Keys.ControlKey,
        ["CONTROL"] = Keys.ControlKey,
        ["ALT"] = Keys.Menu,
        ["SHIFT"] = Keys.ShiftKey,
        ["WIN"] = Keys.LWin,
        ["WINDOWS"] = Keys.LWin,
        ["ENTER"] = Keys.Enter,
        ["RETURN"] = Keys.Enter,
        ["ESC"] = Keys.Escape,
        ["ESCAPE"] = Keys.Escape,
        ["SPACE"] = Keys.Space,
        ["TAB"] = Keys.Tab,
        ["UP"] = Keys.Up,
        ["DOWN"] = Keys.Down,
        ["LEFT"] = Keys.Left,
        ["RIGHT"] = Keys.Right,
        ["PGUP"] = Keys.PageUp,
        ["PGDN"] = Keys.Next,
        ["DEL"] = Keys.Delete,
        ["DELETE"] = Keys.Delete,
        ["INS"] = Keys.Insert,
        ["BACKSPACE"] = Keys.Back,
        ["HOME"] = Keys.Home,
        ["END"] = Keys.End
    };

    public Point GetCursorPosition()
    {
        NativeMethods.GetCursorPos(out var point);
        return new Point(point.X, point.Y);
    }

    public void MoveCursor(int x, int y)
    {
        NativeMethods.SetCursorPos(x, y);
        Thread.Sleep(5);
    }

    public void SendMouse(MouseButtonKind button)
    {
        switch (button)
        {
            case MouseButtonKind.LeftClick:
                SendMouseClick(NativeMethods.MOUSEEVENTF_LEFTDOWN, NativeMethods.MOUSEEVENTF_LEFTUP);
                break;
            case MouseButtonKind.LeftDouble:
                SendDoubleClick(NativeMethods.MOUSEEVENTF_LEFTDOWN, NativeMethods.MOUSEEVENTF_LEFTUP);
                break;
            case MouseButtonKind.MiddleClick:
                SendMouseClick(NativeMethods.MOUSEEVENTF_MIDDLEDOWN, NativeMethods.MOUSEEVENTF_MIDDLEUP);
                break;
            case MouseButtonKind.MiddleDouble:
                SendDoubleClick(NativeMethods.MOUSEEVENTF_MIDDLEDOWN, NativeMethods.MOUSEEVENTF_MIDDLEUP);
                break;
            case MouseButtonKind.RightClick:
                SendMouseClick(NativeMethods.MOUSEEVENTF_RIGHTDOWN, NativeMethods.MOUSEEVENTF_RIGHTUP);
                break;
            case MouseButtonKind.RightDouble:
                SendDoubleClick(NativeMethods.MOUSEEVENTF_RIGHTDOWN, NativeMethods.MOUSEEVENTF_RIGHTUP);
                break;
            default:
                SendMouseClick(NativeMethods.MOUSEEVENTF_LEFTDOWN, NativeMethods.MOUSEEVENTF_LEFTUP);
                break;
        }
    }

    public void SendKeyDown(Keys key) => SendKeyboardInput((ushort)key, true);

    public void SendKeyUp(Keys key) => SendKeyboardInput((ushort)key, false);

    public void SendKeySequence(string rawSequence)
    {
        if (string.IsNullOrWhiteSpace(rawSequence))
        {
            return;
        }

        var keys = new List<ushort>();
        foreach (var token in rawSequence.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmed = token.Trim();
            if (trimmed.Length == 0)
            {
                continue;
            }

            var key = TranslateToken(trimmed);
            if (!key.HasValue)
            {
                throw new InvalidOperationException(string.Format("Không nhận dạng được phím \"{0}\".", trimmed));
            }

            keys.Add(key.Value);
        }

        foreach (var key in keys)
        {
            SendKeyboardInput(key, true);
        }

        for (var i = keys.Count - 1; i >= 0; i--)
        {
            SendKeyboardInput(keys[i], false);
        }
    }

    private void SendMouseClick(uint downFlag, uint upFlag)
    {
        SendMouseInput(downFlag);
        SendMouseInput(upFlag);
    }

    private void SendDoubleClick(uint downFlag, uint upFlag)
    {
        SendMouseClick(downFlag, upFlag);
        Thread.Sleep(70);
        SendMouseClick(downFlag, upFlag);
    }

    private void SendMouseInput(uint flag)
    {
        var mouseInput = new NativeMethods.INPUT
        {
            type = NativeMethods.INPUT_MOUSE,
            U = new NativeMethods.InputUnion
            {
                mi = new NativeMethods.MOUSEINPUT
                {
                    dwFlags = flag,
                    dx = 0,
                    dy = 0,
                    mouseData = 0,
                    time = 0,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo()
                }
            }
        };

        NativeMethods.SendInput(1, new[] { mouseInput }, Marshal.SizeOf(typeof(NativeMethods.INPUT)));
    }

    private void SendKeyboardInput(ushort key, bool isDown)
    {
        var keyboardInput = new NativeMethods.INPUT
        {
            type = NativeMethods.INPUT_KEYBOARD,
            U = new NativeMethods.InputUnion
            {
                ki = new NativeMethods.KEYBDINPUT
                {
                    wVk = key,
                    wScan = 0,
                    dwFlags = isDown ? 0u : NativeMethods.KEYEVENTF_KEYUP,
                    dwExtraInfo = NativeMethods.GetMessageExtraInfo(),
                    time = 0
                }
            }
        };

        NativeMethods.SendInput(1, new[] { keyboardInput }, Marshal.SizeOf(typeof(NativeMethods.INPUT)));
    }

    private ushort? TranslateToken(string token)
    {
        if (KeyAliases.TryGetValue(token, out var alias))
        {
            return (ushort)alias;
        }

        if (token.StartsWith("F", StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(token.Substring(1), out var fn) && fn >= 1 && fn <= 24)
        {
            return (ushort)((int)Keys.F1 + fn - 1);
        }

        if (Enum.TryParse<Keys>(token, true, out var parsed))
        {
            return (ushort)parsed;
        }

        if (token.Length == 1)
        {
            var ch = char.ToUpperInvariant(token[0]);
            if (ch >= 'A' && ch <= 'Z')
            {
                return (ushort)((int)Keys.A + (ch - 'A'));
            }

            if (ch >= '0' && ch <= '9')
            {
                return (ushort)((int)Keys.D0 + (ch - '0'));
            }
        }

        return null;
    }

    private static class NativeMethods
    {
        internal const uint INPUT_MOUSE = 0;
        internal const uint INPUT_KEYBOARD = 1;
        internal const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        internal const uint MOUSEEVENTF_LEFTUP = 0x0004;
        internal const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        internal const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        internal const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        internal const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        internal const uint KEYEVENTF_KEYUP = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetMessageExtraInfo();
    }
    }
}

