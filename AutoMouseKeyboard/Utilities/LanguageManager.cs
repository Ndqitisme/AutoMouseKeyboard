
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace AutoMouseKeyboard.Utilities
{
    public enum Language
    {
        English,
        Vietnamese,
        Chinese,
        ChineseTraditional,
        Spanish,
        Arabic,
        Hindi,
        Portuguese,
        French,
        Russian,
        Japanese,
        German,
        Korean,
        Italian,
        Turkish,
        Indonesian,
        Bengali,
        Polish,
        Dutch
    }

    public class LanguageChangedEventArgs : EventArgs
    {
        public Language Language { get; }

        public LanguageChangedEventArgs(Language language)
        {
            Language = language;
        }
    }

    public static class LanguageManager
    {
        private static Language _currentLanguage = Language.English;
        private static ResourceManager? _resourceManager;
        private static readonly Dictionary<Language, CultureInfo> LanguageCultures = new Dictionary<Language, CultureInfo>()
    {
        { Language.English, new CultureInfo("en") },
        { Language.Vietnamese, new CultureInfo("vi") },
        { Language.Chinese, new CultureInfo("zh-CN") },
        { Language.ChineseTraditional, new CultureInfo("zh-TW") },
        { Language.Spanish, new CultureInfo("es") },
        { Language.Arabic, new CultureInfo("ar") },
        { Language.Hindi, new CultureInfo("hi") },
        { Language.Portuguese, new CultureInfo("pt") },
        { Language.French, new CultureInfo("fr") },
        { Language.Russian, new CultureInfo("ru") },
        { Language.Japanese, new CultureInfo("ja") },
        { Language.German, new CultureInfo("de") },
        { Language.Korean, new CultureInfo("ko") },
        { Language.Italian, new CultureInfo("it") },
        { Language.Turkish, new CultureInfo("tr") },
        { Language.Indonesian, new CultureInfo("id") },
        { Language.Bengali, new CultureInfo("bn") },
        { Language.Polish, new CultureInfo("pl") },
        { Language.Dutch, new CultureInfo("nl") }
    };

        private static readonly List<Form> RegisteredForms = new List<Form>();

        public static Language CurrentLanguage => _currentLanguage;

        public static event EventHandler<LanguageChangedEventArgs>? LanguageChanged;

        static LanguageManager()
        {
            LoadLanguage();
        }

        private static void LoadLanguage()
        {
            try
            {
                var savedLang = LoadSavedLanguage();
                if (savedLang.HasValue)
                {
                    SetLanguage(savedLang.Value, false);
                }
            }
            catch
            {
                SetLanguage(Language.English, false);
            }
        }

        private static Language? LoadSavedLanguage()
        {
            try
            {
                var lang = AppSettings.Language;

                var baseFolder = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "AutoMouseKeyboard");
                var oldLangFile = System.IO.Path.Combine(baseFolder, "language.txt");
                if (System.IO.File.Exists(oldLangFile))
                {
                    try
                    {
                        var langStr = System.IO.File.ReadAllText(oldLangFile).Trim();
                        if (Enum.TryParse<Language>(langStr, out var oldLang))
                        {
                            AppSettings.Language = oldLang;
                            System.IO.File.Delete(oldLangFile);
                            return oldLang;
                        }
                    }
                    catch
                    {
                    }
                }

                return lang;
            }
            catch
            {
            }
            return null;
        }

        private static void SaveLanguage(Language language)
        {
            try
            {
                AppSettings.Language = language;
            }
            catch
            {
            }
        }

        public static void SetLanguage(Language language, bool save = true)
        {
            if (_currentLanguage == language)
            {
                return;
            }

            _currentLanguage = language;
            var culture = LanguageCultures[language];
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            _resourceManager = new ResourceManager(
                "AutoMouseKeyboard.Resources.LanguageResources",
                typeof(LanguageManager).Assembly);

            if (save)
            {
                SaveLanguage(language);
            }

            foreach (var form in RegisteredForms.ToArray())
            {
                if (form != null && !form.IsDisposed)
                {
                    UpdateFormLanguage(form);
                }
            }

            LanguageChanged?.Invoke(null, new LanguageChangedEventArgs(language));
        }

        public static string GetString(string key)
        {
            try
            {
                if (_resourceManager == null)
                {
                    _resourceManager = new ResourceManager(
                        "AutoMouseKeyboard.Resources.LanguageResources",
                        typeof(LanguageManager).Assembly);
                }

                var culture = LanguageCultures[_currentLanguage];
                var value = _resourceManager.GetString(key, culture);
                return value ?? key;
            }
            catch
            {
                return key;
            }
        }

        public static void RegisterForm(Form form)
        {
            if (!RegisteredForms.Contains(form))
            {
                RegisteredForms.Add(form);
                UpdateFormLanguage(form);
            }
        }

        public static void UnregisterForm(Form form)
        {
            RegisteredForms.Remove(form);
        }

        private static void UpdateFormLanguage(Form form)
        {
            if (form == null || form.IsDisposed)
            {
                return;
            }

            var method = form.GetType().GetMethod("UpdateLanguage",
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            method?.Invoke(form, null);
        }

        public static string GetLanguageDisplayName(Language language)
        {
            return language switch
            {
                Language.English => "English",
                Language.Vietnamese => "Tiếng Việt",
                Language.Chinese => "中文 (简体)",
                Language.ChineseTraditional => "中文 (繁體)",
                Language.Spanish => "Español",
                Language.Arabic => "العربية",
                Language.Hindi => "हिन्दी",
                Language.Portuguese => "Português",
                Language.French => "Français",
                Language.Russian => "Русский",
                Language.Japanese => "日本語",
                Language.German => "Deutsch",
                Language.Korean => "한국어",
                Language.Italian => "Italiano",
                Language.Turkish => "Türkçe",
                Language.Indonesian => "Bahasa Indonesia",
                Language.Bengali => "বাংলা",
                Language.Polish => "Polski",
                Language.Dutch => "Nederlands",
                _ => language.ToString()
            };
        }
    }
}

