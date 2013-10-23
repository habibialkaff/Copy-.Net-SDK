using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CopySDK.Helper;
using CopySDK.Helpers;
using CopySDK.HttpRequest;
using CopySDK.Models;
using Newtonsoft.Json;

namespace CopySDK
{
    public class CopyAuth : ICopyAuth
    {
        public string CallbackURL { get; set; }
        public Config Config { get; set; }
        public Scope Scope { get; set; }

        public Uri AuthCodeUri { get; set; }

        private string Token { get; set; }
        private string TokenSecret { get; set; }

        //public CopyConfig(string consumerKey, string consumerSecret)
        //{
        //    Config = new Config()
        //    {
        //        ConsumerKey = consumerKey,
        //        ConsumerSecret = consumerSecret
        //    };
        //}

        public CopyAuth(string callbackURL, string consumerKey, string consumerSecret, Scope scope)
        {
            CallbackURL = callbackURL;
            Scope = scope;

            Config = new Config()
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };
        }

        public async Task<OAuthToken> GetRequestTokenAsync()
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
            string executeAsync = await httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            if (!string.IsNullOrEmpty(executeAsync))
            {
                string[] kvpairs = executeAsync.Split('&');
                Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('='))
                                                               .ToDictionary(kv => kv[0], kv => kv[1]);


                Token = parameters["oauth_token"];
                TokenSecret = parameters["oauth_token_secret"];
                AuthCodeUri = new Uri(string.Format("{0}?oauth_token={1}", URL.Authorize, Token));

                return new OAuthToken()
                {
                    Token = Token,
                    TokenSecret = TokenSecret
                };
            }

            return new OAuthToken();
        }

        public async Task<CopyClient> GetAccessTokenAsync(string verifier)
        {
            string authzHeader = AuthorizationHeader.CreateForAccess(Config.ConsumerKey, Config.ConsumerSecret, Token, TokenSecret, verifier);

            string url = string.Format(URL.AccessToken);

            HttpRequestItem httpRequestItem = new HttpRequestItem()
            {
                URL = URL.AccessToken,
                HttpMethod = HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            HttpRequestHandler httpRequestHandler = new HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            string[] kvpairs = executeAsync.Split('&');
            Dictionary<string, string> parameters = kvpairs.Select(pair => pair.Split('='))
                                                        .ToDictionary(kv => kv[0], kv => kv[1]);

            if (parameters.ContainsKey("oauth_error_message"))
            {
                return null;
            }
            else
            {
                return new CopyClient(Config, new OAuthToken()
                {
                    Token = parameters["oauth_token"],
                    TokenSecret = parameters["oauth_token_secret"]
                });
            }
        }
    }
}
