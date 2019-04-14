using System;
using System.Collections.Generic;
using AutoMapper;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace InternetAuction.BLL.Services
{

    public class AuctionService : IAuctionService
    {
        private IUnitOfWork Database { get; }

        public AuctionService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void CreateBet(BetDto betDto)
        {
            var lot = Database.Lots.Get(betDto.LotId);
            var user = Database.UserManager.FindByName(betDto.UserName);
            Database.Bets.Create(new Bet
            {
                Value = betDto.Value,
                PlacingTime = betDto.PlacingTime,
                LotId = betDto.LotId,
                Lot = lot,
                UserId = user.Id,
                User = user
            });
            Database.Save();
        }

        public void CreateCategory(CategoryDto categoryDto)
        {
            Database.Categories.Create(new Category {Name = categoryDto.Name});
            Database.Save();
        }

        public void CreateLot(LotDto lotDto)
        {
            var category = Database.Categories.Get(lotDto.CategoryId);
            var owner = Database.UserManager.FindByName(lotDto.OwnerName);
            var lot = new Lot
            {
                Name = lotDto.Name,
                Description = lotDto.Description,
                StartTime = lotDto.StartTime,
                FinishTime = lotDto.FinishTime,
                StartPrice = lotDto.StartPrice,
                OwnerId = owner.Id,
                LotOwner = owner,
                CategoryId = lotDto.CategoryId,
                Category = category
            };
            foreach (var imageDto in lotDto.Images)
            {
                var image = new Image {Picture = imageDto.Picture, LotId = lotDto.Id, Lot = lot};
                Database.Images.Create(image);
                lot.Images.Add(image);
            }

            Database.Lots.Create(lot);
            Database.Save();
        }

        public void DeleteBet(int betId)
        {
            var bet = Database.Bets.Get(betId);
            if (bet == null) throw new ArgumentException("no bet exists with such id");
            Database.Bets.Delete(bet);
            Database.Save();
        }

        public void DeleteCategory(int categoryId)
        {
            var category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("no category exists with such id");
            Database.Categories.Delete(category);
            Database.Save();
        }

        public void DeleteLot(int lotId)
        {
            var lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("no lot exists with such id");
            Database.Lots.Delete(lot);
            Database.Save();
        }

        public void EditCategory(CategoryDto categoryDto)
        {
            Database.Categories.Update(new Category {Id = categoryDto.Id, Name = categoryDto.Name});
            Database.Save();
        }

        public void EditLot(LotDto lotDto)
        {
            var category = Database.Categories.Get(lotDto.CategoryId);
            var owner = Database.UserManager.FindByName(lotDto.OwnerName);
            var lot = new Lot
            {
                Id = lotDto.Id,
                Name = lotDto.Name,
                Description = lotDto.Description,
                StartTime = lotDto.StartTime,
                FinishTime = lotDto.FinishTime,
                StartPrice = lotDto.StartPrice,
                OwnerId = owner.Id,
                LotOwner = owner,
                CategoryId = lotDto.CategoryId,
                Category = category
            };
            foreach (var i in Database.Images.Find(i => i.LotId == lotDto.Id)) Database.Images.Delete(i);
            foreach (var imageDto in lotDto.Images)
            {
                var newImage = new Image {Picture = imageDto.Picture, LotId = lotDto.Id, Lot = lot};
                Database.Images.Create(newImage);
                lot.Images.Add(newImage);
            }

            Database.Lots.Update(lot);
            Database.Save();
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(Database.Categories.GetAll());
        }

        public IEnumerable<LotDto> GetAllLots()
        {
            return Mapper.Map<IEnumerable<Lot>, IEnumerable<LotDto>>(Database.Lots.GetAll());
        }

        public BetDto GetBet(int betId)
        {
            var bet = Database.Bets.Get(betId);
            if (bet == null) throw new ArgumentException("no bet exists with such id");
            return Mapper.Map<Bet, BetDto>(bet);
        }

        public CategoryDto GetCategory(int categoryId)
        {
            var category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("no category exists with such id");
            return Mapper.Map<Category, CategoryDto>(category);
        }

        public LotDto GetLot(int lotId)
        {
            var lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("no lot exists with such id");
            return Mapper.Map<Lot, LotDto>(lot);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<LotDto> GetLotsByCategory(int categoryId)
        {
            var category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("no category exists with such id");
            return Mapper.Map<IEnumerable<Lot>, List<LotDto>>(Database.Lots.Find(l => l.CategoryId == categoryId));
        }

        public IEnumerable<BetDto> GetBetsByLot(int lotId)
        {
            var lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("no lot exists with such id");
            return Mapper.Map<IEnumerable<Bet>, IEnumerable<BetDto>>(Database.Bets.Find(b => b.LotId == lotId));
        }

        public IEnumerable<BetDto> GetAllBets()
        {
            return Mapper.Map<IEnumerable<Bet>, IEnumerable<BetDto>>(Database.Bets.GetAll());
        }
    }
}
