using CopySDK.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class FileSystemBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }
    }

    public class FileSystem : FileSystemBase
    {        
        [JsonProperty(PropertyName = "path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemType Type { get; set; }

        [JsonProperty(PropertyName = "stub", NullValueHandling = NullValueHandling.Ignore)]
        public bool Stub { get; set; }

        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystem[] Children { get; set; }        

        [JsonProperty(PropertyName = "date_last_synced", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateLastSynced { get; set; }

        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "revision_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RevisionID { get; set; }

        [JsonProperty(PropertyName = "revisions", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemRevision[] FileSystemRevisions { get; set; }

        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string token { get; set; }

        //[JsonProperty(PropertyName = "counts", NullValueHandling = NullValueHandling.Ignore)]
        //public Counts Counts { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileSystemType
    {
        File,
        Dir,
        Root,
        Copy,
        Inbox,
        Share
    }


    public class FileSystemRevision : FileSystemBase
    {
        [JsonProperty(PropertyName = "revision_id", NullValueHandling = NullValueHandling.Ignore)]
        public int RevisionID { get; set; }

        [JsonProperty(PropertyName = "latest", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLatest { get; set; }

        [JsonProperty(PropertyName = "conflict", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsConflict { get; set; }

        [JsonProperty(PropertyName = "creator", NullValueHandling = NullValueHandling.Ignore)]
        public User Creator { get; set; }
    }

    //public class Counts
    //{
    //    [JsonProperty(PropertyName = "count_new")]
    //    public int CountNew { get; set; }

    //    [JsonProperty(PropertyName = "count_viewed")]
    //    public int CountViewed { get; set; }

    //    [JsonProperty(PropertyName = "count_hidden")]
    //    public int CountHidden { get; set; }
    //}
}
