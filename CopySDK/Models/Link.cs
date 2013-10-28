using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopySDK.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopySDK.Models
{
    public class Link
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "url_short", NullValueHandling = NullValueHandling.Ignore)]
        public string UrlShort { get; set; }

        [JsonProperty(PropertyName = "creator_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatorId { get; set; }

        [JsonProperty(PropertyName = "created_time", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedTime { get; set; }

        [JsonProperty(PropertyName = "confirmation_required", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsConfirmationRequired { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "permissions", NullValueHandling = NullValueHandling.Ignore)]
        public LinkPermissionType Permissions { get; set; }

        [JsonProperty(PropertyName = "recipients", NullValueHandling = NullValueHandling.Ignore)]
        public User[] Recipients { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LinkPermissionType
    {
        Read,
        Sync
    }

    public class LinkCreate
    {
        [JsonProperty(PropertyName = "public", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPublic { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "paths", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Paths { get; set; }
    }

    public class LinkUpdate : LinkCreate
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "recipients", NullValueHandling = NullValueHandling.Ignore)]
        public User[] Recipients { get; set; }
    }
}
