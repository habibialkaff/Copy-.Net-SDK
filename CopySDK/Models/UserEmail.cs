using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopySDK.Models
{
    public class UserEmail
    {
        [JsonProperty(PropertyName = "primary")]
        public bool Primary { get; set; }

        [JsonProperty(PropertyName = "confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
    }
}
