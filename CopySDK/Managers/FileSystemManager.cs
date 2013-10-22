using CopySDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       
        public Task<FileSystem> GetInformationAsync(string id)
        {
            return null;
        }

        public Task<byte[]> DownloadFile(string id)
        {
            return null;
        }               
    }
}
