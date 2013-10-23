using System.Threading.Tasks;
using CopySDK.Models;

namespace CopySDK
{
    public interface ICopyAuth
    {        
        Task<OAuthToken> GetRequestTokenAsync();
    }
}
