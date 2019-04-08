﻿using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.Services;
using InternetAuction.BLL.Infrastructure;
using InternetAuction.WEB.Interfaces;
using InternetAuction.WEB.Providers;
using Microsoft.Owin.Security.OAuth;

namespace InternetAuction.WEB.Utils
{
    public class AuctionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
            Bind<IAuctionService>().To<AuctionService>();
            Bind<IBetValidator>().To<BetValidator>();
            Bind<IBetEditValidator>().To<BetEditValidator>();
            Bind<ICategoryValidator>().To<CategoryValidator>();
            Bind<ICategoryEditValidator>().To<CategoryEditValidator>();
            Bind<ILotValidator>().To<LotValidator>();
            Bind<ILotEditValidator>().To<LotEditValidator>();
            Bind<IImageValidator>().To<ImageValidator>();
            Bind<IOAuthAuthorizationServerOptions>().To<MyOAuthAuthorizationServerOptions>();
            Bind<IOAuthAuthorizationServerProvider>().To<SimpleAuthorizationServerProvider>();
        }
    }
}