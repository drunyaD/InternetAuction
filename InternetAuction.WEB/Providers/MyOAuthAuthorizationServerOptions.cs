using InternetAuction.WEB.Interfaces;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InternetAuction.WEB.Providers
{
    public class MyOAuthAuthorizationServerOptions : IOAuthAuthorizationServerOptions
    {
        private IOAuthAuthorizationServerProvider _provider;

        public MyOAuthAuthorizationServerOptions(IOAuthAuthorizationServerProvider provider)
        {
            _provider = provider;
        }
        public OAuthAuthorizationServerOptions GetOptions()
        {

            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = _provider
            };
        }
    }
}