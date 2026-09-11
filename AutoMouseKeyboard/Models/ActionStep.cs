using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.Models
{
    public enum ActionKind
{
    Mouse,
    Keyboard
}

public enum MouseButtonKind
{
    LeftClick = 0,
    RightClick = 1,
    LeftDouble = 2,
    RightDouble = 3,
    MiddleClick = 4,
    MiddleDouble = 5
}

public class ActionStep
{
    public sealed class MouseActionOption
    {
        public MouseActionOption(MouseButtonKind kind, string label)
        {
            Kind = kind;
            Label = label;
        }

        public MouseButtonKind Kind { get; }
        public string Label { get; }
    }

    private static MouseActionOption[] CreateMouseOptions()
    {
        return new[]
        {
            new MouseActionOption(MouseButtonKind.LeftClick, GetMouseActionLabel(MouseButtonKind.LeftClick)),
            new MouseActionOption(MouseButtonKind.LeftDouble, GetMouseActionLabel(MouseButtonKind.LeftDouble)),
            new MouseActionOption(MouseButtonKind.MiddleClick, GetMouseActionLabel(MouseButtonKind.MiddleClick)),
            new MouseActionOption(MouseButtonKind.MiddleDouble, GetMouseActionLabel(MouseButtonKind.MiddleDouble)),
            new MouseActionOption(MouseButtonKind.RightClick, GetMouseActionLabel(MouseButtonKind.RightClick)),
            new MouseActionOption(MouseButtonKind.RightDouble, GetMouseActionLabel(MouseButtonKind.RightDouble))
        };
    }

    private static string GetMouseActionLabel(MouseButtonKind kind)
    {
        return kind switch
        {
            MouseButtonKind.LeftClick => LanguageManager.GetString("MouseAction_LeftClick"),
            MouseButtonKind.LeftDouble => LanguageManager.GetString("MouseAction_LeftDouble"),
            MouseButtonKind.MiddleClick => LanguageManager.GetString("MouseAction_MiddleClick"),
            MouseButtonKind.MiddleDouble => LanguageManager.GetString("MouseAction_MiddleDouble"),
            MouseButtonKind.RightClick => LanguageManager.GetString("MouseAction_RightClick"),
            MouseButtonKind.RightDouble => LanguageManager.GetString("MouseAction_RightDouble"),
            _ => kind.ToString()
        };
    }

    [JsonProperty("type")]
    public ActionKind Type { get; set; } = ActionKind.Mouse;

    [JsonProperty("position")]
    public ActionPosition Position { get; set; } = new ActionPosition();

    [JsonProperty("mouseButton")]
    public MouseButtonKind MouseButton { get; set; } = MouseButtonKind.LeftClick;

    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    [JsonProperty("delay")]
    public int Delay { get; set; } = 300;

    [JsonProperty("repeat")]
    public int Repeat { get; set; } = 1;

    [JsonProperty("holdCtrl")]
    public bool HoldCtrl { get; set; }

    [JsonProperty("holdAlt")]
    public bool HoldAlt { get; set; }

    [JsonProperty("holdShift")]
    public bool HoldShift { get; set; }

    [JsonIgnore]
    public int X
    {
        get
        {
            if (Position == null)
            {
                return 0;
            }
            return Position.X;
        }
        set
        {
            if (Position == null)
            {
                Position = new ActionPosition();
            }
            Position.X = value;
        }
    }

    [JsonIgnore]
    public int Y
    {
        get
        {
            if (Position == null)
            {
                return 0;
            }
            return Position.Y;
        }
        set
        {
            if (Position == null)
            {
                Position = new ActionPosition();
            }
            Position.Y = value;
        }
    }

    [JsonIgnore]
    public string DisplayKey
        => Type == ActionKind.Mouse
            ? DescribeMouse(MouseButton)
            : BuildDisplayKey();

    [JsonIgnore]
    public string DisplayCharacters
    {
        get
        {
            return Type == ActionKind.Mouse
                ? string.Format("{0},{1}", X, Y)
                : KeyTokenFormatter.ToCharacterPreview(Key);
        }
    }

    [JsonIgnore]
    public int CountDisplay => Repeat;

    [JsonIgnore]
    public int IntervalDisplay => Delay;

    private static MouseActionOption[]? _mouseOptionsCache;
    
    public static IReadOnlyList<MouseActionOption> MouseOptions
    {
        get
        {
            if (_mouseOptionsCache == null)
            {
                _mouseOptionsCache = CreateMouseOptions();
            }
            return _mouseOptionsCache;
        }
    }

    public static void RefreshMouseOptions()
    {
        _mouseOptionsCache = null;
    }

    public static string DescribeMouse(MouseButtonKind kind) => GetMouseActionLabel(kind);

    public ActionStep Clone()
    {
        return new ActionStep
        {
            Type = Type,
            MouseButton = MouseButton,
            Key = Key,
            Delay = Delay,
            Repeat = Repeat,
            HoldCtrl = HoldCtrl,
            HoldAlt = HoldAlt,
            HoldShift = HoldShift,
            X = X,
            Y = Y
        };
    }

    private string BuildDisplayKey()
    {
        var parts = new List<string>();
        if (HoldCtrl) parts.Add("Ctrl");
        if (HoldAlt) parts.Add("Alt");
        if (HoldShift) parts.Add("Shift");

        var friendlyKey = KeyTokenFormatter.ToFriendlyString(Key);
        if (!string.IsNullOrWhiteSpace(friendlyKey))
        {
            parts.Add(friendlyKey);
        }

        return string.Join("+", parts);
    }

    public static string GetMouseButtonText(MouseButtonKind kind) => DescribeMouse(kind);
    }
}

