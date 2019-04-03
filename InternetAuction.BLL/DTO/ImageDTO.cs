using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public byte[] Picture { get; set; }
        public int LotId { get; set; }
    }
}
