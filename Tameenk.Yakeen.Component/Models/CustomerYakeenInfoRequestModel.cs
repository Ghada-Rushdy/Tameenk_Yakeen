using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YakeenComponent
{
    public class CustomerYakeenInfoRequestModel
    {
        [Required]
        public string Nin { get; set; }

        [Required]
        [Range(1, 12)]
        public int BirthMonth { get; set; }

        [Required]
        public int BirthYear { get; set; }

        [Required]
        public bool IsSpecialNeed { get; set; }

        [Required]
        public int EducationId { get; set; }

        public int ChildrenBelow16Years { get; set; }

        /// <summary>
        /// Driver percentage of using the vehicle related to other driver(s) useage.
        /// </summary>
        public int DrivingPercentage { get; set; }

        public int MedicalConditionId { get; set; }


        public Guid? ParentRequestId { get; set; }

        public Guid? UserId { get; set; }
        public string UserName { get; set; }
        public List<DriverExtraLicenseModel> DriverExtraLicenses { get; set; }

        public List<int> ViolationIds { get; set; }

        public string Channel { set; get; }

    }
}
