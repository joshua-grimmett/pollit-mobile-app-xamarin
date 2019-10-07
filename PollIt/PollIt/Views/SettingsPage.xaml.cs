using PollIt.Helpers;
using PollIt.ViewModels;
using PollIt.Models;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext =  new SettingsViewModel();
        }

        private void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            // Retreive the picker which was called
            Picker picker = sender as Picker;
            // Retrieve the theme which was selected
            AppTheme theme = (AppTheme) picker.SelectedItem;

            // Change the theme to new, selected theme
            ThemeManager.ChangeTheme(theme.ThemeId);
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Reset token
            Settings.Token = null;

            await Navigation.PopToRootAsync();

            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}