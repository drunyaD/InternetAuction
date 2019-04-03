using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.DTO
{
    public class BetDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime PlacingTime { get; set; }
        public int LotId { get; set; }
        public string UserId { get; set; }
    }
}
