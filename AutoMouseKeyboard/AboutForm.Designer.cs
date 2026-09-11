using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AutoMouseKeyboard
{
    partial class AboutForm
    {
        private IContainer components = null!;
        private Label lblTitle = null!;
        private Label lblVersion = null!;
        private Label lblAuthor = null!;
        private Label lblDescription = null!;
        private Button btnOk = null!;
        private PictureBox picQRCode = null!;
        private Label lblDonateTitle = null!;
        private Label lblDonateInfo = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.picQRCode = new System.Windows.Forms.PictureBox();
            this.lblDonateTitle = new System.Windows.Forms.Label();
            this.lblDonateInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(17, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(179, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AutoMouseKeyboard";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(17, 48);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version: 1.0.0";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(17, 69);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(196, 13);
            this.lblAuthor.TabIndex = 2;
            this.lblAuthor.Text = "Author: NDQITVN";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(17, 95);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(271, 43);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Automation tool for mouse clicks and keyboard input.";
            this.lblDescription.UseMnemonic = false;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(440, 369);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 26);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // picQRCode
            // 
            this.picQRCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picQRCode.Location = new System.Drawing.Point(305, 142);
            this.picQRCode.Name = "picQRCode";
            this.picQRCode.Size = new System.Drawing.Size(199, 206);
            this.picQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picQRCode.TabIndex = 5;
            this.picQRCode.TabStop = false;
            // 
            // lblDonateTitle
            // 
            this.lblDonateTitle.AutoSize = true;
            this.lblDonateTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDonateTitle.Location = new System.Drawing.Point(17, 156);
            this.lblDonateTitle.Name = "lblDonateTitle";
            this.lblDonateTitle.Size = new System.Drawing.Size(110, 19);
            this.lblDonateTitle.TabIndex = 6;
            this.lblDonateTitle.Text = "Ủng hộ tác giả:";
            // 
            // lblDonateInfo
            // 
            this.lblDonateInfo.Location = new System.Drawing.Point(17, 182);
            this.lblDonateInfo.Name = "lblDonateInfo";
            this.lblDonateInfo.Size = new System.Drawing.Size(274, 89);
            this.lblDonateInfo.TabIndex = 7;
            this.lblDonateInfo.Text = "Ngân hàng: [Tên ngân hàng]\r\nSố tài khoản: [Số tài khoản]\r\nChủ tài khoản: [Tên chủ" +
    " tài khoản]\r\n\r\nHoặc quét mã QR bên cạnh để chuyển khoản nhanh.";
            this.lblDonateInfo.Click += new System.EventHandler(this.lblDonateInfo_Click);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 407);
            this.Controls.Add(this.lblDonateInfo);
            this.Controls.Add(this.lblDonateTitle);
            this.Controls.Add(this.picQRCode);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

