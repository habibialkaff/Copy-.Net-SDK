using System.Net.Http;
using CopySDK.Helper;
using CopySDK.HttpRequest;
using CopySDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopySDK.Managers
{
    public class UserManager
    {
        public Config Config { get; set; }
        public AuthToken AuthToken { get; set; }

        public UserManager(Config config, AuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;
        }

        public async Task<User> GetUser()
        {
            string url = string.Format("{0}/{1}", URL.RESTRoot, "user");

            string authzHeader = AuthorizationHeader.CreateForREST(Config.ConsumerKey, Config.ConsumerSecret, AuthToken.Token, AuthToken.TokenSecret, url, "GET");

            HttpRequestItem httpRequestItem = new HttpRequestItem()
            {
                URL = url,
                HttpMethod = HttpMethod.Get,
                AuthzHeader = authzHeader,
                HttpContent = null,
                IsDataRequest = true
            };

            HttpRequestHandler httpRequestHandler = new HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ExecuteAsync(httpRequestItem);

            return JsonConvert.DeserializeObject<User>(executeAsync);
        }

        public async Task<User> UpdateUser(UserUpdate userUpdate)
        {
            string url = string.Format("{0}/{1}", URL.RESTRoot, "user");

            string authzHeader = AuthorizationHeader.CreateForREST(Config.ConsumerKey, Config.ConsumerSecret, AuthToken.Token, AuthToken.TokenSecret, url, "PUT");

            string serializeObject = JsonConvert.SerializeObject(userUpdate);

            HttpContent httpContent = new StringContent(serializeObject);

            HttpRequestItem httpRequestItem = new HttpRequestItem()
            {
                URL = url,
                HttpMethod = HttpMethod.Put,
                AuthzHeader = authzHeader,
                HttpContent = httpContent,
                IsDataRequest = true
            };

            HttpRequestHandler httpRequestHandler = new HttpRequestHandler();
            string executeAsync = await httpRequestHandler.ExecuteAsync(httpRequestItem);

            //TODO : Investigate
            return JsonConvert.DeserializeObject<User>(executeAsync);
        }
    }
}
