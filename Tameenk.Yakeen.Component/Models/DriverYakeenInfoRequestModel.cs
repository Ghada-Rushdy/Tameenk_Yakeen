using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace YakeenComponent
{
    public class DriverYakeenInfoRequestModel
    {
        [Required]
        public long Nin { get; set; }

        //[Range(1, 12)]
        public int? LicenseExpiryMonth { get; set; }

        public int? LicenseExpiryYear { get; set; }

        [Required]
        [Range(1, 12)]
        public int BirthMonth { get; set; }

        [Required]
        public int BirthYear { get; set; }
        public Guid? ParentRequestId { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }

        [Required]
        public int EducationId { get; set; }
        public int ChildrenBelow16Years { get; set; }    
        public int DrivingPercentage { get; set; }
        public int MedicalConditionId { get; set; }
        public List<DriverExtraLicenseModel> DriverExtraLicenses { get; set; }
        public List<int> ViolationIds { get; set; }
        public int Channel { set; get; }
        public string ReferenceNumber { set; get; }
        public bool IsCitizen { set; get; }
        public string licenseExpiryDate { set; get; }
    }
}
