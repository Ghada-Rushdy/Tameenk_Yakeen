using Tameenk.Yakeen.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class Alien
    {
        [Key]
        public int ID { get; set; }
        public Alien()
        {
            Addresses = new HashSet<Address>();           
            DriverLicenses = new HashSet<DriverLicense>();           
            DriverViolations = new HashSet<DriverViolation>();           
           // SocialStatusId = (int)SocialStatus.SingleMale;
            MedicalConditionId = null;
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
        public int GenderId { get; set; }
        public int? NCDFreeYears { get; set; }       
        public string NCDReference { get; set; }
        public bool? IsSpecialNeed { get; set; }
        public string IdIssuePlace { get; set; }
        public string IdExpiryDate { get; set; }      
        public int? DrivingPercentage { get; set; }       
        public int? ChildrenBelow16Years { get; set; }
        public int EducationId { get; set; }
        public int? SocialStatusId { get; set; }
        public int? OccupationId { get; set; }        
        public int? MedicalConditionId { get; set; }        
        public string ResidentOccupation { get; set; }
        [ForeignKey("City")]
        public long? CityId { get; set; }   
        [ForeignKey("WorkCity")]
        public long? WorkCityId { get; set; }
        public int? NOALast5Years { get; set; }       
        public int? NOCLast5Years { get; set; }       
        public Education Education
        {
            get { return (Education)EducationId; }
            set { EducationId = (int)value; }
        }        
        public Gender Gender
        {
            get { return (Gender)GenderId; }
            set { GenderId = (int)value; }
        }       
        public ICollection<DriverViolation> DriverViolations { get; set; }
        public virtual Occupation Occupation { get; set; }
        public SocialStatus SocialStatus
        {
            get { return (SocialStatus)SocialStatusId.GetValueOrDefault(); }
            set { SocialStatusId = (int)SocialStatus.SingleMale; }
        }
        public MedicalCondition MedicalCondition
        {
            get { return (MedicalCondition)MedicalConditionId.GetValueOrDefault(); }
            set { MedicalConditionId = null; }
        }       
        public ICollection<Address> Addresses { get; set; }

        //public ICollection<CheckoutAdditionalDriver> CheckoutAdditionalDrivers { get; set; }

      //  public ICollection<CheckoutDetail> CheckoutDetails { get; set; }

        public ICollection<DriverLicense> DriverLicenses { get; set; }

       // public ICollection<QuotationRequest> AdditionalDriverQuotationRequests { get; set; }

      //  public ICollection<QuotationRequest> QuotationRequests { get; set; }

        public virtual ICollection<DriverExtraLicense> DriverExtraLicenses { get; set; }
        public City City { get; set; }       
        public City WorkCity { get; set; }
        public bool IsDeleted { get; set; }       
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string ServerIP { set; get; }
        public string UserIP { set; get; }
        public string UserAgent { set; get; }

    }
}
