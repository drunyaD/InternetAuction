using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Infrastructure;
using InternetAuction.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InternetAuction.WEB.Controllers
{
    public class BetsController : ApiController
    {
        IAuctionService service;
        public BetsController(IAuctionService serv)
        {
            service = serv;
        }

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
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        public HttpResponseMessage GetAllBets()
        {
            try
            {
                IEnumerable<BetDTO> bets = service.GetAllBets();
                return Request.CreateResponse(HttpStatusCode.OK, bets);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateBet([FromBody]BetDTO betDTO)
        {
            try
            {
                service.CreateBet(betDTO);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, betDTO);
                response.Headers.Location = new Uri("/api/bets/" + betDTO.Id);
                return response;

            }
            catch(ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangeBet([FromBody]BetDTO betDTO)
        {
            try
            {
                service.EditBet(betDTO);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }

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
