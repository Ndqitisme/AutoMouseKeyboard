using System.Collections.Generic;
using System.Drawing;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard.UI
{
    /// <summary>
    /// Immutable set of design-token colors for a single <see cref="ThemeMode"/>.
    /// Obtain instances via <see cref="Get(ThemeMode)"/>; instances are cached.
    /// </summary>
    public sealed class ThemePalette
    {
        private static readonly Dictionary<ThemeMode, ThemePalette> Cache = new Dictionary<ThemeMode, ThemePalette>();

        public Color WindowBack { get; }
        public Color CardBack { get; }
        public Color InputBack { get; }
        public Color Border { get; }
        public Color Text { get; }
        public Color TextMuted { get; }
        public Color Accent { get; }
        public Color AccentHover { get; }
        public Color AccentPressed { get; }
        public Color AccentText { get; }
        public Color HoverBack { get; }
        public Color SelectionBack { get; }
        public Color SelectionText { get; }
        public Color GridLine { get; }
        public Color MenuBack { get; }
        public Color MenuHover { get; }
        public Color MenuText { get; }

        private ThemePalette(
            Color windowBack, Color cardBack, Color inputBack, Color border,
            Color text, Color textMuted, Color accent, Color accentHover,
            Color accentPressed, Color accentText, Color hoverBack,
            Color selectionBack, Color selectionText, Color gridLine,
            Color menuBack, Color menuHover, Color menuText)
        {
            WindowBack = windowBack;
            CardBack = cardBack;
            InputBack = inputBack;
            Border = border;
            Text = text;
            TextMuted = textMuted;
            Accent = accent;
            AccentHover = accentHover;
            AccentPressed = accentPressed;
            AccentText = accentText;
            HoverBack = hoverBack;
            SelectionBack = selectionBack;
            SelectionText = selectionText;
            GridLine = gridLine;
            MenuBack = menuBack;
            MenuHover = menuHover;
            MenuText = menuText;
        }

        /// <summary>Returns the cached palette for the given theme.</summary>
        public static ThemePalette Get(ThemeMode mode)
        {
            lock (Cache)
            {
                if (!Cache.TryGetValue(mode, out var palette))
                {
                    palette = Build(mode);
                    Cache[mode] = palette;
                }

                return palette;
            }
        }

        private static ThemePalette Build(ThemeMode mode)
        {
            switch (mode)
            {
                case ThemeMode.Light:
                    return new ThemePalette(
                        windowBack: Hex("#F3F3F3"), cardBack: Hex("#FFFFFF"), inputBack: Hex("#FFFFFF"),
                        border: Hex("#D8D8D8"), text: Hex("#1F1F1F"), textMuted: Hex("#616161"),
                        accent: Hex("#0E639C"), accentHover: Hex("#1177BB"), accentPressed: Hex("#0B5A8E"),
                        accentText: Hex("#FFFFFF"), hoverBack: Hex("#E8E8E8"),
                        selectionBack: Hex("#D6EBFF"), selectionText: Hex("#1F1F1F"),
                        gridLine: Hex("#E5E5E5"), menuBack: Hex("#F8F8F8"),
                        menuHover: Hex("#E8E8E8"), menuText: Hex("#1F1F1F"));

                case ThemeMode.Dark:
                    return new ThemePalette(
                        windowBack: Hex("#1E1E1E"), cardBack: Hex("#252526"), inputBack: Hex("#3C3C3C"),
                        border: Hex("#3C3C3C"), text: Hex("#CCCCCC"), textMuted: Hex("#9D9D9D"),
                        accent: Hex("#0E639C"), accentHover: Hex("#1177BB"), accentPressed: Hex("#0B5A8E"),
                        accentText: Hex("#FFFFFF"), hoverBack: Hex("#2A2D2E"),
                        selectionBack: Hex("#094771"), selectionText: Hex("#FFFFFF"),
                        gridLine: Hex("#2D2D30"), menuBack: Hex("#1B1B1C"),
                        menuHover: Hex("#2A2D2E"), menuText: Hex("#CCCCCC"));

                // The 9 color themes keep their original BackColor/ForeColor/
                // Accent/BorderColor values (from the old GetThemeColors) as
                // WindowBack/Text/Accent/Border; the remaining tokens are
                // derived deterministically per the lerp rules.
                case ThemeMode.Cyan:
                    return Derived(
                        windowBack: Color.FromArgb(224, 247, 250), text: Color.FromArgb(0, 96, 100),
                        accent: Color.FromArgb(0, 188, 212), border: Color.FromArgb(0, 151, 167));

                case ThemeMode.Pink:
                    return Derived(
                        windowBack: Color.FromArgb(252, 228, 236), text: Color.FromArgb(136, 14, 79),
                        accent: Color.FromArgb(233, 30, 99), border: Color.FromArgb(194, 24, 91));

                case ThemeMode.Green:
                    return Derived(
                        windowBack: Color.FromArgb(200, 230, 201), text: Color.FromArgb(27, 94, 32),
                        accent: Color.FromArgb(76, 175, 80), border: Color.FromArgb(56, 142, 60));

                case ThemeMode.Red:
                    return Derived(
                        windowBack: Color.FromArgb(255, 235, 238), text: Color.FromArgb(183, 28, 28),
                        accent: Color.FromArgb(244, 67, 54), border: Color.FromArgb(198, 40, 40));

                case ThemeMode.Orange:
                    return Derived(
                        windowBack: Color.FromArgb(255, 243, 224), text: Color.FromArgb(230, 81, 0),
                        accent: Color.FromArgb(255, 152, 0), border: Color.FromArgb(245, 124, 0));

                case ThemeMode.Yellow:
                    return Derived(
                        windowBack: Color.FromArgb(255, 253, 231), text: Color.FromArgb(102, 51, 0),
                        accent: Color.FromArgb(255, 193, 7), border: Color.FromArgb(255, 152, 0));

                case ThemeMode.Blue:
                    return Derived(
                        windowBack: Color.FromArgb(227, 242, 253), text: Color.FromArgb(13, 71, 161),
                        accent: Color.FromArgb(33, 150, 243), border: Color.FromArgb(25, 118, 210));

                case ThemeMode.Indigo:
                    return Derived(
                        windowBack: Color.FromArgb(232, 234, 246), text: Color.FromArgb(26, 35, 126),
                        accent: Color.FromArgb(63, 81, 181), border: Color.FromArgb(48, 63, 159));

                case ThemeMode.Violet:
                    return Derived(
                        windowBack: Color.FromArgb(243, 229, 245), text: Color.FromArgb(74, 20, 140),
                        accent: Color.FromArgb(156, 39, 176), border: Color.FromArgb(123, 31, 162));

                default:
                    return Get(ThemeMode.Light);
            }
        }

        /// <summary>
        /// Builds the palette for one of the 9 accent-color themes by deriving
        /// every token from the four seed colors.
        /// </summary>
        private static ThemePalette Derived(Color windowBack, Color text, Color accent, Color border)
        {
            var cardBack = GraphicsExtensions.Lerp(windowBack, Color.White, 0.55f);
            var hoverBack = GraphicsExtensions.Lerp(windowBack, accent, 0.15f);

            return new ThemePalette(
                windowBack: windowBack,
                cardBack: cardBack,
                inputBack: Color.White,
                border: border,
                text: text,
                textMuted: GraphicsExtensions.Lerp(text, windowBack, 0.35f),
                accent: accent,
                accentHover: GraphicsExtensions.Lerp(accent, Color.White, 0.15f),
                accentPressed: GraphicsExtensions.Lerp(accent, Color.Black, 0.15f),
                accentText: Color.White,
                hoverBack: hoverBack,
                selectionBack: GraphicsExtensions.Lerp(windowBack, accent, 0.30f),
                selectionText: text,
                gridLine: GraphicsExtensions.Lerp(windowBack, accent, 0.20f),
                menuBack: cardBack,
                menuHover: hoverBack,
                menuText: text);
        }

        private static Color Hex(string hex) => ColorTranslator.FromHtml(hex);
    }
}
