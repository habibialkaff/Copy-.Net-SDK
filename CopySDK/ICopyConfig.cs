using CopySDK.Authentication;
using System;

namespace CopySDK
{
    public interface ICopyConfig
    {
        string ConsumerKey { get; set; }
        string ConsumerSecret { get; set; }
        OAuthSession GetRequestToken();
    }
}
