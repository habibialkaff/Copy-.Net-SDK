using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class Email
    {
        public bool Primary { get; set; }

        public bool Confirmed { get; set; }

        public string Email { get; set; }

        public string Gravatar { get; set; }
    }
}
