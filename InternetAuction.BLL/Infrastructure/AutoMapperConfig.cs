using AutoMapper;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.Infrastructure
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Image, ImageDTO>();
                config.CreateMap<Bet, BetDTO>();
                config.CreateMap<Category, CategoryDTO>();
                config.CreateMap<Lot,LotDTO>()
                    .ForMember(lot => lot.Images, lot => lot.MapFrom(e => Mapper.Map<IEnumerable<Image>, IEnumerable<ImageDTO>>(e.Images)))
                    .ForMember(lot => lot.Bets, lot => lot.MapFrom(e => Mapper.Map<IEnumerable<Bet>, IEnumerable<BetDTO>>(e.Bets)));
                config.CreateMap<User, UserDTO>()
                    .ForMember(u => u.Role, u => u.MapFrom(e => e.Roles.First()));
            });
        }
    }
}
