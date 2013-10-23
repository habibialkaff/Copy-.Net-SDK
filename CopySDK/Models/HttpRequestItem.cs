using System.Net.Http;

namespace CopySDK.Models
{
    public class HttpRequestItem
    {
        public string URL { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string AuthzHeader { get; set; }

        public HttpContent HttpContent { get; set; }

        public bool IsDataRequest { get; set; }

        public bool IsFileUpload { get; set; }
    }
}
