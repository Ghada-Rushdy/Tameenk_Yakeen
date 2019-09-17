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
    public static class CitizenServices 
    {
        private static CitizenDataAccess citizenDataAccess = new CitizenDataAccess();
        private static CitizenOutput citizenOutput = new CitizenOutput();
        private static ChannelDataAccess channelData = new ChannelDataAccess();

        private static CitizenRequestLogDataAccess citizinLogDataAccess = new CitizenRequestLogDataAccess();
        private static CitizenRequestLog citizenRequestLog = new CitizenRequestLog();

        public static CitizenOutput GetCitizenByTameenkId(Guid CitizenId)
        {            
            citizenRequestLog.Method = "GetCitizenByTameenkId";
            citizenRequestLog.ServerIP = Utilities.GetServerIP();
            citizenRequestLog.CitizenId = CitizenId;

            try
            {
                if (CitizenId == Guid.Empty)
                {
                    citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.NullRequest;
                    citizenRequestLog.ErrorDescription = "Citizen Id is null";
                    citizenRequestLog.CitizenId = CitizenId;
                    citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);

                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.NullRequest;
                    citizenOutput.ErrorDescription = "Citizen ID is null";
                    citizenOutput.Citizen = null;

                    return citizenOutput;
                }                

                DriverYakeenInfoModel driver = null;

                Citizen driverData = citizenDataAccess.Find(d => d.DriverId == CitizenId, e => e.OrderByDescending(x => x.CreatedDate))
                    .FirstOrDefault();

                if (driverData != null)
                {
                    driver = driverData.ToModel();

                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.Success;
                    citizenOutput.ErrorDescription = "success";
                    citizenOutput.Citizen = driver;
                }
                else
                {
                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.NullResponse;
                    citizenOutput.ErrorDescription = "Citizen ID not exist";
                    citizenOutput.Citizen = null;
                }
            }
            catch(Exception exp)
            {               
                citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.ServiceException;
                citizenRequestLog.ErrorDescription = exp.ToString();
                citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);

                citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.ServiceException;
                citizenOutput.ErrorDescription = "GetCitizenByTameenkId through exception";
                citizenOutput.Citizen = null;
            } 

            return citizenOutput;
        }

        public static CitizenOutput GetCitizenByOfficialIdAndLicenseExpiryDate(DriverYakeenInfoRequestModel driverInfoRequest)
        {
            ServicesUtilities.AcceptCertificate();

            citizenRequestLog.Method = "GetDriverByOfficialIdAndLicenseExpiryDate";
            citizenRequestLog.ServerIP = Utilities.GetServerIP(); 
     
            try
            {
                if (driverInfoRequest == null)
                {
                    citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.NullRequest;
                    citizenRequestLog.ErrorDescription = "DriverInfoRequest is null";
                    citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);

                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.NullRequest;
                    citizenOutput.ErrorDescription = "DriverInfoRequest is null";
                    citizenOutput.Citizen = null;                    
                    
                    return citizenOutput;
                }

                citizenRequestLog.NiN = driverInfoRequest.Nin;
                citizenRequestLog.Channel = driverInfoRequest.Channel;
                citizenRequestLog.ReferenceNumber = driverInfoRequest.ReferenceNumber;
                citizenRequestLog.IsCitizen = driverInfoRequest.IsCitizen;

                if (driverInfoRequest.Nin==0 
                    || driverInfoRequest.Channel==0
                    || string.IsNullOrEmpty(driverInfoRequest.licenseExpiryDate)                 
                    || string.IsNullOrEmpty(driverInfoRequest.ReferenceNumber))
                {
                    citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.NullRequest;
                    citizenRequestLog.ErrorDescription = "Request mandatory fields are not exist";                   
                    citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);

                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.NullRequest;
                    citizenOutput.ErrorDescription = "Request mandatory fields are not exist";
                    citizenOutput.Citizen = null;

                    return citizenOutput;
                }
                              
                Citizen citizenData = getDriverEntityFromNin(driverInfoRequest.Nin);
                       
                DriverYakeenInfoModel driver = null;
                int channelExpireByDays = channelData.GetChannelExpireDateByID(driverInfoRequest.Channel);

                if (citizenData == null || citizenData.CreatedDate >= DateTime.Now.AddDays(channelExpireByDays))
                {
                    var driverInfo = YakeenClient.GetCitizenInfo(driverInfoRequest.ReferenceNumber,
                        driverInfoRequest.Nin.ToString(), driverInfoRequest.licenseExpiryDate, 
                        driverInfoRequest.Channel.ToString());

                    if (driverInfo.ErrorCode == Tameenk.Yakeen.Service.WebClients.YakeenOutput.ErrorCodes.Success)
                    {
                        if (citizenData == null)
                            citizenData = InsertDriverInfoIntoDb(driverInfoRequest, driverInfo.DriverYakeenInfoDto);
                        else
                            UpdateCiitizenLicensesInDb(citizenData, driverInfo.DriverYakeenInfoDto);
                    }
                    else
                    {
                        citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.NullResponse;
                        citizenOutput.ErrorDescription = driverInfo.ErrorDescription;
                        citizenOutput.Citizen = null;
                    }
                }

                if (citizenData != null)
                {
                    driver = citizenData.ToModel();

                    citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.Success;
                    citizenOutput.ErrorDescription = "Success";
                    citizenOutput.Citizen = driver;
                }
            }
            catch(Exception exp)
            {                
                citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.MethodException;
                citizenRequestLog.ErrorDescription = exp.ToString();
                citizenRequestLog.NiN = driverInfoRequest.Nin;
                citizenRequestLog.Channel = driverInfoRequest.Channel;
                citizenRequestLog.licenseExpiryDate = driverInfoRequest.licenseExpiryDate;
                citizenRequestLog.ReferenceNumber = driverInfoRequest.ReferenceNumber;
                citizenRequestLog.IsCitizen = driverInfoRequest.IsCitizen;
                citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);

                citizenOutput.ErrorCode = CitizenOutput.ErrorCodes.MethodException;
                citizenOutput.ErrorDescription = "DriverInfoRequest through exception";
                citizenOutput.Citizen = null;
            }          

            return citizenOutput;
        }

        private static Citizen InsertDriverInfoIntoDb(DriverYakeenInfoRequestModel driverInitialData, DriverYakeenInfoDto driverInfo)
        {           
            var driverData = new Citizen();

            try
            {
                driverData = new Citizen()
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
                citizenDataAccess.Add(driverData);               

            }
            catch(Exception exp)
            {
                citizenRequestLog.Method = "InsertDriverInfoIntoDb";
                citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.MethodException;
                citizenRequestLog.ErrorDescription = exp.ToString();
                citizenRequestLog.NiN = driverInitialData.Nin;
                citizenRequestLog.Channel = driverInitialData.Channel;
                citizenRequestLog.licenseExpiryDate = driverInitialData.licenseExpiryDate;
                citizenRequestLog.ReferenceNumber = driverInitialData.ReferenceNumber;
                citizenRequestLog.IsCitizen = driverInitialData.IsCitizen;
                citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);
            }

            return driverData;
        }

        private static void UpdateCiitizenLicensesInDb(Citizen driverData, DriverYakeenInfoDto driverInfo)
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

                citizenDataAccess.Update(driverData);
            }
            catch(Exception exp)
            {
                citizenRequestLog.Method = "UpdateCiitizenLicensesInDb";
                citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.MethodException;
                citizenRequestLog.ErrorDescription = exp.ToString();              
                citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);
            }
           
        }

        private static Citizen getDriverEntityFromNin(long nin)
        {
            Citizen driverData = null;

            try
            {
               driverData = citizenDataAccess.Find(d => d.NIN == nin.ToString() & !d.IsDeleted,
               e => e.OrderByDescending(x => x.CreatedDate)).
              FirstOrDefault();

                if (driverData == null)
                    return null;              

            }
            catch (Exception exp)
            {
                citizenRequestLog.Method = "getDriverEntityFromNin";
                citizenRequestLog.ErrorCode = CitizenRequestLog.ErrorCodes.MethodException;
                citizenRequestLog.ErrorDescription = exp.ToString();
                citizenRequestLog.NiN = nin;
                citizinLogDataAccess.AddToCistizenLog(citizenRequestLog);
            }

            return driverData;
        }
    }
}