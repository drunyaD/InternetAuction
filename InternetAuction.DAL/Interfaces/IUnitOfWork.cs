using System;
using InternetAuction.DAL.Entities;
using InternetAuction.DAL.Identity;

namespace InternetAuction.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        IRepository<Bet> Bets { get; }
        IRepository<Category> Categories { get; }
        IRepository<Image> Images { get; }
        IRepository<Lot> Lots { get; }
        void Save();
    }
}

