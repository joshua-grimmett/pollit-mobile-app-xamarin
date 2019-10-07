using PollIt.Helpers;
using PollIt.Models;

using System.Collections.Generic;

namespace PollIt.ViewModels
{

    class SettingsViewModel
    {
        public string Username { get => Settings.Username; }
        public List<AppTheme> Themes { get; set; }
        
        public SettingsViewModel()
        {
            Themes = new List<AppTheme>
            {
                new AppTheme { ThemeId = Theme.Light, Title = "Light Theme" },
                new AppTheme { ThemeId = Theme.Dark, Title = "Dark Theme" }
            };
        }
    }
}
