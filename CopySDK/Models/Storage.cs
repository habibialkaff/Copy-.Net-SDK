using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Models
{
    public class Storage
    {
        public long Used { get; set; }

        public long Quota { get; set; }

        public long Saved { get; set; }
    }
}
