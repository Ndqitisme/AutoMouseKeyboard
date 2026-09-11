
#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class KeyActionForm : Form
    {
    private sealed class KeyOption
    {
        public string Label { get; }
        public string Token { get; }

        public KeyOption(string label, string token)
        {
            Label = label;
            Token = token;
        }
    }

    public ActionStep Result { get; private set; }

    public KeyActionForm(string defaultKey = null)
    {
        Result = null;
        InitializeComponent();
        PopulateKeys(defaultKey);
        ThemeManager.RegisterForm(this);
        LanguageManager.RegisterForm(this);
        LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
        UpdateLanguage();
    }

    private void LanguageManager_LanguageChanged(object sender, LanguageChangedEventArgs e)
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        Text = LanguageManager.GetString("KeyActionForm_Title");
        lblKey.Text = LanguageManager.GetString("KeyActionForm_Key");
        lblModifiers.Text = LanguageManager.GetString("KeyActionForm_Modifiers");
        chkCtrl.Text = LanguageManager.GetString("KeyActionForm_Ctrl");
        chkAlt.Text = LanguageManager.GetString("KeyActionForm_Alt");
        chkShift.Text = LanguageManager.GetString("KeyActionForm_Shift");
        lblCount.Text = LanguageManager.GetString("KeyActionForm_Count");
        lblInterval.Text = LanguageManager.GetString("KeyActionForm_Interval");
        lblIntervalHint.Text = LanguageManager.GetString("KeyActionForm_IntervalHint");
        btnOk.Text = LanguageManager.GetString("KeyActionForm_OK");
        btnCancel.Text = LanguageManager.GetString("KeyActionForm_Cancel");
    }

    public KeyActionForm(ActionStep existingStep) : this(ParseBaseKey(existingStep?.Key, existingStep, out var parsedCtrl, out var parsedAlt, out var parsedShift))
    {
        chkCtrl.Checked = parsedCtrl || existingStep.HoldCtrl;
        chkAlt.Checked = parsedAlt || existingStep.HoldAlt;
        chkShift.Checked = parsedShift || existingStep.HoldShift;
        numCount.Value = Math.Max(1, Math.Min(1000, existingStep.Repeat));
        numInterval.Value = Math.Max(0, Math.Min(600000, existingStep.Delay));
    }

    private void PopulateKeys(string defaultKey)
    {
        cboKey.DisplayMember = "Label";
        cboKey.ValueMember = "Token";

        var allKeys = new List<KeyOption>();

        allKeys.AddRange(KeyHelper.EnumerateKeys(i => string.Format("F{0}", i), 1, 12).Select(k => new KeyOption(k.Item1, k.Item2)));
        allKeys.AddRange(KeyHelper.EnumerateKeys(i => ((char)('A' + i - 1)).ToString(), 1, 26).Select(k => new KeyOption(k.Item1, k.Item2)));
        allKeys.AddRange(KeyHelper.EnumerateKeys(i => (i - 1).ToString(), 1, 10).Select(k => new KeyOption(k.Item1, k.Item2)));
        allKeys.AddRange(new[]
        {
            new KeyOption("-", "OemMinus"),
            new KeyOption("=", "Oemplus"),
            new KeyOption("[", "OemOpenBrackets"),
            new KeyOption("]", "OemCloseBrackets"),
            new KeyOption(";", "OemSemicolon"),
            new KeyOption("'", "OemQuotes"),
            new KeyOption(",", "Oemcomma"),
            new KeyOption(".", "OemPeriod"),
            new KeyOption("/", "OemQuestion"),
            new KeyOption("\\", "OemPipe"),
            new KeyOption("`", "Oemtilde")
        });

        allKeys.AddRange(KeyHelper.ToOptions(new[]
        {
            "NumPad0", "NumPad1", "NumPad2", "NumPad3", "NumPad4", "NumPad5",
            "NumPad6", "NumPad7", "NumPad8", "NumPad9", "Multiply", "Divide", "Add", "Subtract"
        }).Select(k => new KeyOption(k.Item1, k.Item2)));

        allKeys.AddRange(KeyHelper.ToOptions(new[]
        {
            "Enter", "Space", "Tab", "Esc", "Backspace", "Insert", "Delete",
            "Home", "End", "PageUp", "PageDown", "Up", "Down", "Left", "Right"
        }).Select(k => new KeyOption(k.Item1, k.Item2)));

        foreach (var key in allKeys)
        {
            cboKey.Items.Add(key);
        }

        if (!string.IsNullOrWhiteSpace(defaultKey))
        {
            var defaultIndex = allKeys.FindIndex(k =>
                k.Token.Equals(defaultKey, StringComparison.OrdinalIgnoreCase));
            if (defaultIndex >= 0)
            {
                cboKey.SelectedIndex = defaultIndex;
            }
        }

        if (string.IsNullOrWhiteSpace(defaultKey))
        {
            cboKey.SelectedIndex = -1;
        }
    }

    private static string ParseBaseKey(string raw, ActionStep existingStep, out bool holdCtrl, out bool holdAlt, out bool holdShift)
    {
        holdCtrl = existingStep?.HoldCtrl ?? false;
        holdAlt = existingStep?.HoldAlt ?? false;
        holdShift = existingStep?.HoldShift ?? false;

        if (string.IsNullOrWhiteSpace(raw))
        {
            return raw;
        }

        var tokens = raw.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList();

        string baseKey = null;
        foreach (var token in tokens)
        {
            switch (token.ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                case "controlkey":
                    holdCtrl = true;
                    continue;
                case "alt":
                case "menu":
                    holdAlt = true;
                    continue;
                case "shift":
                case "shiftkey":
                    holdShift = true;
                    continue;
                default:
                    baseKey = token;
                    break;
            }
        }

        if (string.IsNullOrWhiteSpace(baseKey))
        {
            baseKey = null;
        }

        return baseKey;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        var option = cboKey.SelectedItem as KeyOption;

        Result = new ActionStep
        {
            Type = ActionKind.Keyboard,
            Key = option?.Token ?? string.Empty,
            HoldCtrl = chkCtrl.Checked,
            HoldAlt = chkAlt.Checked,
            HoldShift = chkShift.Checked,
            Repeat = (int)numCount.Value,
            Delay = (int)numInterval.Value
        };

        DialogResult = DialogResult.OK;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        ThemeManager.UnregisterForm(this);
        LanguageManager.UnregisterForm(this);
        base.OnFormClosing(e);
    }
    }
}

