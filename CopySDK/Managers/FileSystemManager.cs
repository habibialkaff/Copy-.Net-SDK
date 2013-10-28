using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        Task<FileSystem> GetFileSystemInformationAsync(string id);
        Task<byte[]> DownloadFileAsync(string id);
    }

    public class FileSystemManager : IFileSystemManager
    {
        public Config Config { get; set; }
        public OAuthToken AuthToken { get; set; }

        private readonly HttpRequestHandler _httpRequestHandler;

        public FileSystemManager(Config config, OAuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;

            _httpRequestHandler = new HttpRequestHandler();
        }

        public async Task<FileSystem> GetFileSystemInformationAsync(string id)
        {
            id = NormalizeId(id);

            if (id != null)
            {
                string url = string.Format("{0}/meta{1}", URL.RESTRoot, id);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

                string executeAsync = await _httpRequestHandler.ReadAsStringAsync(httpRequestItem);

                return JsonConvert.DeserializeObject<FileSystem>(executeAsync);
            }
            return null;
        }

        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            fileId = NormalizeId(fileId);

            if (fileId != null)
            {
                fileId = fileId.Replace("/copy", "/files");

                string url = string.Format("{0}{1}", URL.RESTRoot, fileId);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

                return await _httpRequestHandler.ReadAsByteArrayAsync(httpRequestItem);
            }
            return null;
        }

        public async Task<byte[]> DownloadThumbnailImageAsync(string fileId, int size)
        {
            fileId = NormalizeId(fileId);

            if (fileId != null)
            {
                fileId = fileId.Replace("/copy", "/thumbs");

                string url = string.Format("{0}{1}", URL.RESTRoot, fileId);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await _httpRequestHandler.ReadAsByteArrayAsync(httpRequestItem);
                }
            }
            return null;
        }

        public async Task<bool> RenameFileAsync(string fileId, string newFileName, bool overwriteFileWithTheSameName)
        {
            bool result = false;

            fileId = NormalizeId(fileId);

            if (fileId != null)
            {
                fileId = fileId.Replace("/copy", "/files");

                string url = string.Format("{0}{1}?name={2}&overwrite={3}", URL.RESTRoot, fileId, newFileName, overwriteFileWithTheSameName.ToLowerString());

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Put);

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> MoveFileAsync(string fileId, string newParentFolderId, string newFileName, bool overwriteFileWithTheSameName)
        {
            bool result = false;

            fileId = NormalizeId(fileId);
            string targetFileId = NormalizeId(string.Format("{0}/{1}", newParentFolderId, newFileName));

            if (fileId != null && targetFileId != null)
            {
                fileId = fileId.Replace("/copy", "/files");

                string url = string.Format("{0}{1}?path={2}&overwrite={3}", URL.RESTRoot, fileId, targetFileId, overwriteFileWithTheSameName.ToLowerString());

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Put);

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> CreateNewFolderAsync(string parentFolderId, string folderName, bool overwriteFolderWithTheSameName)
        {
            bool result = false;

            parentFolderId = NormalizeId(parentFolderId);

            if (parentFolderId != null)
            {
                parentFolderId = parentFolderId.Replace("/copy", "/files");

                string url = string.Format("{0}{1}/{2}?overwrite={3}", URL.RESTRoot, parentFolderId, folderName, overwriteFolderWithTheSameName.ToLowerString());

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Post);

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<bool> UploadNewFileAsync(string parentFolderId, string fileName, byte[] newFile, bool overwriteFileWithTheSameName)
        {
            bool result = false;

            parentFolderId = NormalizeId(parentFolderId);

            if (parentFolderId != null && !string.IsNullOrEmpty(fileName) && newFile.Length <= 1073741824)
            {
                parentFolderId = parentFolderId.Replace("/copy", "/files");

                string url = string.Format("{0}{1}?overwrite={2}", URL.RESTRoot, parentFolderId, overwriteFileWithTheSameName.ToLowerString());

                HttpContent httpContent = new ByteArrayContent(newFile);

                ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileName
                };
                
                httpContent.Headers.ContentDisposition = contentDisposition;

                MultipartFormDataContent formContent = new MultipartFormDataContent(new Random().Next(10000, 99999).ToString()) 
                { 
                    httpContent 
                };


                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Post, formContent);
                httpRequestItem.IsFileUpload = true;

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task UpdateExistingFileAsync(string fileId, byte[] newFile)
        {

        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result = false;

            id = NormalizeId(id);

            if (id != null)
            {
                id = id.Replace("/copy", "/files");

                string url = string.Format("{0}{1}", URL.RESTRoot, id);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Delete);

                HttpResponseMessage httpResponseMessage = await _httpRequestHandler.ExecuteAsync(httpRequestItem);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }

        public async Task<FileSystem> ListFileRevisionsAsync(string fileId)
        {
            fileId = NormalizeId(fileId);

            if (fileId != null)
            {
                string url = string.Format("{0}/meta{1}/@activity", URL.RESTRoot, fileId);

                HttpRequestItem httpRequestItem = CreateHttpRequestItem(url, HttpMethod.Get);

                string executeAsync = await _httpRequestHandler.ReadAsStringAsync(httpRequestItem);

                return JsonConvert.DeserializeObject<FileSystem>(executeAsync);
            }
            return null;
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

        private string NormalizeId(string id)
        {
            if (id != null)
            {
                if (!id.StartsWith("/"))
                {
                    id = "/" + id;
                }
            }

            return id;
        }
    }
}
