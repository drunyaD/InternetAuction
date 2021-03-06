﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface IUserService: IDisposable
    {
        void Create(UserDto userDto);
        ClaimsIdentity Authenticate(UserDto userDto);
        UserDto GetUser(string userId);
        IEnumerable<UserDto> GetUsers();
        void ChangeRole(string userId, string roleName);
    }
}
