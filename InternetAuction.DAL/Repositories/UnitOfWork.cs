﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.EF;
using InternetAuction.DAL.Entities;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InternetAuction.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private AuctionContext _db;
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

        public IRepository<Bet> Bets
        {
            get
            {
                if (_betRepository == null)
                    _betRepository = new Repository<Bet>(_db);
                return _betRepository;
            }
        }

        public IRepository<Category> Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new Repository<Category>(_db);
                return _categoryRepository;
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                if (_imageRepository == null)
                    _imageRepository = new Repository<Image>(_db);
                    return _imageRepository;
            }
        }

            public IRepository<Lot> Lots
            {
                get
                {
                    if (_lotRepository == null)
                        _lotRepository = new Repository<Lot>(_db);
                    return _lotRepository;
                }
            }


        public AppUserManager UserManager
        {

            get
            {
                if (_userManager == null)
                    _userManager = new AppUserManager(new UserStore<User>(_db));
                return _userManager;
            }
        }


        public AppRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = new AppRoleManager(new RoleStore<Role>(_db));
                return _roleManager;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }


        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                    _userManager.Dispose();
                    _roleManager.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
