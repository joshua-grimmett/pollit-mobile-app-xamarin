using PollIt.Models;
using PollIt.Helpers;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PollIt.Helpers
{
    /// <summary>
    ///     A helper class
    /// </summary>
    public class PollItApi
    {
        public static string baseUrl = "https://pollit.jooshua.com/";

        /// <summary>
        /// Register user in pollit database with an email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<bool> SignUpWithEmailAndPassword(string email, string password)
        {
            // Initialise HTTP client
            HttpClient client = new HttpClient();
            // Create model to bind JSON to
            User model = new User
            {
                Email = email,
                Password = password
            };

            // Serialize model
            var json = Serialize.ToJson(model);
            
            // Initialise payload
            HttpContent content = new StringContent(json);
            // Set ContentType header
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Post request to api
            var response = await client.PostAsync(baseUrl + "api/users/signup", content);
            client.Dispose();
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Sign in user with an email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<string> SignInWithEmailAndPassword(string email, string password)
        {
            // Initialise HTTP client
            HttpClient client = new HttpClient();
            User model = new User
            {
                Email = email,
                Password = password
            };

            // Serialize model
            var json = Serialize.ToJson(model);
            // Initialise payload
            HttpContent content = new StringContent(json);
            // Set header to json
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Get request to api
            var response = await client.PostAsync(baseUrl + "api/users/login", content);
            // Read response as string
            var jwt = await response.Content.ReadAsStringAsync();
            // Convert to json
            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(jwt);

            // Get and return token
            var token = jwtDynamic.Value<string>("token");
            client.Dispose();
            return token;
        }

        /// <summary>
        /// Get a list of polls from database belonging to user
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<List<Poll>> GetAllPolls(string token)
        {
            // Initialise http client
            HttpClient client = new HttpClient();
            // Apply token in headers
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get request to api
            var result = await client.GetAsync(baseUrl + "api/polls/");
            // If none send empty list
            if (!result.IsSuccessStatusCode)
            {
                // Dispose client
                client.Dispose();
                return new List<Poll>();
            }

            // Extract json string from result
            var json = await result.Content.ReadAsStringAsync();
            // Extract polls
            List<Poll> polls = JsonConvert.DeserializeObject<List<Poll>>(json);

            // Dispose client
            client.Dispose();
            return polls;
        }

        public static async Task<Poll> GetPoll(string id)
        {
            // Initialise HTTP client
            HttpClient client = new HttpClient();
            // Authorize client
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", Settings.Token);

            // Send get request
            var result = await client.GetAsync($"{baseUrl}api/polls/{id}");
            if (!result.IsSuccessStatusCode)
            {
                client.Dispose();
                return null;
            }
                
            // Extract json
            var json = await result.Content.ReadAsStringAsync();
            // Extract poll
            Poll poll = Poll.FromJson(json);

            // Dispose client
            client.Dispose();

            return poll;
        }

        /// <summary>
        /// Create a new poll
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public static async Task<string> CreatePoll(Poll poll)
        {
            // Initialise HTTP client
            HttpClient client = new HttpClient();
            // Authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);

            // Serialize poll
            var payload = Serialize.ToJson(poll);

            // Initialise payload
            HttpContent content = new StringContent(payload);
            // Set header to json
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Get request to api
            var response = await client.PostAsync(baseUrl + "api/polls/", content);

            if (!response.IsSuccessStatusCode)
            {
                client.Dispose();
                return null;
            }

            // Extract json
            var json = await response.Content.ReadAsStringAsync();
            // Extract poll
            poll = Poll.FromJson(json);

            client.Dispose();
            return poll.Id;
        }

        public static async Task<bool> DeletePoll(Poll poll)
        {
            // Initialise client
            HttpClient client = new HttpClient();
            // Authorize client
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", Settings.Token);

            // Send delete request
            var response = await client.DeleteAsync($"{baseUrl}api/polls/{poll.Id}");

            // Dispose of client
            client.Dispose();

            return response.IsSuccessStatusCode;
        }
    }
}
