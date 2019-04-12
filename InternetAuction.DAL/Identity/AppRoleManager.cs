using InternetAuction.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InternetAuction.DAL.Identity
{
    public class AppRoleManager : RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store) : base(store)
        {
        }
    }
}
