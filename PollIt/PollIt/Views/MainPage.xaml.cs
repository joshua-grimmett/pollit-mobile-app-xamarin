using PollIt.Models;
using PollIt.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Polls, (NavigationPage) Detail);
        }

        protected override void OnAppearing()
        {
            if (!Settings.VerifyToken(true))
            {
                Navigation.PushModalAsync(new LoginPage());
            }
        }

        public async Task NavigateFromMenu(int id)
        {
            // If page is not added already, add the page as navigation page
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int) MenuItemType.Polls:
                        MenuPages.Add(id, new NavigationPage(new PollsPage()));
                        break;
                    case (int) MenuItemType.CreatePoll:
                        MenuPages.Add(id, new NavigationPage(new EditPollPage()));
                        break;
                    case (int) MenuItemType.Settings:
                        MenuPages.Add(id, new NavigationPage(new SettingsPage()));
                        break;
                }
            }

            // Select page to navigate to
            var newPage = MenuPages[id];

            // Check new page exists, and not already viewing new page
            if (newPage != null && Detail != newPage)
            {
                // Update detail view to new page
                Detail = newPage;

                // Add delay on android
                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                // No longer being presented
                IsPresented = false;
            }
        }
    }
}