using System;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

using PollIt.Helpers;

namespace PollIt.Models
{
    /// <summary>
    /// A class to represent a Poll
    /// </summary>
    public partial class Poll
    {
        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; }

        [JsonProperty("options")]
        public ObservableCollection<Option> Options { get; set; }

        [JsonProperty("ownerID")]
        public string OwnerId { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("voters")]
        public string[] Voters { get; set; }

        /// <summary>
        /// Calculate total votes of all options
        /// </summary>
        [JsonIgnore]
        public int TotalVotes
        {
            get
            {
                int total = 0;
                foreach (Option option in Options)
                {
                    total += option.Votes;
                }
                return total;
            }
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("creationDate")]
        public int _creationDate { get; set; }

        [JsonIgnore]
        public DateTime CreationDate
        {
            get => UnixDateTimeHelper.FromUnixTime(_creationDate);
            set { _creationDate = (int)UnixDateTimeHelper.ToUnixTime(value); }
        }

        [JsonProperty("closeDate")]
        public int _closeDate { get; set; }

        [JsonIgnore]
        public DateTime CloseDate
        {
            get => UnixDateTimeHelper.FromUnixTime(_closeDate);
            set { _closeDate = (int)UnixDateTimeHelper.ToUnixTime(value); }
        }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public class Option : ObservableObject
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("votes")]
        public int Votes { get; set; }

        [JsonIgnore]
        string _percentage = "0%";

        [JsonIgnore]
        public string Percentage
        {
            get { return _percentage; }
            set { SetProperty(ref _percentage, value); }
        }

        public Option()
        {
            Title = "Enter option text";
            Votes = 0;
        }
    }

    /// <summary>
    ///     Convert Poll from JSON
    /// </summary>
    public partial class Poll
    {
        public static Poll FromJson(string json) => JsonConvert.DeserializeObject<Poll>(json);
    }

    /// <summary>
    ///     Convert Poll to JSON
    /// </summary>
    public static partial class Serialize
    {
        public static string ToJson(this Poll self) => JsonConvert.SerializeObject(self);
    }
}
