using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class MouseActionForm
{
    private IContainer components = null;
    private TableLayoutPanel rootLayout = null;
    private TableLayoutPanel fieldsLayout = null;
    private Label lblAction = null;
    private ModernSelect cboMouseAction = null;
    private Label lblModifiers = null;
    private FlowLayoutPanel modifiersFlow = null;
    private ModernCheckBox chkCtrl = null;
    private ModernCheckBox chkAlt = null;
    private ModernCheckBox chkShift = null;
    private Label lblPosition = null;
    private FlowLayoutPanel positionFlow = null;
    private ModernNumericUpDown numPosX = null;
    private ModernNumericUpDown numPosY = null;
    private ModernButton btnGetPosition = null;
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
        lblAction = new Label();
        cboMouseAction = new ModernSelect();
        lblModifiers = new Label();
        modifiersFlow = new FlowLayoutPanel();
        chkCtrl = new ModernCheckBox();
        chkAlt = new ModernCheckBox();
        chkShift = new ModernCheckBox();
        lblPosition = new Label();
        positionFlow = new FlowLayoutPanel();
        numPosX = new ModernNumericUpDown();
        numPosY = new ModernNumericUpDown();
        btnGetPosition = new ModernButton();
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
        positionFlow.SuspendLayout();
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
        fieldsLayout.Controls.Add(lblAction, 0, 0);
        fieldsLayout.Controls.Add(cboMouseAction, 1, 0);
        fieldsLayout.Controls.Add(lblModifiers, 0, 1);
        fieldsLayout.Controls.Add(modifiersFlow, 1, 1);
        fieldsLayout.Controls.Add(lblPosition, 0, 2);
        fieldsLayout.Controls.Add(positionFlow, 1, 2);
        fieldsLayout.Controls.Add(lblCount, 0, 3);
        fieldsLayout.Controls.Add(numCount, 1, 3);
        fieldsLayout.Controls.Add(lblInterval, 0, 4);
        fieldsLayout.Controls.Add(intervalFlow, 1, 4);
        fieldsLayout.Dock = DockStyle.Fill;
        fieldsLayout.Margin = new Padding(0);
        fieldsLayout.Name = "fieldsLayout";
        fieldsLayout.RowCount = 5;
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        fieldsLayout.TabIndex = 0;
        //
        // lblAction
        //
        lblAction.Anchor = AnchorStyles.Left;
        lblAction.AutoSize = true;
        lblAction.BackColor = Color.Transparent;
        lblAction.Margin = new Padding(0, 0, 8, 0);
        lblAction.Name = "lblAction";
        lblAction.TabIndex = 0;
        lblAction.Text = "Loại click (Key):";
        //
        // cboMouseAction
        //
        cboMouseAction.Dock = DockStyle.Fill;
        cboMouseAction.Margin = new Padding(0, 0, 0, 6);
        cboMouseAction.Name = "cboMouseAction";
        cboMouseAction.Size = new Size(220, 28);
        cboMouseAction.TabIndex = 0;
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
        // lblPosition
        //
        lblPosition.Anchor = AnchorStyles.Left;
        lblPosition.AutoSize = true;
        lblPosition.BackColor = Color.Transparent;
        lblPosition.Margin = new Padding(0, 0, 8, 0);
        lblPosition.Name = "lblPosition";
        lblPosition.TabIndex = 2;
        lblPosition.Text = "Tọa độ X,Y";
        //
        // positionFlow
        //
        positionFlow.AutoSize = true;
        positionFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        positionFlow.BackColor = Color.Transparent;
        positionFlow.Controls.Add(numPosX);
        positionFlow.Controls.Add(numPosY);
        positionFlow.Controls.Add(btnGetPosition);
        positionFlow.Dock = DockStyle.Fill;
        positionFlow.FlowDirection = FlowDirection.LeftToRight;
        positionFlow.Margin = new Padding(0, 0, 0, 6);
        positionFlow.Name = "positionFlow";
        positionFlow.TabIndex = 2;
        positionFlow.WrapContents = false;
        //
        // numPosX
        //
        numPosX.Margin = new Padding(0, 0, 8, 0);
        numPosX.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
        numPosX.Minimum = new decimal(new int[] { 20000, 0, 0, -2147483648 });
        numPosX.Name = "numPosX";
        numPosX.Size = new Size(80, 30);
        numPosX.TabIndex = 4;
        numPosX.TextAlign = HorizontalAlignment.Center;
        //
        // numPosY
        //
        numPosY.Margin = new Padding(0, 0, 8, 0);
        numPosY.Maximum = new decimal(new int[] { 20000, 0, 0, 0 });
        numPosY.Minimum = new decimal(new int[] { 20000, 0, 0, -2147483648 });
        numPosY.Name = "numPosY";
        numPosY.Size = new Size(80, 30);
        numPosY.TabIndex = 5;
        numPosY.TextAlign = HorizontalAlignment.Center;
        //
        // btnGetPosition
        //
        btnGetPosition.AutoSize = true;
        btnGetPosition.Margin = new Padding(0);
        btnGetPosition.Name = "btnGetPosition";
        btnGetPosition.TabIndex = 6;
        btnGetPosition.Text = "Đặt tọa độ";
        btnGetPosition.Click += btnGetPosition_Click;
        //
        // lblCount
        //
        lblCount.Anchor = AnchorStyles.Left;
        lblCount.AutoSize = true;
        lblCount.BackColor = Color.Transparent;
        lblCount.Margin = new Padding(0, 0, 8, 0);
        lblCount.Name = "lblCount";
        lblCount.TabIndex = 3;
        lblCount.Text = "Count";
        //
        // numCount
        //
        numCount.Margin = new Padding(0, 0, 0, 6);
        numCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCount.Name = "numCount";
        numCount.Size = new Size(110, 30);
        numCount.TabIndex = 7;
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
        lblInterval.TabIndex = 4;
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
        intervalFlow.TabIndex = 3;
        intervalFlow.WrapContents = false;
        //
        // numInterval
        //
        numInterval.Increment = new decimal(new int[] { 50, 0, 0, 0 });
        numInterval.Margin = new Padding(0, 0, 8, 0);
        numInterval.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
        numInterval.Name = "numInterval";
        numInterval.Size = new Size(110, 30);
        numInterval.TabIndex = 8;
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
        lblIntervalHint.TabIndex = 5;
        lblIntervalHint.Tag = ThemeManager.MutedTextTag;
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
        buttonsFlow.TabIndex = 4;
        buttonsFlow.WrapContents = false;
        //
        // btnOk
        //
        btnOk.Margin = new Padding(0, 0, 8, 0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(96, 32);
        btnOk.StyleKind = ButtonStyleKind.Primary;
        btnOk.TabIndex = 9;
        btnOk.Text = "OK";
        btnOk.Click += btnOk_Click;
        //
        // btnCancel
        //
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Margin = new Padding(0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(96, 32);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        //
        // MouseActionForm
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(440, 300);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "MouseActionForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Add Mouse Action";
        modifiersFlow.ResumeLayout(false);
        modifiersFlow.PerformLayout();
        positionFlow.ResumeLayout(false);
        positionFlow.PerformLayout();
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
