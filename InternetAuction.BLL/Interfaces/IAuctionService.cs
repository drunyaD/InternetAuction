using System.Collections.Generic;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface IAuctionService
    {
        void CreateLot(LotDto lotDto);
        void CreateCategory(CategoryDto categoryDto);
        void CreateBet(BetDto betDto);
        void DeleteLot(int lotId);
        void DeleteCategory(int categoryId);
        void DeleteBet(int betId);
        void EditLot(LotDto lotDto);
        void EditCategory(CategoryDto categoryDto);
        IEnumerable<LotDto> GetAllLots();
        IEnumerable<LotDto> GetLotsByCategory(int categoryId);
        IEnumerable<BetDto> GetBetsByLot(int lotId);
        IEnumerable<BetDto> GetAllBets();
        IEnumerable<CategoryDto> GetAllCategories();
        BetDto GetBet(int betId);
        CategoryDto GetCategory(int categoryId);
        LotDto GetLot(int lotId);
        void Dispose();
    }
}
