using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class User
    {
        public string Id { get; set; }

        public Storage Storage { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Developer { get; set; }

        public int CreatedTime { get; set; }

        public string Email { get; set; }

        public Email[] Emails { get; set; }
    }
}
