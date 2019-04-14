using System;
using System.Collections.Generic;
using System.Linq;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace InternetAuction.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(UserDto userDto)
        {
            var user = new User { Email = userDto.Email, UserName = userDto.UserName };
            Database.UserManager.Create(user, userDto.Password);
            Database.UserManager.AddToRole(user.Id, userDto.Role);
        }

        public ClaimsIdentity Authenticate(UserDto userDto)
        {
            ClaimsIdentity claim = null;
            var user = Database.UserManager.Find(userDto.UserName, userDto.Password);
            if (user != null)
                claim = Database.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public UserDto GetUser(string userId)
        {
            var user = Database.UserManager.FindById(userId);
            if (user == null) throw new ArgumentException("no user exists with such id");
            return Mapper.Map<User, UserDto>(user);          
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return Mapper.Map<IQueryable<User>, IEnumerable<UserDto>>(Database.UserManager.Users);
        }

        public void ChangeRole(string userId, string roleName)
        {
            var user = Database.UserManager.FindById(userId);
            if (user == null) throw new ArgumentException("No user exists with such id");
            var role = Database.RoleManager.FindByName(roleName);
            if (role == null) throw new ArgumentException("No role exists with such name");

            Database.UserManager.RemoveFromRole(userId, 
                Database.RoleManager.FindById(user.Roles.First().RoleId).Name);

            Database.UserManager.AddToRole(userId, roleName);
        }
    }
}
