using InternetAuction.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace InternetAuction.DAL.Identity
{
    public class AppUserManager : UserManager<User>
    {
        public AppUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}
