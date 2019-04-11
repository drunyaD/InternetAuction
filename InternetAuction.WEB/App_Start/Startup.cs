using Microsoft.Owin;
using Owin;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject.Web.WebApi;
using Microsoft.Owin.Security.OAuth;
using InternetAuction.BLL.Interfaces;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using InternetAuction.BLL.Infrastructure;

[assembly: OwinStartup(typeof(InternetAuction.WEB.App_Start.Startup))]
namespace InternetAuction.WEB.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfig.Initialize();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
            });

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new NinjectDependencyResolver(new Ninject.Web.Common.Bootstrapper().Kernel);
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}