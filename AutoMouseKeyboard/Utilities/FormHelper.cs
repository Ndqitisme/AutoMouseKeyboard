
using System;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public static class FormHelper
    {
    public static IDisposable HideFormForOperation(Form form, bool hideOwner = false, bool restoreOnDispose = true)
    {
        if (form == null || form.IsDisposed || form.Disposing)
        {
            return new EmptyDisposable();
        }

        return new FormHider(form, hideOwner, restoreOnDispose);
    }

    public static void SafeInvoke(Form form, Action action)
    {
        if (form == null || form.IsDisposed || form.Disposing || action == null)
        {
            return;
        }

        if (form.InvokeRequired)
        {
            form.Invoke(new Action(() =>
            {
                if (!form.IsDisposed && !form.Disposing)
                {
                    action();
                }
            }));
        }
        else
        {
            action();
        }
    }

    private class FormHider : IDisposable
    {
        private readonly Form _form;
        private readonly Form? _owner;
        private readonly FormWindowState _previousState;
        private readonly FormWindowState _ownerPreviousState;
        private readonly bool _hideOwner;
        private readonly bool _restoreOnDispose;
        private bool _disposed;

        public FormHider(Form form, bool hideOwner, bool restoreOnDispose)
        {
            _form = form;
            _hideOwner = hideOwner;
            _restoreOnDispose = restoreOnDispose;
            _previousState = form.WindowState;
            _owner = null;
            _ownerPreviousState = FormWindowState.Normal;

            if (hideOwner && form.Owner != null && !form.Owner.IsDisposed)
            {
                _owner = form.Owner;
                _ownerPreviousState = _owner.WindowState;
            }

            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() =>
                {
                    if (!_form.IsDisposed && !_form.Disposing)
                    {
                        _form.WindowState = FormWindowState.Minimized;

                        if (_owner != null && !_owner.IsDisposed && !_owner.Disposing)
                        {
                            _owner.WindowState = FormWindowState.Minimized;
                        }
                    }
                }));
            }
            else
            {
                if (!_form.IsDisposed && !_form.Disposing)
                {
                    _form.WindowState = FormWindowState.Minimized;

                    if (_owner != null && !_owner.IsDisposed && !_owner.Disposing)
                    {
                        _owner.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (!_restoreOnDispose)
            {
                return;
            }

            if (_form != null && !_form.IsDisposed && !_form.Disposing)
            {
                if (_form.InvokeRequired)
                {
                    try
                    {
                        _form.Invoke(new Action(() =>
                        {
                            RestoreForm();
                        }));
                    }
                    catch
                    {
                        _form.BeginInvoke(new Action(() =>
                        {
                            RestoreForm();
                        }));
                    }
                }
                else
                {
                    RestoreForm();
                }
            }
        }

        private void RestoreForm()
        {
            if (_form.IsDisposed || _form.Disposing)
            {
                return;
            }

            try
            {
                if (!_form.Visible)
                {
                    _form.Show();
                }

                _form.WindowState = _previousState;
                _form.Activate();
                _form.BringToFront();

                if (_owner != null && !_owner.IsDisposed && !_owner.Disposing)
                {
                    if (!_owner.Visible)
                    {
                        _owner.Show();
                    }

                    _owner.WindowState = _ownerPreviousState;
                    _owner.Activate();
                    _owner.BringToFront();
                }
            }
            catch
            {
            }
        }
    }

    private class EmptyDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
    }
}

