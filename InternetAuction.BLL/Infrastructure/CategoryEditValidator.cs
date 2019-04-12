using InternetAuction.DAL.Interfaces;
using FluentValidation;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Infrastructure
{
    public class CategoryEditValidator : CategoryValidator, ICategoryEditValidator
    {
        public CategoryEditValidator(IUnitOfWork database) : base(database)
        {
            RuleFor(category => category.Id).Must(id => {
                var category = database.Categories.Get(id);
                return category != null;
            }).WithMessage("No category exists with such id");          
        }
        public override bool HaveUniqueName(CategoryDto category) { return true; }
    }
}
