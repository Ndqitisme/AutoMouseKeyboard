using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class SettingForm
{
    private IContainer components = null;
    private TableLayoutPanel rootLayout = null;
    private CardPanel grpAppearance = null;
    private TableLayoutPanel appearanceLayout = null;
    private Label lblTheme = null;
    private ModernSelect cboTheme = null;
    private Label lblLanguage = null;
    private ModernSelect cboLanguage = null;
    private CardPanel grpBehavior = null;
    private ModernCheckBox chkRunOnStartup = null;
    private ModernCheckBox chkHideOnComplete = null;
    private ModernButton btnOk = null;

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
        grpAppearance = new CardPanel();
        appearanceLayout = new TableLayoutPanel();
        lblTheme = new Label();
        cboTheme = new ModernSelect();
        lblLanguage = new Label();
        cboLanguage = new ModernSelect();
        grpBehavior = new CardPanel();
        chkRunOnStartup = new ModernCheckBox();
        chkHideOnComplete = new ModernCheckBox();
        btnOk = new ModernButton();
        rootLayout.SuspendLayout();
        grpAppearance.SuspendLayout();
        appearanceLayout.SuspendLayout();
        grpBehavior.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(grpAppearance, 0, 0);
        rootLayout.Controls.Add(grpBehavior, 0, 1);
        rootLayout.Controls.Add(btnOk, 0, 2);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(12);
        rootLayout.RowCount = 3;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.TabIndex = 0;
        //
        // grpAppearance
        //
        grpAppearance.AutoSize = true;
        grpAppearance.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpAppearance.Controls.Add(appearanceLayout);
        grpAppearance.Dock = DockStyle.Fill;
        grpAppearance.Margin = new Padding(0, 0, 0, 8);
        grpAppearance.Name = "grpAppearance";
        grpAppearance.TabIndex = 0;
        grpAppearance.TabStop = false;
        grpAppearance.Text = "Appearance";
        //
        // appearanceLayout
        //
        appearanceLayout.AutoSize = true;
        appearanceLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        appearanceLayout.BackColor = Color.Transparent;
        appearanceLayout.ColumnCount = 2;
        appearanceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        appearanceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        appearanceLayout.Controls.Add(lblTheme, 0, 0);
        appearanceLayout.Controls.Add(cboTheme, 1, 0);
        appearanceLayout.Controls.Add(lblLanguage, 0, 1);
        appearanceLayout.Controls.Add(cboLanguage, 1, 1);
        appearanceLayout.Dock = DockStyle.Top;
        appearanceLayout.Margin = new Padding(0);
        appearanceLayout.Name = "appearanceLayout";
        appearanceLayout.RowCount = 2;
        appearanceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        appearanceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        appearanceLayout.TabIndex = 0;
        //
        // lblTheme
        //
        lblTheme.Anchor = AnchorStyles.Left;
        lblTheme.AutoSize = true;
        lblTheme.BackColor = Color.Transparent;
        lblTheme.Margin = new Padding(0, 0, 8, 0);
        lblTheme.Name = "lblTheme";
        lblTheme.TabIndex = 0;
        lblTheme.Text = "Theme:";
        //
        // cboTheme
        //
        cboTheme.Dock = DockStyle.Fill;
        cboTheme.Margin = new Padding(0, 0, 0, 6);
        cboTheme.Name = "cboTheme";
        cboTheme.Size = new Size(200, 28);
        cboTheme.TabIndex = 1;
        //
        // lblLanguage
        //
        lblLanguage.Anchor = AnchorStyles.Left;
        lblLanguage.AutoSize = true;
        lblLanguage.BackColor = Color.Transparent;
        lblLanguage.Margin = new Padding(0, 0, 8, 0);
        lblLanguage.Name = "lblLanguage";
        lblLanguage.TabIndex = 2;
        lblLanguage.Text = "Language:";
        //
        // cboLanguage
        //
        cboLanguage.Dock = DockStyle.Fill;
        cboLanguage.Margin = new Padding(0);
        cboLanguage.Name = "cboLanguage";
        cboLanguage.Size = new Size(200, 28);
        cboLanguage.TabIndex = 3;
        //
        // grpBehavior
        //
        grpBehavior.AutoSize = true;
        grpBehavior.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        grpBehavior.Controls.Add(chkHideOnComplete);
        grpBehavior.Controls.Add(chkRunOnStartup);
        grpBehavior.Dock = DockStyle.Fill;
        grpBehavior.Margin = new Padding(0);
        grpBehavior.Name = "grpBehavior";
        grpBehavior.TabIndex = 1;
        grpBehavior.TabStop = false;
        grpBehavior.Text = "Behavior";
        //
        // chkRunOnStartup
        //
        chkRunOnStartup.Dock = DockStyle.Top;
        chkRunOnStartup.Margin = new Padding(0, 0, 0, 4);
        chkRunOnStartup.Name = "chkRunOnStartup";
        chkRunOnStartup.Size = new Size(320, 26);
        chkRunOnStartup.TabIndex = 0;
        chkRunOnStartup.Text = "Run when Windows starts";
        //
        // chkHideOnComplete
        //
        chkHideOnComplete.Dock = DockStyle.Top;
        chkHideOnComplete.Margin = new Padding(0);
        chkHideOnComplete.Name = "chkHideOnComplete";
        chkHideOnComplete.Size = new Size(320, 26);
        chkHideOnComplete.TabIndex = 1;
        chkHideOnComplete.Text = "Keep window hidden after finishing actions";
        //
        // btnOk
        //
        btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnOk.Margin = new Padding(0, 8, 0, 0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(96, 32);
        btnOk.StyleKind = ButtonStyleKind.Primary;
        btnOk.TabIndex = 2;
        btnOk.Text = "OK";
        btnOk.Click += btnOk_Click;
        //
        // SettingForm
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(420, 320);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        appearanceLayout.ResumeLayout(false);
        appearanceLayout.PerformLayout();
        grpAppearance.ResumeLayout(false);
        grpAppearance.PerformLayout();
        grpBehavior.ResumeLayout(false);
        grpBehavior.PerformLayout();
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
    }
}
