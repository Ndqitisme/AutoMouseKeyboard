
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Services;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class MouseActionForm : Form
    {
    public ActionStep? Result { get; private set; }
    private static bool _hasShownCapturePositionHint = false;

    public MouseActionForm(MouseButtonKind defaultKind)
    {
        Result = null;
        InitializeComponent();
        PopulateMouseActions(defaultKind);
        ThemeManager.RegisterForm(this);
        LanguageManager.RegisterForm(this);
        LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
        UpdateLanguage();
    }

    private void LanguageManager_LanguageChanged(object? sender, LanguageChangedEventArgs e)
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        Text = LanguageManager.GetString("MouseActionForm_Title");
        lblAction.Text = LanguageManager.GetString("MouseActionForm_Action");
        lblModifiers.Text = LanguageManager.GetString("MouseActionForm_Modifiers");
        chkCtrl.Text = LanguageManager.GetString("MouseActionForm_Ctrl");
        chkAlt.Text = LanguageManager.GetString("MouseActionForm_Alt");
        chkShift.Text = LanguageManager.GetString("MouseActionForm_Shift");
        lblPosition.Text = LanguageManager.GetString("MouseActionForm_Position");
        btnGetPosition.Text = LanguageManager.GetString("MouseActionForm_SetPosition");
        lblCount.Text = LanguageManager.GetString("MouseActionForm_Count");
        lblInterval.Text = LanguageManager.GetString("MouseActionForm_Interval");
        lblIntervalHint.Text = LanguageManager.GetString("MouseActionForm_IntervalHint");
        btnOk.Text = LanguageManager.GetString("MouseActionForm_OK");
        btnCancel.Text = LanguageManager.GetString("MouseActionForm_Cancel");
    }

    public MouseActionForm(ActionStep existingStep) : this(existingStep.MouseButton)
    {
        chkCtrl.Checked = existingStep.HoldCtrl;
        chkAlt.Checked = existingStep.HoldAlt;
        chkShift.Checked = existingStep.HoldShift;
        numPosX.Value = ClampToRange(existingStep.X, numPosX);
        numPosY.Value = ClampToRange(existingStep.Y, numPosY);
        numCount.Value = Math.Max(1, Math.Min(1000, existingStep.Repeat));
        numInterval.Value = Math.Max(0, Math.Min(600000, existingStep.Delay));
    }

    private void PopulateMouseActions(MouseButtonKind defaultKind)
    {
        cboMouseAction.DisplayMember = "Label";
        foreach (var option in ActionStep.MouseOptions)
        {
            cboMouseAction.Items.Add(option);
        }

        var defaultIndex = 0;
        for (var i = 0; i < ActionStep.MouseOptions.Count; i++)
        {
            if (ActionStep.MouseOptions[i].Kind == defaultKind)
            {
                defaultIndex = i;
                break;
            }
        }

        cboMouseAction.SelectedIndex = defaultIndex;
    }

    private async void btnGetPosition_Click(object? sender, EventArgs e)
    {
        await CapturePositionAsync();
    }

    private async Task CapturePositionAsync()
    {
        if (!_hasShownCapturePositionHint && FirstRunHelper.ShouldShowCapturePositionHint())
        {
            MessageBox.Show(LanguageManager.GetString("Msg_CapturePosition"), LanguageManager.GetString("Msg_CapturePositionTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            FirstRunHelper.MarkCapturePositionHintShown();
            _hasShownCapturePositionHint = true;
        }

        if (IsDisposed || Disposing)
        {
            return;
        }

        using (FormHelper.HideFormForOperation(this, hideOwner: false))
        {
            try
            {
                var point = await MouseCaptureService.CaptureAsync();

                if (IsDisposed || Disposing)
                {
                    return;
                }

                FormHelper.SafeInvoke(this, () =>
                {
                    if (!IsDisposed && !Disposing)
                    {
                        numPosX.Value = ClampToRange(point.X, numPosX);
                        numPosY.Value = ClampToRange(point.Y, numPosY);
                    }
                });
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing)
                {
                    return;
                }

                FormHelper.SafeInvoke(this, () =>
                {
                    if (!IsDisposed && !Disposing)
                    {
                        MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_CaptureFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
        }
    }

    private decimal ClampToRange(int value, ModernNumericUpDown control)
    {
        if (value > control.Maximum)
        {
            return control.Maximum;
        }

        if (value < control.Minimum)
        {
            return control.Minimum;
        }

        return value;
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        var option = cboMouseAction.SelectedItem as ActionStep.MouseActionOption;
        if (option == null)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }

        var kind = option.Kind;

        Result = new ActionStep
        {
            Type = ActionKind.Mouse,
            MouseButton = kind,
            HoldCtrl = chkCtrl.Checked,
            HoldAlt = chkAlt.Checked,
            HoldShift = chkShift.Checked,
            X = (int)numPosX.Value,
            Y = (int)numPosY.Value,
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

