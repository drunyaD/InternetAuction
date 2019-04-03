using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetAuction.DAL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string Name { get; set; }

        public virtual ICollection<Lot> Lots {get; set;}

        public Category()
        {
            Lots = new HashSet<Lot>();
        }
    }
}
