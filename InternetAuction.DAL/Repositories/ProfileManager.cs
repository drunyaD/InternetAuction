using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.EF;
using InternetAuction.DAL.Entities;

namespace InternetAuction.DAL.Repositories
{
   public class ProfileManager : IProfileManager
   {
        public AuctionContext Database { get; set; }
        public ProfileManager(AuctionContext db)
        {
            Database = db;
        }

        public void Create(Profile item)
        {
            Database.Profiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
