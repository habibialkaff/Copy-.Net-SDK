using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class FileSystem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public FileSystemType Type { get; set; }

        [JsonProperty(PropertyName = "stub")]
        public bool Stub { get; set; }

        [JsonProperty(PropertyName = "children")]
        public FileSystem[] Children { get; set; }

        [JsonProperty(PropertyName = "counts", NullValueHandling = NullValueHandling.Ignore)]
        public Counts Counts { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileSystemType
    {
        FILE,
        DIR,
        ROOT,
        COPY,
        INBOX,
        SHARE
    }

    public class Counts
    {
        [JsonProperty(PropertyName = "count_new")]
        public int CountNew { get; set; }

        [JsonProperty(PropertyName = "count_viewed")]
        public int CountViewed { get; set; }

        [JsonProperty(PropertyName = "count_hidden")]
        public int CountHidden { get; set; }
    }    
}
