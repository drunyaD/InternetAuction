using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Infrastructure;
using InternetAuction.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace InternetAuction.BLL.Interfaces
{
    public interface IUserService: IDisposable
    {
        void Create(UserDTO userDto);
        ClaimsIdentity Authenticate(UserDTO userDto);
        void SetInitialData(UserDTO adminDto, List<string> roles);
        UserDTO GetUser(string UserId);
        IEnumerable<UserDTO> GetUsers();
        void ChangeRole(string userId, string roleName);
    }
}
