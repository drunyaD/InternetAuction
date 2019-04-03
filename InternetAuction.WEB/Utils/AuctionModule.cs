using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.Services;

namespace InternetAuction.WEB.Utils
{
    public class AuctionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
            Bind<IAuctionService>().To<AuctionService>();
        }
    }
}