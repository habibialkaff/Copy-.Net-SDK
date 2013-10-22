using System.Net.Http;
using CopySDK.Helper;
using CopySDK.Helpers;
using CopySDK.HttpRequest;
using CopySDK.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopySDK.Managers
{
    public interface IFileSystemManager
    {
        Task<FileSystem> GetInformationAsync(string id);
        Task<byte[]> DownloadFile(string id);
    }

    public class FileSystemManager : IFileSystemManager
    {
        public Config Config { get; set; }
        public OAuthToken AuthToken { get; set; }

        public FileSystemManager(Config config, OAuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;
        }

        public async Task<FileSystem> GetInformationAsync(string id)
        {
            if (id != null)
            {
                if (!id.StartsWith("/"))
                {
                    id = "/" + id;
                }

                string url = string.Format("{0}/meta{1}", URL.RESTRoot, id);

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
                string executeAsync = await httpRequestHandler.ReadAsStringAsync(httpRequestItem);

                return JsonConvert.DeserializeObject<FileSystem>(executeAsync);
            }
            return null;
        }

        public async Task<byte[]> DownloadFile(string id)
        {
            if (id != null)
            {
                if (!id.StartsWith("/"))
                {
                    id = "/" + id;
                }

                id = id.Replace("/copy/", "/files/");

                string url = string.Format("{0}{1}", URL.RESTRoot, id);

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
                return await httpRequestHandler.ReadAsByteArrayAsync(httpRequestItem);
            }
            return null;
        }
    }
}
