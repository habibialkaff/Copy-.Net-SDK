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
