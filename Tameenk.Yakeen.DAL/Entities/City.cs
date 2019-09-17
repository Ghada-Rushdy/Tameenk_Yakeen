using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
   public class City
    {
        [Key]
        public long ID { get; set; }
        public long Code { get; set; }

        public string EnglishDescription { get; set; }

        public string ArabicDescription { get; set; }

        public int? RegionId { get; set; }

        public Region Region { get; set; }

    }
}
