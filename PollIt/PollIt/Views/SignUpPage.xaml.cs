using PollIt.Helpers;

using System;

using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        public async void SignUp_Clicked(object sender, EventArgs e)
        {
            // Check for internet
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("No internet", "", "OK");
                return;
            }

            bool valid = true;

            // Check email
            if (string.IsNullOrEmpty(EmailInput.Text) || !EmailInput.Text.Contains("@"))
            {
                VisualStateManager.GoToState(EmailInput, "Invalid");
                valid = false;
            }
            // Check passwords match
            if (PasswordInput.Text != ConfirmPasswordInput.Text)
            {
                VisualStateManager.GoToState(PasswordInput, "Invalid");
                valid = false;
            }
            // Check password criteria
            if (string.IsNullOrEmpty(PasswordInput.Text) || PasswordInput.Text.Length < 10)
            {
                VisualStateManager.GoToState(PasswordInput, "Invalid");
                valid = false;
            }

            if (!valid)
                return;

            // Sign user up with API
            bool success = await PollItApi.SignUpWithEmailAndPassword(EmailInput.Text, PasswordInput.Text);
            // Reset entries
            EmailInput.Text = "";
            PasswordInput.Text = "";
            ConfirmPasswordInput.Text = "";
            
            if (!success)
            {
                await DisplayAlert("Error", "An internal error has occured", "OK");
                return;
            }

            await Navigation.PopModalAsync();
        }

        public async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            await Navigation.PushModalAsync(new LoginPage());
        }
    }
}