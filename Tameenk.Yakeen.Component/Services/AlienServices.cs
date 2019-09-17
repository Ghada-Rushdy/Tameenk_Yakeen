using Tameenk.Yakeen.DAL.Enums;
using Tameenk.Yakeen.Component.Extensions;
using System;
using System.Linq;
using Tameenk.Yakeen.Service.Models;
using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Component;
using Tameenk.Yakeen.Service.Utilities;

namespace YakeenComponent
{
    public class AlienServices
    {
        private static AlienDataAccess alienDataAccess = new AlienDataAccess();
        private static AlienOutput alienOutput = new AlienOutput();
        private static ChannelDataAccess channelData = new ChannelDataAccess();

        private static  AlienRequestLog alienRequestLog = new AlienRequestLog();
        private static AlienRequestLogDataAccess alienRequestLogDataAccess = new AlienRequestLogDataAccess();

        public static AlienOutput GetAlienByTameenkId(Guid AlienId)
        {
            alienRequestLog.Method = "GetAlienByTameenkId";
            alienRequestLog.ServerIP = Utilities.GetServerIP();
            alienRequestLog.AlienID = AlienId;

            try
            {
                if (AlienId == Guid.Empty)
                {
                    alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.NullRequest;
                    alienRequestLog.ErrorDescription = "Alien Id is null";
                    alienRequestLog.AlienID = AlienId;
                    alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);

                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.NullRequest;
                    alienOutput.ErrorDescription = "Citizen ID is null";
                    alienOutput = null;

                    return alienOutput;
                }

                AlienYakeenInfoModel driver = null;

                Alien driverData = alienDataAccess.Find(d => d.DriverId == AlienId, e => e.OrderByDescending(x => x.CreatedDate))
                    .FirstOrDefault();

                if (driverData != null)
                {
                    driver = driverData.ToModel();

                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.Success;
                    alienOutput.ErrorDescription = "success";
                    alienOutput.Alien = driver;
                }
                else
                {
                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.NullResponse;
                    alienOutput.ErrorDescription = "Citizen ID not exist";
                    alienOutput.Alien = null;
                }
            }
            catch (Exception exp)
            {
                alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.ServiceException;
                alienRequestLog.ErrorDescription = exp.ToString();
                alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);

                alienOutput.ErrorCode = AlienOutput.ErrorCodes.ServiceException;
                alienOutput.ErrorDescription = "GetCitizenByTameenkId through exception";
                alienOutput.Alien = null;
            }

            return alienOutput;
        }

        public static AlienOutput GetAlienByOfficialIdAndLicenseExpiryDate(DriverYakeenInfoRequestModel driverInfoRequest)
        {
            ServicesUtilities.AcceptCertificate();

            alienRequestLog.Method = "GetAlienByOfficialIdAndLicenseExpiryDate";
            alienRequestLog.ServerIP = Utilities.GetServerIP();
           
            try
            {
                if (driverInfoRequest == null)
                {
                    alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.NullRequest;
                    alienRequestLog.ErrorDescription = "DriverInfoRequest is null";
                    alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);

                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.NullRequest;
                    alienOutput.ErrorDescription = "DriverInfoRequest is null";
                    alienOutput.Alien = null;

                    return alienOutput;
                }

                if (driverInfoRequest.Nin == 0 
                    || driverInfoRequest.Channel == 0
                    || string.IsNullOrEmpty(driverInfoRequest.licenseExpiryDate)
                    || string.IsNullOrEmpty(driverInfoRequest.ReferenceNumber))
                {

                    alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.NullRequest;
                    alienRequestLog.ErrorDescription = "Request mandatory fields are not exist";
                    alienRequestLog.NiN = driverInfoRequest.Nin;
                    alienRequestLog.Channel = driverInfoRequest.Channel;
                    //alienRequestLog.ReferenceNumber = driverInfoRequest.ReferenceNumber;
                    //alienRequestLog.IsCitizen = driverInfoRequest.IsCitizen;
                    alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);

                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.NullRequest;
                    alienOutput.ErrorDescription = "Request mandatory fields are not exist";
                    alienOutput.Alien = null;

                    return alienOutput;
                }

                Alien driverData = getDriverEntityFromNin(driverInfoRequest.Nin);

                AlienYakeenInfoModel driver = null;
                int channelExpireByDays = channelData.GetChannelExpireDateByID(driverInfoRequest.Channel);

                if (driverData == null || driverData.CreatedDate >= DateTime.Now.AddDays(channelExpireByDays))
                {
                    var driverInfo = YakeenClient.GetAlienInfo(driverInfoRequest.ReferenceNumber,
                        driverInfoRequest.Nin.ToString(), driverInfoRequest.licenseExpiryDate, 
                        driverInfoRequest.Channel.ToString());

                    if (driverInfo.ErrorCode == Tameenk.Yakeen.Service.WebClients.YakeenOutput.ErrorCodes.Success)
                    {
                        if (driverData == null)
                            driverData = InsertDriverInfoIntoDb(driverInfoRequest, driverInfo.DriverYakeenInfoDto);
                        else
                            UpdateAlienLicensesInDb(driverData, driverInfo.DriverYakeenInfoDto);
                    }
                    else
                    {
                        alienOutput.ErrorCode = AlienOutput.ErrorCodes.NullResponse;
                        alienOutput.ErrorDescription = driverInfo.ErrorDescription;
                        alienOutput.Alien = null;
                    }
                }

                if (driverData != null)
                {
                    driver = driverData.ToModel();

                    alienOutput.ErrorCode = AlienOutput.ErrorCodes.Success;
                    alienOutput.ErrorDescription = "Success";
                    alienOutput.Alien = driver;
                }
            }
            catch (Exception exp)
            {
                alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.MethodException;
                alienRequestLog.ErrorDescription = exp.ToString();
                alienRequestLog.NiN = driverInfoRequest.Nin;
                alienRequestLog.Channel = driverInfoRequest.Channel;
                //alienRequestLog.licenseExpiryDate = driverInfoRequest.licenseExpiryDate;
               // alienRequestLog.ReferenceNumber = driverInfoRequest.ReferenceNumber;
                //alienRequestLog.IsCitizen = driverInfoRequest.IsCitizen;
                alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);                

                alienOutput.ErrorCode = AlienOutput.ErrorCodes.MethodException;
                alienOutput.ErrorDescription = "DriverInfoRequest through exception";
                alienOutput.Alien = null;
            }

            return alienOutput;
        }

        private static Alien InsertDriverInfoIntoDb(DriverYakeenInfoRequestModel driverInitialData, DriverYakeenInfoDto driverInfo)
        {
            var driverData = new Alien();

            try
            {
                driverData = new Alien()
                {
                    DriverId = Guid.NewGuid(),
                    IsCitizen = driverInitialData.Nin.ToString().StartsWith("1"),
                    EnglishFirstName = driverInfo.EnglishFirstName,
                    EnglishLastName = driverInfo.EnglishLastName,
                    EnglishSecondName = string.IsNullOrWhiteSpace(driverInfo.EnglishSecondName) ? "-" : driverInfo.EnglishSecondName,
                    EnglishThirdName = string.IsNullOrWhiteSpace(driverInfo.EnglishThirdName) ? "-" : driverInfo.EnglishThirdName,
                    LastName = driverInfo.LastName,
                    SecondName = string.IsNullOrWhiteSpace(driverInfo.SecondName) ? "-" : driverInfo.SecondName,
                    FirstName = driverInfo.FirstName,
                    ThirdName = string.IsNullOrWhiteSpace(driverInfo.ThirdName) ? "-" : driverInfo.ThirdName,
                    SubtribeName = driverInfo.SubtribeName,
                    DateOfBirthG = driverInfo.DateOfBirthG,
                    NationalityCode = driverInfo.NationalityCode,
                    DateOfBirthH = driverInfo.DateOfBirthH,
                    NIN = driverInitialData.Nin.ToString(),
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                foreach (var lic in driverInfo.Licenses)
                {
                    driverData.DriverLicenses.Add(new DriverLicense()
                    {
                        ExpiryDateH = lic.ExpiryDateH,
                        TypeDesc = lic.TypeDesc,
                    });
                }

                driverData.Gender = Extensions.FromCode<Gender>(driverInfo.Gender.ToString());
                alienDataAccess.Add(driverData);

            }
            catch (Exception exp)
            {
                alienRequestLog.Method = "InsertDriverInfoIntoDb";
                alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.MethodException;
                alienRequestLog.ErrorDescription = exp.ToString();
                alienRequestLog.NiN = driverInitialData.Nin;
                alienRequestLog.Channel = driverInitialData.Channel;
                //alienRequestLog.licenseExpiryDate = driverInitialData.licenseExpiryDate;
                //alienRequestLog.ReferenceNumber = driverInitialData.ReferenceNumber;
                //alienRequestLog.IsCitizen = driverInitialData.IsCitizen;
                alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);
            }

            return driverData;
        }

        private static void UpdateAlienLicensesInDb(Alien driverData, DriverYakeenInfoDto driverInfo)
        {
            try
            {
                foreach (var lic in driverInfo.Licenses)
                {
                    driverData.DriverLicenses.Add(new DriverLicense()
                    {
                        ExpiryDateH = lic.ExpiryDateH,
                        TypeDesc = lic.TypeDesc,
                    });
                }

                alienDataAccess.Update(driverData);
            }
            catch (Exception exp)
            {
                alienRequestLog.Method = "UpdateCiitizenLicensesInDb";
                alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.MethodException;
                alienRequestLog.ErrorDescription = exp.ToString();
                alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);
            }

        }

        private static Alien getDriverEntityFromNin(long nin)
        {
            Alien driverData = null;

            try
            {
                driverData = alienDataAccess.Find(d => d.NIN == nin.ToString() & !d.IsDeleted,
                e => e.OrderByDescending(x => x.CreatedDate)).
               FirstOrDefault();

                if (driverData == null)
                    return null;

            }
            catch (Exception exp)
            {
                alienRequestLog.Method = "getDriverEntityFromNin";
                alienRequestLog.ErrorCode = AlienRequestLog.ErrorCodes.MethodException;
                alienRequestLog.ErrorDescription = exp.ToString();
                alienRequestLog.NiN = nin;
                alienRequestLogDataAccess.AddToAlienLog(alienRequestLog);
            }

            return driverData;
        }


    }
}
