using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using InternetAuction.DAL.Entities;
using InternetAuction.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InternetAuction.DAL.EF
{
    public class AuctionContext : IdentityDbContext<User>
    {
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public AuctionContext(string connectionString) : base(connectionString)
        {
            InitializeDatabase();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Lots)
                .WithRequired(l => l.LotOwner)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
        protected virtual void InitializeDatabase()
        {
            if (!Database.Exists())
            {
                Database.Initialize(true);
                new DatabaseInitializer().Seed(this);
            }
        }
    }

    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<AuctionContext>
    {

        public new void Seed(AuctionContext db)
        {
            var storeStore = new RoleStore<Role>(db);
            var roleManager = new AppRoleManager(storeStore);
            roleManager.Create(new Role() { Name = "user" });
            roleManager.Create(new Role() { Name = "bannedUser" });
            roleManager.Create(new Role() { Name = "moderator" });
            roleManager.Create(new Role() { Name = "administrator" });

            var userStore = new UserStore<User>(db);
            var userManager = new AppUserManager(userStore);
            User admin = new User
            {
                Email = "admin@mail.ru",
                UserName = "MainAdmin"
            };
            userManager.Create(admin, "adminpassword");
            userManager.AddToRole(admin.Id, "administrator");

            db.Categories.Add(new Category { Name = "Бонистика" });
            db.Categories.Add(new Category { Name = "Бытовая техника" });
            db.Categories.Add(new Category { Name = "Детский мир" });
            db.Categories.Add(new Category { Name = "Домашние животные" });
            db.Categories.Add(new Category { Name = "Редкие книги" });
            db.Categories.Add(new Category { Name = "Антиквариат" });
            db.SaveChanges();
            base.Seed(db);
        }
    }
}
