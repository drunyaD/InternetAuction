using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        public DateTime StartTime { get; set; }
        [Required] public DateTime FinishTime { get; set; }
        [Required] public int StartPrice { get; set; }
        public string OwnerName { get; set; }
        [Required] public int CategoryId { get; set; }
        public ICollection<ImageDto> Images { get; set; }
        public ICollection<BetDto> Bets { get; set; }
    }
}
