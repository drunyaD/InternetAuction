using FluentValidation;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
   public interface ILotValidator: IValidator<LotDto>
    {
        bool HaveExistingCategory(int categoryId);
        bool HaveDuration(LotDto lotDto);
    }
}
