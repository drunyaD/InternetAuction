using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Infrastructure;
using InternetAuction.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FluentValidation;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class BetsController : ApiController
    {
        IAuctionService service;
        IBetValidator createValidator;
        IBetEditValidator editValidator;
        public BetsController(IAuctionService serv, IBetValidator createV, IBetEditValidator editV)
        {
            service = serv;
            createValidator = createV;
            editValidator = editV;
        }
        [AllowAnonymous]
        public HttpResponseMessage GetBet(int betId)
        {
            try
            {
                BetDTO bet = service.GetBet(betId);
                return Request.CreateResponse(HttpStatusCode.OK, bet);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

        }
        [AllowAnonymous]
        public HttpResponseMessage GetAllBets()
        {
           
           IEnumerable<BetDTO> bets = service.GetAllBets();
           return Request.CreateResponse(HttpStatusCode.OK, bets);
            
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        public HttpResponseMessage CreateBet([FromBody]BetDTO betDTO)
        {
            var validResult = createValidator.Validate(betDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }
            service.CreateBet(betDTO);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, betDTO);
            return response;
        }

        [HttpPut]
        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage ChangeBet([FromBody]BetDTO betDTO)
        {
            var validResult = editValidator.Validate(betDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }
            service.EditBet(betDTO);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage DeleteBet(int betId)
        {
            try
            {
                service.DeleteBet(betId);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
