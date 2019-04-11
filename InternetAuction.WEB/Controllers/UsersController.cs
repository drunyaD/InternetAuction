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
using InternetAuction.BLL.Infrastructure;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private IUserService userService;
        private IAuthenticationManager authenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public UsersController(IUserService service)
        {
            userService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Autentification")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO {UserName = model.UserName, Password = model.Password };
                ClaimsIdentity claim = userService.Authenticate(userDto);
                if (claim == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"неверный логин или пароль");
                }
                else
                {
                    authenticationManager.SignOut();
                    authenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        
        [Route("api/Autentification")]
        [HttpDelete]
        [AllowAnonymous]
        public IHttpActionResult Logout()
        {
            authenticationManager.SignOut();
            return Ok();
        }

        [HttpPost]
        [Route("api/Users")]
        [AllowAnonymous]
        public HttpResponseMessage Register([FromBody]RegisterModel registerModel)
        {              
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "data is not valid");
            }
            try
            {
                userService.Create(new UserDTO
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    Password = registerModel.Password,
                    Role = "user"
                });
            }

            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPut]
        [Authorize(Roles = "administrator")]
        public HttpResponseMessage ChangeRole([FromUri]string userId, [FromBody]string roleName)
        {
            try
            {
                userService.ChangeRole(userId, roleName);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetUsers()
        {
            var users = userService.GetUsers();
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetUser(string userId)
        {
            UserDTO user;
            try
            {
                user = userService.GetUser(userId);
            }
            catch(ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userService.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SetInitialDataAsync()
        {
            userService.SetInitialData(new UserDTO
            {
                Email = "petr@mail.ru",
                UserName = "petr@mail.ru",
                Password = "adminpassword",
                Role = "administrator",
            }, new List<string> { "user", "administrator", "moderator", "bannedUser"});
        }


    }
}
