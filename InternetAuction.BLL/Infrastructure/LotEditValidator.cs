using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;
using FluentValidation;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
    public class LotEditValidator : LotValidator, ILotEditValidator
    {
        public LotEditValidator(IUnitOfWork database) : base(database)
        {
            RuleFor(lot => lot.Id).Must((id) => {
                Lot lot = database.Lots.Get(id);
                return lot != null;
            }).WithMessage("No lot exists with such id");
        }
    }
}
