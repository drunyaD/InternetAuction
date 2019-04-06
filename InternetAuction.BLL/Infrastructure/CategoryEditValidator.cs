using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;
using FluentValidation;
using InternetAuction.BLL.Interfaces;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Infrastructure
{
    public class CategoryEditValidator : CategoryValidator, ICategoryEditValidator
    {
        public CategoryEditValidator(IUnitOfWork database) : base(database)
        {
            RuleFor(category => category.Id).Must((id) => {
                Category category = database.Categories.Get(id);
                return category != null;
            }).WithMessage("No category exists with such id");          
        }
        public override bool HaveUniqueName(CategoryDTO category) { return true; }
    }
}
