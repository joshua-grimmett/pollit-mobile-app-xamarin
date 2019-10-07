using PollIt.Models;

using System;
using System.Windows.Input;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace PollIt.ViewModels
{
    class EditPollViewModel : BaseViewModel
    {
        public Poll Poll { get; set; }
        public bool MinOptions
        {
            get { return Poll.Options.Count > 2; }
        }
        
        public EditPollViewModel()
        {
            Title = "New poll";

            // Assign new Poll model
            Poll = new Poll
            {
                CreationDate = DateTime.Now,
                Options = new ObservableCollection<Option>(),
                Enabled = true,
                IsPrivate = false,
                CloseDate = DateTime.Now
            };

            // Fill options with placeholders
            FillEmptyOptions(3);

            // Initialise delete last option command
            RemoveOptionCommand = new Command<Option>((model) => ExecuteRemoveOption(model));
        }

        private void FillEmptyOptions(int quantity)
        {
            // Loop for *quantity times
            for (int i = 0; i < quantity; i++)
            {
                // Create new Option model with placeholder title
                Option option = new Option
                {
                    Title = "Enter option text",
                    Votes = 0
                };

                Poll.Options.Add(option);
            }
        }

        public Command RemoveOptionCommand { get; set; }
        private void ExecuteRemoveOption(Option model)
        {
            // If there is at least 3 options, delete the last
            if (MinOptions)
                Poll.Options.Remove(model);
        }
    }
}
