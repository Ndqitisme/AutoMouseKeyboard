namespace AutoMouseKeyboard.UI
{
    /// <summary>
    /// Implemented by custom controls that apply a <see cref="ThemePalette"/> to
    /// themselves. <see cref="Utilities.ThemeManager"/> calls this before any
    /// legacy per-type coloring.
    /// </summary>
    public interface IThemedControl
    {
        void ApplyTheme(ThemePalette palette);
    }
}
