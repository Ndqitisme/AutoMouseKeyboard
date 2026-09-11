#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Services;
using AutoMouseKeyboard.UI.Controls;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class MainForm : Form
    {
        private const int DefaultActionDelay = 300;

        private readonly ConfigStorage _storage = new ConfigStorage();
        private readonly InputDispatcher _inputDispatcher = new InputDispatcher();
        private readonly AutomationRunner _runner;

        private BindingList<ActionStep> _actions = new BindingList<ActionStep>();
        private CancellationTokenSource _runTokenSource;
        private List<ActionConfig> _configCache = new List<ActionConfig>();
        private string _loadedConfigName;
        private bool _hasShownTelexWarning = false;
        private bool _hasShownCapturePositionHint = false;
        private EscapeKeyMonitor _escapeKeyMonitor;
        private ActionConfig _copiedConfig;
        private string _editingColumnName;
        private int _editingRowIndex = -1;
        private List<int> _selectedRowIndices = new List<int>();
        private string _currentStatusKey;
        private object[] _currentStatusParams;
        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private ToolStripMenuItem _trayOpenMenuItem;
        private ToolStripMenuItem _trayExitMenuItem;
        private bool _exitRequested;
        private bool _hasShownTrayHint;
        private readonly ModifierKeyTracker _modifierTracker = new ModifierKeyTracker();

        private int _dragIndex = -1;
        private Point _mouseDownLocation;
        private bool _isDragging = false;
        private const int DragThreshold = 5;

        public MainForm()
        {
            InitializeComponent();
            LoadFormIcon();
            KeyPreview = true;
            KeyDown += MainForm_KeyDown;
            Deactivate += MainForm_Deactivate;
            Activated += MainForm_Activated;
            _runner = new AutomationRunner(_inputDispatcher);
            ConfigureGrid();
            InitializeActionMenus();
            SetupDragAndDrop();
            LoadConfigs();
            LoadSettings();
            ThemeManager.RegisterForm(this);
            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
            LanguageManager.RegisterForm(this);
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            UpdateLanguage();
            UpdateStatus(LanguageManager.GetString("Status_Ready"), "Status_Ready");
            InitializeTrayIcon();

            numLoopCount.ValueChanged += NumLoopCount_ValueChanged;
            numGlobalDelay.ValueChanged += NumGlobalDelay_ValueChanged;
            Shown += MainForm_Shown;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (gridActions != null && gridActions.IsHandleCreated)
                        {
                            AdjustColumnWidths();
                        }
                    }));
                }
            }
            catch
            {
            }
        }

        private void LoadSettings()
        {
            var savedLoopCount = FirstRunHelper.LoadLoopCount(30);
            var savedGlobalDelay = FirstRunHelper.LoadGlobalDelay(0);

            numLoopCount.Value = Math.Max(numLoopCount.Minimum, Math.Min(numLoopCount.Maximum, savedLoopCount));
            numGlobalDelay.Value = Math.Max(numGlobalDelay.Minimum, Math.Min(numGlobalDelay.Maximum, savedGlobalDelay));
        }

        private void NumLoopCount_ValueChanged(object sender, EventArgs e)
        {
            FirstRunHelper.SaveLoopCount((int)numLoopCount.Value);
        }

        private void NumGlobalDelay_ValueChanged(object sender, EventArgs e)
        {
            FirstRunHelper.SaveGlobalDelay((int)numGlobalDelay.Value);
        }

        private void LanguageManager_LanguageChanged(object sender, LanguageChangedEventArgs e)
        {
            UpdateLanguage();
        }

        private void ThemeManager_ThemeChanged(object sender, ThemeMode e)
        {
            RefreshMenuRenderers();
        }

        private void RefreshMenuRenderers()
        {
            var palette = ThemeManager.Palette;
            ctxAddKey.Renderer = new ModernToolStripRenderer(palette);
            ctxAddMouse.Renderer = new ModernToolStripRenderer(palette);
            if (_trayMenu != null)
            {
                _trayMenu.Renderer = new ModernToolStripRenderer(palette);
            }

            // MultiColumnMenuStrip caches BackColor + a palette-bound renderer at
            // construction; the lazily-created section dropdowns would otherwise
            // stay on the old palette after a theme switch.
            foreach (var item in ctxAddKey.Items.OfType<ToolStripMenuItem>())
            {
                if (item.DropDown is MultiColumnMenuStrip dropDown)
                {
                    dropDown.Renderer = new ModernToolStripRenderer(palette);
                    dropDown.BackColor = palette.MenuBack;
                }
            }
        }

        public void UpdateLanguage()
        {
            Text = LanguageManager.GetString("MainForm_Title");
            grpConfigs.Text = LanguageManager.GetString("MainForm_ConfigList");
            grpActions.Text = LanguageManager.GetString("MainForm_ActionDetails");
            lblConfigName.Text = LanguageManager.GetString("MainForm_ConfigName");
            btnNew.Text = LanguageManager.GetString("MainForm_New");
            btnSave.Text = LanguageManager.GetString("MainForm_Save");
            btnDelete.Text = LanguageManager.GetString("MainForm_Delete");
            btnSetting.Text = LanguageManager.GetString("MainForm_Setting");
            btnAbout.Text = LanguageManager.GetString("MainForm_About");
            btnImport.Text = LanguageManager.GetString("MainForm_Import");
            btnExport.Text = LanguageManager.GetString("MainForm_Export");
            btnAddKey.Text = LanguageManager.GetString("MainForm_AddKey");
            btnAddMouse.Text = LanguageManager.GetString("MainForm_AddMouse");
            btnMoveUp.Text = LanguageManager.GetString("MainForm_MoveUp");
            btnMoveDown.Text = LanguageManager.GetString("MainForm_MoveDown");
            btnDeleteAction.Text = LanguageManager.GetString("MainForm_Delete");
            btnStart.Text = LanguageManager.GetString("MainForm_Start");
            lblLoop.Text = LanguageManager.GetString("MainForm_LoopCount");
            lblGlobalDelay.Text = LanguageManager.GetString("MainForm_GlobalDelay");
            colIndex.HeaderText = LanguageManager.GetString("MainForm_ColIndex");
            colKeys.HeaderText = LanguageManager.GetString("MainForm_ColKeys");
            colCharacters.HeaderText = LanguageManager.GetString("MainForm_ColCharacters");
            colCount.HeaderText = LanguageManager.GetString("MainForm_ColCount");
            colDelay.HeaderText = LanguageManager.GetString("MainForm_ColDelay");
            colGetPosition.Text = LanguageManager.GetString("MainForm_ColSetPosition");

            if (!string.IsNullOrEmpty(_currentStatusKey))
            {
                string statusMessage;
                if (_currentStatusParams != null && _currentStatusParams.Length > 0)
                {
                    statusMessage = string.Format(LanguageManager.GetString(_currentStatusKey), _currentStatusParams);
                }
                else
                {
                    statusMessage = LanguageManager.GetString(_currentStatusKey);
                }
                lblStatus.Text = statusMessage;
            }

            UpdateTrayLanguage();

            try
            {
                if (IsHandleCreated && gridActions != null)
                {
                    BeginInvoke(new Action(() =>
                    {
                        AdjustColumnWidths();
                    }));
                }
                else
                {
                    AdjustColumnWidths();
                }
            }
            catch
            {
            }

            ActionStep.RefreshMouseOptions();
            InitializeActionMenus();
        }

        private void UpdateTrayLanguage()
        {
            if (_trayOpenMenuItem != null)
            {
                _trayOpenMenuItem.Text = LanguageManager.GetString("Tray_OpenApp");
            }

            if (_trayExitMenuItem != null)
            {
                _trayExitMenuItem.Text = LanguageManager.GetString("Tray_ExitApp");
            }

            if (_trayIcon != null)
            {
                _trayIcon.Text = LanguageManager.GetString("Tray_Tooltip");
                _trayIcon.BalloonTipTitle = LanguageManager.GetString("Tray_Tooltip");
                _trayIcon.BalloonTipText = LanguageManager.GetString("Tray_RunningInBackground");
            }
        }

        private void InitializeTrayIcon()
        {
            try
            {
                _trayOpenMenuItem = new ToolStripMenuItem(LanguageManager.GetString("Tray_OpenApp"), null, (s, e) => ShowFromTray());
                _trayExitMenuItem = new ToolStripMenuItem(LanguageManager.GetString("Tray_ExitApp"), null, (s, e) => ExitApplication());

                _trayMenu = new ContextMenuStrip(components);
                _trayMenu.Items.AddRange(new ToolStripItem[] { _trayOpenMenuItem, _trayExitMenuItem });
                _trayMenu.Renderer = new ModernToolStripRenderer(ThemeManager.Palette);

                _trayIcon = new NotifyIcon(components)
                {
                    Icon = Icon ?? SystemIcons.Application,
                    Visible = true,
                    ContextMenuStrip = _trayMenu
                };

                UpdateTrayLanguage();
                _trayIcon.DoubleClick += (s, e) => ShowFromTray();
            }
            catch
            {
            }
        }

        private void HideToTray()
        {
            try
            {
                if (_trayIcon != null)
                {
                    _trayIcon.Visible = true;
                    if (!_hasShownTrayHint)
                    {
                        _trayIcon.ShowBalloonTip(2000);
                        _hasShownTrayHint = true;
                    }
                }

                ShowInTaskbar = false;
                Hide();
            }
            catch
            {
            }
        }

        private void ShowFromTray()
        {
            try
            {
                ShowInTaskbar = true;
                Show();

                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }

                Activate();
                TopMost = true;
                TopMost = false;
                BringToFront();
            }
            catch
            {
            }
        }

        private void ExitApplication()
        {
            try
            {
                _exitRequested = true;
                if (_trayIcon != null)
                {
                    _trayIcon.Visible = false;
                }
                Close();
            }
            catch
            {
            }
        }

        private void AdjustColumnWidths()
        {
            try
            {
                if (gridActions == null || gridActions.Columns.Count == 0)
                {
                    return;
                }

                if (!gridActions.IsHandleCreated)
                {
                    if (IsHandleCreated)
                    {
                        BeginInvoke(new Action(AdjustColumnWidths));
                    }
                    return;
                }

                var headerFont = gridActions.ColumnHeadersDefaultCellStyle.Font ?? gridActions.Font;
                var padding = 30;

                using (var g = gridActions.CreateGraphics())
                {
                    AdjustColumnWidth(colIndex, headerFont, padding, g, 50);
                    AdjustColumnWidth(colCharacters, headerFont, padding, g, 80);
                    AdjustColumnWidth(colCount, headerFont, padding, g, 0);
                    AdjustColumnWidth(colDelay, headerFont, padding, g, 80);
                    AdjustButtonColumnWidth(colGetPosition, headerFont, padding, g, 100);
                }

                gridActions.Refresh();
            }
            catch
            {
            }
        }

        private static void AdjustColumnWidth(DataGridViewColumn column, Font font, int padding, Graphics g, int minWidth)
        {
            if (column == null || string.IsNullOrEmpty(column.HeaderText))
            {
                return;
            }

            var textSize = g.MeasureString(column.HeaderText, font);
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            var width = (int)Math.Ceiling(textSize.Width) + padding;
            column.Width = minWidth > 0 ? Math.Max(width, minWidth) : width;
        }

        private static void AdjustButtonColumnWidth(DataGridViewButtonColumn column, Font font, int padding, Graphics g, int minWidth)
        {
            if (column == null || string.IsNullOrEmpty(column.Text))
            {
                return;
            }

            var textSize = g.MeasureString(column.Text, font);
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            column.Width = Math.Max((int)Math.Ceiling(textSize.Width) + padding, minWidth);
        }

        private void LoadFormIcon()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();

                var pngStream = assembly.GetManifestResourceStream("AutoMouseKeyboard.assets.app.png");
                if (pngStream != null)
                {
                    using (var bitmap = new Bitmap(pngStream))
                    {
                        Icon = ConvertBitmapToIcon(bitmap);
                        if (Icon != null) return;
                    }
                }

                var icoStream = assembly.GetManifestResourceStream("AutoMouseKeyboard.assets.app.ico");
                if (icoStream != null)
                {
                    Icon = new Icon(icoStream);
                    return;
                }

                var assetsPngPath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "app.png");
                if (File.Exists(assetsPngPath))
                {
                    Icon = ConvertPngToIcon(assetsPngPath);
                    if (Icon != null) return;
                }

                var assetsIconPath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "app.ico");
                if (File.Exists(assetsIconPath))
                {
                    Icon = new Icon(assetsIconPath);
                }
            }
            catch
            {
            }
        }

        private Icon ConvertBitmapToIcon(Bitmap bitmap)
        {
            try
            {
                using (var resized = new Bitmap(bitmap, new Size(256, 256)))
                {
                    var iconHandle = resized.GetHicon();
                    var icon = Icon.FromHandle(iconHandle);
                    return new Icon(icon, icon.Size);
                }
            }
            catch
            {
                return null;
            }
        }

        private Icon ConvertPngToIcon(string pngPath)
        {
            try
            {
                using (var bitmap = new Bitmap(pngPath))
                {
                    using (var resized = new Bitmap(bitmap, new Size(256, 256)))
                    {
                        var iconHandle = resized.GetHicon();
                        var icon = Icon.FromHandle(iconHandle);
                        return new Icon(icon, icon.Size);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_runTokenSource != null)
            {
                _runTokenSource.Cancel();
                return;
            }

            ActionConfig config;
            try
            {
                config = BuildConfigFromForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_RunFailed"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var loops = (int)numLoopCount.Value;
            var globalDelay = (int)numGlobalDelay.Value;

            _runTokenSource = new CancellationTokenSource();
            ToggleRunningState(true);

            _escapeKeyMonitor = new EscapeKeyMonitor(() =>
            {
                if (_runTokenSource != null)
                {
                    _runTokenSource.Cancel();
                }
            });

            using (_escapeKeyMonitor)
            using (FormHelper.HideFormForOperation(this, false, !AppSettings.HideOnCompletion))
            {
                try
                {
                    await Task.Run(() => _runner.Run(config.Actions, loops, globalDelay, _runTokenSource.Token));
                    UpdateStatus(LanguageManager.GetString("Status_Completed"), "Status_Completed");
                }
                catch (OperationCanceledException)
                {
                    UpdateStatus(LanguageManager.GetString("Status_EscPressed"), "Status_EscPressed");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_RunError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus(LanguageManager.GetString("Status_Error"), "Status_Error");
                }
                finally
                {
                    if (_runTokenSource != null)
                    {
                        _runTokenSource.Dispose();
                    }
                    _runTokenSource = null;
                    _escapeKeyMonitor = null;
                    ToggleRunningState(false, false);
                }
            }
        }

        private void ToggleRunningState(bool isRunning, bool updateIdleStatus = true)
        {
            grpConfigs.Enabled = !isRunning;
            grpActions.Enabled = !isRunning;
            btnStart.Text = isRunning ? LanguageManager.GetString("MainForm_Stop") : LanguageManager.GetString("MainForm_Start");

            if (isRunning)
            {
                UpdateStatus(LanguageManager.GetString("Status_Running"), "Status_Running");
            }
            else if (updateIdleStatus)
            {
                UpdateStatus(LanguageManager.GetString("Status_Stopped"), "Status_Stopped");
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            using (var dialog = new SettingForm() { Owner = this })
            {
                dialog.ShowDialog(this);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (var dialog = new AboutForm() { Owner = this })
            {
                dialog.ShowDialog(this);
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _runTokenSource != null)
            {
                _runTokenSource.Cancel();
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                if (lstConfigs.Focused && lstConfigs.SelectedItem is ActionConfig selected)
                {
                    CopyConfig(selected);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            if (e.Control && e.KeyCode == Keys.V)
            {
                if (lstConfigs.Focused && _copiedConfig != null)
                {
                    PasteConfig();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
            }
        }

        private void MainForm_Activated(object sender, EventArgs e) => ResetModifierTracking();

        private void MainForm_Deactivate(object sender, EventArgs e) => ResetModifierTracking();

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_exitRequested && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                HideToTray();
                return;
            }

            if (_runTokenSource != null)
            {
                _runTokenSource.Cancel();
            }
            LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
            ThemeManager.ThemeChanged -= ThemeManager_ThemeChanged;
            ThemeManager.UnregisterForm(this);
            LanguageManager.UnregisterForm(this);
            _trayIcon?.Dispose();
            base.OnFormClosing(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == AppInstanceManager.ShowWindowMessage)
            {
                ShowFromTray();
            }

            base.WndProc(ref m);
        }

        private void UpdateStatus(string message, string statusKey = null, params object[] parameters)
        {
            lblStatus.Text = message;
            _currentStatusKey = statusKey;
            _currentStatusParams = parameters;
        }

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            ctxAddKey.Show(btnAddKey, new Point(0, btnAddKey.Height));
        }

        private void btnAddMouse_Click(object sender, EventArgs e)
        {
            ctxAddMouse.Show(btnAddMouse, new Point(0, btnAddMouse.Height));
        }
    }
}

