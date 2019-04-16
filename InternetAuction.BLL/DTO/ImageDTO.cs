using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    public class ImageDto
    {
        public int Id { get; set; }
        [Required] public byte[] Picture { get; set; }
        public int LotId { get; set; }
    }
}
