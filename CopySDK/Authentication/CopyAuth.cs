namespace CopySDK.Authentication
{
    public class CopyAuth : ICopyAuth
    {
        public string CallbackURL { get; set; }
        public Models.Config Config { get; set; }
        public Models.Scope Scope { get; set; }

        public System.Uri AuthCodeUri { get; set; }

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

        public CopyAuth(string callbackURL, string consumerKey, string consumerSecret, Models.Scope scope)
        {
            CallbackURL = callbackURL;
            Scope = scope;

            Config = new Models.Config()
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret
            };
        }

        public async System.Threading.Tasks.Task<Models.OAuthToken> GetRequestTokenAsync()
        {
            string url;
            if (Scope != null)
            {
                string serializedScope = Newtonsoft.Json.JsonConvert.SerializeObject(Scope);

                url = string.Format("{0}?scope={1}", Helper.URL.RequestToken, System.Net.WebUtility.UrlEncode(serializedScope));
            }
            else
            {
                url = Helper.URL.RequestToken;
            }

            string authzHeader = Helpers.AuthorizationHeader.CreateForRequest(CallbackURL, Config.ConsumerKey, Config.ConsumerSecret, url);

            Models.HttpRequestItem httpRequestItem = new Models.HttpRequestItem()
            {
                URL = url,
                HttpMethod = System.Net.Http.HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            HttpRequest.HttpRequestHandler httpRequestHandler = new HttpRequest.HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            if (!string.IsNullOrEmpty(executeAsync))
            {
                string[] kvpairs = executeAsync.Split('&');
                System.Collections.Generic.Dictionary<string, string> parameters = System.Linq.Enumerable.ToDictionary(System.Linq.Enumerable.Select(kvpairs, pair => pair.Split('=')), kv => kv[0], kv => kv[1]);


                Token = parameters["oauth_token"];
                TokenSecret = parameters["oauth_token_secret"];
                AuthCodeUri = new System.Uri(string.Format("{0}?oauth_token={1}", Helper.URL.Authorize, Token));

                return new Models.OAuthToken()
                {
                    Token = Token,
                    TokenSecret = TokenSecret
                };
            }

            return new Models.OAuthToken();
        }

        public async System.Threading.Tasks.Task<CopyClient> GetAccessTokenAsync(string verifier)
        {
            string authzHeader = Helpers.AuthorizationHeader.CreateForAccess(Config.ConsumerKey, Config.ConsumerSecret, Token, TokenSecret, verifier);

            string url = string.Format(Helper.URL.AccessToken);

            Models.HttpRequestItem httpRequestItem = new Models.HttpRequestItem()
            {
                URL = Helper.URL.AccessToken,
                HttpMethod = System.Net.Http.HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = false
            };

            HttpRequest.HttpRequestHandler httpRequestHandler = new HttpRequest.HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            string[] kvpairs = executeAsync.Split('&');
            System.Collections.Generic.Dictionary<string, string> parameters = System.Linq.Enumerable.ToDictionary(System.Linq.Enumerable.Select(kvpairs, pair => pair.Split('=')), kv => kv[0], kv => kv[1]);

            if (parameters.ContainsKey("oauth_error_message"))
            {
                return null;
            }
            else
            {
                return new CopyClient(Config, new Models.OAuthToken()
                {
                    Token = parameters["oauth_token"],
                    TokenSecret = parameters["oauth_token_secret"]
                });
            }
        }
    }
}
