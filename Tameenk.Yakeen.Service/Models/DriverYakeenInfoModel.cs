using System;
using System.Collections.Generic;


namespace Tameenk.Yakeen.Service.Models
{
    public class DriverYakeenInfoModel
    {  
        public Guid TameenkId { get; set; }

        public bool IsCitizen { get; set; }

        public string EnglishFirstName { get; set; }

        public string EnglishLastName { get; set; }

        public string EnglishSecondName { get; set; }

        public string EnglishThirdName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string ThirdName { get; set; }

        public string SubtribeName { get; set; }

        public DateTime DateOfBirthG { get; set; }

        public short? NationalityCode { get; set; }

        public string DateOfBirthH { get; set; }

        public string NIN { get; set; }

        public string Gender { get; set; }

        public bool? IsSpecialNeed { get; set; }

        public string IdIssuePlace { get; set; }

        public string IdExpiryDate { get; set; }
        
        public string MaritalStatus { get; set; }
        public int? NumOfChildsUnder16 { get; set; }
        public string Occupation { get; set; }
        public string RoadConvictions { get; set; }

        public IList<DriverLicenseYakeenInfoModel> Licenses { get; set; }
    }
}
