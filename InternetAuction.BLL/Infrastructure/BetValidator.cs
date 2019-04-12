using System.Linq;
using FluentValidation;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using FluentValidation.Validators;
using System.Data.Entity;

namespace InternetAuction.BLL.Infrastructure
{
    public class BetValidator : AbstractValidator<BetDto>, IBetValidator
    {
        private IUnitOfWork Database { get; }

        public BetValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(bet => bet.Value).GreaterThan(0);
            RuleFor(bet => bet).Custom(ReconcileWithDb);
        }

        public void ReconcileWithDb(BetDto bet, CustomContext context)
        {
            var lot = Database.Lots.GetQuery()
                .Include(l => l.Bets)
                .Include(l => l.LotOwner)
                .FirstOrDefault(l => l.Id == bet.LotId);
            if (lot == null)
            {
                context.AddFailure("LotId", "No lot exists with such id");
            }
            else
            {
                if (lot.LotOwner.UserName == bet.UserName)
                {
                    context.AddFailure("UserName", "User can't make bet on his lot");
                }
                else
                {
                    if (bet.PlacingTime >= lot.FinishTime) context.AddFailure("PlacingTime", "Must not be expired");
                    if (lot.Bets == null)
                    {
                        if (bet.Value < lot.StartPrice) context.AddFailure("Value", "Too small bet");
                    }
                    else
                    {
                        var bets = lot.Bets.OrderBy(b => b.Value);
                        if (bet.Value <= bets.First().Value) context.AddFailure("Value", "Too small bet");
                    }
                }
            }
        }
    }
}
