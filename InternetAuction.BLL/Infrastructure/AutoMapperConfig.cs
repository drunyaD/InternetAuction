using AutoMapper;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace InternetAuction.BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Image, ImageDto>();
                config.CreateMap<Bet, BetDto>()
                    .ForMember(b => b.UserName, b => b
                        .MapFrom(e => e.User.UserName));
                config.CreateMap<Category, CategoryDto>();
                config.CreateMap<Lot, LotDto>()
                    .ForMember(lot => lot.OwnerName, lot => lot
                        .MapFrom(e => e.LotOwner.UserName))
                    .ForMember(lot => lot.Images,lot => lot
                        .MapFrom(e => Mapper.Map<IEnumerable<Image>, IEnumerable<ImageDto>>(e.Images)))
                    .ForMember(lot => lot.Bets,lot => lot
                        .MapFrom(e => Mapper.Map<IEnumerable<Bet>, IEnumerable<BetDto>>(e.Bets)));
                config.CreateMap<User, UserDto>().ForMember(u => u.Role, u => u
                        .MapFrom(e => e.Roles.First()));
            });
        }
    }
}
