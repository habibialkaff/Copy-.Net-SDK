using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CopySDK.Authentication;
using CopySDK.Helper;
using Newtonsoft.Json;

namespace CopySDK
{
    public class CopyConfig
    {
        public string CallbackURL { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public Scope Scope { get; set; }

        public string Token { get; set; }
        public string TokenSecret { get; set; }

        public CopyConfig() { }

        public CopyConfig(string callbackURL, string consumerKey, string consumerSecret, Scope scope)
        {
            CallbackURL = callbackURL;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            Scope = scope;
        }

        public OAuthResponse GetRequestToken()
        {
            string authzHeader = AuthorizationHeader.CreateForRequest(CallbackURL, ConsumerKey, ConsumerSecret);


            string serializedScope = JsonConvert.SerializeObject(Scope);

            string url = string.Format("{0}?scope={1}", URL.RequestToken, WebUtility.UrlEncode(serializedScope));

            // prepare the token request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", authzHeader);
            request.Method = "POST";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        OAuthResponse oAuthResponse = new OAuthResponse(reader.ReadToEnd());
                        Token = oAuthResponse["oauth_token"];
                        TokenSecret = oAuthResponse["oauth_token_secret"];
                        return oAuthResponse;
                    }
                }
            }
            return null;
        }

        public OAuthResponse GetAccessToken(string verifier)
        {
            string authzHeader = AuthorizationHeader.CreateForAccess(ConsumerKey, ConsumerSecret, Token, TokenSecret, verifier);


            string serializedScope = JsonConvert.SerializeObject(Scope);

            string url = string.Format("{0}?scope={1}", URL.RequestToken, WebUtility.UrlEncode(serializedScope));

            // prepare the token request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", authzHeader);
            request.Method = "POST";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        OAuthResponse oAuthResponse = new OAuthResponse(reader.ReadToEnd());
                        Token = oAuthResponse["oauth_token"];
                        TokenSecret = oAuthResponse["oauth_token_secret"];
                        return oAuthResponse;
                    }
                }
            }
            return null;
        }
    }
}
