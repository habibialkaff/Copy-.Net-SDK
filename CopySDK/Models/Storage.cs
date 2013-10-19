using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopySDK.Models
{
    public class Storage
    {
        [JsonProperty(PropertyName = "used")]
        public long Used { get; set; }

        [JsonProperty(PropertyName = "quota")]
        public long Quota { get; set; }

        [JsonProperty(PropertyName = "saved")]
        public long Saved { get; set; }
    }
}
