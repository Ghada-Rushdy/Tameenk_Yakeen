using Tameenk.Yakeen.DAL.Enums;
using System;


namespace YakeenComponent
{
    public class CustomerYakeenInfoModel
    {
        public CustomerYakeenInfoModel()
        {

        }
        public bool Success { get; set; }
       
        public ErrorCodes ErrorCode
        {
            get;
            set;
        }
        public string ErrorDescription
        {
            get;
            set;
        }
        public Guid TameenkId { get; set; }

        public bool IsCitizen { get; set; }

        public string EnglishFirstName { get; set; }

        public string EnglishLastName { get; set; }

        public string EnglishSecondName { get; set; }

        public string EnglishMiddleName { get; set; }

        public string ArabicLastName { get; set; }

        public string ArabicSecondName { get; set; }

        public string ArabicFirstName { get; set; }

        public string ArabicMiddleName { get; set; }

        public string SubtribeName { get; set; }

        public DateTime DateOfBirthG { get; set; }
        public string DateOfBirthH { get; set; }

        public short? NationalityCode { get; set; }    

        public string NIN { get; set; }

        public int? OccupationId { get; set; }
        public string OccupationCode { get; set; }

        public int? SocialStatusId { get; set; }

        public Gender Gender { get; set; }

        public bool? IsSpecialNeed { get; set; }

        public string IdIssuePlace { get; set; }

        public string IdExpiryDate { get; set; }

        //additional fields

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
