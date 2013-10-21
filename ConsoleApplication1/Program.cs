using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopySDK;
using CopySDK.Authentication;
using CopySDK.Helper;
using CopySDK.Models;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Scope scope = new Scope()
            {
                Profile = new ProfilePermission()
                {
                    Read = true,
                    Write = true,
                    Email = new EmailPermission()
                    {
                        Read = true
                    }
                }
            };

            CopyConfig copyConfig = new CopyConfig("http://copysdk", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", scope);

            //Task<AuthToken> requestToken = copyConfig.GetRequestToken();

            //Task.WhenAll(requestToken);

            //AuthToken authToken = requestToken.Result;

            //var url = string.Format("{0}?oauth_token={1}", URL.Authorize, authToken.Token);

            //System.Diagnostics.Process.Start(url);

            OAuth oAuth = new OAuth();
            oAuth["callback"] = "http://copysdk";
            oAuth["consumer_key"] = "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1";
            oAuth["consumer_secret"] = "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK";

            string generateAuthzHeader = oAuth.GenerateAuthzHeader(URL.RequestToken, "GET");
            //string forRequest = AuthorizationHeader.CreateForRequest("oob", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", URL.RequestToken);

            OAuthSession oAuthSession = oAuth.AcquireRequestToken(URL.RequestToken, "GET");



        }
    }
}
