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
    public class CopyConfig : ICopyConfig
    {
        public string CallbackURL { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public Scope Scope { get; set; }

        public CopyConfig(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public CopyConfig(string callbackURL, string consumerKey, string consumerSecret, Scope scope)
        {
            CallbackURL = callbackURL;
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            Scope = scope;
        }

        public OAuthSession GetRequestToken()
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
                        OAuthSession oAuthSession = new OAuthSession(reader.ReadToEnd());
                        
                        return oAuthSession;
                    }
                }
            }
            return null;
        }        
    }
}
