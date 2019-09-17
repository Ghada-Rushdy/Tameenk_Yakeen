using Tameenk.Yakeen.DAL.Enums;
using Tameenk.Yakeen.Service.Yakeen4Bcare;
using System;

namespace YakeenComponent
{
    public class CustomerIdYakeenInfoDto
    {
        public CustomerIdYakeenInfoDto()
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

        public bool IsCitizen { get; set; }

        public int LogId { get; set; }

        public string IdIssuePlace { get; set; }

        public EGender Gender { get; set; }

        public short NationalityCode { get; set; }

        /// <summary>
        /// format : (dd-MM-yyyy)
        /// </summary>
        /// 
        public DateTime DateOfBirthG { get; set; }

        /// <summary>
        /// format : (dd-MM-yyyy)
        /// </summary>
        /// 
        public string DateOfBirthH { get; set; }

        /// <summary>
        /// format : (dd-MM-yyyy)
        /// </summary>
        /// 
        public string IdExpiryDate { get; set; }

        public string SocialStatus { get; set; }

        public string OccupationCode { get; set; }

        public string EnglishFirstName { get; set; }

        public string EnglishLastName { get; set; }

        public string EnglishSecondName { get; set; }

        public string EnglishThirdName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string ThirdName { get; set; }

        public string SubtribeName { get; set; }

        public licenseList[] licenseListListField { get; set; }


    }
}