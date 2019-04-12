using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetAuction.DAL.Entities
{
    public class Image
    {
        [Key] public int Id { get; set; }
        [Required] public byte[] Picture { get; set; }
        [Required] [ForeignKey("Lot")] public int LotId { get; set; }
        [Required] public virtual Lot Lot { get; set; }
    }
}
