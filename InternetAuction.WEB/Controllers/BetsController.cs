using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Runtime.CompilerServices;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class BetsController : ApiController
    {
        private IAuctionService Service { get; }
        private IBetValidator Validator { get; }
        public BetsController(IAuctionService service, IBetValidator validator)
        {
            Service = service;
            Validator = validator;
        }
        [AllowAnonymous]
        [Route("api/bets/{betId}")]
        public HttpResponseMessage GetBet(int betId)
        {
            try
            {
                var bet = Service.GetBet(betId);
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
           
           var bets = Service.GetAllBets();
           return Request.CreateResponse(HttpStatusCode.OK, bets);
            
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public HttpResponseMessage CreateBet([FromBody]BetDto betDto)
        {
            if (betDto == null || !ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Data is not full");
            betDto.UserName = HttpContext.Current.User.Identity.Name;
            betDto.PlacingTime = DateTime.Now;
            var validResult = Validator.Validate(betDto);
            if (!validResult.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            Service.CreateBet(betDto);
            return Request.CreateResponse(HttpStatusCode.Created, betDto);
        }
        [HttpDelete]
        [Route("api/bets/{betId}")]
        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage DeleteBet(int betId)
        {
            try
            {
                Service.DeleteBet(betId);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) Service.Dispose();
            base.Dispose(disposing);
        }
    }
}
