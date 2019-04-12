using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class LotsController : ApiController
    {
        private IAuctionService Service { get; }
        private ILotValidator CreationValidator { get; }
        private ILotEditValidator EditingValidator { get; }
        public LotsController(IAuctionService service, ILotValidator createV, ILotEditValidator editingV)
        {
            Service = service;
            CreationValidator = createV;
            EditingValidator = editingV;
        }
        [AllowAnonymous]
        public HttpResponseMessage GetLot(int lotId)
        {
            try
            {
                var lot = Service.GetLot(lotId);
                return Request.CreateResponse(HttpStatusCode.OK, lot);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

        }
        [AllowAnonymous]
        public HttpResponseMessage GetLots()
        {
            
            var lots = Service.GetAllLots();
            return Request.CreateResponse(HttpStatusCode.OK, lots);
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        public HttpResponseMessage CreateLot([FromBody]LotDto lotDto)
        {
            lotDto.OwnerName = HttpContext.Current.User.Identity.Name;
            lotDto.StartTime = DateTime.Now;
            var validResult = CreationValidator.Validate(lotDto);
            if (!validResult.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            Service.CreateLot(lotDto);
            var response = Request.CreateResponse(HttpStatusCode.Created, lotDto);
            return response;
        }

        [HttpPut]
        [Authorize(Roles ="administrator, moderator")]
        public HttpResponseMessage ChangeLot([FromBody]LotDto lotDto)
        {
            var validResult = EditingValidator.Validate(lotDto);
            if (!validResult.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            Service.EditLot(lotDto);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [Authorize(Roles ="administrator, moderator")]
        public HttpResponseMessage DeleteLot(int id)
        {
            try
            {
                Service.DeleteLot(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }
        [AllowAnonymous]
        [Route("api/lots/{id}/bets")]
        public HttpResponseMessage GetBetsByLot(int lotId)
        {
            try
            {
                var bets = Service.GetBetsByLot(lotId);
                return Request.CreateResponse(HttpStatusCode.OK, bets);
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
