using System;
using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    public class BetDto
    {
        public int Id { get; set; }
        [Required ]public int Value { get; set; }
        public DateTime PlacingTime { get; set; }
        [Required] public int LotId { get; set; }
        public string UserName { get; set; }
    }
}
