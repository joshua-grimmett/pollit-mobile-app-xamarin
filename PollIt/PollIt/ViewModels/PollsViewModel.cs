using PollIt.Views;
using PollIt.Models;
using PollIt.Helpers;

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;


namespace PollIt.ViewModels
{
    class PollsViewModel : BaseViewModel
    {
        public ObservableCollection<Poll> Polls { get; set; }
        public bool EmptyPolls
        {
            get { return Polls.Count > 0; }
        }

        public PollsViewModel()
        {
            Title = "Polls";

            IsBusy = false;

            // Initialise polls collection
            Polls = new ObservableCollection<Poll>();
            // Initialise load polls command
            LoadPollsCommand = new Command(async () => await ExecuteLoadPollsCommand());

            MessagingCenter.Subscribe<EditPollPage, Poll>(this, "AddPoll", (obj, poll) =>
            {
                Polls.Add(poll as Poll);
            });

            MessagingCenter.Subscribe<PollDetailPage, Poll>(this, "DeletePoll", (obj, poll) =>
            {
                Polls.Remove(poll as Poll);
            });

            MessagingCenter.Subscribe<PollDetailPage, Poll>(this, "UpdatePoll", (obj, newPoll) =>
            {
                Poll poll = Polls.Single(p => p.Id == newPoll.Id);
                poll = newPoll;
            });
        }

        public Command LoadPollsCommand { get; set; }
        async Task ExecuteLoadPollsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                // Reset collection
                Polls.Clear();
                // Retrieve polls from api
                var polls = await PollItApi.GetAllPolls(Settings.Token);
                // Add each poll to view collection
                foreach (var poll in polls)
                {
                    Polls.Add(poll);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

}
