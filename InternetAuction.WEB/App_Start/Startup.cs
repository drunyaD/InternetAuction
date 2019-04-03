using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using InternetAuction.BLL.Interfaces;

[assembly: OwinStartup(typeof(InternetAuction.WEB.App_Start.Startup))]

namespace InternetAuction.WEB.App_Start
{
    public class Startup
    {
        IUserService userService;
        public void Configuration(IAppBuilder app, IUserService serv)
        {

            app.CreatePerOwinContext(() => serv);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}