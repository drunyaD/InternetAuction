using FluentValidation;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface ICategoryValidator: IValidator<CategoryDto>
    {
        bool HaveUniqueName(CategoryDto category);
    }
}
