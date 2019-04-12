using FluentValidation;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface ICategoryEditValidator: IValidator<CategoryDto>
    {
    }
}
