using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class AboutForm
{
    private IContainer components = null;
    private TableLayoutPanel rootLayout = null;
    private CardPanel cardAbout = null;
    private TableLayoutPanel aboutLayout = null;
    private TableLayoutPanel textLayout = null;
    private Label lblTitle = null;
    private Label lblVersion = null;
    private Label lblAuthor = null;
    private Label lblDescription = null;
    private Label lblDonateTitle = null;
    private Label lblDonateInfo = null;
    private PictureBox picQRCode = null;
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
        cardAbout = new CardPanel();
        aboutLayout = new TableLayoutPanel();
        textLayout = new TableLayoutPanel();
        lblTitle = new Label();
        lblVersion = new Label();
        lblAuthor = new Label();
        lblDescription = new Label();
        lblDonateTitle = new Label();
        lblDonateInfo = new Label();
        picQRCode = new PictureBox();
        btnOk = new ModernButton();
        ((ISupportInitialize)picQRCode).BeginInit();
        rootLayout.SuspendLayout();
        cardAbout.SuspendLayout();
        aboutLayout.SuspendLayout();
        textLayout.SuspendLayout();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 1;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(cardAbout, 0, 0);
        rootLayout.Controls.Add(btnOk, 0, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(12);
        rootLayout.RowCount = 2;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.TabIndex = 0;
        //
        // cardAbout
        //
        cardAbout.Controls.Add(aboutLayout);
        cardAbout.Dock = DockStyle.Fill;
        cardAbout.Margin = new Padding(0, 0, 0, 8);
        cardAbout.Name = "cardAbout";
        cardAbout.TabIndex = 0;
        cardAbout.TabStop = false;
        //
        // aboutLayout
        //
        aboutLayout.BackColor = Color.Transparent;
        aboutLayout.ColumnCount = 2;
        aboutLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        aboutLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        aboutLayout.Controls.Add(textLayout, 0, 0);
        aboutLayout.Controls.Add(picQRCode, 1, 0);
        aboutLayout.Dock = DockStyle.Fill;
        aboutLayout.Margin = new Padding(0);
        aboutLayout.Name = "aboutLayout";
        aboutLayout.RowCount = 1;
        aboutLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        aboutLayout.TabIndex = 0;
        //
        // textLayout
        //
        textLayout.BackColor = Color.Transparent;
        textLayout.ColumnCount = 1;
        textLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        textLayout.Controls.Add(lblTitle, 0, 0);
        textLayout.Controls.Add(lblVersion, 0, 1);
        textLayout.Controls.Add(lblAuthor, 0, 2);
        textLayout.Controls.Add(lblDescription, 0, 3);
        textLayout.Controls.Add(lblDonateTitle, 0, 4);
        textLayout.Controls.Add(lblDonateInfo, 0, 5);
        textLayout.Dock = DockStyle.Fill;
        textLayout.Margin = new Padding(0);
        textLayout.Name = "textLayout";
        textLayout.RowCount = 6;
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        textLayout.TabIndex = 0;
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.Margin = new Padding(0, 0, 0, 4);
        lblTitle.Name = "lblTitle";
        lblTitle.TabIndex = 0;
        lblTitle.Text = "AutoMouseKeyboard";
        //
        // lblVersion
        //
        lblVersion.AutoSize = true;
        lblVersion.BackColor = Color.Transparent;
        lblVersion.ForeColor = ThemeManager.Palette.TextMuted;
        lblVersion.Margin = new Padding(0, 0, 0, 4);
        lblVersion.Name = "lblVersion";
        lblVersion.TabIndex = 1;
        lblVersion.Tag = ThemeManager.MutedTextTag;
        lblVersion.Text = "Version: 1.0.0";
        //
        // lblAuthor
        //
        lblAuthor.AutoSize = true;
        lblAuthor.BackColor = Color.Transparent;
        lblAuthor.ForeColor = ThemeManager.Palette.TextMuted;
        lblAuthor.Margin = new Padding(0, 0, 0, 8);
        lblAuthor.Name = "lblAuthor";
        lblAuthor.TabIndex = 2;
        lblAuthor.Tag = ThemeManager.MutedTextTag;
        lblAuthor.Text = "Author: NDQITVN";
        //
        // lblDescription
        //
        lblDescription.BackColor = Color.Transparent;
        lblDescription.Dock = DockStyle.Fill;
        lblDescription.Margin = new Padding(0, 0, 0, 8);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(280, 60);
        lblDescription.TabIndex = 3;
        lblDescription.Text = "Automation tool for mouse clicks and keyboard input.";
        lblDescription.UseMnemonic = false;
        //
        // lblDonateTitle
        //
        lblDonateTitle.AutoSize = true;
        lblDonateTitle.BackColor = Color.Transparent;
        lblDonateTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDonateTitle.Margin = new Padding(0, 8, 0, 4);
        lblDonateTitle.Name = "lblDonateTitle";
        lblDonateTitle.TabIndex = 4;
        lblDonateTitle.Text = "Ủng hộ tác giả:";
        //
        // lblDonateInfo
        //
        lblDonateInfo.BackColor = Color.Transparent;
        lblDonateInfo.Dock = DockStyle.Fill;
        lblDonateInfo.Margin = new Padding(0);
        lblDonateInfo.Name = "lblDonateInfo";
        lblDonateInfo.Size = new Size(280, 90);
        lblDonateInfo.TabIndex = 5;
        lblDonateInfo.Text = "Ngân hàng: [Tên ngân hàng]\r\nSố tài khoản: [Số tài khoản]\r\nChủ tài khoản: [Tên chủ tài khoản]\r\n\r\nHoặc quét mã QR bên cạnh để chuyển khoản nhanh.";
        lblDonateInfo.Click += lblDonateInfo_Click;
        //
        // picQRCode
        //
        picQRCode.Anchor = AnchorStyles.Top;
        picQRCode.BackColor = Color.Transparent;
        picQRCode.BorderStyle = BorderStyle.None;
        picQRCode.Margin = new Padding(8, 0, 0, 0);
        picQRCode.Name = "picQRCode";
        picQRCode.Size = new Size(200, 200);
        picQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
        picQRCode.TabIndex = 1;
        picQRCode.TabStop = false;
        //
        // btnOk
        //
        btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnOk.Margin = new Padding(0);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(96, 32);
        btnOk.StyleKind = ButtonStyleKind.Primary;
        btnOk.TabIndex = 1;
        btnOk.Text = "OK";
        btnOk.Click += btnOk_Click;
        //
        // AboutForm
        //
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(560, 420);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "About";
        textLayout.ResumeLayout(false);
        textLayout.PerformLayout();
        aboutLayout.ResumeLayout(false);
        aboutLayout.PerformLayout();
        cardAbout.ResumeLayout(false);
        cardAbout.PerformLayout();
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        ((ISupportInitialize)picQRCode).EndInit();
        ResumeLayout(false);
    }
    }
}
