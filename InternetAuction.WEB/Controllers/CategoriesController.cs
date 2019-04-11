using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Infrastructure;
using FluentValidation;
using System.Web;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class CategoriesController : ApiController
    {
        IAuctionService service;
        ICategoryValidator createValidator;
        ICategoryEditValidator editValidator;
        public CategoriesController(IAuctionService serv, ICategoryValidator createV, ICategoryEditValidator editV)
        {
            service = serv;
            createValidator = createV;
            editValidator = editV;
        }
        [AllowAnonymous]
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
            
        }
        [AllowAnonymous]
        public HttpResponseMessage GetCategories()
        {
            
            IEnumerable<CategoryDTO> categories = service.GetAllCategories();
            return Request.CreateResponse(HttpStatusCode.OK, categories);
            
        }
        [Authorize(Roles ="administrator, moderator")]
        [HttpPost]
        public HttpResponseMessage CreateCategory([FromBody]CategoryDTO categoryDTO)
        {       
            var validResult = createValidator.Validate(categoryDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }   
            service.CreateCategory(categoryDTO);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, categoryDTO);
            return response;       
        }
        [Authorize(Roles ="administrator, moderator")]
        [HttpPut]
        public HttpResponseMessage ChangeCategory([FromBody]CategoryDTO categoryDTO)
        {

            var validResult = editValidator.Validate(categoryDTO);
            if (!validResult.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            }
            service.EditCategory(categoryDTO);
            return Request.CreateResponse(HttpStatusCode.OK);
            
        }
        [Authorize(Roles ="administrator, moderator")]
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
        [AllowAnonymous]
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
