using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class VehicleModel 
    {
        [Key]
        public int Id { set; get; }
        public long Code { get; set; }

        public short VehicleMakerCode { get; set; }

        public string EnglishDescription { get; set; }

        public string ArabicDescription { get; set; }

        public VehicleMaker VehicleMaker { get; set; }
    }
}
