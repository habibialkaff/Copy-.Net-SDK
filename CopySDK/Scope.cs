using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK
{
    public class Scope
    {
        public ProfilePermission Profile { get; set; }
        public InboxPermission Inbox { get; set; }
        public LinksPermission Links { get; set; }
        public FileSystemPermission FileSystem { get; set; }
    }

    public class ProfilePermission : WritePermisson
    {
        public EmailPermission Email { get; set; }
    }

    public class EmailPermission : ReadPermisson { }

    public class InboxPermission : ReadPermisson { }

    public class LinksPermission : WritePermisson { }

    public class FileSystemPermission : WritePermisson { }

    public class WritePermisson : ReadPermisson
    {
        public bool Write { get; set; }
    }

    public class ReadPermisson
    {
        public bool Read { get; set; }
    }
}
