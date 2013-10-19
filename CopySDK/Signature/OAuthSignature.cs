using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopySDK.Signature;

namespace CopySDK
{
    public class OAuthSignature
    {
        public byte[] Key { get; set; }

        public OAuthSignature() { }

        public OAuthSignature(byte[] key)
        {
            Key = key;
        }

        public byte[] ComputeHash(byte[] buffer)
        {
            HMac hMac = new HMac(new Sha1Digest());

            byte[] resBuf = new byte[hMac.GetMacSize()];

            hMac.Init(new KeyParameter(Key));
            hMac.BlockUpdate(buffer, 0, buffer.Length);
            hMac.DoFinal(resBuf, 0);

            return resBuf;
        }
    }
}
