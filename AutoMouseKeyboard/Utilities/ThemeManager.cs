
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public enum ThemeMode
    {
    Light,
    Dark,
    Cyan,
    Pink,
    Green,
    Red,
    Orange,
    Yellow,
    Blue,
    Indigo,
    Violet
}

public static class ThemeManager
{
    private static ThemeMode _currentTheme = ThemeMode.Light;
    private static readonly List<Form> RegisteredForms = new List<Form>();

    public static ThemeMode CurrentTheme => _currentTheme;

    public static event EventHandler<ThemeMode>? ThemeChanged;

    static ThemeManager()
    {
        LoadTheme();
    }

    public static void RegisterForm(Form form)
    {
        if (!RegisteredForms.Contains(form))
        {
            RegisteredForms.Add(form);
            ApplyTheme(form, _currentTheme);
        }
    }

    public static void UnregisterForm(Form form)
    {
        RegisteredForms.Remove(form);
    }

    public static void SetTheme(ThemeMode theme, bool save = true)
    {
        if (_currentTheme == theme)
        {
            return;
        }

        _currentTheme = theme;
        foreach (var form in RegisteredForms.ToList())
        {
            if (form != null && !form.IsDisposed)
            {
                ApplyTheme(form, theme);
            }
        }

        if (save)
        {
            SaveTheme(theme);
        }

        ThemeChanged?.Invoke(null, theme);
    }

    private static void LoadTheme()
    {
        try
        {
            var theme = AppSettings.Theme;
            SetTheme(theme, false);
            
            var baseFolder = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AutoMouseKeyboard");
            var oldThemeFile = System.IO.Path.Combine(baseFolder, "theme.txt");
            if (System.IO.File.Exists(oldThemeFile))
            {
                try
                {
                    var themeStr = System.IO.File.ReadAllText(oldThemeFile).Trim();
                    if (Enum.TryParse<ThemeMode>(themeStr, out var oldTheme))
                    {
                        AppSettings.Theme = oldTheme;
                        SetTheme(oldTheme, false);
                        System.IO.File.Delete(oldThemeFile);
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    private static void SaveTheme(ThemeMode theme)
    {
        try
        {
            AppSettings.Theme = theme;
        }
        catch
        {
        }
    }

    public static void ApplyTheme(Form form, ThemeMode theme)
    {
        if (form == null || form.IsDisposed)
        {
            return;
        }

        var isDark = theme == ThemeMode.Dark;
        var isCyan = theme == ThemeMode.Cyan;
        var isPink = theme == ThemeMode.Pink;
        var isGreen = theme == ThemeMode.Green;
        var isRed = theme == ThemeMode.Red;
        var isOrange = theme == ThemeMode.Orange;
        var isYellow = theme == ThemeMode.Yellow;
        var isBlue = theme == ThemeMode.Blue;
        var isIndigo = theme == ThemeMode.Indigo;
        var isViolet = theme == ThemeMode.Violet;

        if (isDark)
        {
            form.BackColor = Color.FromArgb(30, 30, 30);
            form.ForeColor = Color.White;
        }
        else if (isCyan)
        {
            form.BackColor = Color.FromArgb(224, 247, 250);
            form.ForeColor = Color.FromArgb(0, 96, 100);
        }
        else if (isPink)
        {
            form.BackColor = Color.FromArgb(252, 228, 236);
            form.ForeColor = Color.FromArgb(136, 14, 79);
        }
        else if (isGreen)
        {
            form.BackColor = Color.FromArgb(200, 230, 201);
            form.ForeColor = Color.FromArgb(27, 94, 32);
        }
        else if (isRed)
        {
            form.BackColor = Color.FromArgb(255, 235, 238);
            form.ForeColor = Color.FromArgb(183, 28, 28);
        }
        else if (isOrange)
        {
            form.BackColor = Color.FromArgb(255, 243, 224);
            form.ForeColor = Color.FromArgb(230, 81, 0);
        }
        else if (isYellow)
        {
            form.BackColor = Color.FromArgb(255, 253, 231);
            form.ForeColor = Color.FromArgb(102, 51, 0);
        }
        else if (isBlue)
        {
            form.BackColor = Color.FromArgb(227, 242, 253);
            form.ForeColor = Color.FromArgb(13, 71, 161);
        }
        else if (isIndigo)
        {
            form.BackColor = Color.FromArgb(232, 234, 246);
            form.ForeColor = Color.FromArgb(26, 35, 126);
        }
        else if (isViolet)
        {
            form.BackColor = Color.FromArgb(243, 229, 245);
            form.ForeColor = Color.FromArgb(74, 20, 140);
        }
        else
        {
            form.BackColor = SystemColors.Control;
            form.ForeColor = Color.Black;
        }

        ApplyThemeToControls(form.Controls, theme);
    }

    private static (Color BackColor, Color ForeColor, Color AccentColor, Color BorderColor, Color PanelBackColor) GetThemeColors(ThemeMode theme)
    {
        return theme switch
        {
            ThemeMode.Dark => (Color.FromArgb(30, 30, 30), Color.White, Color.FromArgb(60, 60, 60), Color.FromArgb(80, 80, 80), Color.FromArgb(40, 40, 40)),
            ThemeMode.Cyan => (Color.FromArgb(224, 247, 250), Color.FromArgb(0, 96, 100), Color.FromArgb(0, 188, 212), Color.FromArgb(0, 151, 167), Color.FromArgb(178, 235, 242)),
            ThemeMode.Pink => (Color.FromArgb(252, 228, 236), Color.FromArgb(136, 14, 79), Color.FromArgb(233, 30, 99), Color.FromArgb(194, 24, 91), Color.FromArgb(248, 187, 208)),
            ThemeMode.Green => (Color.FromArgb(200, 230, 201), Color.FromArgb(27, 94, 32), Color.FromArgb(76, 175, 80), Color.FromArgb(56, 142, 60), Color.FromArgb(165, 214, 167)),
            ThemeMode.Red => (Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28), Color.FromArgb(244, 67, 54), Color.FromArgb(198, 40, 40), Color.FromArgb(239, 154, 154)),
            ThemeMode.Orange => (Color.FromArgb(255, 243, 224), Color.FromArgb(230, 81, 0), Color.FromArgb(255, 152, 0), Color.FromArgb(245, 124, 0), Color.FromArgb(255, 204, 128)),
            ThemeMode.Yellow => (Color.FromArgb(255, 253, 231), Color.FromArgb(102, 51, 0), Color.FromArgb(255, 193, 7), Color.FromArgb(255, 152, 0), Color.FromArgb(255, 245, 157)),
            ThemeMode.Blue => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161), Color.FromArgb(33, 150, 243), Color.FromArgb(25, 118, 210), Color.FromArgb(144, 202, 249)),
            ThemeMode.Indigo => (Color.FromArgb(232, 234, 246), Color.FromArgb(26, 35, 126), Color.FromArgb(63, 81, 181), Color.FromArgb(48, 63, 159), Color.FromArgb(159, 168, 218)),
            ThemeMode.Violet => (Color.FromArgb(243, 229, 245), Color.FromArgb(74, 20, 140), Color.FromArgb(156, 39, 176), Color.FromArgb(123, 31, 162), Color.FromArgb(206, 147, 216)),
            _ => (SystemColors.Control, SystemColors.ControlText, SystemColors.ButtonFace, SystemColors.ControlDark, SystemColors.Control)
        };
    }

    private static void ApplyThemeToControls(Control.ControlCollection controls, ThemeMode theme)
    {
        var isDark = theme == ThemeMode.Dark;
        var colors = GetThemeColors(theme);

        foreach (Control control in controls)
        {
            if (control == null)
            {
                continue;
            }

            if (control is DataGridView grid)
            {
                ApplyThemeToDataGridView(grid, theme);
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.BackColor = isDark ? Color.FromArgb(30, 30, 30) : colors.BackColor;
                groupBox.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
                ApplyThemeToControls(groupBox.Controls, theme);
            }
            else if (control is Panel || control is ToolStrip)
            {
                control.BackColor = isDark ? Color.FromArgb(40, 40, 40) : colors.PanelBackColor;
                control.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
                ApplyThemeToControls(control.Controls, theme);
            }
            else if (control is Label label)
            {
                label.BackColor = isDark ? Color.FromArgb(30, 30, 30) : colors.BackColor;
                label.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
                textBox.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
                comboBox.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }
            else if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
                numericUpDown.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = isDark ? Color.FromArgb(45, 45, 45) : Color.White;
                listBox.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }
            else if (control is Button button)
            {
                if (theme == ThemeMode.Light)
                {
                    button.BackColor = SystemColors.ButtonFace;
                    button.ForeColor = Color.Black;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = SystemColors.ControlDark;
                }
                else if (theme == ThemeMode.Yellow)
                {
                    button.BackColor = colors.AccentColor;
                    button.ForeColor = Color.FromArgb(102, 51, 0);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = colors.BorderColor;
                }
                else
                {
                    button.BackColor = isDark ? Color.FromArgb(60, 60, 60) : colors.AccentColor;
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = isDark ? Color.FromArgb(80, 80, 80) : colors.BorderColor;
                }
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.BackColor = isDark ? Color.FromArgb(30, 30, 30) : colors.BackColor;
                checkBox.ForeColor = isDark ? Color.White : (theme == ThemeMode.Light ? Color.Black : colors.ForeColor);
            }

            if (control.HasChildren)
            {
                ApplyThemeToControls(control.Controls, theme);
            }
        }
    }

    private static void ApplyThemeToDataGridView(DataGridView grid, ThemeMode theme)
    {
        var isDark = theme == ThemeMode.Dark;
        var colors = GetThemeColors(theme);

        if (isDark)
        {
            grid.BackgroundColor = Color.FromArgb(30, 30, 30);
            grid.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            grid.DefaultCellStyle.ForeColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            grid.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.GridColor = Color.FromArgb(60, 60, 60);
        }
        else
        {
            grid.BackgroundColor = colors.BackColor;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = theme == ThemeMode.Light ? Color.Black : colors.ForeColor;
            grid.DefaultCellStyle.SelectionBackColor = theme == ThemeMode.Light ? SystemColors.Highlight : colors.AccentColor;
            grid.DefaultCellStyle.SelectionForeColor = theme == ThemeMode.Light ? SystemColors.HighlightText : Color.White;
            grid.ColumnHeadersDefaultCellStyle.BackColor = colors.PanelBackColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = theme == ThemeMode.Light ? Color.Black : colors.ForeColor;
            grid.RowHeadersDefaultCellStyle.BackColor = colors.PanelBackColor;
            grid.RowHeadersDefaultCellStyle.ForeColor = theme == ThemeMode.Light ? Color.Black : colors.ForeColor;
            if (theme == ThemeMode.Light)
            {
                grid.GridColor = SystemColors.ControlDark;
            }
            else
            {
                var gridColor = Color.FromArgb(
                    Math.Min(255, colors.AccentColor.R + 50),
                    Math.Min(255, colors.AccentColor.G + 50),
                    Math.Min(255, colors.AccentColor.B + 50));
                grid.GridColor = gridColor;
            }
        }
    }
    }
}

