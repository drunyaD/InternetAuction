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
    public class LotValidator: AbstractValidator<LotDTO>, ILotValidator
    {
        IUnitOfWork Database;
        public LotValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(lot => lot.Name).Length(3, 80);
            RuleFor(lot => lot.Description).Length(0, 1500);
            RuleFor(lot => lot).Must(HaveDuration).WithMessage("Finish Time less then Start Time");
            RuleFor(lot => lot.StartPrice).GreaterThan(0);
            RuleFor(lot => lot.CategoryId).Must(HaveExistingCategory).WithMessage("No category exists with such id");
            RuleForEach(lot => lot.Images).SetValidator(new ImageValidator(database));
        }

        public bool HaveExistingCategory(int categoryId)
        {
            Category category = Database.Categories.Get(categoryId);
            return category != null;
        }

        public bool HaveDuration(LotDTO lotDTO)
        {
            return lotDTO.StartTime < lotDTO.FinishTime;
        }
    }
}
