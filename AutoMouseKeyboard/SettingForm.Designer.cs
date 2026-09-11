namespace AutoMouseKeyboard
{
    partial class SettingForm
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.GroupBox grpAppearance = null!;
        private System.Windows.Forms.GroupBox grpBehavior = null!;
        private System.Windows.Forms.Label lblTheme = null!;
        private System.Windows.Forms.ComboBox cboTheme = null!;
        private System.Windows.Forms.Label lblLanguage = null!;
        private System.Windows.Forms.ComboBox cboLanguage = null!;
        private System.Windows.Forms.Button btnOk = null!;
        private System.Windows.Forms.CheckBox chkRunOnStartup = null!;
        private System.Windows.Forms.CheckBox chkHideOnComplete = null!;

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
            components = new System.ComponentModel.Container();
            grpAppearance = new System.Windows.Forms.GroupBox();
            grpBehavior = new System.Windows.Forms.GroupBox();
            lblTheme = new System.Windows.Forms.Label();
            cboTheme = new System.Windows.Forms.ComboBox();
            lblLanguage = new System.Windows.Forms.Label();
            cboLanguage = new System.Windows.Forms.ComboBox();
            btnOk = new System.Windows.Forms.Button();
            chkRunOnStartup = new System.Windows.Forms.CheckBox();
            chkHideOnComplete = new System.Windows.Forms.CheckBox();

            grpAppearance.SuspendLayout();
            grpBehavior.SuspendLayout();
            SuspendLayout();

            // 
            // grpAppearance
            // 
            grpAppearance.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpAppearance.Controls.Add(lblTheme);
            grpAppearance.Controls.Add(cboTheme);
            grpAppearance.Controls.Add(lblLanguage);
            grpAppearance.Controls.Add(cboLanguage);
            grpAppearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            grpAppearance.Location = new System.Drawing.Point(15, 15);
            grpAppearance.Name = "grpAppearance";
            grpAppearance.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            grpAppearance.Size = new System.Drawing.Size(320, 100);
            grpAppearance.TabIndex = 0;
            grpAppearance.TabStop = false;
            grpAppearance.Text = "Appearance";

            // 
            // lblTheme
            // 
            lblTheme.AutoSize = true;
            lblTheme.Location = new System.Drawing.Point(18, 30);
            lblTheme.Name = "lblTheme";
            lblTheme.Size = new System.Drawing.Size(50, 15);
            lblTheme.TabIndex = 0;
            lblTheme.Text = "Theme:";

            // 
            // cboTheme
            // 
            cboTheme.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboTheme.FormattingEnabled = true;
            cboTheme.Location = new System.Drawing.Point(100, 27);
            cboTheme.Name = "cboTheme";
            cboTheme.Size = new System.Drawing.Size(185, 23);
            cboTheme.TabIndex = 1;

            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Location = new System.Drawing.Point(18, 63);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new System.Drawing.Size(62, 15);
            lblLanguage.TabIndex = 2;
            lblLanguage.Text = "Language:";

            // 
            // cboLanguage
            // 
            cboLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboLanguage.FormattingEnabled = true;
            cboLanguage.Location = new System.Drawing.Point(100, 60);
            cboLanguage.Name = "cboLanguage";
            cboLanguage.Size = new System.Drawing.Size(185, 23);
            cboLanguage.TabIndex = 3;
            cboLanguage.SelectedIndexChanged += CboLanguage_SelectedIndexChanged;

            // 
            // grpBehavior
            // 
            grpBehavior.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            grpBehavior.Controls.Add(chkRunOnStartup);
            grpBehavior.Controls.Add(chkHideOnComplete);
            grpBehavior.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            grpBehavior.Location = new System.Drawing.Point(15, 125);
            grpBehavior.Name = "grpBehavior";
            grpBehavior.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            grpBehavior.Size = new System.Drawing.Size(320, 90);
            grpBehavior.TabIndex = 1;
            grpBehavior.TabStop = false;
            grpBehavior.Text = "Behavior";

            // 
            // chkRunOnStartup
            // 
            chkRunOnStartup.AutoSize = true;
            chkRunOnStartup.Location = new System.Drawing.Point(18, 28);
            chkRunOnStartup.Name = "chkRunOnStartup";
            chkRunOnStartup.Size = new System.Drawing.Size(180, 19);
            chkRunOnStartup.TabIndex = 0;
            chkRunOnStartup.Text = "Run when Windows starts";
            chkRunOnStartup.UseVisualStyleBackColor = true;
            chkRunOnStartup.CheckedChanged += chkRunOnStartup_CheckedChanged;

            // 
            // chkHideOnComplete
            // 
            chkHideOnComplete.AutoSize = true;
            chkHideOnComplete.Location = new System.Drawing.Point(18, 55);
            chkHideOnComplete.Name = "chkHideOnComplete";
            chkHideOnComplete.Size = new System.Drawing.Size(280, 19);
            chkHideOnComplete.TabIndex = 1;
            chkHideOnComplete.Text = "Keep window hidden after finishing actions";
            chkHideOnComplete.UseVisualStyleBackColor = true;
            chkHideOnComplete.CheckedChanged += chkHideOnComplete_CheckedChanged;

            // 
            // btnOk
            // 
            btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            btnOk.Location = new System.Drawing.Point(250, 230);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(85, 32);
            btnOk.TabIndex = 2;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;

            // 
            // SettingForm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(350, 280);
            Controls.Add(grpAppearance);
            Controls.Add(grpBehavior);
            Controls.Add(btnOk);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Settings";

            grpAppearance.ResumeLayout(false);
            grpAppearance.PerformLayout();
            grpBehavior.ResumeLayout(false);
            grpBehavior.PerformLayout();
            ResumeLayout(false);
        }
    }
}
