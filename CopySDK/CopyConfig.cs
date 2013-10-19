using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CopySDK.Helper;
using CopySDK.HttpRequest;
using CopySDK.Models;
using Newtonsoft.Json;

namespace CopySDK
{
    public class CopyConfig : ICopyConfig
    {
        public string CallbackURL { get; set; }
        public Config Config { get; set; }
        public Scope Scope { get; set; }

        public CopyConfig(string consumerKey, string consumerSecret)
        {
            Config = new Config()
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };
        }

        public CopyConfig(string callbackURL, string consumerKey, string consumerSecret, Scope scope)
        {
            CallbackURL = callbackURL;
            Scope = scope;

            Config = new Config()
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };
        }

        public async Task<AuthToken> GetRequestToken()
        {
            string authzHeader = AuthorizationHeader.CreateForRequest(CallbackURL, Config.ConsumerKey, Config.ConsumerSecret);

            string serializedScope = JsonConvert.SerializeObject(Scope);

            string url = string.Format("{0}?scope={1}", URL.RequestToken, WebUtility.UrlEncode(serializedScope));

            HttpRequestItem httpRequestItem = new HttpRequestItem()
            {
                URL = url,
                HttpMethod = HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            HttpRequestHandler httpRequestHandler = new HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ExecuteAsync(httpRequestItem);

            string[] kvpairs = executeAsync.Split('&');
            Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('='))
                                                        .ToDictionary(kv => kv[0], kv => kv[1]);

            return new AuthToken()
            {
                Token = parameters["oauth_token"],
                TokenSecret = parameters["oauth_token_secret"]
            };

            // prepare the token request
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Headers.Add("Authorization", authzHeader);
            //request.Method = "POST";

            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    Stream responseStream = response.GetResponseStream();
            //    if (responseStream != null)
            //    {
            //        using (StreamReader reader = new StreamReader(responseStream))
            //        {
            //            AuthToken oAuthSession = new AuthToken(reader.ReadToEnd());

            //            return oAuthSession;
            //        }
            //    }
            //}            
        }
    }
}
