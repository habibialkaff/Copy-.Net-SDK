using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class HttpRequestItem
    {
        public string URL { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string AuthzHeader { get; set; }

        public HttpContent HttpContent { get; set; }

        public bool IsDataRequest { get; set; }
    }
}
