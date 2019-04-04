﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using InternetAuction.BLL.Interfaces;
using InternetAuction.DAL.Interfaces;
using InternetAuction.BLL.Infrastructure;
namespace InternetAuction.BLL.Services
{
    public class AuctionService : IAuctionService
    {
        static IMapper imagetoDTO = new MapperConfiguration(cfg => cfg.CreateMap<Image, ImageDTO>()).CreateMapper();
        static IMapper betToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Bet, BetDTO>()).CreateMapper();
        static IMapper categoryToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
        static IMapper lotToDTO = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotDTO>()
        .ForMember(lot => lot.Images, lot => lot.MapFrom(e => imagetoDTO.Map<IEnumerable<Image>, IEnumerable<ImageDTO>>(e.Images)))
        .ForMember(lot => lot.Bets, lot => lot.MapFrom(e => imagetoDTO.Map<IEnumerable<Bet>, IEnumerable<BetDTO>>(e.Bets)))).CreateMapper();

        IUnitOfWork Database { get; set; }

        public AuctionService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async void CreateBet(BetDTO betDTO)
        {
            Lot lot = Database.Lots.Get(betDTO.LotId);
            User user = await Database.UserManager.FindByIdAsync(betDTO.UserId);
            Bet lastBet = lot.Bets.Where(b => b.Value == lot.Bets.Max(bet => bet.Value)).First();
            if (user == null) throw new ValidationException("no user with such id","UserId");
            if (lot == null) throw new ValidationException("no lot with such id","LotId");
            if (betDTO.PlacingTime > lot.FinishTime) throw new ValidationException("time is over", "PlacingTime");
            if (betDTO.Value < 0) throw new ValidationException("value < 0","Value");
            if (lastBet != null && betDTO.Value <= lastBet.Value) throw new ValidationException("too small value","Value");
            Database.Bets.Create(new Bet
            {
                Value = betDTO.Value,
                PlacingTime = betDTO.PlacingTime,
                LotId = betDTO.LotId,
                Lot = lot,
                UserId = betDTO.UserId,
                User = user
            });
            Database.Save();
        }

        public void CreateCategory(CategoryDTO categoryDTO)
        {
            Database.Categories.Create(new Category {Name = categoryDTO.Name});
        }

        public async void CreateLot(LotDTO lotDTO)
        {
            DateTime nowTime = DateTime.Now;
            if (lotDTO.FinishTime < nowTime) throw new ValidationException("finishTime < current time", "FinishTime");
            if (lotDTO.StartPrice < 0) throw new ValidationException("startprice < 0", "StartPrice");
            Category category = Database.Categories.Get(lotDTO.CategoryId);
            if (category == null) throw new ValidationException("no category with such id", "Category");
            User owner =  await Database.UserManager.FindByIdAsync(lotDTO.OwnerId);
            if (owner == null) throw new ValidationException("no user with such id", "OwnerId");
            Lot lot = new Lot()
            {
                Name = lotDTO.Name,
                Description = lotDTO.Description,
                StartTime = nowTime,
                FinishTime = lotDTO.FinishTime,
                StartPrice = lotDTO.StartPrice,
                OwnerId = lotDTO.OwnerId,
                LotOwner = owner,
                CategoryId = lotDTO.CategoryId,
                Category = category
            };
            foreach (var imageDTO in lotDTO.Images)
            {
                Image image = new Image
                {
                    Picture = imageDTO.Picture,
                    LotId = lotDTO.Id,
                    Lot = lot
                };
                Database.Images.Create(image);
                lot.Images.Add(image);
            }
            Database.Lots.Create(lot);
            Database.Save();
        }

        public void DeleteBet(int betId)
        {
            Bet bet = Database.Bets.Get(betId);
            if (bet == null) throw new ArgumentException("no bet with such id");
            Database.Bets.Delete(betId);
            Database.Save();
        }

        public void DeleteCategory(int categoryId)
        {
            Category category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("no category with such id");
            Database.Categories.Delete(categoryId);
            Database.Save();
        }

        public void DeleteLot(int lotId)
        {
            Lot lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("no lot with such id");
            Database.Lots.Delete(lotId);
            Database.Save();
        }

        public async void EditBet(BetDTO betDTO)
        {
            Bet bet = Database.Bets.Get(betDTO.Id);
            if (bet == null)
                throw new ValidationException("No bet with such id", "Id");
            Lot lot = Database.Lots.Get(betDTO.LotId);
            User user = await Database.UserManager.FindByIdAsync(betDTO.UserId);
            Bet lastBet = lot.Bets.Where(b => b.Value == lot.Bets.Max(B => B.Value)).First();
            if (user == null) throw new ValidationException("no user with such id", "UserId");
            if (lot == null) throw new ValidationException("no lot with such id", "LotId");
            if (betDTO.PlacingTime > lot.FinishTime) throw new ValidationException("time is over", "PlacingTime");
            if (betDTO.Value < 0) throw new ValidationException("value < 0", "Value");
            if (lastBet != null && betDTO.Value <= lastBet.Value) throw new ValidationException("too small value", "Value");
            Bet editedBet = new Bet
            {
                Id = betDTO.Id,
                Value = betDTO.Value,
                PlacingTime = betDTO.PlacingTime,
                LotId = betDTO.LotId,
                Lot = lot,
                UserId = betDTO.UserId,
                User = user
            };
            Database.Bets.Update(editedBet);
            Database.Save();
        }

        public void EditCategory(CategoryDTO categoryDTO)
        {
            Category category = Database.Categories.Get(categoryDTO.Id);
            if (category == null)
                throw new ValidationException("no category with such id", "Id");
            Database.Categories.Update(new Category { Id = categoryDTO.Id, Name = categoryDTO.Name });
            Database.Save();
        }

        public async void EditLot(LotDTO lotDTO)
        {
            Lot lot = Database.Lots.Get(lotDTO.Id);
            if (lot == null)
                throw new ValidationException("no lot with such id", "Id");
            DateTime nowTime = DateTime.Now;
            if (lotDTO.FinishTime < nowTime) throw new ValidationException("finishTime < current time", "FinishTime");
            if (lotDTO.StartPrice < 0) throw new ValidationException("startprice < 0", "StartPrice");
            Category category = Database.Categories.Get(lotDTO.CategoryId);
            if (category == null) throw new ValidationException("no category with such id", "Category");
            User owner = await Database.UserManager.FindByIdAsync(lotDTO.OwnerId);
            if (owner == null) throw new ValidationException("no user with such id", "OwnerId");
            Lot editedLot = new Lot()
            {
                Id = lotDTO.Id,
                Name = lotDTO.Name,
                Description = lotDTO.Description,
                StartTime = nowTime,
                FinishTime = lotDTO.FinishTime,
                StartPrice = lotDTO.StartPrice,
                OwnerId = lotDTO.OwnerId,
                LotOwner = owner,
                CategoryId = lotDTO.CategoryId,
                Category = category
            };
            foreach (var i in Database.Images.Find(i => i.LotId == lotDTO.Id))
            {
                Database.Images.Delete(i.Id);
            }
            foreach (var imageDTO in lotDTO.Images)
            {
                Image newImage = new Image
                {
                    Picture = imageDTO.Picture,
                    LotId = lotDTO.Id,
                    Lot = lot
                };
                Database.Images.Create(newImage);
                lot.Images.Add(newImage);
            }
            Database.Lots.Update(lot);
            Database.Save();

        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            return categoryToDTO.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(
                Database
                .Categories
                .GetAll()
            );
        }

        public IEnumerable<LotDTO> GetAllLots()
        {
            return lotToDTO.Map<IEnumerable<Lot>, IEnumerable<LotDTO>>(
                Database
                .Lots
                .GetAll()
            );
        }

        public BetDTO GetBet(int betId)
        {
            var bet = Database.Bets.Get(betId);
            if (bet == null) throw new ArgumentException("no bet with such id");
            return betToDTO.Map<Bet, BetDTO>(bet);
        }

        public CategoryDTO GetCategory(int categoryId)
        {
            var category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("no category with such id");
            return categoryToDTO.Map<Category, CategoryDTO>(category);
        }

        public ImageDTO GetImage(int imageId)
        {
            var image = Database.Images.Get(imageId);
            if (image == null) throw new ArgumentException("no image with such id");
            return imagetoDTO.Map<Image, ImageDTO>(image);
        }

        public LotDTO GetLot(int lotId)
        {
            var lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("No lot with such id");
            return lotToDTO.Map<Lot, LotDTO>(lot);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<LotDTO> GetLotsByCategory(int categoryId)
        {
            var category = Database.Categories.Get(categoryId);
            if (category == null) throw new ArgumentException("No category with such id");
            return lotToDTO.Map<IEnumerable<Lot>, List<LotDTO>>(
                Database
                .Lots
                .Find(l => l.CategoryId == categoryId)
            );
        }

        public IEnumerable<BetDTO> GetBetsByLot(int lotId)
        {
            Lot lot = Database.Lots.Get(lotId);
            if (lot == null) throw new ArgumentException("No lot with such id");
            return betToDTO.Map<IEnumerable<Bet>, IEnumerable<BetDTO>>(
                Database
                .Bets
                .Find(b => b.LotId == lotId)
                );
        }

        public IEnumerable<BetDTO> GetAllBets()
        {
            return betToDTO.Map<IEnumerable<Bet>, IEnumerable<BetDTO>>(
                Database
                .Bets
                .GetAll()
            );
        }
    }
}
