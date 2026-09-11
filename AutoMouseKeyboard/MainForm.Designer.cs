using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class MainForm
{
    private IContainer components = null;
    private GroupBox grpConfigs = null;
    private ListBox lstConfigs = null;
    private Button btnNew = null;
    private Button btnSave = null;
    private Button btnDelete = null;
    private Button btnSetting = null;
    private Button btnAbout = null;
    private Button btnImport = null;
    private Button btnExport = null;
    private GroupBox grpActions = null;
    private Label lblConfigName = null;
    private TextBox txtConfigName = null;
    private ToolStrip toolActionMenu = null;
    private ToolStripDropDownButton menuAddKey = null;
    private ToolStripDropDownButton menuAddMouse = null;
    private EnterAwareDataGridView gridActions = null;
    private Button btnMoveUp = null;
    private Button btnMoveDown = null;
    private Button btnDeleteAction = null;
    private Label lblLoop = null;
    private NumericUpDown numLoopCount = null;
    private Label lblGlobalDelay = null;
    private NumericUpDown numGlobalDelay = null;
    private Button btnStart = null;
    private Label lblStatus = null;
    private DataGridViewTextBoxColumn colIndex = null;
    private DataGridViewTextBoxColumn colKeys = null;
    private DataGridViewTextBoxColumn colCharacters = null;
    private DataGridViewButtonColumn colGetPosition = null;
    private DataGridViewTextBoxColumn colCount = null;
    private DataGridViewTextBoxColumn colDelay = null;

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
        grpConfigs = new GroupBox();
        btnExport = new Button();
        btnImport = new Button();
        btnAbout = new Button();
        btnSetting = new Button();
        btnDelete = new Button();
        btnSave = new Button();
        btnNew = new Button();
        lstConfigs = new ListBox();
        grpActions = new GroupBox();
        btnMoveUp = new Button();
        btnMoveDown = new Button();
        btnDeleteAction = new Button();
        gridActions = new EnterAwareDataGridView();
        colIndex = new DataGridViewTextBoxColumn();
        colKeys = new DataGridViewTextBoxColumn();
        colCharacters = new DataGridViewTextBoxColumn();
        colGetPosition = new DataGridViewButtonColumn();
        colCount = new DataGridViewTextBoxColumn();
        colDelay = new DataGridViewTextBoxColumn();
        txtConfigName = new TextBox();
        lblConfigName = new Label();
        toolActionMenu = new ToolStrip();
        menuAddKey = new ToolStripDropDownButton();
        menuAddMouse = new ToolStripDropDownButton();
        lblLoop = new Label();
        numLoopCount = new NumericUpDown();
        lblGlobalDelay = new Label();
        numGlobalDelay = new NumericUpDown();
        btnStart = new Button();
        lblStatus = new Label();
        grpConfigs.SuspendLayout();
        grpActions.SuspendLayout();
        ((ISupportInitialize)gridActions).BeginInit();
        ((ISupportInitialize)numLoopCount).BeginInit();
        ((ISupportInitialize)numGlobalDelay).BeginInit();
        SuspendLayout();
        // 
        // grpConfigs
        // 
        grpConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        grpConfigs.Controls.Add(btnExport);
        grpConfigs.Controls.Add(btnImport);
        grpConfigs.Controls.Add(btnAbout);
        grpConfigs.Controls.Add(btnSetting);
        grpConfigs.Controls.Add(btnDelete);
        grpConfigs.Controls.Add(btnSave);
        grpConfigs.Controls.Add(btnNew);
        grpConfigs.Controls.Add(lstConfigs);
        grpConfigs.Location = new Point(12, 12);
        grpConfigs.Name = "grpConfigs";
        grpConfigs.Size = new Size(250, 640);
        grpConfigs.TabIndex = 0;
        grpConfigs.TabStop = false;
        grpConfigs.Text = "Danh sách config";
        // 
        // btnNew
        // 
        btnNew.Location = new Point(10, 470);
        btnNew.Name = "btnNew";
        btnNew.Size = new Size(230, 30);
        btnNew.TabIndex = 1;
        btnNew.Text = "New";
        btnNew.UseVisualStyleBackColor = true;
        btnNew.Click += btnNew_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(10, 510);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(110, 30);
        btnSave.TabIndex = 2;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnDelete
        // 
        btnDelete.Location = new Point(130, 510);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(110, 30);
        btnDelete.TabIndex = 3;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnSetting
        // 
        btnSetting.Location = new Point(10, 550);
        btnSetting.Name = "btnSetting";
        btnSetting.Size = new Size(110, 30);
        btnSetting.TabIndex = 4;
        btnSetting.Text = "Setting";
        btnSetting.UseVisualStyleBackColor = true;
        btnSetting.Click += btnSetting_Click;
        // 
        // btnAbout
        // 
        btnAbout.Location = new Point(130, 550);
        btnAbout.Name = "btnAbout";
        btnAbout.Size = new Size(110, 30);
        btnAbout.TabIndex = 5;
        btnAbout.Text = "About";
        btnAbout.UseVisualStyleBackColor = true;
        btnAbout.Click += btnAbout_Click;
        // 
        // btnImport
        // 
        btnImport.Location = new Point(10, 590);
        btnImport.Name = "btnImport";
        btnImport.Size = new Size(110, 30);
        btnImport.TabIndex = 6;
        btnImport.Text = "Import";
        btnImport.UseVisualStyleBackColor = true;
        btnImport.Click += btnImport_Click;
        // 
        // btnExport
        // 
        btnExport.Location = new Point(130, 590);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(110, 30);
        btnExport.TabIndex = 7;
        btnExport.Text = "Export";
        btnExport.UseVisualStyleBackColor = true;
        btnExport.Click += btnExport_Click;
        // 
        // lstConfigs
        // 
        lstConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstConfigs.FormattingEnabled = true;
        lstConfigs.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point);
        lstConfigs.ItemHeight = 20;
        lstConfigs.Location = new Point(10, 22);
        lstConfigs.Name = "lstConfigs";
        lstConfigs.SelectionMode = SelectionMode.MultiExtended;
        lstConfigs.Size = new Size(230, 439);
        lstConfigs.TabIndex = 0;
        lstConfigs.SelectedIndexChanged += lstConfigs_SelectedIndexChanged;
        // 
        // grpActions
        // 
        grpActions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpActions.Controls.Add(btnMoveUp);
        grpActions.Controls.Add(btnMoveDown);
        grpActions.Controls.Add(btnDeleteAction);
        grpActions.Controls.Add(gridActions);
        grpActions.Controls.Add(toolActionMenu);
        grpActions.Controls.Add(txtConfigName);
        grpActions.Controls.Add(lblConfigName);
        grpActions.Location = new Point(268, 12);
        grpActions.Name = "grpActions";
        grpActions.Size = new Size(820, 540);
        grpActions.TabIndex = 1;
        grpActions.TabStop = false;
        grpActions.Text = "Chi tiết hành động";
        // 
        // btnMoveUp
        // 
        btnMoveUp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnMoveUp.Location = new Point(160, 496);
        btnMoveUp.Name = "btnMoveUp";
        btnMoveUp.Size = new Size(100, 30);
        btnMoveUp.TabIndex = 6;
        btnMoveUp.Text = "Move Up";
        btnMoveUp.UseVisualStyleBackColor = true;
        btnMoveUp.Click += btnMoveUp_Click;
        // 
        // btnMoveDown
        // 
        btnMoveDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnMoveDown.Location = new Point(270, 496);
        btnMoveDown.Name = "btnMoveDown";
        btnMoveDown.Size = new Size(100, 30);
        btnMoveDown.TabIndex = 7;
        btnMoveDown.Text = "Move Down";
        btnMoveDown.UseVisualStyleBackColor = true;
        btnMoveDown.Click += btnMoveDown_Click;
        // 
        // btnDeleteAction
        // 
        btnDeleteAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDeleteAction.Location = new Point(380, 496);
        btnDeleteAction.Name = "btnDeleteAction";
        btnDeleteAction.Size = new Size(100, 30);
        btnDeleteAction.TabIndex = 8;
        btnDeleteAction.Text = "Delete";
        btnDeleteAction.UseVisualStyleBackColor = true;
        btnDeleteAction.Click += btnDeleteAction_Click;
        // 
        // gridActions
        // 
        gridActions.AllowUserToAddRows = false;
        gridActions.AllowUserToDeleteRows = false;
        gridActions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gridActions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridActions.Columns.AddRange(new DataGridViewColumn[] { colIndex, colKeys, colCharacters, colCount, colDelay, colGetPosition });
        gridActions.EditMode = DataGridViewEditMode.EditOnEnter;
        gridActions.Location = new Point(12, 94);
        gridActions.MultiSelect = true;
        gridActions.Name = "gridActions";
        gridActions.ReadOnly = false;
        gridActions.RowHeadersVisible = false;
        gridActions.RowTemplate.Height = 25;
        gridActions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridActions.Size = new Size(796, 390);
        gridActions.TabIndex = 2;
        gridActions.KeyDown += gridActions_KeyDown;
        gridActions.KeyUp += gridActions_KeyUp;
        gridActions.PreviewKeyDown += gridActions_PreviewKeyDown;
        gridActions.DataError += gridActions_DataError;
        gridActions.CellDoubleClick += gridActions_CellDoubleClick;
        // 
        // colIndex
        // 
        colIndex.HeaderText = "STT";
        colIndex.Name = "colIndex";
        colIndex.ReadOnly = true;
        colIndex.Width = 60;
        // 
        // colKeys
        // 
        colKeys.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colKeys.DataPropertyName = "DisplayKey";
        colKeys.HeaderText = "Keys";
        colKeys.Name = "colKeys";
        colKeys.ReadOnly = true;
        // 
        // colCharacters
        // 
        colCharacters.DataPropertyName = "DisplayCharacters";
        colCharacters.HeaderText = "Characters";
        colCharacters.Name = "colCharacters";
        colCharacters.ReadOnly = true;
        colCharacters.Width = 100;
        // 
        // colGetPosition
        // 
        colGetPosition.HeaderText = "";
        colGetPosition.Name = "colGetPosition";
        colGetPosition.ReadOnly = true;
        colGetPosition.UseColumnTextForButtonValue = true;
        colGetPosition.Width = 80;
        // 
        // colCount
        // 
        colCount.DataPropertyName = "Repeat";
        colCount.HeaderText = "Count";
        colCount.Name = "colCount";
        colCount.ReadOnly = false;
        colCount.Width = 80;
        // 
        // colDelay
        // 
        colDelay.DataPropertyName = "Delay";
        colDelay.HeaderText = "Delay";
        colDelay.Name = "colDelay";
        colDelay.ReadOnly = false;
        colDelay.Width = 110;
        // 
        // txtConfigName
        // 
        txtConfigName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtConfigName.Location = new Point(134, 26);
        txtConfigName.Name = "txtConfigName";
        txtConfigName.Size = new Size(674, 23);
        txtConfigName.TabIndex = 1;
        // 
        // lblConfigName
        // 
        lblConfigName.AutoSize = true;
        lblConfigName.Location = new Point(12, 29);
        lblConfigName.Name = "lblConfigName";
        lblConfigName.Size = new Size(116, 15);
        lblConfigName.TabIndex = 0;
        lblConfigName.Text = "Tên cấu hình";
        // 
        // toolActionMenu
        // 
        toolActionMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        toolActionMenu.AutoSize = false;
        toolActionMenu.BackColor = System.Drawing.Color.Gainsboro;
        toolActionMenu.Dock = DockStyle.None;
        toolActionMenu.GripStyle = ToolStripGripStyle.Hidden;
        toolActionMenu.Items.AddRange(new ToolStripItem[] { menuAddKey, menuAddMouse });
        toolActionMenu.Location = new Point(12, 60);
        toolActionMenu.Name = "toolActionMenu";
        toolActionMenu.Padding = new Padding(6, 4, 6, 4);
        toolActionMenu.Size = new Size(796, 31);
        toolActionMenu.TabIndex = 6;
        toolActionMenu.Text = "toolStrip1";
        // 
        // menuAddKey
        // 
        menuAddKey.Name = "menuAddKey";
        menuAddKey.Size = new Size(72, 23);
        menuAddKey.Text = "Add Key";
        // 
        // menuAddMouse
        // 
        menuAddMouse.Name = "menuAddMouse";
        menuAddMouse.Size = new Size(88, 23);
        menuAddMouse.Text = "Add Mouse";
        // 
        // lblLoop
        // 
        lblLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblLoop.AutoSize = true;
        lblLoop.Location = new Point(271, 565);
        lblLoop.Name = "lblLoop";
        lblLoop.Size = new Size(79, 15);
        lblLoop.TabIndex = 2;
        lblLoop.Text = "Số lần lặp lại";
        // 
        // numLoopCount
        // 
        numLoopCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        numLoopCount.Location = new Point(356, 563);
        numLoopCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numLoopCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numLoopCount.Name = "numLoopCount";
        numLoopCount.Size = new Size(120, 23);
        numLoopCount.TabIndex = 3;
        numLoopCount.Value = new decimal(new int[] { 30, 0, 0, 0 });
        // 
        // lblGlobalDelay
        // 
        lblGlobalDelay.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblGlobalDelay.AutoSize = true;
        lblGlobalDelay.Location = new Point(490, 565);
        lblGlobalDelay.Name = "lblGlobalDelay";
        lblGlobalDelay.Size = new Size(116, 15);
        lblGlobalDelay.TabIndex = 4;
        lblGlobalDelay.Text = "Delay chung (ms)";
        // 
        // numGlobalDelay
        // 
        numGlobalDelay.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        numGlobalDelay.Increment = new decimal(new int[] { 50, 0, 0, 0 });
        numGlobalDelay.Location = new Point(612, 563);
        numGlobalDelay.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
        numGlobalDelay.Name = "numGlobalDelay";
        numGlobalDelay.Size = new Size(120, 23);
        numGlobalDelay.TabIndex = 5;
        numGlobalDelay.Value = new decimal(new int[] { 0, 0, 0, 0 });
        // 
        // btnStart
        // 
        btnStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
        btnStart.Location = new Point(940, 558);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(148, 50);
        btnStart.TabIndex = 6;
        btnStart.Text = "Start";
        btnStart.UseVisualStyleBackColor = true;
        btnStart.Click += btnStart_Click;
        // 
        // lblStatus
        // 
        lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(271, 607);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(112, 15);
        lblStatus.TabIndex = 7;
        lblStatus.Text = "Trạng thái: Unknown";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 664);
        Controls.Add(lblStatus);
        Controls.Add(btnStart);
        Controls.Add(numGlobalDelay);
        Controls.Add(lblGlobalDelay);
        Controls.Add(numLoopCount);
        Controls.Add(lblLoop);
        Controls.Add(grpActions);
        Controls.Add(grpConfigs);
        MinimumSize = new Size(1000, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AutoMouseKeyboard";
        grpConfigs.ResumeLayout(false);
        grpActions.ResumeLayout(false);
        grpActions.PerformLayout();
        toolActionMenu.ResumeLayout(false);
        toolActionMenu.PerformLayout();
        ((ISupportInitialize)gridActions).EndInit();
        ((ISupportInitialize)numLoopCount).EndInit();
        ((ISupportInitialize)numGlobalDelay).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    }
}
