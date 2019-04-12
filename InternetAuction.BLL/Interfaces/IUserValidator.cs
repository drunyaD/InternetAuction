using FluentValidation;
using FluentValidation.Validators;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface IUserValidator: IValidator<UserDto>
    {
        void ReconcileWithDb(UserDto user, CustomContext context);
    }
}
