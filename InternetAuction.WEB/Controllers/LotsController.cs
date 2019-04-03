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
    public class LotsController : ApiController
    {
        IAuctionService service;
        public LotsController(IAuctionService serv)
        {
            service = serv;
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
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        public HttpResponseMessage GetLots()
        {
            try
            {
                IEnumerable<LotDTO> lots = service.GetAllLots();
                return Request.CreateResponse(HttpStatusCode.OK, lots);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateLot([FromBody]LotDTO lotDTO)
        {
            try
            {
                service.CreateLot(lotDTO);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, lotDTO);
                response.Headers.Location = new Uri("/api/lots/" + lotDTO.Id);
                return response;
            }
            catch(ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangeLot([FromBody]LotDTO lotDTO)
        {
            try
            {
                service.EditLot(lotDTO);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
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
