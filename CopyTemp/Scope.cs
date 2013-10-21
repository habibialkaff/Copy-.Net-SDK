using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopySDK
{
    public class Scope
    {
        [JsonProperty(PropertyName="profile")]
        public ProfilePermission Profile { get; set; }
        //public InboxPermission Inbox { get; set; }
        //public LinksPermission Links { get; set; }
        //public FileSystemPermission FileSystem { get; set; }
    }

    public class ProfilePermission : WritePermisson
    {
        [JsonProperty(PropertyName = "email")]
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
