namespace CopySDK.Authentication
{
    public interface ICopyAuth
    {        
        System.Threading.Tasks.Task<Models.OAuthToken> GetRequestTokenAsync();
    }
}
