using PollIt.Models;

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PollIt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;

        public MenuPage()
        {
            InitializeComponent();

            // Initialise menu items with pages
            menuItems = new List<HomeMenuItem>()
            {
                new HomeMenuItem { Id = MenuItemType.Polls, Title="Polls" },
                new HomeMenuItem { Id = MenuItemType.CreatePoll, Title="Create poll" },
                new HomeMenuItem { Id = MenuItemType.Settings, Title="Settings" }
            };

            // Assign menu listview's source to hardcode menu
            ListViewMenu.ItemsSource = menuItems;

            // Reset listview
            ListViewMenu.SelectedItem = menuItems[0];
            // Set menu item selected handler
            ListViewMenu.ItemSelected += MenuItemSelected;

        }

        private async void MenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Catch deselection
            if (e.SelectedItem == null)
                return;

            // Get item selected as id
            var id = (int) ((HomeMenuItem) e.SelectedItem).Id;
            // Navigate
            await RootPage.NavigateFromMenu(id);
        }
    }
}