using System.ComponentModel;
using System.Windows.Forms;

namespace AutoMouseKeyboard
{
    partial class MouseActionForm
{
    private IContainer components = null!;
    private ComboBox cboMouseAction = null!;
    private Label lblAction = null!;
    private CheckBox chkCtrl = null!;
    private CheckBox chkAlt = null!;
    private CheckBox chkShift = null!;
    private Label lblModifiers = null!;
    private Label lblPosition = null!;
    private NumericUpDown numPosX = null!;
    private NumericUpDown numPosY = null!;
    private Button btnGetPosition = null!;
    private Label lblCount = null!;
    private NumericUpDown numCount = null!;
    private Label lblInterval = null!;
    private NumericUpDown numInterval = null!;
    private Label lblIntervalHint = null!;
    private Button btnOk = null!;
    private Button btnCancel = null!;

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
            this.cboMouseAction = new System.Windows.Forms.ComboBox();
            this.lblAction = new System.Windows.Forms.Label();
            this.chkCtrl = new System.Windows.Forms.CheckBox();
            this.chkAlt = new System.Windows.Forms.CheckBox();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.lblModifiers = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.numPosX = new System.Windows.Forms.NumericUpDown();
            this.numPosY = new System.Windows.Forms.NumericUpDown();
            this.btnGetPosition = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.lblIntervalHint = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // cboMouseAction
            // 
            this.cboMouseAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMouseAction.FormattingEnabled = true;
            this.cboMouseAction.Location = new System.Drawing.Point(120, 16);
            this.cboMouseAction.Name = "cboMouseAction";
            this.cboMouseAction.Size = new System.Drawing.Size(189, 21);
            this.cboMouseAction.TabIndex = 0;
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Location = new System.Drawing.Point(17, 18);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(82, 13);
            this.lblAction.TabIndex = 1;
            this.lblAction.Text = "Loại click (Key):";
            // 
            // chkCtrl
            // 
            this.chkCtrl.AutoSize = true;
            this.chkCtrl.Location = new System.Drawing.Point(120, 50);
            this.chkCtrl.Name = "chkCtrl";
            this.chkCtrl.Size = new System.Drawing.Size(41, 17);
            this.chkCtrl.TabIndex = 2;
            this.chkCtrl.Text = "Ctrl";
            this.chkCtrl.UseVisualStyleBackColor = true;
            // 
            // chkAlt
            // 
            this.chkAlt.AutoSize = true;
            this.chkAlt.Location = new System.Drawing.Point(171, 50);
            this.chkAlt.Name = "chkAlt";
            this.chkAlt.Size = new System.Drawing.Size(38, 17);
            this.chkAlt.TabIndex = 3;
            this.chkAlt.Text = "Alt";
            this.chkAlt.UseVisualStyleBackColor = true;
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Location = new System.Drawing.Point(223, 50);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(47, 17);
            this.chkShift.TabIndex = 4;
            this.chkShift.Text = "Shift";
            this.chkShift.UseVisualStyleBackColor = true;
            // 
            // lblModifiers
            // 
            this.lblModifiers.AutoSize = true;
            this.lblModifiers.Location = new System.Drawing.Point(17, 51);
            this.lblModifiers.Name = "lblModifiers";
            this.lblModifiers.Size = new System.Drawing.Size(76, 13);
            this.lblModifiers.TabIndex = 5;
            this.lblModifiers.Text = "Giữ kèm phím:";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(17, 84);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(62, 13);
            this.lblPosition.TabIndex = 6;
            this.lblPosition.Text = "Tọa độ X,Y";
            // 
            // numPosX
            // 
            this.numPosX.Location = new System.Drawing.Point(120, 82);
            this.numPosX.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numPosX.Minimum = new decimal(new int[] {
            20000,
            0,
            0,
            -2147483648});
            this.numPosX.Name = "numPosX";
            this.numPosX.Size = new System.Drawing.Size(69, 20);
            this.numPosX.TabIndex = 5;
            this.numPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numPosY
            // 
            this.numPosY.Location = new System.Drawing.Point(197, 82);
            this.numPosY.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numPosY.Minimum = new decimal(new int[] {
            20000,
            0,
            0,
            -2147483648});
            this.numPosY.Name = "numPosY";
            this.numPosY.Size = new System.Drawing.Size(69, 20);
            this.numPosY.TabIndex = 6;
            this.numPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnGetPosition
            // 
            this.btnGetPosition.Location = new System.Drawing.Point(274, 81);
            this.btnGetPosition.Name = "btnGetPosition";
            this.btnGetPosition.Size = new System.Drawing.Size(69, 22);
            this.btnGetPosition.TabIndex = 7;
            this.btnGetPosition.Text = "Đặt tọa độ";
            this.btnGetPosition.UseVisualStyleBackColor = true;
            this.btnGetPosition.Click += new System.EventHandler(this.btnGetPosition_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(17, 118);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 10;
            this.lblCount.Text = "Count";
            // 
            // numCount
            // 
            this.numCount.Location = new System.Drawing.Point(120, 116);
            this.numCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(103, 20);
            this.numCount.TabIndex = 8;
            this.numCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(17, 152);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(64, 13);
            this.lblInterval.TabIndex = 12;
            this.lblInterval.Text = "Interval (ms)";
            // 
            // numInterval
            // 
            this.numInterval.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numInterval.Location = new System.Drawing.Point(120, 150);
            this.numInterval.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(103, 20);
            this.numInterval.TabIndex = 9;
            this.numInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numInterval.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // lblIntervalHint
            // 
            this.lblIntervalHint.AutoSize = true;
            this.lblIntervalHint.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblIntervalHint.Location = new System.Drawing.Point(227, 152);
            this.lblIntervalHint.Name = "lblIntervalHint";
            this.lblIntervalHint.Size = new System.Drawing.Size(116, 13);
            this.lblIntervalHint.TabIndex = 14;
            this.lblIntervalHint.Text = "(1 giây = 1000 mili giây)";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(194, 205);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(73, 28);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(272, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 28);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // MouseActionForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(363, 247);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblIntervalHint);
            this.Controls.Add(this.numInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnGetPosition);
            this.Controls.Add(this.numPosY);
            this.Controls.Add(this.numPosX);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.lblModifiers);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.chkAlt);
            this.Controls.Add(this.chkCtrl);
            this.Controls.Add(this.lblAction);
            this.Controls.Add(this.cboMouseAction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MouseActionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Mouse Action";
            ((System.ComponentModel.ISupportInitialize)(this.numPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    }
}

