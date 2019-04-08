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
        Task<IdentityResult> RegisterUser(UserDTO userDTO);
        Task<UserDTO> FindUser(string userName, string password);
    }
}
