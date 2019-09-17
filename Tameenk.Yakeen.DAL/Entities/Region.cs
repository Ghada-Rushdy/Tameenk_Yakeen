using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
   public class Region
    {
        public Region()
        {
            Cities = new HashSet<City>();
        }
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
