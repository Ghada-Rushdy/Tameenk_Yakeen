using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class LicenseType
    {
        [Key]
        public int Id { set; get; }
        public short Code { get; set; }

        public string EnglishDescription { get; set; }

        public string ArabicDescription { get; set; }
    }
}
