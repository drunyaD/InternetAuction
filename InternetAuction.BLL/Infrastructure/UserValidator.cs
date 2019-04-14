using FluentValidation;
using FluentValidation.Validators;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace InternetAuction.BLL.Infrastructure
{
   public class UserValidator : AbstractValidator<UserDto>, IUserValidator
    {
        private IUnitOfWork Database { get; }
        public UserValidator(IUnitOfWork database)
        {
            Database = database;
            RuleFor(user => user.Email).MinimumLength(3).MaximumLength(254).EmailAddress();
            RuleFor(user => user.UserName).MinimumLength(3).MaximumLength(50);
            RuleFor(user => user.Password).MinimumLength(8).MaximumLength(50);
            RuleFor(user => user).Custom(ReconcileWithDb);
        }

        public void ReconcileWithDb(UserDto user, CustomContext context)
        {
            var userByName = Database.UserManager.FindByName(user.UserName);
            if (userByName != null) context.AddFailure("UserName", "User with such name already exists");
            var userByEmail = Database.UserManager.FindByEmail(user.Email);
            if (userByEmail != null) context.AddFailure("Email", "User with such email already exists");
        }
    }
}
