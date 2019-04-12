using InternetAuction.DAL.Interfaces;
using FluentValidation;
using InternetAuction.BLL.Interfaces;

namespace InternetAuction.BLL.Infrastructure
{
    public class LotEditValidator : LotValidator, ILotEditValidator
    {
        public LotEditValidator(IUnitOfWork database) : base(database)
        {
            RuleFor(lot => lot.Id).Must(id => {
                var lot = database.Lots.Get(id);
                return lot != null;
            }).WithMessage("No lot exists with such id");
        }
    }
}
