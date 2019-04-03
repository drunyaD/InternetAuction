using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.DTO
{
    public class LotDTO
    {
        public LotDTO()
        {
            Images = new HashSet<ImageDTO>();
            Bets = new HashSet<BetDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public int StartPrice { get; set; }
        public string OwnerId { get; set; }
        public int? LastBetId { get; set; }
        public int CategoryId { get; set; }
        public ICollection<ImageDTO> Images { get; set; }
        public ICollection<BetDTO> Bets { get; set; }
    }
}
