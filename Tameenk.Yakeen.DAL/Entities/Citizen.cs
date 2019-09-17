using Tameenk.Yakeen.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tameenk.Yakeen.DAL
{
    public class Citizen
    {
        public Citizen()
        {
            Addresses = new HashSet<Address>();           
            DriverLicenses = new HashSet<DriverLicense>();         
            DriverViolations = new HashSet<DriverViolation>();           
            MedicalConditionId = null;
            Occupation = new Occupation();
        }
        public string FullEnglishName
        {
            get
            {
                return EnglishFirstName + " " + EnglishSecondName + " " + EnglishLastName;
            }
        }

        public string FullArabicName
        {
            get
            {
                return FirstName + " " + SecondName + " " + LastName;
            }
        }

        [Key]
        public int ID { get; set; }
        public Guid DriverId { get; set; }

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
        /// <summary>
        /// Driver gender.
        /// </summary>
        public int? GenderId { get; set; }

        /// <summary>
        /// NCD Free Years
        /// </summary>
        public int? NCDFreeYears { get; set; }
        /// <summary>
        /// NCD Reference.
        /// </summary>
        public string NCDReference { get; set; }

        public bool? IsSpecialNeed { get; set; }

        public string IdIssuePlace { get; set; }

        public string IdExpiryDate { get; set; }
        /// <summary>
        /// Driver Driving percentage of the vehicle
        /// </summary>
        public int? DrivingPercentage { get; set; }

        /// <summary>
        /// Number of children under age 16 years.
        /// </summary>
        public int? ChildrenBelow16Years { get; set; }
        public int? EducationId { get; set; }
        public int? SocialStatusId { get; set; }

        /// <summary>
        /// Driver Occupation identifier
        /// </summary>
        public int? OccupationId { get; set; }

        /// <summary>
        /// Medical condition id.
        /// </summary>
        public int? MedicalConditionId { get; set; }

        /// <summary>
        /// Driver male/Femal resident occupation.
        /// </summary>
        public string ResidentOccupation { get; set; }

        /// <summary>
        /// City code selected by the driver
        /// </summary>
        public long? CityId { get; set; }
        /// <summary>
        /// Driver Work City Code
        /// </summary>
        public long? WorkCityId { get; set; }

        /// <summary>
        /// Number of at-fault accidents in the last 5 years
        /// </summary>
        public int? NOALast5Years { get; set; }
        /// <summary>
        /// Number of at-fault claims in the last 5 years
        /// </summary>
        public int? NOCLast5Years { get; set; }


        /// <summary>
        /// Driver education level.
        /// </summary>
        public Education Education
        {
            get { return (Education)EducationId; }
            set { EducationId = (int)value; }
        }
        /// <summary>
        /// The gender.
        /// </summary>
        public Gender Gender
        {
            get { return (Gender)GenderId; }
            set { GenderId = (int)value; }
        }

        /// <summary>
        /// Driver driving violations.
        /// </summary>
        public ICollection<DriverViolation> DriverViolations { get; set; }


        /// <summary>
        /// Driver Occupation
        /// </summary>
        public virtual Occupation Occupation { get; set; }

        /// <summary>
        /// Driver socail status.
        /// </summary>
        public SocialStatus SocialStatus
        {
            get { return (SocialStatus)SocialStatusId.GetValueOrDefault(); }
            set { SocialStatusId = (int)SocialStatus.SingleMale; }
        }

        /// <summary>
        /// Driver medical condition.
        /// </summary>
        public MedicalCondition MedicalCondition
        {
            get { return (MedicalCondition)MedicalConditionId.GetValueOrDefault(); }
            set { MedicalConditionId = null; }
        }
        /// <summary>
        /// Driver addresses.
        /// </summary>
        public ICollection<Address> Addresses { get; set; }

      //  public ICollection<CheckoutAdditionalDriver> CheckoutAdditionalDrivers { get; set; }

       // public ICollection<CheckoutDetail> CheckoutDetails { get; set; }

        public ICollection<DriverLicense> DriverLicenses { get; set; }

       // public ICollection<QuotationRequest> AdditionalDriverQuotationRequests { get; set; }

      //  public ICollection<QuotationRequest> QuotationRequests { get; set; }

        public virtual ICollection<DriverExtraLicense> DriverExtraLicenses { get; set; }

        /// <summary>
        /// Driver Home City
        /// </summary>
        public City City { get; set; }
        /// <summary>
        /// Driver Work City.
        /// </summary>
        public City WorkCity { get; set; }

       //additional fields
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string ServerIP { set; get; }
        public string UserIP { set; get; }
        public string UserAgent { set; get; }

        public int? ViolationId { get; set; }        

        public int? MaritalStatusCode { get; set; }

        public int? NumOfChildsUnder16 { get; set; }

        public int? DrivingLicenseTypeCode { get; set; }

        public int? SaudiLicenseHeldYears { get; set; }

        public int? EligibleForNoClaimsDiscountYears { get; set; }

        public int? NumOfFaultAccidentInLast5Years { get; set; }

        public int? NumOfFaultclaimInLast5Years { get; set; }

        public string RoadConvictions { get; set; }        


    }
}
