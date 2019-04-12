using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.DTO;

namespace InternetAuction.WEB.Controllers
{
    [Authorize]
    public class CategoriesController : ApiController
    {
        private IAuctionService Service { get; }
        private ICategoryValidator CreationValidator { get; }
        private ICategoryEditValidator EditingValidator { get; }
        public CategoriesController(IAuctionService service, 
            ICategoryValidator creationV, ICategoryEditValidator editingV)
        {
            Service = service;
            CreationValidator = creationV;
            EditingValidator = editingV;
        }
        [AllowAnonymous]
        public HttpResponseMessage GetCategory(int categoryId)
        {   
            try
            {
                var category = Service.GetCategory(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, category);
            }
            catch (ArgumentException e) {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            
        }
        [AllowAnonymous]
        public HttpResponseMessage GetCategories()
        {
            
            var categories = Service.GetAllCategories();
            return Request.CreateResponse(HttpStatusCode.OK, categories);
            
        }
        [Authorize(Roles ="administrator, moderator")]
        [HttpPost]
        public HttpResponseMessage CreateCategory([FromBody]CategoryDto categoryDto)
        {       
            var validResult = CreationValidator.Validate(categoryDto);
            if (!validResult.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            Service.CreateCategory(categoryDto);
            var response = Request.CreateResponse(HttpStatusCode.Created, categoryDto);
            return response;       
        }
        [Authorize(Roles ="administrator, moderator")]
        [HttpPut]
        public HttpResponseMessage ChangeCategory([FromBody]CategoryDto categoryDto)
        {

            var validResult = EditingValidator.Validate(categoryDto);
            if (!validResult.IsValid) return Request.CreateResponse(HttpStatusCode.BadRequest, validResult.Errors);
            Service.EditCategory(categoryDto);
            return Request.CreateResponse(HttpStatusCode.OK);
            
        }
        [Authorize(Roles ="administrator, moderator")]
        public HttpResponseMessage DeleteCategory(int id)
        {
            try
            {
                Service.DeleteCategory(id);
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
                var lots = Service.GetLotsByCategory(categoryId);
                return Request.CreateResponse(HttpStatusCode.OK, lots);
            }
            catch(ArgumentException e)
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
