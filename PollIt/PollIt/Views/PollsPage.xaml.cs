using PollIt.Models;
using PollIt.ViewModels;
using PollIt.Helpers;

using System;
using System.ComponentModel;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.SecureStorage;

using Xamarin.Forms;

namespace PollIt.Views
{
    [DesignTimeVisible(false)]
    public partial class PollsPage : ContentPage
    {
        PollsViewModel viewModel;
        MainPage mainPage;
        public PollsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PollsViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            mainPage = App.Current.MainPage as MainPage;

            // Handle connectivity changed
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChangedAsync;

            if (viewModel.Polls.Count < 1)
                viewModel.LoadPollsCommand.Execute(null);
        }

        private async void ConnectivityChangedAsync(object sender, ConnectivityChangedEventArgs e)
        {
            // Show message if not connected
            if (!e.IsConnected)
                await DisplayAlert("No internet", "You are not connected to the internet", "OK");
        }

        private async void CreatePoll_Clicked(object sender, EventArgs e)
        {
            // Login required to create poll
            if (string.IsNullOrEmpty(Settings.Token))
            {
                await DisplayAlert("Not logged in", "You must be logged in, in order to create a poll.", "OK");
                await Navigation.PushAsync(new LoginPage());
                return;
            }

            // Navigate to poll creation page
            await mainPage.NavigateFromMenu((int)MenuItemType.CreatePoll);
            return;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            // Show detail page
            await Navigation.PushAsync(new PollDetailPage(e.Item as Poll));
        }
    }
}
