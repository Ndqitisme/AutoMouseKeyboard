using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class KeyActionForm
{
    private IContainer components = null;
    private TableLayoutPanel rootLayout = null;
    private TableLayoutPanel fieldsLayout = null;
    private Label lblKey = null;
    private ModernSelect cboKey = null;
    private Label lblModifiers = null;
    private FlowLayoutPanel modifiersFlow = null;
    private ModernCheckBox chkCtrl = null;
    private ModernCheckBox chkAlt = null;
    private ModernCheckBox chkShift = null;
    private Label lblCount = null;
    private ModernNumericUpDown numCount = null;
    private Label lblInterval = null;
    private FlowLayoutPanel intervalFlow = null;
    private ModernNumericUpDown numInterval = null;
    private Label lblIntervalHint = null;
    private FlowLayoutPanel buttonsFlow = null;
    private ModernButton btnOk = null;
    private ModernButton btnCancel = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        rootLayout = new TableLayoutPanel();
        fieldsLayout = new TableLayoutPanel();
        lblKey = new Label();
        cboKey = new ModernSelect();
        lblModifiers = new Label();
        modifiersFlow = new FlowLayoutPanel();
        chkCtrl = new ModernCheckBox();
        chkAlt = new ModernCheckBox();
        chkShift = new ModernCheckBox();
        lblCount = new Label();
        numCount = new ModernNumericUpDown();
        lblInterval = new Label();
        intervalFlow = new FlowLayoutPanel();
        numInterval = new ModernNumericUpDown();
        lblIntervalHint = new Label();
        buttonsFlow = new FlowLayoutPanel();
        btnOk = new ModernButton();
        btnCancel = new ModernButton();
        rootLayout.SuspendLayout();
        fieldsLayout.SuspendLayout();
        modifiersFlow.SuspendLayout();
        intervalFlow.SuspendLayout();
        buttonsFlow.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(fieldsLayout, 0, 0);
        rootLayout.Controls.Add(buttonsFlow, 0, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(12);
        rootLayout.RowCount = 2;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.TabIndex = 0;
        //
        // fieldsLayout
        //
        fieldsLayout.AutoSize = true;
        fieldsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        fieldsLayout.BackColor = Color.Transparent;
        fieldsLayout.ColumnCount = 2;
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        fieldsLayout.Controls.Add(lblKey, 0, 0);
        fieldsLayout.Controls.Add(cboKey, 1, 0);
        fieldsLayout.Controls.Add(lblModifiers, 0, 1);
        fieldsLayout.Controls.Add(modifiersFlow, 1, 1);
        fieldsLayout.Controls.Add(lblCount, 0, 2);
        fieldsLayout.Controls.Add(numCount, 1, 2);
        fieldsLayout.Controls.Add(lblInterval, 0, 3);
        fieldsLayout.Controls.Add(intervalFlow, 1, 3);
        fieldsLayout.Dock = DockStyle.Fill;
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 4;
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.TabIndex = 0;
        //
        // lblKey
        //
        lblKey.Anchor = AnchorStyles.Left;
        lblKey.AutoSize = true;
        lblKey.BackColor = Color.Transparent;
        lblKey.Margin = new Padding(0, 0, 8, 0);
        lblKey.Name = "lblKey";
        lblKey.TabIndex = 0;
        lblKey.Text = "Key:";
        //
        // cboKey
        //
        cboKey.Dock = DockStyle.Fill;
        cboKey.Margin = new Padding(0, 0, 0, 6);
        cboKey.Name = "cboKey";
        cboKey.Size = new Size(220, 28);
        cboKey.TabIndex = 0;
        //
        // lblModifiers
        //
        lblModifiers.Anchor = AnchorStyles.Left;
        lblModifiers.AutoSize = true;
        lblModifiers.BackColor = Color.Transparent;
        lblModifiers.Margin = new Padding(0, 0, 8, 0);
        lblModifiers.Name = "lblModifiers";
        lblModifiers.TabIndex = 1;
        lblModifiers.Text = "Giữ kèm phím:";
        //
        // modifiersFlow
        //
        modifiersFlow.AutoSize = true;
        modifiersFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        modifiersFlow.BackColor = Color.Transparent;
        modifiersFlow.Controls.Add(chkCtrl);
        modifiersFlow.Controls.Add(chkAlt);
        modifiersFlow.Controls.Add(chkShift);
        modifiersFlow.Dock = DockStyle.Fill;
        modifiersFlow.FlowDirection = FlowDirection.LeftToRight;
        modifiersFlow.Margin = new Padding(0, 0, 0, 6);
        modifiersFlow.Name = "modifiersFlow";
        modifiersFlow.TabIndex = 1;
        modifiersFlow.WrapContents = false;
        //
        // chkCtrl
        //
        chkCtrl.Anchor = AnchorStyles.None;
        chkCtrl.AutoSize = true;
        chkCtrl.Margin = new Padding(0, 0, 12, 0);
        chkCtrl.Name = "chkCtrl";
        chkCtrl.TabIndex = 1;
        chkCtrl.Text = "Ctrl";
        //
        // chkAlt
        //
        chkAlt.Anchor = AnchorStyles.None;
        chkAlt.AutoSize = true;
        chkAlt.Margin = new Padding(0, 0, 12, 0);
        chkAlt.Name = "chkAlt";
        chkAlt.TabIndex = 2;
        chkAlt.Text = "Alt";
        //
        // chkShift
        //
        chkShift.Anchor = AnchorStyles.None;
        chkShift.AutoSize = true;
        chkShift.Margin = new Padding(0);
        chkShift.Name = "chkShift";
        chkShift.TabIndex = 3;
        chkShift.Text = "Shift";
        //
        // lblCount
        //
        lblCount.Anchor = AnchorStyles.Left;
        lblCount.AutoSize = true;
        lblCount.BackColor = Color.Transparent;
        lblCount.Margin = new Padding(0, 0, 8, 0);
        lblCount.Name = "lblCount";
        lblCount.TabIndex = 2;
        lblCount.Text = "Count";
        //
        // numCount
        //
        numCount.Margin = new Padding(0, 0, 0, 6);
        numCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCount.Name = "numCount";
        numCount.Size = new Size(110, 30);
        numCount.TabIndex = 4;
        numCount.TextAlign = HorizontalAlignment.Center;
        numCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // lblInterval
        //
        lblInterval.Anchor = AnchorStyles.Left;
        lblInterval.AutoSize = true;
        lblInterval.BackColor = Color.Transparent;
        lblInterval.Margin = new Padding(0, 0, 8, 0);
        lblInterval.Name = "lblInterval";
        lblInterval.TabIndex = 3;
        lblInterval.Text = "Interval (ms)";
        //
        // intervalFlow
        //
        intervalFlow.AutoSize = true;
        intervalFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        intervalFlow.BackColor = Color.Transparent;
        intervalFlow.Controls.Add(numInterval);
        intervalFlow.Controls.Add(lblIntervalHint);
        intervalFlow.Dock = DockStyle.Fill;
        intervalFlow.FlowDirection = FlowDirection.LeftToRight;
        intervalFlow.Margin = new Padding(0);
        intervalFlow.Name = "intervalFlow";
        intervalFlow.TabIndex = 2;
        intervalFlow.WrapContents = false;
        //
        // numInterval
        //
        numInterval.Increment = new decimal(new int[] { 50, 0, 0, 0 });
        numInterval.Margin = new Padding(0, 0, 8, 0);
        numInterval.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
        numInterval.Name = "numInterval";
        numInterval.Size = new Size(110, 30);
        numInterval.TabIndex = 5;
        numInterval.TextAlign = HorizontalAlignment.Center;
        numInterval.Value = new decimal(new int[] { 300, 0, 0, 0 });
        //
        // lblIntervalHint
        //
        lblIntervalHint.Anchor = AnchorStyles.None;
        lblIntervalHint.AutoSize = true;
        lblIntervalHint.BackColor = Color.Transparent;
        lblIntervalHint.ForeColor = ThemeManager.Palette.TextMuted;
        lblIntervalHint.Margin = new Padding(0);
        lblIntervalHint.Name = "lblIntervalHint";
        lblIntervalHint.TabIndex = 4;
        lblIntervalHint.Tag = ThemeManager.SkipThemeTag;
        lblIntervalHint.Text = "(1 giây = 1000 mili giây)";
        //
        // buttonsFlow
        //
        buttonsFlow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonsFlow.AutoSize = true;
        buttonsFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonsFlow.BackColor = Color.Transparent;
        buttonsFlow.Controls.Add(btnOk);
        buttonsFlow.Controls.Add(btnCancel);
        buttonsFlow.FlowDirection = FlowDirection.LeftToRight;
        buttonsFlow.Margin = new Padding(0, 8, 0, 0);
        buttonsFlow.Name = "buttonsFlow";
        buttonsFlow.TabIndex = 3;
        buttonsFlow.WrapContents = false;
        //
        // btnOk
        //
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(96, 32);
        btnOk.StyleKind = ButtonStyleKind.Primary;
        btnOk.TabIndex = 6;
        btnOk.Text = "OK";
        btnOk.Click += btnOk_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Margin = new Padding(0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(96, 32);
        btnCancel.TabIndex = 7;
        btnCancel.Text = "Cancel";
        //
        // KeyActionForm
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(420, 240);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "KeyActionForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Add Key";
        modifiersFlow.ResumeLayout(false);
        modifiersFlow.PerformLayout();
        intervalFlow.ResumeLayout(false);
        intervalFlow.PerformLayout();
        buttonsFlow.ResumeLayout(false);
        buttonsFlow.PerformLayout();
        fieldsLayout.ResumeLayout(false);
        fieldsLayout.PerformLayout();
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    }
}
