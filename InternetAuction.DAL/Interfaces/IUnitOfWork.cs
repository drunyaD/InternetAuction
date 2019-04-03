using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Entities;
using InternetAuction.DAL.Identity;

namespace InternetAuction.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        IProfileManager ProfileManager { get; }
        AppRoleManager RoleManager { get; }
        IRepository<Bet> Bets { get; }
        IRepository<Category> Categories { get; }
        IRepository<Image> Images { get; }
        IRepository<Lot> Lots { get; }
        void Save();
        void RejectChanges();
    }
}

