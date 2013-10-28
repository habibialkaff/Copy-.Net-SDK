using System;
using System.Threading.Tasks;
using CopySDK;
using CopySDK.Helper;
using CopySDK.Models;
using System.Net.Http;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string generateAuthzHeader = oAuth.GenerateAuthzHeader(URL.RequestToken, "GET");
            //string forRequest = AuthorizationHeader.CreateForRequest("oob", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", URL.RequestToken);

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

            CopyAuth copyConfig = new CopyAuth("http://copysdk", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", scope);

            Task<OAuthToken> requestToken = copyConfig.GetRequestTokenAsync();

            Task.WhenAll(requestToken);

            OAuthToken authToken = requestToken.Result;

            var url = string.Format("{0}?oauth_token={1}", URL.Authorize, authToken.Token);

            //System.Diagnostics.Process.Start(url);

            //string verifier = "";            

            //Task<CopyClient> copyClient = copyConfig.GetAccessTokenAsync(verifier);

            //Task.WhenAll(requestToken);

            //OAuthToken result = copyClient.Result.AuthToken;




            //OAuth oAuth = new OAuth();
            //oAuth["callback"] = "http://copysdk";
            //oAuth["consumer_key"] = "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1";
            //oAuth["consumer_secret"] = "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK";

            //OAuthSession oAuthSession = oAuth.AcquireRequestToken(URL.RequestToken, "GET");

            //var url = string.Format("{0}?oauth_token={1}", URL.Authorize, oAuthSession.Token);

            //System.Diagnostics.Process.Start(url);

            //string verifier = "";

            //oAuthSession = oAuth.AcquireAccessToken(URL.AccessToken, "GET", verifier);





            //string token = "Nyfr5GWBCX5gaUKElefZcdJSNobTC8Ln";
            //string tokenSecret = "Jfbaqv2SgaX3HvNZ8QXt1BdHV7Z00KxlP745b2JSxpjGlQ4I";
            //string verifier = "291708487082923003087fc9ed34a7c2";
            //string forRequest = AuthorizationHeader.CreateForAccess("cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", token, tokenSecret, verifier);

            HttpMethod httpMethod = HttpMethod.Get;

            Console.WriteLine(httpMethod.ToString());

        }
    }
}
