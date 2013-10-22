using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CopySDK.Helper;
using OAuthSignature;

namespace CopySDK.Helpers
{
    public static class AuthorizationHeader
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static Dictionary<string, string> parameters; //TODO: Fix, cannot initialized at the top. The ordering is messed up

        private static string OauthConsumerSecret { get; set; }
        private static string OauthTokenSecret { get; set; }

        public static string CreateForRequest(string callbackURL, string consumerKey, string consumerSecret, string url)
        {
            parameters = new Dictionary<string, string>()
            {
                {"oauth_callback",""},
                {"oauth_consumer_key",""},
                {"oauth_nonce",""},
                {"oauth_signature",""},
                {"oauth_signature_method","HMAC-SHA1"},            
                {"oauth_timestamp",""},
                {"oauth_token",""},
                {"oauth_verifier",""},
                {"oauth_version","1.0"},                        
            };

            OauthConsumerSecret = consumerSecret;

            parameters.Remove("oauth_token");
            parameters.Remove("oauth_verifier");

            parameters["oauth_callback"] = callbackURL;
            parameters["oauth_consumer_key"] = consumerKey;
            parameters["oauth_nonce"] = GenerateNonce();
            parameters["oauth_timestamp"] = GenerateTimeStamp();
            parameters["oauth_signature"] = GenerateSignature(url, "GET");

            return "OAuth " + EncodeRequestParameters();
        }

        public static string CreateForAccess(string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier)
        {
            parameters = new Dictionary<string, string>()
            {
                {"oauth_callback",""},
                {"oauth_consumer_key",""},
                {"oauth_nonce",""},
                {"oauth_signature",""},
                {"oauth_signature_method","HMAC-SHA1"},            
                {"oauth_timestamp",""},
                {"oauth_token",""},
                {"oauth_verifier",""},
                {"oauth_version","1.0"},                        
            };

            OauthConsumerSecret = consumerSecret;
            OauthTokenSecret = tokenSecret;

            parameters.Remove("oauth_callback");

            parameters["oauth_consumer_key"] = consumerKey;
            parameters["oauth_nonce"] = GenerateNonce();
            parameters["oauth_timestamp"] = GenerateTimeStamp();
            parameters["oauth_token"] = token;
            parameters["oauth_verifier"] = verifier;
            parameters["oauth_signature"] = GenerateSignature(URL.AccessToken, "GET");

            return "OAuth " + EncodeRequestParameters();
        }

        public static string CreateForREST(string consumerKey, string consumerSecret, string token, string tokenSecret, string url, string method)
        {
            parameters = new Dictionary<string, string>()
            {
                {"oauth_callback",""},
                {"oauth_consumer_key",""},
                {"oauth_nonce",""},
                {"oauth_signature",""},
                {"oauth_signature_method","HMAC-SHA1"},            
                {"oauth_timestamp",""},
                {"oauth_token",""},
                {"oauth_verifier",""},
                {"oauth_version","1.0"},                        
            };

            OauthConsumerSecret = consumerSecret;
            OauthTokenSecret = tokenSecret;

            parameters.Remove("oauth_callback");
            parameters.Remove("oauth_verifier");

            parameters["oauth_consumer_key"] = consumerKey;
            parameters["oauth_nonce"] = GenerateNonce();
            parameters["oauth_timestamp"] = GenerateTimeStamp();
            parameters["oauth_token"] = token;
            parameters["oauth_signature"] = GenerateSignature(url, method);

            return "OAuth " + EncodeRequestParameters();
        }

        /// <summary>
        /// Formats the list of request parameters into string a according
        /// to the requirements of oauth. The resulting string could be used
        /// in the Authorization header of the request.
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     See http://dev.twitter.com/pages/auth#intro  for some
        ///     background.  The output of this is not suitable for signing.
        ///   </para>
        ///   <para>
        ///     There are 2 formats for specifying the list of oauth
        ///     parameters in the oauth spec: one suitable for signing, and
        ///     the other suitable for use within Authorization HTTP Headers.
        ///     This method emits a string suitable for the latter.
        ///   </para>
        /// </remarks>
        ///
        ///
        /// <returns>a string representing the parameters</returns>
        private static string EncodeRequestParameters()
        {
            var sb = new System.Text.StringBuilder();
            foreach (KeyValuePair<String, String> item in parameters)
            {
                sb.AppendFormat("{0}=\"{1}\", ", item.Key, UrlEncode(item.Value));
            }

            return sb.ToString().TrimEnd(' ').TrimEnd(',');
        }


        /// <summary>
        /// Generate the timestamp for the signature.
        /// </summary>
        /// <returns>The timestamp, in string form.</returns>
        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - _epoch;
            return Convert.ToInt64(ts.TotalSeconds).ToString();

            //return "1382344140";
        }

        /// <summary>
        /// Generate an oauth nonce.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     According to RFC 5849, A nonce is a random string,
        ///     uniquely generated by the client to allow the server to
        ///     verify that a request has never been made before and
        ///     helps prevent replay attacks when requests are made over
        ///     a non-secure channel.  The nonce value MUST be unique
        ///     across all requests with the same timestamp, client
        ///     credentials, and token combinations.
        ///   </para>
        ///   <para>
        ///     One way to implement the nonce is just to use a
        ///     monotonically-increasing integer value.  It starts at zero and
        ///     increases by 1 for each new request or signature generated.
        ///     Keep in mind the nonce needs to be unique only for a given
        ///     timestamp!  So if your app makes less than one request per
        ///     second, then using a static nonce of "0" will work.
        ///   </para>
        ///   <para>
        ///     Most oauth nonce generation routines are waaaaay over-engineered,
        ///     and this one is no exception.
        ///   </para>
        /// </remarks>
        /// <returns>the nonce</returns>
        private static string GenerateNonce()
        {
            //Random random = new Random();

            //var sb = new StringBuilder();
            //for (int i = 0; i < 8; i++)
            //{
            //    int g = random.Next(3);
            //    switch (g)
            //    {
            //        case 0:
            //            // lowercase alpha
            //            sb.Append((char)(random.Next(26) + 97), 1);
            //            break;
            //        default:
            //            // numeric digits
            //            sb.Append((char)(random.Next(10) + 48), 1);
            //            break;
            //    }
            //}
            //return sb.ToString();            

            //return "44b95d54-27ae-4de0-b267-8a2a86d418a3";
            return Guid.NewGuid().ToString();
        }

        public static string GenerateSignature(string uri, string method)
        {
            var signatureBase = GetSignatureBase(uri, method);
            HMacSha1 oAuthSignature = GetHash();

            byte[] dataBuffer = Encoding.UTF8.GetBytes(signatureBase);
            byte[] hashBytes = oAuthSignature.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Formats the list of request parameters into "signature base" string as
        /// defined by RFC 5849.  This will then be MAC'd with a suitable hash.
        /// </summary>
        private static string GetSignatureBase(string url, string method)
        {
            // normalize the URI
            var uri = new Uri(url);
            var normUrl = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            if (!((uri.Scheme == "http" && uri.Port == 80) ||
                  (uri.Scheme == "https" && uri.Port == 443)))
                normUrl += ":" + uri.Port;

            normUrl += uri.AbsolutePath;

            // the sigbase starts with the method and the encoded URI
            var sb = new System.Text.StringBuilder();
            sb.Append(method)
                .Append('&')
                .Append(UrlEncode(normUrl))
                .Append('&');

            Dictionary<string, string> queryParameters = ExtractQueryParameters(uri.Query);

            // concat+format all those params except oauth_signature
            var sb1 = new StringBuilder();
            foreach (KeyValuePair<String, String> item in parameters.Where(x => x.Key != "oauth_signature"))
            {
                string itemValue = item.Value;

                // Specific Fix : double encode Callback
                if (item.Key == "oauth_callback")
                {
                    itemValue = UrlEncode(item.Value);
                }

                // even "empty" params need to be encoded this way.
                sb1.AppendFormat("{0}={1}&", item.Key, itemValue);
            }

            foreach (KeyValuePair<String, String> item in queryParameters)
            {
                // even "empty" params need to be encoded this way.
                sb1.AppendFormat("{0}={1}&", item.Key, item.Value);
            }

            // append the UrlEncoded version of that string to the sigbase
            sb.Append(UrlEncode(sb1.ToString().TrimEnd('&')));
            var result = sb.ToString();
            //Tracing.Trace("Sigbase: '{0}'", result);
            return result;
        }



        private static HMacSha1 GetHash()
        {
            string keystring = string.Format("{0}&{1}", UrlEncode(OauthConsumerSecret), UrlEncode(OauthTokenSecret ?? string.Empty));
            //Tracing.Trace("keystring: '{0}'", keystring);
            return new HMacSha1
            {
                Key = Encoding.UTF8.GetBytes(keystring)
            };
        }

        /// <summary>
        ///   This is an oauth-compliant Url Encoder.  The default .NET
        ///   encoder outputs the percent encoding in lower case.  While this
        ///   is not a problem with the percent encoding defined in RFC 3986,
        ///   OAuth (RFC 5849) requires that the characters be upper case
        ///   throughout OAuth.
        /// </summary>
        ///
        /// <param name="value">The value to encode</param>
        ///
        /// <returns>the Url-encoded version of that string</returns>
        public static string UrlEncode(string value)
        {
            const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            StringBuilder result = new StringBuilder();
            foreach (char symbol in value)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                    result.Append(symbol);
                else
                    result.Append('%' + String.Format("{0:X2}", (int)symbol));
            }
            return result.ToString();
        }

        /// <summary>
        /// Internal function to extract from a URL all query string
        /// parameters that are not related to oauth - in other words all
        /// parameters not begining with "oauth_".
        /// </summary>
        ///
        /// <remarks>
        ///   <para>
        ///     For example, given a url like http://foo?a=7&guff, the
        ///     returned value will be a Dictionary of string-to-string
        ///     relations.  There will be 2 entries in the Dictionary: "a"=>7,
        ///     and "guff"=>"".
        ///   </para>
        /// </remarks>
        ///
        /// <param name="queryString">The query string part of the Url</param>
        ///
        /// <returns>A Dictionary containing the set of
        /// parameter names and associated values</returns>
        private static Dictionary<String, String> ExtractQueryParameters(string queryString)
        {
            if (queryString.StartsWith("?"))
                queryString = queryString.Remove(0, 1);

            var result = new Dictionary<String, String>();

            if (string.IsNullOrEmpty(queryString))
                return result;

            foreach (string s in queryString.Split('&'))
            {
                if (!string.IsNullOrEmpty(s) && !s.StartsWith("oauth_"))
                {
                    if (s.IndexOf('=') > -1)
                    {
                        string[] temp = s.Split('=');
                        result.Add(temp[0], temp[1]);
                    }
                    else
                        result.Add(s, string.Empty);
                }
            }

            return result;
        }
    }
}
