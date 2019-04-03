using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Infrastructure;

namespace InternetAuction.WEB.Controllers
{
    public class СategoriesController : ApiController
    {
        IAuctionService service;
        public СategoriesController(IAuctionService serv)
        {
            service = serv;
        }

        public HttpResponseMessage GetCategory(int categoryId)
        {
            try
            {
                CategoryDTO category = service.GetCategory(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            catch (ArgumentException e) {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }

        public HttpResponseMessage GetCategories()
        {
            try
            {
                IEnumerable<CategoryDTO> categories = service.GetAllCategories();
                return Request.CreateResponse(HttpStatusCode.OK, categories);
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateCategory([FromBody]CategoryDTO categoryDTO)
        {
            try
            {
                service.CreateCategory(categoryDTO);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, categoryDTO);
                response.Headers.Location = new Uri("/api/categories/" + categoryDTO.Id);
                return response;

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "this category already exists");
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangeCategory([FromBody]CategoryDTO categoryDTO)
        {
            try
            {
                service.EditCategory(categoryDTO);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }

        public HttpResponseMessage DeleteCategory(int id)
        {
            try
            {
                service.DeleteCategory(id);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch(ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }

        [Route("api/categories/{id}/lots")]
        public HttpResponseMessage GetLotsByCategory(int categoryId)
        {
            try
            {
                var lots = service.GetLotsByCategory(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, lots);
            }
            catch(ArgumentException e)
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
