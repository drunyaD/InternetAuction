using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

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
