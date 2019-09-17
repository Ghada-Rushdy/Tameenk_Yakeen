using Tameenk.Yakeen.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
   public class Occupation
    {
        public Occupation()
        {
            Citizens = new List<Citizen>();
        }
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsCitizen { get; set; }
        public bool? IsMale { get; set; } 
        public virtual ICollection<Citizen> Citizens { get; set; }

    }
}
