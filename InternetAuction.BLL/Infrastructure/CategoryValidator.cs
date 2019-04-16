using System.Linq;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Interfaces;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
  
    public class CategoryValidator: AbstractValidator<CategoryDto>, ICategoryValidator
    {
        private IUnitOfWork Database { get; }
        public CategoryValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(category => category.Name).NotNull().Length(3, 200);
            RuleFor(category => category).Must(HaveUniqueName)
                .WithMessage("Name must be unique").WithName("Name");
        }

        public virtual bool HaveUniqueName(CategoryDto category)
        {
            var cat = Database.Categories.Find(c => c.Name == category.Name).FirstOrDefault();
            return cat == null;
        } 
    }
}
