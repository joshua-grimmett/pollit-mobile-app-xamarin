using Newtonsoft.Json;

namespace PollIt.Models
{
    public partial class Vote
    {
        [JsonProperty("optionId")]
        public int OptionId { get; set; }

        [JsonProperty("pollID")]
        public string PollId { get; set; }
    }

    /// <summary>
    ///     Convert a Vote from JSON
    /// </summary>
    public partial class Vote
    {
        public static User FromJson(string json) => JsonConvert.DeserializeObject<User>(json);
    }

    /// <summary>
    ///     Convert a Vote to JSON
    /// </summary>
    public static partial class Serialize
    {
        public static string ToJson(this Vote self) => JsonConvert.SerializeObject(self);
    }
}
