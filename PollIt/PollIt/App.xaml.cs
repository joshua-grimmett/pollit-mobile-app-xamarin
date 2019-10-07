using System;

using PollIt.Views;
using PollIt.Helpers;

using Xamarin.Forms;

namespace PollIt
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ThemeManager.LoadTheme();

            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            // Verify if logged in
            bool loggedIn = Settings.VerifyToken(true);
            try
            {
                // Attempt sign in with saved details
                if (!loggedIn)
                {
                    if (Settings.Email != null && Settings.Password != null)
                    {
                        string token = await PollItApi.SignInWithEmailAndPassword(Settings.Email, Settings.Password);
                        Settings.Token = token;
                    }
                }
            } catch (System.Net.Http.HttpRequestException)
            {
                System.Diagnostics.Debug.WriteLine("No internet connection");
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
