using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;
using AutoMouseKeyboard.Utilities;
using Newtonsoft.Json;

namespace AutoMouseKeyboard
{
    public partial class MainForm
    {
        private void LoadConfigs(string? selectName = null)
        {
            var previousSelected = lstConfigs.SelectedItem as ActionConfig;

            _dragIndex = -1;
            _isDragging = false;

            var allConfigs = _storage.LoadAll().ToList();
            var order = LoadConfigOrder();

            if (order != null && order.Count > 0)
            {
                var orderedConfigs = new List<ActionConfig>();
                var configDict = allConfigs.ToDictionary(c => c.Name, StringComparer.CurrentCultureIgnoreCase);

                foreach (var name in order)
                {
                    if (configDict.TryGetValue(name, out var config))
                    {
                        orderedConfigs.Add(config);
                        configDict.Remove(name);
                    }
                }

                orderedConfigs.AddRange(configDict.Values);

                _configCache = orderedConfigs;
            }
            else
            {
                _configCache = allConfigs;
            }

            lstConfigs.SelectedIndexChanged -= lstConfigs_SelectedIndexChanged;

            lstConfigs.DataSource = null;
            lstConfigs.DataSource = _configCache;
            lstConfigs.DisplayMember = nameof(ActionConfig.Name);
            lstConfigs.SelectedIndex = -1;

            if (!string.IsNullOrWhiteSpace(selectName))
            {
                var index = _configCache.FindIndex(c => c.Name.Equals(selectName, StringComparison.CurrentCultureIgnoreCase));
                if (index >= 0)
                {
                    lstConfigs.SelectedIndex = index;
                }
            }
            else if (previousSelected != null)
            {
                var index = _configCache.FindIndex(c => c.Name.Equals(previousSelected.Name, StringComparison.CurrentCultureIgnoreCase));
                if (index >= 0)
                {
                    lstConfigs.SelectedIndex = index;
                }
            }

            lstConfigs.SelectedIndexChanged += lstConfigs_SelectedIndexChanged;
        }

        private void SetupDragAndDrop()
        {
            lstConfigs.AllowDrop = true;
            lstConfigs.MouseDown += lstConfigs_MouseDown;
            lstConfigs.MouseMove += lstConfigs_MouseMove;
            lstConfigs.DragOver += lstConfigs_DragOver;
            lstConfigs.DragDrop += lstConfigs_DragDrop;
            lstConfigs.DragLeave += lstConfigs_DragLeave;
        }

        private void lstConfigs_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var index = lstConfigs.IndexFromPoint(e.Location);
                if (index >= 0 && index < lstConfigs.Items.Count)
                {
                    _dragIndex = index;
                    _mouseDownLocation = e.Location;
                    _isDragging = false;
                }
            }
        }

        private void lstConfigs_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _dragIndex >= 0 && !_isDragging)
            {
                if (_dragIndex < 0 || _dragIndex >= lstConfigs.Items.Count)
                {
                    _dragIndex = -1;
                    _isDragging = false;
                    return;
                }

                var distance = Math.Abs(e.X - _mouseDownLocation.X) + Math.Abs(e.Y - _mouseDownLocation.Y);
                if (distance > DragThreshold)
                {
                    _isDragging = true;
                    lstConfigs.DoDragDrop(lstConfigs.Items[_dragIndex], DragDropEffects.Move);
                    _isDragging = false;
                    _dragIndex = -1;
                }
            }
        }

        private void lstConfigs_DragOver(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            var point = lstConfigs.PointToClient(new Point(e.X, e.Y));
            var index = lstConfigs.IndexFromPoint(point);

            if (index >= 0 && index < _configCache.Count && index != _dragIndex)
            {
                lstConfigs.SelectedIndex = index;
            }
        }

        private void lstConfigs_DragLeave(object? sender, EventArgs e)
        {
            if (_dragIndex >= 0)
            {
                _dragIndex = -1;
                _isDragging = false;
            }
        }

        private void lstConfigs_DragDrop(object? sender, DragEventArgs e)
        {
            if (_dragIndex < 0 || _dragIndex >= _configCache.Count)
            {
                _dragIndex = -1;
                _isDragging = false;
                return;
            }

            var point = lstConfigs.PointToClient(new Point(e.X, e.Y));
            var dropIndex = lstConfigs.IndexFromPoint(point);

            if (dropIndex < 0 || dropIndex >= _configCache.Count || dropIndex == _dragIndex)
            {
                _dragIndex = -1;
                _isDragging = false;
                return;
            }

            var item = _configCache[_dragIndex];
            _configCache.RemoveAt(_dragIndex);
            _configCache.Insert(dropIndex, item);

            SaveConfigOrder();

            var selectedName = item.Name;
            lstConfigs.DataSource = null;
            lstConfigs.DataSource = _configCache;
            lstConfigs.DisplayMember = nameof(ActionConfig.Name);

            var newIndex = _configCache.FindIndex(c => c.Name.Equals(selectedName, StringComparison.CurrentCultureIgnoreCase));
            if (newIndex >= 0)
            {
                lstConfigs.SelectedIndex = newIndex;
            }

            _dragIndex = -1;
            _isDragging = false;
            UpdateStatus(string.Format(LanguageManager.GetString("Status_ConfigMoved"), item.Name), "Status_ConfigMoved", item.Name);
        }

        private void SaveConfigOrder()
        {
            try
            {
                var order = _configCache.Select(c => c.Name).ToList();
                AppSettings.ConfigOrder = order;

                var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoMouseKeyboard");
                var oldOrderFile = Path.Combine(baseFolder, "config_order.json");
                if (File.Exists(oldOrderFile))
                {
                    try
                    {
                        File.Delete(oldOrderFile);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private List<string> LoadConfigOrder()
        {
            try
            {
                var order = AppSettings.ConfigOrder;
                if (order != null && order.Count > 0)
                {
                    return order;
                }

                var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AutoMouseKeyboard");
                var oldOrderFile = Path.Combine(baseFolder, "config_order.json");
                if (File.Exists(oldOrderFile))
                {
                    try
                    {
                        var json = File.ReadAllText(oldOrderFile);
                        var oldOrder = JsonConvert.DeserializeObject<List<string>>(json);
                        if (oldOrder != null && oldOrder.Count > 0)
                        {
                            AppSettings.ConfigOrder = oldOrder;
                            File.Delete(oldOrderFile);
                            return oldOrder;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return null!;
        }

        private void lstConfigs_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstConfigs.SelectedItems.Count == 1 && lstConfigs.SelectedItem is ActionConfig selected)
            {
                LoadConfig(selected);
            }
        }

        private void LoadConfig(ActionConfig config)
        {
            _loadedConfigName = config.Name;
            txtConfigName.Text = config.Name;
            ApplyActions(config.Actions);
            UpdateStatus(string.Format(LanguageManager.GetString("Status_ConfigLoaded"), config.Name), "Status_ConfigLoaded", config.Name);
        }

        private void ApplyActions(IEnumerable<ActionStep> actions)
        {
            _actions.ListChanged -= Actions_ListChanged;
            _actions = new BindingList<ActionStep>(actions.Select(a => a.Clone()).ToList());
            _actions.ListChanged += Actions_ListChanged;
            gridActions.DataSource = _actions;
            UpdateRowNumbers();
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            txtConfigName.Text = string.Format("{0} {1}", GetDefaultConfigName(), DateTime.Now.ToString("HHmmss"));
            _loadedConfigName = null;
            ApplyActions(Array.Empty<ActionStep>());
            UpdateStatus(LanguageManager.GetString("Status_CreatingNew"), "Status_CreatingNew");
        }

        private static string GetDefaultConfigName()
        {
            var defaultName = LanguageManager.GetString("MainForm_DefaultConfigName");
            return string.IsNullOrWhiteSpace(defaultName) ? "New Config" : defaultName;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                var name = txtConfigName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show(
                        LanguageManager.GetString("Msg_ConfigNameRequired"),
                        LanguageManager.GetString("Msg_Confirm"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var config = BuildConfigFromForm();
                var newName = config.Name.Trim();

                if (!string.IsNullOrWhiteSpace(newName) &&
                    !string.Equals(_loadedConfigName, newName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var existingConfig = _configCache.FirstOrDefault(c =>
                        string.Equals(c.Name, newName, StringComparison.CurrentCultureIgnoreCase));

                    if (existingConfig != null)
                    {
                        MessageBox.Show(
                            string.Format(LanguageManager.GetString("Msg_DuplicateConfigName"), newName),
                            LanguageManager.GetString("Msg_Confirm"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }

                _storage.Save(config, _loadedConfigName);
                _loadedConfigName = config.Name;

                SaveConfigOrder();

                LoadConfigs(config.Name);
                UpdateStatus(LanguageManager.GetString("Status_ConfigSaved"), "Status_ConfigSaved");
                MessageBox.Show(LanguageManager.GetString("Msg_SaveSuccess"), LanguageManager.GetString("Msg_Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_SaveFailed"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CopyConfig(ActionConfig config)
        {
            _copiedConfig = CloneConfig(config);
            UpdateStatus(string.Format(LanguageManager.GetString("Status_ConfigCopied"), config.Name), "Status_ConfigCopied", config.Name);
        }

        private void PasteConfig()
        {
            if (_copiedConfig == null)
            {
                return;
            }

            try
            {
                var baseName = _copiedConfig.Name.Trim();
                var newName = GenerateCopyName(baseName);
                var newConfig = CloneConfig(_copiedConfig);
                newConfig.Name = newName;

                _storage.Save(newConfig);
                LoadConfigs(newName);
                UpdateStatus(string.Format(LanguageManager.GetString("Status_ConfigCreated"), newName), "Status_ConfigCreated", newName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_PasteFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ActionConfig CloneConfig(ActionConfig config)
        {
            return new ActionConfig
            {
                Name = config.Name,
                Actions = config.Actions.Select(a => a.Clone()).ToList()
            };
        }

        private string GenerateCopyName(string baseName)
        {
            var index = 1;
            var candidate = string.Format("{0} {1}", baseName, index);

            while (_configCache.Any(c => string.Equals(c.Name, candidate, StringComparison.CurrentCultureIgnoreCase)))
            {
                index++;
                candidate = string.Format("{0} {1}", baseName, index);
            }

            return candidate;
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            var selectedConfigs = lstConfigs.SelectedItems.Cast<ActionConfig>().ToList();
            if (selectedConfigs.Count == 0)
            {
                return;
            }

            string confirmMessage;
            if (selectedConfigs.Count == 1)
            {
                confirmMessage = string.Format(LanguageManager.GetString("Msg_ConfirmDelete"), selectedConfigs[0].Name);
            }
            else
            {
                confirmMessage = string.Format(LanguageManager.GetString("Msg_ConfirmDeleteMultiple"), selectedConfigs.Count);
            }

            var confirm = MessageBox.Show(confirmMessage, LanguageManager.GetString("Msg_Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            foreach (var config in selectedConfigs)
            {
                _storage.Delete(config.Name);
            }

            _loadedConfigName = null;
            LoadConfigs();
            ApplyActions(Array.Empty<ActionStep>());
            UpdateStatus(LanguageManager.GetString("Status_ConfigDeleted"), "Status_ConfigDeleted");
        }

        private ActionConfig BuildConfigFromForm()
        {
            var name = txtConfigName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = string.Format("{0} {1}", GetDefaultConfigName(), DateTime.Now.ToString("HHmmss"));
            }

            var list = _actions.Where(a => a != null).Select(a => a.Clone()).ToList();
            if (list.Count == 0)
            {
                throw new InvalidOperationException(LanguageManager.GetString("Msg_ConfigMustHaveActions"));
            }

            ValidateActions(list);
            return new ActionConfig
            {
                Name = name,
                Actions = list
            };
        }

        private static void ValidateActions(IReadOnlyList<ActionStep> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var action = actions[i];
                if (action.Type == ActionKind.Keyboard && string.IsNullOrWhiteSpace(action.Key))
                {
                    throw new InvalidOperationException(
                        string.Format(LanguageManager.GetString("Msg_ActionMissingKey"), i + 1));
                }

                action.Delay = Math.Max(0, action.Delay);
                action.Repeat = Math.Max(1, action.Repeat);
            }
        }

        private void btnImport_Click(object? sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog
            {
                Filter = "JSON (*.json)|*.json",
                Title = LanguageManager.GetString("MainForm_ImportConfig"),
                Multiselect = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var fileNames = dialog.FileNames;
                if (fileNames.Length == 1)
                {
                    try
                    {
                        var name = _storage.Import(fileNames[0]);
                        LoadConfigs(name);
                        UpdateStatus(string.Format(LanguageManager.GetString("Status_ConfigImported"), name), "Status_ConfigImported", name);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, LanguageManager.GetString("Msg_ImportFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var importedCount = 0;
                    string? lastImportedName = null;
                    foreach (var fileName in fileNames)
                    {
                        try
                        {
                            lastImportedName = _storage.Import(fileName);
                            importedCount++;
                        }
                        catch
                        {
                        }
                    }

                    LoadConfigs(lastImportedName);
                    UpdateStatus(
                        string.Format(LanguageManager.GetString("Status_MultiConfigImported"), importedCount),
                        "Status_MultiConfigImported",
                        importedCount.ToString());
                    MessageBox.Show(
                        string.Format(LanguageManager.GetString("Msg_MultiImportSuccess"), importedCount),
                        LanguageManager.GetString("Msg_Success"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            var selectedConfigs = lstConfigs.SelectedItems.Cast<ActionConfig>().ToList();

            if (selectedConfigs.Count == 0)
            {
                MessageBox.Show(
                    LanguageManager.GetString("Msg_NoConfigSelected"),
                    LanguageManager.GetString("Msg_ExportFailed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (selectedConfigs.Count == 1)
            {
                ExportSingleConfig(selectedConfigs[0]);
            }
            else
            {
                ExportMultipleConfigs(selectedConfigs);
            }
        }

        private void ExportSingleConfig(ActionConfig config)
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = "JSON (*.json)|*.json",
                FileName = string.Format("{0}.json", config.Name),
                Title = LanguageManager.GetString("MainForm_ExportConfig")
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _storage.Export(config, dialog.FileName);
                UpdateStatus(LanguageManager.GetString("Status_ConfigExported"), "Status_ConfigExported");
            }
        }

        private void ExportMultipleConfigs(List<ActionConfig> configs)
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = LanguageManager.GetString("MainForm_SelectExportFolder"),
                ShowNewFolderButton = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var exportedCount = 0;
                foreach (var config in configs)
                {
                    try
                    {
                        var filePath = Path.Combine(dialog.SelectedPath, string.Format("{0}.json", config.Name));
                        _storage.Export(config, filePath);
                        exportedCount++;
                    }
                    catch
                    {
                    }
                }

                UpdateStatus(
                    string.Format(LanguageManager.GetString("Status_MultiConfigExported"), exportedCount),
                    "Status_MultiConfigExported",
                    exportedCount.ToString());
                MessageBox.Show(
                    string.Format(LanguageManager.GetString("Msg_MultiExportSuccess"), exportedCount),
                    LanguageManager.GetString("Msg_Success"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}

