using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using InternetAuction.DAL.Entities;
using FluentValidation.Validators;
using System.Data.Entity;

namespace InternetAuction.BLL.Infrastructure
{
    public class BetValidator: AbstractValidator<BetDTO>, IBetValidator
    {
        IUnitOfWork Database { get; set; }
        public BetValidator(IUnitOfWork database)
        {
            Database = database;

            RuleFor(bet => bet.Value).GreaterThan(0);

            RuleFor(bet => bet).Custom(ReconcileWithDb);
        }

       public void ReconcileWithDb(BetDTO bet, CustomContext context)
        {
            var lot = Database.Lots
                .GetQuery()
                .Include(l => l.Bets)
                .FirstOrDefault(l => l.Id == bet.LotId);
            if (lot == null)
            {
                context.AddFailure("LotId", "No lot exists with such id");
            }
            else
            {
                if (bet.PlacingTime >= lot.FinishTime)
                {
                    context.AddFailure("PlacingTime", "Must not be expired");
                }

                var lastBet = lot.Bets.Where(b => b.Value == lot.Bets.Max(Bet => Bet.Value)).First();

                if (bet.Value <= lastBet.Value)
                {
                    context.AddFailure("Value", "Too small bet");
                }
            }
        }
    }
}
