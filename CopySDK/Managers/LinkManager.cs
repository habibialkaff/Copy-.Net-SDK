using System.Net.Http;
using CopySDK.Helper;
using CopySDK.Helpers;
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
    public interface ILinkManager
    {
        Task<Link> GetLinkInformationAsync(string token);
        Task<Link[]> GetAllLinksAsync();
        Task<Link> CreateLink(LinkCreate newLink);
    }

    public class LinkManager : ILinkManager
    {
        public Config Config { get; set; }
        public OAuthToken AuthToken { get; set; }

        private readonly HttpRequestHandler _httpRequestHandler;

        public LinkManager(Config config, OAuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;

            _httpRequestHandler = new HttpRequestHandler();
        }

        public async Task<Link> GetLinkInformationAsync(string token)
        {
            if (token != null)
            {
                string url = string.Format("{0}/links/{1}", URL.RESTRoot, token);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

                string executeAsync = await _httpRequestHandler.ReadAsStringAsync(httpRequestItem);

                return JsonConvert.DeserializeObject<Link>(executeAsync);
            }
            return null;
        }

        public async Task<Link[]> GetAllLinksAsync()
        {
            string url = string.Format("{0}/links", URL.RESTRoot);

            HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

            string executeAsync = await _httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            return JsonConvert.DeserializeObject<Link[]>(executeAsync);
        }

        public async Task<Link> CreateLink(LinkCreate newLink)
        {
            string url = string.Format("{0}/links", URL.RESTRoot);

            string serializeObject = JsonConvert.SerializeObject(newLink);

            HttpContent httpContent = new StringContent(serializeObject);

            HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Post, httpContent);

            string executeAsync = await _httpRequestHandler.ReadAsStringAsync(httpRequestItem);

            return JsonConvert.DeserializeObject<Link>(executeAsync);
        }

        private HttpRequestItem CreateHttpRequestItem(string url, HttpMethod httpMethod, HttpContent httpContent = null)
        {
            string authzHeader = AuthorizationHeader.CreateForREST(Config.ConsumerKey, Config.ConsumerSecret, AuthToken.Token, AuthToken.TokenSecret, url, httpMethod.ToString());

            return new HttpRequestItem()
            {
                URL = url,
                HttpMethod = httpMethod,
                AuthzHeader = authzHeader,
                HttpContent = httpContent,
                IsDataRequest = true
            };
        }
    }
}
