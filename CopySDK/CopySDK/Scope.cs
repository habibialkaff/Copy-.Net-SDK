using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK
{
    public class Scope
    {
        public Profile Profile { get; set; }
        public Inbox Inbox { get; set; }
        public Links Links { get; set; }
        public FileSystem FileSystem { get; set; }
    }

    public class Profile : WritePermisson
    {
        public Email Email { get; set; }
    }

    public class Email : ReadPermisson { }

    public class Inbox : ReadPermisson { }

    public class Links : WritePermisson { }

    public class FileSystem : WritePermisson { }

    public class WritePermisson : ReadPermisson
    {
        public bool Write { get; set; }
    }

    public class ReadPermisson
    {
        public bool Read { get; set; }
    }
}
