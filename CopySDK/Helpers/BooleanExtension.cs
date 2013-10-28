using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK.Helpers
{
    public static class BooleanExtension
    {

        public static string ToLowerString(this bool input)
        {
            return input.ToString().ToLower();
        }
    }
}
