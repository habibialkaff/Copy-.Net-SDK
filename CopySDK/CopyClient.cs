using System.Threading.Tasks;
using CopySDK.Managers;
using CopySDK.Models;

namespace CopySDK
{
    public interface ICopyClient
    {
        Task<FileSystem> GetRootFolder();
        Task<FileSystem> GetSharedFolder();
    }

    public class CopyClient : ICopyClient
    {
        public Config Config { get; set; }
        public OAuthToken AuthToken { get; set; }

        public UserManager UserManager { get; set; }
        public FileSystemManager FileSystemManager { get; set; }
        public LinkManager LinkManager { get; set; }

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
            LinkManager = new LinkManager(Config, AuthToken);
        }

        public Task<FileSystem> GetRootFolder()
        {
            return FileSystemManager.GetFileSystemInformationAsync("/copy");
        }

        public Task<FileSystem> GetSharedFolder()
        {
            return FileSystemManager.GetFileSystemInformationAsync("/inbox");
        } 
    }
}
