using CopySDK.Authentication;
using CopySDK.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CopySDK
{
    public class CopyClient
    {
        public ICopyConfig CopyConfig { get; set; }
        public OAuthSession OAuthSession { get; set; }

        public OAuthSession GetAccessToken(string verifier)
        {
            string authzHeader = AuthorizationHeader.CreateForAccess(CopyConfig.ConsumerKey, CopyConfig.ConsumerSecret, OAuthSession.Token, OAuthSession.TokenSecret, verifier);

            string url = string.Format(URL.AccessToken);

            // prepare the token request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", authzHeader);
            request.Method = "POST";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        OAuthSession oAuthSession = new OAuthSession(reader.ReadToEnd());

                        return oAuthSession;
                    }
                }
            }
            return null;
        }
    }
}
