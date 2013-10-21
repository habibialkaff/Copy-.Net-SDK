using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public Uri AuthCodeUri { get; set; }

        //public CopyConfig(string consumerKey, string consumerSecret)
        //{
        //    Config = new Config()
        //    {
        //        ConsumerKey = consumerKey,
        //        ConsumerSecret = consumerSecret
        //    };
        //}

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
            string url;
            if (Scope != null)
            {
                string serializedScope = JsonConvert.SerializeObject(Scope);

                url = string.Format("{0}?scope={1}", URL.RequestToken, WebUtility.UrlEncode(serializedScope));
            }
            else
            {
                url = URL.RequestToken;
            }

            string authzHeader = AuthorizationHeader.CreateForRequest(CallbackURL, Config.ConsumerKey, Config.ConsumerSecret, url);            

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

            if (!string.IsNullOrEmpty(executeAsync))
            {
                string[] kvpairs = executeAsync.Split('&');
                Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('='))
                                                               .ToDictionary(kv => kv[0], kv => kv[1]);

                AuthCodeUri = new Uri(string.Format("{0}?oauth_token={1}", URL.Authorize, parameters["oauth_token"]));

                return new AuthToken()
                {
                    Token = parameters["oauth_token"],
                    TokenSecret = parameters["oauth_token_secret"]
                };
            }

            return new AuthToken();
        }
    }
}
