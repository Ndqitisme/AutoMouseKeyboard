using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public sealed class ModifierComboResult
    {
        public static ModifierComboResult None { get; } = new ModifierComboResult(false, false, false);

        public ModifierComboResult(bool holdCtrl, bool holdAlt, bool holdShift)
        {
            HoldCtrl = holdCtrl;
            HoldAlt = holdAlt;
            HoldShift = holdShift;
        }

        public bool HoldCtrl { get; }
        public bool HoldAlt { get; }
        public bool HoldShift { get; }

        public bool HasValue => HoldCtrl || HoldAlt || HoldShift;

        public static ModifierComboResult FromKeys(IEnumerable<Keys> keys)
        {
            var normalized = keys.Select(ModifierKeyTracker.Normalize).ToList();
            var ctrl = normalized.Contains(Keys.ControlKey);
            var alt = normalized.Contains(Keys.Menu);
            var shift = normalized.Contains(Keys.ShiftKey);
            return new ModifierComboResult(ctrl, alt, shift);
        }
    }

    public sealed class ModifierKeyTracker
    {
        private readonly HashSet<Keys> _pressedModifiers = new HashSet<Keys>();
        private readonly HashSet<Keys> _sessionModifiers = new HashSet<Keys>();
        private bool _sessionHasNonModifier;

        public void HandleKeyDown(Keys keyCode)
        {
            if (IsModifierKey(keyCode))
            {
                var normalized = Normalize(keyCode);
                _pressedModifiers.Add(normalized);
                _sessionModifiers.Add(normalized);
                return;
            }

            if (_pressedModifiers.Count > 0)
            {
                _sessionHasNonModifier = true;
            }
        }

        public ModifierComboResult HandleKeyUp(Keys keyCode)
        {
            if (!IsModifierKey(keyCode))
            {
                return ModifierComboResult.None;
            }

            var normalized = Normalize(keyCode);
            _pressedModifiers.Remove(normalized);

            if (_pressedModifiers.Count == 0)
            {
                var result = !_sessionHasNonModifier && _sessionModifiers.Count > 0
                    ? ModifierComboResult.FromKeys(_sessionModifiers)
                    : ModifierComboResult.None;

                ResetSession();
                return result;
            }

            return ModifierComboResult.None;
        }

        public (bool holdCtrl, bool holdAlt, bool holdShift) GetHoldStates()
        {
            return (
                _pressedModifiers.Contains(Keys.ControlKey),
                _pressedModifiers.Contains(Keys.Menu),
                _pressedModifiers.Contains(Keys.ShiftKey)
            );
        }

        public void MarkNonModifierUsed()
        {
            if (_pressedModifiers.Count > 0)
            {
                _sessionHasNonModifier = true;
            }
        }

        public void CancelSession()
        {
            _sessionHasNonModifier = true;
        }

        public void Reset()
        {
            _pressedModifiers.Clear();
            ResetSession();
        }

        private void ResetSession()
        {
            _sessionModifiers.Clear();
            _sessionHasNonModifier = false;
        }

        public static bool IsModifierKey(Keys keyCode)
        {
            return keyCode == Keys.ControlKey || keyCode == Keys.LControlKey || keyCode == Keys.RControlKey || keyCode == Keys.Control
                   || keyCode == Keys.ShiftKey || keyCode == Keys.LShiftKey || keyCode == Keys.RShiftKey || keyCode == Keys.Shift
                   || keyCode == Keys.Menu || keyCode == Keys.LMenu || keyCode == Keys.RMenu || keyCode == Keys.Alt;
        }

        public static Keys Normalize(Keys keyCode)
        {
            if (keyCode == Keys.Control || keyCode == Keys.LControlKey || keyCode == Keys.RControlKey)
            {
                return Keys.ControlKey;
            }
            if (keyCode == Keys.Shift || keyCode == Keys.LShiftKey || keyCode == Keys.RShiftKey)
            {
                return Keys.ShiftKey;
            }
            if (keyCode == Keys.Alt || keyCode == Keys.LMenu || keyCode == Keys.RMenu || keyCode == Keys.Menu)
            {
                return Keys.Menu;
            }
            return keyCode;
        }
    }
}

