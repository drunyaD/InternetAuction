using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetAuction.DAL.Entities
{
    public class Lot
    {
        public Lot()
        {
            Images = new HashSet<Image>();
            Bets = new HashSet<Bet>();
        }

        [Key] public int Id { get; set; }
        [Required] [StringLength(200)] public string Name { get; set; }
        [Required] [StringLength(1000)] public string Description { get; set; }
        [Required] public DateTime StartTime { get; set; }
        [Required] public DateTime FinishTime { get; set; }
        [Required] public int StartPrice { get; set; }
        [Required] [ForeignKey("LotOwner")] public string OwnerId { get; set; }
        [Required] [ForeignKey("Category")] public int CategoryId { get; set; }
        [Required] public virtual User LotOwner { get; set; }
        [Required] public virtual Category Category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
