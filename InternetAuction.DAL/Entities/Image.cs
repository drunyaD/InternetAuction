﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.DAL.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte[] Picture { get; set; }
        [Required]
        [ForeignKey("Lot")]
        public int LotId { get; set; }
        [Required]
        public virtual Lot Lot { get; set; }
        
    }
}
