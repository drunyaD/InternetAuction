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
    public class LotsController : ApiController
    {
        IAuctionService service;
        ILotValidator createValidator;
        ILotEditValidator editValidator;
        public LotsController(IAuctionService serv, ILotValidator createV, ILotEditValidator editV)
        {
            service = serv;
            createValidator = createV;
            editValidator = editV;
        }

        public HttpResponseMessage GetLot(int lotId)
        {
            try
            {
                LotDTO lot = service.GetLot(lotId);
                return Request.CreateResponse(HttpStatusCode.OK, lot);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

        }

        public HttpResponseMessage GetLots()
        {
            
                IEnumerable<LotDTO> lots = service.GetAllLots();
                return Request.CreateResponse(HttpStatusCode.OK, lots);
 
        }

        [HttpPost]
        public HttpResponseMessage CreateLot([FromBody]LotDTO lotDTO)
        {
            var validResult = createValidator.Validate(lotDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }
            service.CreateLot(lotDTO);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, lotDTO);
            return response;
        }

        [HttpPut]
        public HttpResponseMessage ChangeLot([FromBody]LotDTO lotDTO)
        {
            var validResult = editValidator.Validate(lotDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }
            service.EditLot(lotDTO);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage DeleteLot(int id)
        {
            try
            {
                service.DeleteLot(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }

        [Route("api/lots/{id}/bets")]
        public HttpResponseMessage GetBetsByLot(int lotId)
        {
            try
            {
                var bets = service.GetBetsByLot(lotId);
                return Request.CreateResponse(HttpStatusCode.OK, bets);
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
