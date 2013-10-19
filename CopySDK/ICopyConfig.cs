﻿using System.Threading.Tasks;
using CopySDK.Models;

namespace CopySDK
{
    public interface ICopyConfig
    {        
        Task<AuthToken> GetRequestToken();
    }
}