using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.UI;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    partial class MainForm
{
    private IContainer components = null;
    private TableLayoutPanel rootLayout = null;
    private CardPanel grpConfigs = null;
    private TableLayoutPanel sidebarLayout = null;
    private ModernListBox lstConfigs = null;
    private TableLayoutPanel buttonsGrid = null;
    private ModernButton btnNew = null;
    private ModernButton btnSave = null;
    private ModernButton btnDelete = null;
    private ModernButton btnSetting = null;
    private ModernButton btnAbout = null;
    private ModernButton btnImport = null;
    private ModernButton btnExport = null;
    private CardPanel grpActions = null;
    private TableLayoutPanel contentLayout = null;
    private TableLayoutPanel nameRow = null;
    private Label lblConfigName = null;
    private RoundedTextBox txtConfigName = null;
    private FlowLayoutPanel toolbarFlow = null;
    private ModernButton btnAddKey = null;
    private ModernButton btnAddMouse = null;
    private ContextMenuStrip ctxAddKey = null;
    private ContextMenuStrip ctxAddMouse = null;
    private StyledDataGridView gridActions = null;
    private FlowLayoutPanel actionButtonsFlow = null;
    private ModernButton btnMoveUp = null;
    private ModernButton btnMoveDown = null;
    private ModernButton btnDeleteAction = null;
    private CardPanel statusCard = null;
    private TableLayoutPanel statusLayout = null;
    private Label lblStatus = null;
    private FlowLayoutPanel loopFlow = null;
    private Label lblLoop = null;
    private ModernNumericUpDown numLoopCount = null;
    private FlowLayoutPanel delayFlow = null;
    private Label lblGlobalDelay = null;
    private ModernNumericUpDown numGlobalDelay = null;
    private ModernButton btnStart = null;
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
        rootLayout = new TableLayoutPanel();
        grpConfigs = new CardPanel();
        sidebarLayout = new TableLayoutPanel();
        lstConfigs = new ModernListBox();
        buttonsGrid = new TableLayoutPanel();
        btnNew = new ModernButton();
        btnSave = new ModernButton();
        btnDelete = new ModernButton();
        btnSetting = new ModernButton();
        btnAbout = new ModernButton();
        btnImport = new ModernButton();
        btnExport = new ModernButton();
        grpActions = new CardPanel();
        contentLayout = new TableLayoutPanel();
        nameRow = new TableLayoutPanel();
        lblConfigName = new Label();
        txtConfigName = new RoundedTextBox();
        toolbarFlow = new FlowLayoutPanel();
        btnAddKey = new ModernButton();
        btnAddMouse = new ModernButton();
        ctxAddKey = new ContextMenuStrip(components);
        ctxAddMouse = new ContextMenuStrip(components);
        gridActions = new StyledDataGridView();
        colIndex = new DataGridViewTextBoxColumn();
        colKeys = new DataGridViewTextBoxColumn();
        colCharacters = new DataGridViewTextBoxColumn();
        colGetPosition = new DataGridViewButtonColumn();
        colCount = new DataGridViewTextBoxColumn();
        colDelay = new DataGridViewTextBoxColumn();
        actionButtonsFlow = new FlowLayoutPanel();
        btnMoveUp = new ModernButton();
        btnMoveDown = new ModernButton();
        btnDeleteAction = new ModernButton();
        statusCard = new CardPanel();
        statusLayout = new TableLayoutPanel();
        lblStatus = new Label();
        loopFlow = new FlowLayoutPanel();
        lblLoop = new Label();
        numLoopCount = new ModernNumericUpDown();
        delayFlow = new FlowLayoutPanel();
        lblGlobalDelay = new Label();
        numGlobalDelay = new ModernNumericUpDown();
        btnStart = new ModernButton();
        rootLayout.SuspendLayout();
        grpConfigs.SuspendLayout();
        sidebarLayout.SuspendLayout();
        buttonsGrid.SuspendLayout();
        grpActions.SuspendLayout();
        contentLayout.SuspendLayout();
        nameRow.SuspendLayout();
        toolbarFlow.SuspendLayout();
        actionButtonsFlow.SuspendLayout();
        statusCard.SuspendLayout();
        statusLayout.SuspendLayout();
        loopFlow.SuspendLayout();
        delayFlow.SuspendLayout();
        ((ISupportInitialize)gridActions).BeginInit();
        SuspendLayout();
        //
        // rootLayout
        //
        rootLayout.ColumnCount = 2;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 292F));
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootLayout.Controls.Add(grpConfigs, 0, 0);
        rootLayout.Controls.Add(grpActions, 1, 0);
        rootLayout.Controls.Add(statusCard, 0, 1);
        rootLayout.SetColumnSpan(statusCard, 2);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.Padding = new Padding(12);
        rootLayout.RowCount = 2;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rootLayout.TabIndex = 0;
        //
        // grpConfigs
        //
        grpConfigs.Controls.Add(sidebarLayout);
        grpConfigs.Dock = DockStyle.Fill;
        grpConfigs.Margin = new Padding(0, 0, 6, 0);
        grpConfigs.Name = "grpConfigs";
        grpConfigs.TabIndex = 0;
        grpConfigs.TabStop = false;
        grpConfigs.Text = "Danh sách config";
        //
        // sidebarLayout
        //
        sidebarLayout.BackColor = Color.Transparent;
        sidebarLayout.ColumnCount = 1;
        sidebarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        sidebarLayout.Controls.Add(lstConfigs, 0, 0);
        sidebarLayout.Controls.Add(buttonsGrid, 0, 1);
        sidebarLayout.Dock = DockStyle.Fill;
        sidebarLayout.Margin = new Padding(0);
        sidebarLayout.Name = "sidebarLayout";
        sidebarLayout.RowCount = 2;
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        sidebarLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        sidebarLayout.TabIndex = 0;
        //
        // lstConfigs
        //
        lstConfigs.Dock = DockStyle.Fill;
        lstConfigs.Margin = new Padding(0, 0, 0, 8);
        lstConfigs.Name = "lstConfigs";
        lstConfigs.SelectionMode = SelectionMode.MultiExtended;
        lstConfigs.TabIndex = 0;
        lstConfigs.SelectedIndexChanged += lstConfigs_SelectedIndexChanged;
        //
        // buttonsGrid
        //
        buttonsGrid.AutoSize = true;
        buttonsGrid.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonsGrid.BackColor = Color.Transparent;
        buttonsGrid.ColumnCount = 2;
        buttonsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        buttonsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        buttonsGrid.Controls.Add(btnNew, 0, 0);
        buttonsGrid.SetColumnSpan(btnNew, 2);
        buttonsGrid.Controls.Add(btnSave, 0, 1);
        buttonsGrid.Controls.Add(btnDelete, 1, 1);
        buttonsGrid.Controls.Add(btnSetting, 0, 2);
        buttonsGrid.Controls.Add(btnAbout, 1, 2);
        buttonsGrid.Controls.Add(btnImport, 0, 3);
        buttonsGrid.Controls.Add(btnExport, 1, 3);
        buttonsGrid.Dock = DockStyle.Fill;
        buttonsGrid.Margin = new Padding(0);
        buttonsGrid.Name = "buttonsGrid";
        buttonsGrid.RowCount = 4;
        buttonsGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        buttonsGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        buttonsGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        buttonsGrid.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        buttonsGrid.TabIndex = 1;
        //
        // btnNew
        //
        btnNew.Dock = DockStyle.Fill;
        btnNew.Height = 34;
        btnNew.Margin = new Padding(4, 4, 4, 4);
        btnNew.Name = "btnNew";
        btnNew.TabIndex = 0;
        btnNew.Text = "New";
        btnNew.Click += btnNew_Click;
        //
        // btnSave
        //
        btnSave.Dock = DockStyle.Fill;
        btnSave.Height = 34;
        btnSave.Margin = new Padding(4, 4, 4, 4);
        btnSave.Name = "btnSave";
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.Click += btnSave_Click;
        //
        // btnDelete
        //
        btnDelete.Dock = DockStyle.Fill;
        btnDelete.Height = 34;
        btnDelete.Margin = new Padding(4, 4, 4, 4);
        btnDelete.Name = "btnDelete";
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete";
        btnDelete.Click += btnDelete_Click;
        //
        // btnSetting
        //
        btnSetting.Dock = DockStyle.Fill;
        btnSetting.Height = 34;
        btnSetting.Margin = new Padding(4, 4, 4, 4);
        btnSetting.Name = "btnSetting";
        btnSetting.TabIndex = 3;
        btnSetting.Text = "Setting";
        btnSetting.Click += btnSetting_Click;
        //
        // btnAbout
        //
        btnAbout.Dock = DockStyle.Fill;
        btnAbout.Height = 34;
        btnAbout.Margin = new Padding(4, 4, 4, 4);
        btnAbout.Name = "btnAbout";
        btnAbout.TabIndex = 4;
        btnAbout.Text = "About";
        btnAbout.Click += btnAbout_Click;
        //
        // btnImport
        //
        btnImport.Dock = DockStyle.Fill;
        btnImport.Height = 34;
        btnImport.Margin = new Padding(4, 4, 4, 4);
        btnImport.Name = "btnImport";
        btnImport.TabIndex = 5;
        btnImport.Text = "Import";
        btnImport.Click += btnImport_Click;
        //
        // btnExport
        //
        btnExport.Dock = DockStyle.Fill;
        btnExport.Height = 34;
        btnExport.Margin = new Padding(4, 4, 4, 4);
        btnExport.Name = "btnExport";
        btnExport.TabIndex = 6;
        btnExport.Text = "Export";
        btnExport.Click += btnExport_Click;
        //
        // grpActions
        //
        grpActions.Controls.Add(contentLayout);
        grpActions.Dock = DockStyle.Fill;
        grpActions.Margin = new Padding(6, 0, 0, 0);
        grpActions.Name = "grpActions";
        grpActions.TabIndex = 1;
        grpActions.TabStop = false;
        grpActions.Text = "Chi tiết hành động";
        //
        // contentLayout
        //
        contentLayout.BackColor = Color.Transparent;
        contentLayout.ColumnCount = 1;
        contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        contentLayout.Controls.Add(nameRow, 0, 0);
        contentLayout.Controls.Add(toolbarFlow, 0, 1);
        contentLayout.Controls.Add(gridActions, 0, 2);
        contentLayout.Controls.Add(actionButtonsFlow, 0, 3);
        contentLayout.Dock = DockStyle.Fill;
        contentLayout.Margin = new Padding(0);
        contentLayout.Name = "contentLayout";
        contentLayout.RowCount = 4;
        contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        contentLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        contentLayout.TabIndex = 0;
        //
        // nameRow
        //
        nameRow.AutoSize = true;
        nameRow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        nameRow.BackColor = Color.Transparent;
        nameRow.ColumnCount = 2;
        nameRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        nameRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        nameRow.Controls.Add(lblConfigName, 0, 0);
        nameRow.Controls.Add(txtConfigName, 1, 0);
        nameRow.Dock = DockStyle.Fill;
        nameRow.Margin = new Padding(0, 0, 0, 8);
        nameRow.Name = "nameRow";
        nameRow.RowCount = 1;
        nameRow.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        nameRow.TabIndex = 0;
        //
        // lblConfigName
        //
        lblConfigName.Anchor = AnchorStyles.Left;
        lblConfigName.AutoSize = true;
        lblConfigName.BackColor = Color.Transparent;
        lblConfigName.Margin = new Padding(0, 0, 8, 0);
        lblConfigName.Name = "lblConfigName";
        lblConfigName.TabIndex = 0;
        lblConfigName.Text = "Tên cấu hình";
        //
        // txtConfigName
        //
        txtConfigName.Dock = DockStyle.Fill;
        txtConfigName.Margin = new Padding(0);
        txtConfigName.Name = "txtConfigName";
        txtConfigName.Size = new Size(600, 30);
        txtConfigName.TabIndex = 1;
        //
        // toolbarFlow
        //
        toolbarFlow.AutoSize = true;
        toolbarFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        toolbarFlow.BackColor = Color.Transparent;
        toolbarFlow.Controls.Add(btnAddKey);
        toolbarFlow.Controls.Add(btnAddMouse);
        toolbarFlow.Dock = DockStyle.Top;
        toolbarFlow.FlowDirection = FlowDirection.LeftToRight;
        toolbarFlow.Margin = new Padding(0, 0, 0, 8);
        toolbarFlow.Name = "toolbarFlow";
        toolbarFlow.TabIndex = 1;
        toolbarFlow.WrapContents = false;
        //
        // btnAddKey
        //
        btnAddKey.AutoSize = false;
        btnAddKey.Margin = new Padding(0, 0, 8, 0);
        btnAddKey.Name = "btnAddKey";
        btnAddKey.Size = new Size(120, 32);
        btnAddKey.TabIndex = 0;
        btnAddKey.Text = "Add Key";
        btnAddKey.Click += btnAddKey_Click;
        //
        // btnAddMouse
        //
        btnAddMouse.AutoSize = false;
        btnAddMouse.Margin = new Padding(0, 0, 8, 0);
        btnAddMouse.Name = "btnAddMouse";
        btnAddMouse.Size = new Size(120, 32);
        btnAddMouse.TabIndex = 1;
        btnAddMouse.Text = "Add Mouse";
        btnAddMouse.Click += btnAddMouse_Click;
        //
        // ctxAddKey
        //
        ctxAddKey.Name = "ctxAddKey";
        ctxAddKey.Renderer = new ModernToolStripRenderer(ThemeManager.Palette);
        ctxAddKey.Size = new Size(61, 4);
        //
        // ctxAddMouse
        //
        ctxAddMouse.Name = "ctxAddMouse";
        ctxAddMouse.Renderer = new ModernToolStripRenderer(ThemeManager.Palette);
        ctxAddMouse.Size = new Size(61, 4);
        //
        // gridActions
        //
        gridActions.AllowUserToAddRows = false;
        gridActions.AllowUserToDeleteRows = false;
        gridActions.Columns.AddRange(new DataGridViewColumn[] { colIndex, colKeys, colCharacters, colCount, colDelay, colGetPosition });
        gridActions.Dock = DockStyle.Fill;
        gridActions.EditMode = DataGridViewEditMode.EditOnEnter;
        gridActions.Margin = new Padding(0);
        gridActions.MultiSelect = true;
        gridActions.Name = "gridActions";
        gridActions.ReadOnly = false;
        gridActions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
        // actionButtonsFlow
        //
        actionButtonsFlow.Anchor = AnchorStyles.None;
        actionButtonsFlow.AutoSize = true;
        actionButtonsFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionButtonsFlow.BackColor = Color.Transparent;
        actionButtonsFlow.Controls.Add(btnMoveUp);
        actionButtonsFlow.Controls.Add(btnMoveDown);
        actionButtonsFlow.Controls.Add(btnDeleteAction);
        actionButtonsFlow.Dock = DockStyle.None;
        actionButtonsFlow.FlowDirection = FlowDirection.LeftToRight;
        actionButtonsFlow.Margin = new Padding(0, 8, 0, 0);
        actionButtonsFlow.Name = "actionButtonsFlow";
        actionButtonsFlow.TabIndex = 3;
        actionButtonsFlow.WrapContents = false;
        //
        // btnMoveUp
        //
        btnMoveUp.Margin = new Padding(5, 4, 5, 4);
        btnMoveUp.Name = "btnMoveUp";
        btnMoveUp.Size = new Size(100, 32);
        btnMoveUp.TabIndex = 0;
        btnMoveUp.Text = "Move Up";
        btnMoveUp.Click += btnMoveUp_Click;
        //
        // btnMoveDown
        //
        btnMoveDown.Margin = new Padding(5, 4, 5, 4);
        btnMoveDown.Name = "btnMoveDown";
        btnMoveDown.Size = new Size(100, 32);
        btnMoveDown.TabIndex = 1;
        btnMoveDown.Text = "Move Down";
        btnMoveDown.Click += btnMoveDown_Click;
        //
        // btnDeleteAction
        //
        btnDeleteAction.Margin = new Padding(5, 4, 5, 4);
        btnDeleteAction.Name = "btnDeleteAction";
        btnDeleteAction.Size = new Size(100, 32);
        btnDeleteAction.TabIndex = 2;
        btnDeleteAction.Text = "Delete";
        btnDeleteAction.Click += btnDeleteAction_Click;
        //
        // statusCard
        //
        statusCard.AutoSize = true;
        statusCard.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        statusCard.Controls.Add(statusLayout);
        statusCard.Dock = DockStyle.Fill;
        statusCard.Margin = new Padding(0, 12, 0, 0);
        statusCard.Name = "statusCard";
        statusCard.TabIndex = 2;
        statusCard.TabStop = false;
        //
        // statusLayout
        //
        statusLayout.AutoSize = true;
        statusLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        statusLayout.BackColor = Color.Transparent;
        statusLayout.ColumnCount = 4;
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        statusLayout.Controls.Add(lblStatus, 0, 0);
        statusLayout.Controls.Add(loopFlow, 1, 0);
        statusLayout.Controls.Add(delayFlow, 2, 0);
        statusLayout.Controls.Add(btnStart, 3, 0);
        statusLayout.Dock = DockStyle.Top;
        statusLayout.Margin = new Padding(0);
        statusLayout.Name = "statusLayout";
        statusLayout.RowCount = 1;
        statusLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        statusLayout.TabIndex = 0;
        //
        // lblStatus
        //
        lblStatus.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        lblStatus.AutoEllipsis = true;
        lblStatus.BackColor = Color.Transparent;
        lblStatus.Margin = new Padding(12);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(400, 20);
        lblStatus.TabIndex = 0;
        lblStatus.Text = "Trạng thái: Unknown";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        //
        // loopFlow
        //
        loopFlow.Anchor = AnchorStyles.None;
        loopFlow.AutoSize = true;
        loopFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        loopFlow.BackColor = Color.Transparent;
        loopFlow.Controls.Add(lblLoop);
        loopFlow.Controls.Add(numLoopCount);
        loopFlow.FlowDirection = FlowDirection.LeftToRight;
        loopFlow.Margin = new Padding(12);
        loopFlow.Name = "loopFlow";
        loopFlow.TabIndex = 1;
        loopFlow.WrapContents = false;
        //
        // lblLoop
        //
        lblLoop.Anchor = AnchorStyles.None;
        lblLoop.AutoSize = true;
        lblLoop.BackColor = Color.Transparent;
        lblLoop.Margin = new Padding(0, 0, 8, 0);
        lblLoop.Name = "lblLoop";
        lblLoop.TabIndex = 0;
        lblLoop.Text = "Số lần lặp lại";
        //
        // numLoopCount
        //
        numLoopCount.Margin = new Padding(0);
        numLoopCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numLoopCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numLoopCount.Name = "numLoopCount";
        numLoopCount.Size = new Size(110, 30);
        numLoopCount.TabIndex = 1;
        numLoopCount.Value = new decimal(new int[] { 30, 0, 0, 0 });
        //
        // delayFlow
        //
        delayFlow.Anchor = AnchorStyles.None;
        delayFlow.AutoSize = true;
        delayFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        delayFlow.BackColor = Color.Transparent;
        delayFlow.Controls.Add(lblGlobalDelay);
        delayFlow.Controls.Add(numGlobalDelay);
        delayFlow.FlowDirection = FlowDirection.LeftToRight;
        delayFlow.Margin = new Padding(12);
        delayFlow.Name = "delayFlow";
        delayFlow.TabIndex = 2;
        delayFlow.WrapContents = false;
        //
        // lblGlobalDelay
        //
        lblGlobalDelay.Anchor = AnchorStyles.None;
        lblGlobalDelay.AutoSize = true;
        lblGlobalDelay.BackColor = Color.Transparent;
        lblGlobalDelay.Margin = new Padding(0, 0, 8, 0);
        lblGlobalDelay.Name = "lblGlobalDelay";
        lblGlobalDelay.TabIndex = 0;
        lblGlobalDelay.Text = "Delay chung (ms)";
        //
        // numGlobalDelay
        //
        numGlobalDelay.Increment = new decimal(new int[] { 50, 0, 0, 0 });
        numGlobalDelay.Margin = new Padding(0);
        numGlobalDelay.Maximum = new decimal(new int[] { 600000, 0, 0, 0 });
        numGlobalDelay.Name = "numGlobalDelay";
        numGlobalDelay.Size = new Size(120, 30);
        numGlobalDelay.TabIndex = 1;
        numGlobalDelay.Value = new decimal(new int[] { 0, 0, 0, 0 });
        //
        // btnStart
        //
        btnStart.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnStart.Margin = new Padding(12);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(150, 40);
        btnStart.StyleKind = ButtonStyleKind.Primary;
        btnStart.TabIndex = 3;
        btnStart.Text = "Start";
        btnStart.Click += btnStart_Click;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 664);
        Controls.Add(rootLayout);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(1000, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "AutoMouseKeyboard";
        delayFlow.ResumeLayout(false);
        delayFlow.PerformLayout();
        loopFlow.ResumeLayout(false);
        loopFlow.PerformLayout();
        statusLayout.ResumeLayout(false);
        statusLayout.PerformLayout();
        statusCard.ResumeLayout(false);
        statusCard.PerformLayout();
        actionButtonsFlow.ResumeLayout(false);
        actionButtonsFlow.PerformLayout();
        toolbarFlow.ResumeLayout(false);
        toolbarFlow.PerformLayout();
        nameRow.ResumeLayout(false);
        nameRow.PerformLayout();
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
        grpActions.ResumeLayout(false);
        grpActions.PerformLayout();
        buttonsGrid.ResumeLayout(false);
        buttonsGrid.PerformLayout();
        sidebarLayout.ResumeLayout(false);
        sidebarLayout.PerformLayout();
        grpConfigs.ResumeLayout(false);
        grpConfigs.PerformLayout();
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        ((ISupportInitialize)gridActions).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
    }
}
