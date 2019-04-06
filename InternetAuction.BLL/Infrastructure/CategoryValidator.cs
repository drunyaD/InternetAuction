using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
  
    public class CategoryValidator: AbstractValidator<CategoryDTO>, ICategoryValidator
    {
        IUnitOfWork Database { get; set; }
        public CategoryValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(category => category.Name).Length(3, 50);
            RuleFor(category => category).Must(HaveUniqueName).WithMessage("Name must be unique").WithName("Name");
        }

        public virtual bool HaveUniqueName(CategoryDTO category)
        {
            var cat = Database.Categories.Find(c => c.Name == category.Name).FirstOrDefault();
            if (cat != null) return false;
            else return true;
        } 
    }
}
