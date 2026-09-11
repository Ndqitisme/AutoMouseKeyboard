using System;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public class EnterAwareDataGridView : DataGridView
    {
        public event EventHandler<KeyEventArgs>? EnterKeyPressed;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter && IsCurrentCellInEditMode)
            {
                var args = new KeyEventArgs(keyData);
                EnterKeyPressed?.Invoke(this, args);
                
                if (args.Handled)
                {
                    return true;
                }
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
