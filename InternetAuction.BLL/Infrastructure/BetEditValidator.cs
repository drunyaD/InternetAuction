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
    public class BetEditValidator : BetValidator, IBetEditValidator
    {
        public BetEditValidator(IUnitOfWork database) : base(database)
        {
            RuleFor(bet=> bet.Id).Must((id) => {
                Bet bet = database.Bets.Get(id);
                return bet != null;
            }).WithMessage("No bet exists with such id");
        }
    }
}
