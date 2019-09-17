using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class ServiceRequestLog
    {
        public int ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UserID { get; set; }
        [StringLength(255)]
        public string UserName { get; set; }
        [StringLength(255)]
        public string Method { get; set; }
        public int? CompanyID { get; set; }
        [StringLength(255)]
        public string CompanyName { get; set; }
        [StringLength(255)]
        public string ServiceURL { get; set; }
        public int? ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ServiceRequest { get; set; }
        public string ServiceResponse { get; set; }
        [StringLength(50)]
        public string ServerIP { get; set; }
        public Guid? RequestId { get; set; }
        public double? ServiceResponseTimeInSeconds { get; set; }
        [StringLength(255)]
        public string Channel { get; set; }
        [StringLength(50)]
        public string ServiceErrorCode { get; set; }
        public string ServiceErrorDescription { get; set; }
        public string ReferenceId { get; set; }
        public int? InsuranceTypeCode { get; set; }
        public string DriverNin { get; set; }
        public string VehicleId { get; set; }
        public string PolicyNo { get; set; }
        public string VehicleMaker { get; set; }
        public string VehicleMakerCode { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleModelCode { get; set; }
        public int? VehicleModelYear { get; set; }
    }
}
