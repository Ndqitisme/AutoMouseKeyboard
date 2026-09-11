#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public static class KeyTokenFormatter
    {
    private static readonly Dictionary<Keys, string> SpecialKeyTokens = new Dictionary<Keys, string>()
    {
        { Keys.OemMinus, "OemMinus" },
        { Keys.Oemplus, "Oemplus" },
        { Keys.OemOpenBrackets, "OemOpenBrackets" },
        { Keys.OemCloseBrackets, "OemCloseBrackets" },
        { Keys.OemSemicolon, "OemSemicolon" },
        { Keys.OemQuotes, "OemQuotes" },
        { Keys.Oemcomma, "Oemcomma" },
        { Keys.OemPeriod, "OemPeriod" },
        { Keys.OemQuestion, "OemQuestion" },
        { Keys.OemPipe, "OemPipe" },
        { Keys.Oemtilde, "Oemtilde" }
    };

    private static readonly Dictionary<string, string> FriendlyNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Control"] = "Ctrl",
        ["ControlKey"] = "Ctrl",
        ["Ctrl"] = "Ctrl",
        ["Menu"] = "Alt",
        ["Alt"] = "Alt",
        ["ShiftKey"] = "Shift",
        ["Shift"] = "Shift",
        ["Return"] = "Enter",
        ["Escape"] = "Esc",
        ["Back"] = "Backspace",
        ["Space"] = "Space",
        ["OemMinus"] = "-",
        ["Oemplus"] = "=",
        ["OemOpenBrackets"] = "[",
        ["OemCloseBrackets"] = "]",
        ["OemSemicolon"] = ";",
        ["OemQuotes"] = "'",
        ["Oemcomma"] = ",",
        ["OemPeriod"] = ".",
        ["OemQuestion"] = "/",
        ["OemPipe"] = "\\",
        ["Oemtilde"] = "`"
    };

    private static readonly HashSet<Keys> NavigationKeys = new HashSet<Keys>()
    {
        Keys.Up, Keys.Down, Keys.Left, Keys.Right,
        Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End,
        Keys.Tab, Keys.Capital
    };

    public static bool IsNavigationKey(Keys key) => NavigationKeys.Contains(key);

    public static string BuildToken(KeyEventArgs e)
    {
        if (e == null)
        {
            return string.Empty;
        }

        var keyCode = e.KeyCode;

        if (keyCode == Keys.ControlKey || keyCode == Keys.Menu || keyCode == Keys.ShiftKey)
        {
            return FriendlyNames.TryGetValue(keyCode.ToString(), out var name) ? name : keyCode.ToString();
        }

        var parts = new List<string>();
        if (e.Control)
        {
            parts.Add("Ctrl");
        }

        if (e.Alt)
        {
            parts.Add("Alt");
        }

        if (e.Shift)
        {
            parts.Add("Shift");
        }

        var baseToken = NormalizeKey(keyCode);
        if (string.IsNullOrEmpty(baseToken))
        {
            return string.Empty;
        }

        parts.Add(baseToken);
        return string.Join("+", parts);
    }

    private static string NormalizeKey(Keys keyCode)
    {
        if (SpecialKeyTokens.TryGetValue(keyCode, out var token))
        {
            return token;
        }

        if (keyCode >= Keys.A && keyCode <= Keys.Z)
        {
            return keyCode.ToString();
        }

        if (keyCode >= Keys.D0 && keyCode <= Keys.D9)
        {
            return ((char)('0' + (keyCode - Keys.D0))).ToString();
        }

        if (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9)
        {
            return string.Format("NumPad{0}", keyCode - Keys.NumPad0);
        }

        return keyCode.ToString();
    }

    public static string ToFriendlyString(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return string.Empty;
        }

        var safeToken = token!;
        var parts = safeToken.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < parts.Length; i++)
        {
            var part = parts[i].Trim();
            if (FriendlyNames.TryGetValue(part, out var friendly))
            {
                parts[i] = friendly;
            }
            else if (part.Length == 1)
            {
                parts[i] = part.ToUpperInvariant();
            }
        }

        return string.Join("+", parts);
    }

    public static string ToCharacterPreview(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return string.Empty;
        }

        var safeToken = token!;

        if (safeToken.Length == 1)
        {
            return safeToken.ToLowerInvariant();
        }

        if (safeToken.StartsWith("NumPad", StringComparison.OrdinalIgnoreCase))
        {
            return safeToken.Substring("NumPad".Length);
        }

        if (safeToken.Length == 2 && safeToken[0] == 'D' && char.IsDigit(safeToken[1]))
        {
            return safeToken[1].ToString();
        }

        if (FriendlyNames.TryGetValue(safeToken, out var friendly))
        {
            return friendly;
        }

        return string.Empty;
    }
    }
}

