using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.DAL.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Lot> Lots { get; set; }
        public virtual ICollection<Bet> Bets { get; set; }

        public User()
        {
            Lots = new HashSet<Lot>();
            Bets = new HashSet<Bet>();
        }
    }
}
