using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace InternetAuction.WEB.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUserService _userService;

        public SimpleAuthorizationServerProvider(IUserService service)
        {
            _userService = service;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            UserDTO user;
            try
            {
                user = await _userService.FindUser(context.UserName, context.Password);
            }
            catch(ArgumentException)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }           

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);

        }
    }
}