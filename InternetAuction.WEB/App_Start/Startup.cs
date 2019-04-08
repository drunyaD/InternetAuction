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
using InternetAuction.WEB.Providers;
using InternetAuction.WEB.Interfaces;
using InternetAuction.BLL.Interfaces;

[assembly: OwinStartup(typeof(InternetAuction.WEB.App_Start.Startup))]
namespace InternetAuction.WEB.App_Start
{
    public class Startup
    {
        private IKernel kernel;

        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(kernel.Get<IOAuthAuthorizationServerOptions>().GetOptions());
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            kernel = new Ninject.Web.Common.Bootstrapper().Kernel;
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            ConfigureOAuth(app);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}