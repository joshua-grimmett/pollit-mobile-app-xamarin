using PollIt.Helpers;

using System;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Plugin.SecureStorage;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        readonly MainPage mainPage = Application.Current.MainPage as MainPage;

        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Handle connectivity changed
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;

            // Load saved password
            try
            {
                // Extract secure email and passwords
                string email = CrossSecureStorage.Current.GetValue("email");
                string password = CrossSecureStorage.Current.GetValue("password");

                if (email != null && password != null)
                {
                    // Set email entry to found email
                    EmailInput.Text = email;
                    // Set password entry to found password
                    PasswordInput.Text = password;
                    // Set backgrounds to tint for remember me
                    VisualStateManager.GoToState(EmailInput, "Remembered");
                    VisualStateManager.GoToState(PasswordInput, "Remembered");
                }
            } catch (Exception e)
            {
                // If device does not support secure storage
                throw e;
            }
        }

        private async void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            // Show message if not connected
            if (!e.IsConnected)
                await DisplayAlert("No internet", "You are not connected to the internet", "OK");
        }

        async void Login_Clicked(object sender, EventArgs e)
        {
            string email = EmailInput.Text;
            string password = PasswordInput.Text;

            // If no email given set to invalid
            if (string.IsNullOrEmpty(email))
            {
                VisualStateManager.GoToState(EmailInput, "Invalid");
                return;
            }
            if (string.IsNullOrEmpty(password) || password.Length < 10)
            {
                VisualStateManager.GoToState(PasswordInput, "Invalid");
            }

            // Login with api and get token
            string token = await PollItApi.SignInWithEmailAndPassword(email, password);
            // Check token
            if (string.IsNullOrEmpty(token))
            {
                await DisplayAlert("Invalid login", "The email or password that you entered was incorrect", "OK");
                return;
            }

            // Save token to secure storage
            try
            {
                // Save token
                CrossSecureStorage.Current.SetValue("token", token);
                // Save email & password if remember me
                CrossSecureStorage.Current.SetValue("email", email);
                CrossSecureStorage.Current.SetValue("password", password);

                if (Navigation.ModalStack.Count > 0)
                    await Navigation.PopModalAsync(); 
                await mainPage.NavigateFromMenu(0);
            } catch (Exception ex)
            {
                // Device may not support secure storage
                throw ex;
            }
        }

        async void SignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUpPage());
        }
    }
} 