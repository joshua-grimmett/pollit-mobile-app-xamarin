using PollIt.Models;
using PollIt.ViewModels;
using PollIt.Helpers;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Share;
using Plugin.Share.Abstractions;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PollDetailPage : ContentPage
    {
        readonly PollDetailViewModel viewModel;
        public PollDetailPage(Poll poll)
        {
            InitializeComponent();

            BindingContext = viewModel = new PollDetailViewModel(poll);
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            // Retrieve and delete the poll in the view model
            bool success = await PollItApi.DeletePoll(viewModel.Poll);

            // Let user know if the deletion was unsuccessful
            if (!success)
            {
                await DisplayAlert("Failed to delete", "The poll could not be deleted", "OK");
                return;
            }

            // Notify viewmodel that the poll has been deleted
            MessagingCenter.Send(this, "DeletePoll", viewModel.Poll);

            // Pop back to main page
            await Navigation.PopToRootAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void Share_Clicked(object sender, EventArgs e)
        {
            // Create the url to share
            string url = $"pollit.jooshua.com/p/{viewModel.Poll.Id}";

            await CrossShare.Current.Share(new ShareMessage
            {
                Url = url
            });
        }
    }
}