using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Service.Models;
using YakeenComponent;
//using CompanyYakeenInfoDto = Tameenk.Yakeen.Service.Models.CompanyYakeenInfoDto;

namespace Tameenk.Yakeen.Component.Extensions
{
    public static class MappingExtension
    {
        public static DriverYakeenInfoModel ToModel(this Citizen entity)
        {
            return new DriverYakeenInfoModel
            {
                IsCitizen = entity.IsCitizen,
                EnglishFirstName = entity.EnglishFirstName,
                EnglishLastName = entity.EnglishLastName,
                EnglishSecondName = entity.EnglishSecondName,
                EnglishThirdName = entity.EnglishMiddleName,
                LastName = entity.ArabicLastName,
                SecondName = entity.ArabicSecondName,
                FirstName = entity.ArabicFirstName,
                ThirdName = entity.ArabicMiddleName,
                SubtribeName = entity.SubtribeName,
                DateOfBirthG = entity.DateOfBirthG,
                NationalityCode = entity.NationalityCode,
                DateOfBirthH = entity.DateOfBirthH,
                NIN = entity.NIN,
                IsSpecialNeed = entity.IsSpecialNeed,
                IdIssuePlace = entity.IdIssuePlace,
                IdExpiryDate = entity.IdExpiryDate,
                NumOfChildsUnder16 = entity.NumOfChildsUnder16,
                RoadConvictions = entity.RoadConvictions
            };
        }

        public static AlienYakeenInfoModel ToModel(this Alien entity)
        {
            return new AlienYakeenInfoModel
            {
                IsCitizen = entity.IsCitizen,
                EnglishFirstName = entity.EnglishFirstName,
                EnglishLastName = entity.EnglishLastName,
                EnglishSecondName = entity.EnglishSecondName,
                EnglishThirdName = entity.EnglishThirdName,
                LastName = entity.LastName,
                SecondName = entity.SecondName,
                FirstName = entity.FirstName,
                ThirdName = entity.ThirdName,
                SubtribeName = entity.SubtribeName,
                DateOfBirthG = entity.DateOfBirthG,
                NationalityCode = entity.NationalityCode,
                DateOfBirthH = entity.DateOfBirthH,
                NIN = entity.NIN,
                IsSpecialNeed = entity.IsSpecialNeed,
                IdIssuePlace = entity.IdIssuePlace,
                IdExpiryDate = entity.IdExpiryDate,
                NumOfChildsUnder16 = entity.ChildrenBelow16Years,
                //RoadConvictions = entity.RoadConvictions
            };
        }

        public static VehicleYakeenModel ToModel(this Vehicle entity)
        {
            //return entity.MapTo<Vehicle, VehicleYakeenModel>();
            return new VehicleYakeenModel
            {

                SequenceNumber = entity.SequenceNumber,
                CustomCardNumber = entity.CustomCardNumber,
                Cylinders = entity.Cylinders,
                LicenseExpiryDate = entity.LicenseExpiryDate,
                MajorColor = entity.MajorColor,
                MinorColor = entity.MinorColor,
                ModelYear = entity.ModelYear,
                RegisterationPlace = entity.RegisterationPlace,
                ChassisNumber = entity.ChassisNumber,
                CarPlateText1 = entity.CarPlateText1,
                CarPlateText2 = entity.CarPlateText2,
                CarPlateText3 = entity.CarPlateText3,
                CarPlateNumber = entity.CarPlateNumber,
                CarOwnerNIN = entity.CarOwnerNIN,
                CarOwnerName = entity.CarOwnerName,
                Value = entity.VehicleValue,
                IsUsedCommercially = entity.IsUsedCommercially
            };
        }       
        public static CompanyYakeenInfoDto ToModel(this Company entity)
        {
            //return entity.MapTo<Company, YakeenComponent.CompanyYakeenInfoModel>();
            return new CompanyYakeenInfoDto
            {
                LogId = entity.logId,
                SponsorName = entity.SponsorName,
                TotalNumberOfSponsoredDependents = entity.TotalNumberOfSponsoredDependents,
                TotalNumberOfSponsoreds = entity.TotalNumberOfSponsoreds
            };
        }
        public static CompanyYakeenInfoDto ToModel(this CompanyYakeenInfoDto entity)
        {
            //return entity.MapTo<CompanyYakeenInfoDto, YakeenComponent.CompanyYakeenInfoDto >();
            return new CompanyYakeenInfoDto
            {
                LogId = entity.LogId,
                SponsorName = entity.SponsorName,
                TotalNumberOfSponsoredDependents = entity.TotalNumberOfSponsoredDependents,
                TotalNumberOfSponsoreds = entity.TotalNumberOfSponsoreds
            };
        }
        public static CustomerYakeenInfoModel ToCustomerModel(this Citizen entity)
        {
            return new YakeenComponent.CustomerYakeenInfoModel
            {
                IsCitizen = entity.IsCitizen,
                EnglishFirstName = entity.EnglishFirstName,
                EnglishLastName = entity.EnglishLastName,
                EnglishSecondName = entity.EnglishSecondName,
                EnglishMiddleName = entity.EnglishMiddleName,
                ArabicLastName = entity.ArabicLastName,
                ArabicSecondName = entity.ArabicSecondName,
                ArabicFirstName = entity.ArabicFirstName,
                ArabicMiddleName = entity.ArabicMiddleName,
                SubtribeName = entity.SubtribeName,
                DateOfBirthG = entity.DateOfBirthG,
                NationalityCode = entity.NationalityCode,
                DateOfBirthH = entity.DateOfBirthH,
                NIN = entity.NIN,
                OccupationId = entity.OccupationId,
                OccupationCode = entity.OccupationCode,
                SocialStatusId = entity.SocialStatusId,
                IsSpecialNeed = entity.IsSpecialNeed,
                IdIssuePlace = entity.IdIssuePlace,
                IdExpiryDate = entity.IdExpiryDate,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
