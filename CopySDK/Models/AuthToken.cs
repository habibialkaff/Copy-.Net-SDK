using Newtonsoft.Json;

namespace CopySDK.Models
{
    public class OAuthToken
    {
        [JsonProperty(PropertyName="oauth_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "oauth_token_secret")]
        public string TokenSecret { get; set; }
    }
}
