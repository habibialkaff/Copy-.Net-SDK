using System.Net.Http;
using CopySDK.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CopySDK.HttpRequest;
using CopySDK.Managers;
using CopySDK.Models;
using Newtonsoft.Json;

namespace CopySDK
{
    public class CopyClient
    {
        public Config Config { get; set; }
        public OAuthToken AuthToken { get; set; }

        public UserManager UserManager { get; set; }
        public FileSystemManager FileSystemManager { get; set; }

        public CopyClient(Config config, OAuthToken authToken)
        {
            Config = config;
            AuthToken = authToken;

            InitManagers();
        }

        private void InitManagers()
        {
            UserManager = new UserManager(Config, AuthToken);
            FileSystemManager = new FileSystemManager(Config, AuthToken);
        }

        public Task<FileSystem> GetRootFolder()
        {
            return FileSystemManager.GetInformationAsync("/copy");
        }

        public Task<FileSystem> GetSharedFolder()
        {
            return FileSystemManager.GetInformationAsync("/inbox");
        } 
    }
}
