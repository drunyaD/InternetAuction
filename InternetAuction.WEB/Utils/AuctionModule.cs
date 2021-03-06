﻿using Ninject.Modules;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.Services;
using InternetAuction.BLL.Infrastructure;

namespace InternetAuction.WEB.Utils
{
    public class AuctionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
            Bind<IAuctionService>().To<AuctionService>();
            Bind<IUserValidator>().To<UserValidator>();
            Bind<IBetValidator>().To<BetValidator>();
            Bind<ICategoryValidator>().To<CategoryValidator>();
            Bind<ICategoryEditValidator>().To<CategoryEditValidator>();
            Bind<ILotValidator>().To<LotValidator>();
            Bind<ILotEditValidator>().To<LotEditValidator>();
            Bind<IImageValidator>().To<ImageValidator>();
        }
    }
}