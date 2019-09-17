using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
   public class DriverLicense
    {
        [Key]
        public int ID { get; set; }
        public int LicenseId { get; set; }

        public Guid DriverId { get; set; }

        public short? TypeDesc { get; set; }

        public string ExpiryDateH { get; set; }

        public string IssueDateH { get; set; }

        //public Driver Driver { get; set; }
    }
}
