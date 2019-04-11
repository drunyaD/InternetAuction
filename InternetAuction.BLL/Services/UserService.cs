using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using InternetAuction.BLL.Infrastructure;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace InternetAuction.BLL.Services
{
    public class UserService : IUserService
    {
        //static IMapper usertoDTO = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()
        //.ForMember(u => u.Role, u => u.MapFrom(e => e.Roles.First()))).CreateMapper();

        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(UserDTO userDto)
        {
            User user = Database.UserManager.FindByEmail(userDto.Email);
            if (user == null)
            {
                user = new User { Email = userDto.Email, UserName = userDto.UserName };
                var result = Database.UserManager.Create(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    throw new Exception(result.Errors.FirstOrDefault());
                // добавляем роль
                Database.UserManager.AddToRole(user.Id, userDto.Role);
                Database.Save();
            }
            else
            {
                throw new Exception("this user already exists");
            }
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            User user = Database.UserManager.Find(userDto.UserName, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = Database.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public void SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = Database.RoleManager.FindByName(roleName);
                if (role == null)
                {
                    role = new Role { Name = roleName };
                    Database.RoleManager.Create(role);
                }
            }
            Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public UserDTO GetUser(string UserId)
        {
            User user = Database.UserManager.FindById(UserId);
            if (user == null) throw new ArgumentException("no user exists with such id");
            return Mapper.Map<User, UserDTO>(user);          
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            return Mapper.Map<IQueryable<User>, IEnumerable<UserDTO>>(Database.UserManager.Users);
        }

        public void ChangeRole(string userId, string roleName)
        {
            User user = Database.UserManager.FindById(userId);
            if (user == null) throw new ArgumentException("No user exists with such id");
            Role role = Database.RoleManager.FindByName(roleName);
            if (role == null) throw new ArgumentException("No role exists with such id");

            Database.UserManager.RemoveFromRole(userId, Database.RoleManager.Roles.First().Name);
            Database.UserManager.AddToRole(userId, roleName);
        }
    }
}
