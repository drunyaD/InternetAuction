using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using InternetAuction.WEB.Models;
using Microsoft.Owin.Security;

namespace InternetAuction.WEB.Controllers
{
    [System.Web.Http.Authorize]
    public class UsersController : ApiController
    {
        private IUserService Service { get; }
        private IUserValidator Validator { get; }
        private IAuthenticationManager authenticationManager => HttpContext.Current.GetOwinContext().Authentication;

        public UsersController(IUserService service, IUserValidator validator)
        {
            Service = service;
            Validator = validator;
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/authentication")]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto {UserName = model.UserName, Password = model.Password };
                var claim = Service.Authenticate(userDto);
                if (claim == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"wrong login or password");
                }

                authenticationManager.SignOut();
                authenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
        
        [System.Web.Http.Route("api/authentication")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Logout()
        {
            authenticationManager.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/users")]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Register([FromBody]RegisterModel registerModel)
        {
            var user = new UserDto
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                Password = registerModel.Password,
                Role = "user"
            };
            var result = Validator.Validate(user);
            if (!result.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest, result.Errors);
            Service.Create(user);
            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        [System.Web.Http.HttpPut]
        [System.Web.Http.Authorize(Roles = "administrator")]
        public HttpResponseMessage ChangeRole([FromUri]string userId, [FromBody]string roleName)
        {
            try
            {
                Service.ChangeRole(userId, roleName);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetUsers()
        {
            var users = Service.GetUsers();
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [System.Web.Http.Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetUser(string userId)
        {
            UserDto user;
            try
            {
                user = Service.GetUser(userId);
            }
            catch(ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) Service.Dispose();

            base.Dispose(disposing);
        }

        private void SetInitialDataAsync()
        {
            Service.SetInitialData(new UserDto
            {
                Email = "petr@mail.ru",
                UserName = "petr@mail.ru",
                Password = "adminpassword",
                Role = "administrator"
            }, new List<string> { "user", "administrator", "moderator", "bannedUser"});
        }
    }
}
