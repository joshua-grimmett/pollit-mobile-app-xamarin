using Newtonsoft.Json;

namespace PollIt.Models
{
    public partial class User
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Convert a User to JSON
    /// </summary>
    public static partial class Serialize
    {
        public static string ToJson(this User self) => JsonConvert.SerializeObject(self);
    }
}
