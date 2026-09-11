
using System;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class SettingForm : Form
    {
    private bool _isLoading;

    public SettingForm()
    {
        _isLoading = true;
        InitializeComponent();
        ThemeManager.RegisterForm(this);
        LanguageManager.RegisterForm(this);
        LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;

        // Selections are applied inside UpdateLanguage() once the item lists
        // exist; assigning them here (empty Items) would be a no-op.
        cboTheme!.SelectedIndexChanged += CboTheme_SelectedIndexChanged;
        cboLanguage!.SelectedIndexChanged += CboLanguage_SelectedIndexChanged;

        chkRunOnStartup!.Checked = StartupHelper.IsEnabled();
        AppSettings.RunOnStartup = chkRunOnStartup.Checked;
        chkHideOnComplete!.Checked = AppSettings.HideOnCompletion;
        chkRunOnStartup.CheckedChanged += chkRunOnStartup_CheckedChanged;
        chkHideOnComplete.CheckedChanged += chkHideOnComplete_CheckedChanged;

        UpdateLanguage();
        _isLoading = false;
    }

    private void LanguageManager_LanguageChanged(object? sender, LanguageChangedEventArgs e)
    {
        UpdateLanguage();
    }

    private void CboTheme_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboTheme.SelectedIndex >= 0 && cboTheme.SelectedIndex <= 10)
        {
            var newTheme = (ThemeMode)cboTheme.SelectedIndex;
            ThemeManager.SetTheme(newTheme);
        }
    }

    private void CboLanguage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        // Mirror the theme guard: an out-of-range index would cast to an
        // undefined Language and throw in LanguageCultures[...].
        if (cboLanguage.SelectedIndex < 0 || cboLanguage.SelectedIndex > (int)Language.Dutch)
        {
            return;
        }

        var newLanguage = (Language)cboLanguage.SelectedIndex;
        LanguageManager.SetLanguage(newLanguage);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        ThemeManager.UnregisterForm(this);
        LanguageManager.UnregisterForm(this);
        base.OnFormClosing(e);
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    public void UpdateLanguage()
    {
        grpAppearance.Text = LanguageManager.GetString("SettingForm_AppearanceGroup");
        grpBehavior.Text = LanguageManager.GetString("SettingForm_BehaviorGroup");
        lblTheme.Text = LanguageManager.GetString("SettingForm_Theme");
        lblLanguage.Text = LanguageManager.GetString("SettingForm_Language");
        chkRunOnStartup.Text = LanguageManager.GetString("SettingForm_RunOnStartup");
        chkHideOnComplete.Text = LanguageManager.GetString("SettingForm_HideOnComplete");
        btnOk.Text = LanguageManager.GetString("SettingForm_OK");
        Text = LanguageManager.GetString("SettingForm_Title");

        lblTheme.AutoSize = true;
        lblLanguage.AutoSize = true;

        cboTheme.Items.Clear();
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Light"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Dark"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Cyan"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Pink"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Green"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Red"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Orange"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Yellow"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Blue"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Indigo"));
        cboTheme.Items.Add(LanguageManager.GetString("SettingForm_Violet"));
        cboTheme.SelectedIndex = (int)ThemeManager.CurrentTheme;

        cboLanguage.Items.Clear();
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.English));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Vietnamese));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Chinese));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.ChineseTraditional));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Spanish));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Arabic));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Hindi));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Portuguese));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.French));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Russian));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Japanese));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.German));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Korean));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Italian));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Turkish));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Indonesian));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Bengali));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Polish));
        cboLanguage.Items.Add(LanguageManager.GetLanguageDisplayName(Language.Dutch));
        cboLanguage.SelectedIndex = (int)LanguageManager.CurrentLanguage;
    }

    private void chkRunOnStartup_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        var enabled = chkRunOnStartup.Checked;
        AppSettings.RunOnStartup = enabled;
        StartupHelper.SetStartup(enabled);
    }

    private void chkHideOnComplete_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        AppSettings.HideOnCompletion = chkHideOnComplete.Checked;
    }
    }
}

