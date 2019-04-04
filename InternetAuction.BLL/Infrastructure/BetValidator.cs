using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;

namespace InternetAuction.BLL.Infrastructure
{
    public class BetValidator: AbstractValidator<BetDTO>
    {
        IUnitOfWork Database { get; set; }
        public BetValidator(IUnitOfWork database)
        {
            Database = database;

            RuleFor(bet => bet.LotId).Must((id) =>
            {
                Lot lot = database.Lots.Get(id);
                return lot != null;
            }).WithMessage("No lot exists with such id");

            RuleFor(bet => bet).Must(NotBeExpired).WithMessage("Must not be expired");

            RuleFor(bet => bet.Value).GreaterThan(0);

            RuleFor(bet => bet).Must(HaveMaxValue).WithMessage("Too small value");
        }

        protected bool NotBeExpired(BetDTO bet)
        {
            Lot lot = Database.Lots.Get(bet.LotId);
            if ((lot != null) || (bet.PlacingTime < lot.FinishTime))
                return true;
            else
                return false;
            
        }

        protected bool HaveMaxValue(BetDTO bet)
        {
            Lot lot = Database.Lots.Get(bet.LotId);
            if (lot == null) return false;
            Bet lastBet = lot.Bets.Where(b => b.Value == lot.Bets.Max(Bet => Bet.Value)).First();
            if (lastBet != null && bet.Value <= lastBet.Value) return false;
            else return true;
        }
    }
}
