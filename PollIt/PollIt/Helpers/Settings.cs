using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Plugin.SecureStorage;

namespace PollIt.Helpers
{
    class Settings
    {
        public static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        /// <summary>
        /// A string representing a JSON Web Token used to login to the PollIt API.
        /// </summary>
        public static string Token
        {
            get
            {
                // Check if the token is already being stored locally
                if (CrossSecureStorage.Current.HasKey("token"))
                {
                    // Retrieve token from local storage
                    return CrossSecureStorage.Current.GetValue("token");
                }
                return null;
            }
            set { CrossSecureStorage.Current.SetValue("token", value); }
        }

        /// <summary>
        /// A string representing a user's email
        /// </summary>
        public static string Email
        {
            get
            {
                // Check if the email is being stored locally and retrieve
                if (CrossSecureStorage.Current.HasKey("email"))
                {
                    return CrossSecureStorage.Current.GetValue("email");
                }
                return null;
            }
            set { CrossSecureStorage.Current.SetValue("email", value); }
        }

        /// <summary>
        /// A string representing a user's password
        /// </summary>
        public static string Password
        {
            get
            {
                // Check if the password is being stored locally and retrieve
                if (CrossSecureStorage.Current.HasKey("password"))
                {
                    return CrossSecureStorage.Current.GetValue("password");
                }
                return null;
            }
            set { CrossSecureStorage.Current.SetValue("password", value); }
        }

        /// <summary>
        /// A string representing a user's username / ID
        /// </summary>
        public static string Username
        {
            get
            {
                // Handler to parse JWT tokens
                var handler = new JwtSecurityTokenHandler();
                // Decode token string with handler
                var token = handler.ReadJwtToken(Token);
                // Try get username from token payload
                return token.Claims.SingleOrDefault(c => c.Type == "username").Value;
            }
        }

        public static bool VerifyToken(bool deleteIfInvalid)
        {
            // Invalid if null
            if (string.IsNullOrEmpty(Token))
                return false;

            // JWT handler
            var handler = new JwtSecurityTokenHandler();
            // Decode JWT token
            var token = handler.ReadJwtToken(Token);

            // Convert current time to epoch seconds
            TimeSpan now = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int) now.TotalSeconds;
           
            // Delete the token if it's invalid
            if (token.Payload.Exp < secondsSinceEpoch)
            {
                if(deleteIfInvalid)
                    CrossSecureStorage.Current.DeleteKey("token");
                
                return false;
            }

            return true;
        }
    }
}
