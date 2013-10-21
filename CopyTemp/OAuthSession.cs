using System.Collections.Generic;

namespace CopySDK.Authentication
{
    /// <summary>
    ///   A class to hold an OAuth response message.
    /// </summary>
    public class OAuthSession
    {
        /// <summary>
        ///   All of the text in the response. This is useful if the app wants
        ///   to do its own parsing.
        /// </summary>
        //public string AllText { get; set; }

        private readonly Dictionary<string, string> _params;

         //<summary>
         //  a Dictionary of response parameters.
         //</summary>
        public string this[string ix]
        {
            get
            {
                return _params[ix];
            }
        }

        public string Token
        {
            get
            {
                return _params["oauth_token"];
            }
        }

        public string TokenSecret
        {
            get
            {
                return _params["oauth_token_secret"];
            }
        }


        public OAuthSession(string alltext)
        {
            _params = new Dictionary<string, string>();
            var kvpairs = alltext.Split('&');
            foreach (var pair in kvpairs)
            {
                var kv = pair.Split('=');
                _params.Add(kv[0], kv[1]);
            }
            // expected keys:
            //   oauth_token, oauth_token_secret, user_id, screen_name, etc
        }
    }
}