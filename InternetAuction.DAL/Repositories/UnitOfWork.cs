using System;
using InternetAuction.DAL.EF;
using InternetAuction.DAL.Entities;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;

namespace InternetAuction.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly AuctionContext _db;
        private Repository<Bet> _betRepository;
        private Repository<Category> _categoryRepository;
        private Repository<Image> _imageRepository;
        private Repository<Lot> _lotRepository;
        private AppUserManager _userManager;
        private AppRoleManager _roleManager;

        public UnitOfWork(string connectionString)
        {
            _db = new AuctionContext(connectionString);
        }

        public IRepository<Bet> Bets => 
            _betRepository ?? (_betRepository = new Repository<Bet>(_db));

        public IRepository<Category> Categories =>
            _categoryRepository ?? (_categoryRepository = new Repository<Category>(_db));

        public IRepository<Image> Images => 
            _imageRepository ?? (_imageRepository = new Repository<Image>(_db));

        public IRepository<Lot> Lots => 
            _lotRepository ?? (_lotRepository = new Repository<Lot>(_db));

        public AppUserManager UserManager =>
            _userManager ?? (_userManager = new AppUserManager(new UserStore<User>(_db)));

        public AppRoleManager RoleManager =>
            _roleManager ?? (_roleManager = new AppRoleManager(new RoleStore<Role>(_db)));

        public void Save()
        {
            _db.SaveChanges();             
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed) return;
            if (disposing)
            {
                _db.Dispose();
                _userManager.Dispose();
                _roleManager.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
