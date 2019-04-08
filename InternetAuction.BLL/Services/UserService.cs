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

namespace InternetAuction.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }


        public async Task<IdentityResult> RegisterUser(UserDTO userDTO)
        {
            User user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email
            };
            var result = await Database.UserManager.CreateAsync(user, userDTO.Password);
            Profile clientProfile = new Profile { Id = user.Id, Name = userDTO.Name };
            Database.ProfileManager.Create(clientProfile);
            Database.Save();
            return result;
        }

        public async Task<UserDTO> FindUser(string userName, string password)
        {
            User user = await Database.UserManager.FindAsync(userName, password);
            if (user == null) throw new ArgumentException("no user exists with such id");
            return new UserDTO {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = Database.ProfileManager.Get(user.Id).Name,
                Password = password
            };
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        /*
        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            User user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new User { Email = userDto.Email, UserName = userDto.UserName };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                Profile clientProfile = new Profile { Id = user.Id, Name = userDto.Name };
                Database.ProfileManager.Create(clientProfile);
                Database.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            User user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new Role { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }*/
    }
}
