using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetAuction.BLL.DTO;

namespace InternetAuction.BLL.Interfaces
{
    public interface IAuctionService
    {
        void CreateLot(LotDTO lotDTO);
        void CreateCategory(CategoryDTO categoryDTO);
        void CreateBet(BetDTO betDTO);
        void DeleteLot(int lotId);
        void DeleteCategory(int categoryId);
        void DeleteBet(int betId);
        void EditLot(LotDTO lotDTO);
        void EditBet(BetDTO betDTO);
        void EditCategory(CategoryDTO categoryDTO);
        IEnumerable<LotDTO> GetAllLots();
        IEnumerable<LotDTO> GetLotsByCategory(int categoryId);
        IEnumerable<BetDTO> GetBetsByLot(int lotId);
        IEnumerable<BetDTO> GetAllBets();
        IEnumerable<CategoryDTO> GetAllCategories();
        BetDTO GetBet(int betId);
        CategoryDTO GetCategory(int categoryId);
        ImageDTO GetImage(int imageId);
        LotDTO GetLot(int lotId);
        void Dispose();
    }
}
