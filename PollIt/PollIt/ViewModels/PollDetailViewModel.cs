using PollIt.Models;
using PollIt.Helpers;

using System;

using Xamarin.Forms;

namespace PollIt.ViewModels
{
    class PollDetailViewModel : BaseViewModel
    {
        public Poll Poll { get; set; }

        public PollDetailViewModel(Poll poll = null)
        {
            Title = poll?.Question;

            Poll = poll;
            // Loop and calculate option vote percentages
            foreach (Option option in Poll.Options)
            {
                if (option.Votes > 0)
                    option.Percentage = $"{Math.Round((Decimal)option.Votes / Poll.TotalVotes * 100)}%";
            }
        }

        public async void LoadPoll()
        {
            IsBusy = true;

            try
            {
                // Retrieve updated poll from server
                Poll = await PollItApi.GetPoll(Poll.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            IsBusy = false;
        }
    }
}
