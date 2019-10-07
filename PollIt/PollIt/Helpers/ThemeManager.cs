using PollIt.ThemeResources;
using PollIt.Helpers;
using PollIt.Models;

namespace PollIt.Helpers
{

    class ThemeManager
    {
        /// <summary>
        /// Retrieve the current theme selected from settings
        /// </summary>
        public static Theme CurrentTheme
        {
            get
            {
                return (Theme) Settings.AppSettings.GetValueOrDefault("SelectedTheme", (int) Theme.Light);
            }
        }

        /// <summary>
        /// Change the current theme of the application to new theme given
        /// </summary>
        /// <param name="theme"></param>
        public static void ChangeTheme(Theme theme)
        {
            // Retrieve current resource dictionaries
            var mergedDictionaries = App.Current.Resources.MergedDictionaries;
            if (mergedDictionaries == null) return;

            // Reset dictionaries
            mergedDictionaries.Clear();

            // Add themes to current dictionary according to provided theme
            switch (theme)
            {
                case Theme.Dark:
                    mergedDictionaries.Add(new DarkTheme());
                    break;
                default:
                    mergedDictionaries.Add(new LightTheme());
                    break;
            }
        }

        // Change the theme to the current theme stored locally
        public static void LoadTheme()
        {
            ChangeTheme(CurrentTheme);
        }
    }
}
