using System.ComponentModel.DataAnnotations;

namespace InternetAuction.BLL.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
