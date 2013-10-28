using Newtonsoft.Json;

namespace CopySDK.Models
{
    public class Scope
    {
        [JsonProperty(PropertyName = "profile", NullValueHandling = NullValueHandling.Ignore)]
        public ProfilePermission Profile { get; set; }

        [JsonProperty(PropertyName = "inbox", NullValueHandling = NullValueHandling.Ignore)]
        public InboxPermission Inbox { get; set; }

        [JsonProperty(PropertyName = "links", NullValueHandling = NullValueHandling.Ignore)]
        public LinksPermission Links { get; set; }

        [JsonProperty(PropertyName = "filesystem", NullValueHandling = NullValueHandling.Ignore)]
        public FileSystemPermission FileSystem { get; set; }
    }

    public class ProfilePermission : WritePermisson
    {
        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public EmailPermission Email { get; set; }
    }

    public class EmailPermission : ReadPermisson { }

    public class InboxPermission : ReadPermisson { }

    public class LinksPermission : WritePermisson { }

    public class FileSystemPermission : WritePermisson { }

    public class WritePermisson : ReadPermisson
    {
        [JsonProperty(PropertyName = "write")]
        public bool Write { get; set; }
    }

    public class ReadPermisson
    {
        [JsonProperty(PropertyName = "read")]
        public bool Read { get; set; }
    }
}
