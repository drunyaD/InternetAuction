using System;

namespace InternetAuction.BLL.DTO
{
    public class BetDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime PlacingTime { get; set; }
        public int LotId { get; set; }
        public string UserName { get; set; }
    }
}
