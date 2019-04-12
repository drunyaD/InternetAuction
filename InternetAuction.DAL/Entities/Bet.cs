using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetAuction.DAL.Entities
{
    public class Bet
    {
        [Key] public int Id { get; set; }
        [Required] public int Value { get; set; }
        [Required] public DateTime PlacingTime { get; set; }
        [Required] [ForeignKey("Lot")] public int LotId { get; set; }
        [Required] [ForeignKey("User")] public string UserId { get; set; }
        [Required] public virtual Lot Lot { get; set; }
        [Required] public virtual User User { get; set; }
    }
}
