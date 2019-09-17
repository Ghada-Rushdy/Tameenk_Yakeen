using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.Yakeen.Service.Models
{
    public class VehicleYakeenInfoDto
    {
        public VehicleYakeenInfoDto()
        {
            
        }


        //public bool Success { get; set; }     

        /// <summary>
        /// true --> Registered , flase --> notRegistered
        /// </summary>
        public bool IsRegistered { get; set; }


        public short Cylinders { get; set; }

        public string LicenseExpiryDate { get; set; }

        public int LogId { get; set; }

        public string MajorColor { get; set; }

        public string MinorColor { get; set; }

        public short ModelYear { get; set; }

        public short? PlateTypeCode { get; set; }

        public string RegisterationPlace { get; set; }

        public short BodyCode { get; set; }

        public int Weight { get; set; }

        public int Load { get; set; }

        public int MakerCode { get; set; }

        public int ModelCode { get; set; }

        public string Maker { get; set; }

        public string Model { get; set; }

        public string ChassisNumber { get; set; }

       
    }
}
