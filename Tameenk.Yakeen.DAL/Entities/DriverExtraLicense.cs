using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class DriverExtraLicense
    {
        [Key]
        public int ID { get; set; }
        public Guid DriverId { get; set; }
        public short CountryCode { get; set; }
        public int LicenseYearsId { get; set; }

       // public Driver Driver { get; set; }
    }
}
