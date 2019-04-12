using System;
using System.Collections.Generic;

namespace InternetAuction.BLL.DTO
{
    public class LotDto
    {
        public LotDto()
        {
            Images = new HashSet<ImageDto>();
            Bets = new HashSet<BetDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int StartPrice { get; set; }
        public string OwnerName { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ImageDto> Images { get; set; }
        public ICollection<BetDto> Bets { get; set; }
    }
}
