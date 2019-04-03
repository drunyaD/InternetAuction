using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.DAL.Entities
{
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public string Name {get; set; }
        public virtual User User { get; set; }
    }
}
