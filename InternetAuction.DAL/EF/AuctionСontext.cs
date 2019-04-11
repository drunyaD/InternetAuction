using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InternetAuction.DAL.EF
{
    public class AuctionContext: IdentityDbContext<User>
    {
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public AuctionContext(string connectionString) : base(connectionString) { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
