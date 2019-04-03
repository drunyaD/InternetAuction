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
using System.Web.Mvc;
using System.Security.Claims;

namespace InternetAuction.WEB.Controllers
{
    public class AccountsController : ApiController
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<IUserService>();
            }
        }

    }
}
