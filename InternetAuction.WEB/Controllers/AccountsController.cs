using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using InternetAuction.BLL.Interfaces;
using System.Web;
using System.Security.Claims;
using InternetAuction.BLL.DTO;
using System.Threading.Tasks;
using InternetAuction.WEB.Models;
using Microsoft.AspNet.Identity;

namespace InternetAuction.WEB.Controllers
{
    public class AccountController : ApiController
    {
        private IUserService userService;

        public AccountController(IUserService service)
        {
            userService = service;
        }

        [AllowAnonymous]
        [Route("api/Accounts/Register")]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await userService.RegisterUser(new UserDTO
            {Name = registerModel.Name,
            UserName = registerModel.UserName,
            Email = registerModel.Email,
            Password = registerModel.Password,
            Role = "user"
            });

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userService.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
