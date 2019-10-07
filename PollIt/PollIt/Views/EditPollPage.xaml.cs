using PollIt.Models;
using PollIt.ViewModels;
using PollIt.Helpers;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPollPage : ContentPage
    {
        EditPollViewModel viewModel;
        readonly MainPage mainPage = App.Current.MainPage as MainPage;

        public EditPollPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            BindingContext = viewModel = new EditPollViewModel();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            // Retrieve Poll model from view model
            Poll poll = viewModel.Poll;

            // Create poll with API and retrieve id of created poll
            string id = await PollItApi.CreatePoll(poll);

            // Assert API call did not fail (would return null)
            if (id != null)
            {
                // Assign poll new ID, assigned on creation
                poll.Id = id;
                
                // Notify viewmodel poll added
                MessagingCenter.Send(this, "AddPoll", poll);

                // Switch back to main page
                await mainPage.NavigateFromMenu((int)MenuItemType.Polls);
                return;
            }

            await DisplayAlert("Internal error", "An internal error has occured", "OK");
            return;
        }

        private void AddOption_Clicked(object sender, ItemTappedEventArgs e)
        {
            // Retrieve option model list from viewmodel
            var options = viewModel.Poll.Options;
            // Retrieve the selected option
            var option = e.Item as Option;
            
            // If the option is the last in the list add a new option
            if (options.IndexOf(option) == options.Count - 1)
                viewModel.Poll.Options.Add(new Option());
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            // Pop to root
            await mainPage.NavigateFromMenu((int) MenuItemType.Polls);
        }

        private void RemoveOption_Clicked(object sender, EventArgs e)
        {
            // Get button sender
            var button = sender as Button;
            // Get called Option
            Option option = button?.BindingContext as Option;

            // Call remove option
            viewModel?.RemoveOptionCommand.Execute(option);
        }
    }
}