
using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Service.Models;
using System;
using System.Collections.Generic;
using Tameenk.Yakeen.Service.Yakeen4Bcare;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using Tameenk.Yakeen.DAL.Enums;
using Tameenk.Yakeen.Service.Utilities;
using Tameenk.Yakeen.Service.WebClients;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace YakeenComponent
{
    public static class YakeenClient
    {    
        private static Yakeen4BcareClient _client= new Yakeen4BcareClient();
       
       public static YakeenOutput GetVehicleInfo(string referenceNumber, long ownerNin,
            int vehicleId, int vehicleIdTypeId, short modelYear, string channel)
        {
            ServiceRequestLog predefinedLogInfo = new ServiceRequestLog();
            // if car is registered get by seq else get by custom
            if (vehicleIdTypeId == (int)VehicleIdType.SequenceNumber)
            {
                var res = GetCarInfoBySequence(referenceNumber, ownerNin,
             vehicleId, channel);
                if (res.Output.PlateTypeCode == 0)
                    res.Output.PlateTypeCode = 11; // Change from unknown to temp
                return res;
            }
            else
            {
                return GetCarInfoByCustom(referenceNumber, modelYear, vehicleId.ToString(), channel);
            }
        }
        public static YakeenOutput GetVehiclePlateInfo(int vehicleIdTypeId,
            string referenceNumber, long ownerNin, int vehicleId, string Channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = Channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-GetVehiclePlateInfo";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            log.DriverNin = ownerNin.ToString();
            log.ReferenceId = referenceNumber;
            log.VehicleId = vehicleId.ToString();
            log.Channel = Channel;
            DriverYakeenInfoDto vehiclePlateYakeenInfoDto = new DriverYakeenInfoDto();

            try
            {
                if (string.IsNullOrWhiteSpace(referenceNumber))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "request is null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);                  
                    output.DriverYakeenInfoDto = vehiclePlateYakeenInfoDto;
                    return output;
                }
                if (vehicleIdTypeId != (int)VehicleIdType.SequenceNumber)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "Car is not Registered";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);                  
                    output.DriverYakeenInfoDto = vehiclePlateYakeenInfoDto;
                    return output;
                }

                getCarPlateInfoBySequence carPlate = new getCarPlateInfoBySequence();
                carPlate.CarPlateInfoBySequenceRequest = new carPlateInfoBySequenceRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    ownerID = ownerNin,
                    sequenceNumber = vehicleId
                };
                log.ServiceRequest = JsonConvert.SerializeObject(carPlate);
                DateTime dtBeforeCalling = DateTime.Now;
                var response = _client.getCarPlateInfoBySequence(carPlate.CarPlateInfoBySequenceRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);               
                    output.DriverYakeenInfoDto = vehiclePlateYakeenInfoDto;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(response);

              
                vehiclePlateYakeenInfoDto.LogId = response.logId;
                vehiclePlateYakeenInfoDto.ChassisNumber = response.chassisNumber;
                vehiclePlateYakeenInfoDto.OwnerName = response.ownerName;
                vehiclePlateYakeenInfoDto.PlateNumber = response.plateNumber;
                vehiclePlateYakeenInfoDto.PlateText1 = response.plateText1;
                vehiclePlateYakeenInfoDto.PlateText2 = response.plateText2;
                vehiclePlateYakeenInfoDto.PlateText3 = response.plateText3;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.DriverYakeenInfoDto = vehiclePlateYakeenInfoDto;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.ToString();
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();                   
                    output.DriverYakeenInfoDto = vehiclePlateYakeenInfoDto;
                }
                return output;
            }
        }
        public static YakeenOutput GetCustomerNameInfo(string nin
            , string referenceNumber, string DateOfBirth, string Channel, bool isCitizen)
        {
            // if user is citizen so get citizen else get alien 
            if (isCitizen)
            {
                // get citizen name
                return GetCitizenNameInfo(nin, referenceNumber, DateOfBirth, Channel);
            }
            else
            {
                return GetAlienNameInfoByIqama(nin, referenceNumber, DateOfBirth, Channel);
            }
        }
        public static YakeenOutput GetCustomerIdInfo(string nin, string referenceNumber, string dateOfBirth, bool isCitizen, string channel)
        {
            if (isCitizen)
            {
                return GetCitizenIdInfo(nin, referenceNumber, dateOfBirth, channel);
            }
            else
            {
                return GetAlienInfoByIqamaInfo(nin, referenceNumber, dateOfBirth, channel);
            }
        }

        public static YakeenOutput GetCitizenInfo(string referenceNumber,
           string nin, string licenseExpiryDate, string channel)
        {
            return GetCitizenDriverInfo(referenceNumber,nin, licenseExpiryDate, channel);
        }

        public static YakeenOutput GetAlienInfo(string referenceNumber,
            string nin, string licenseExpiryDate, string channel)
        {
                return GetAlienDriverInfoByIqama(referenceNumber,
                 nin, licenseExpiryDate, channel);           
        }

        private static YakeenOutput GetCarInfoBySequence(string referenceNumber, long ownerNin,
            int vehicleId, string channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCarInfoBySequence";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;

            VehicleYakeenInfoDto vehicleYakeenInfoDto = new VehicleYakeenInfoDto();

            try
            {               
                getCarInfoBySequence carSequence = new getCarInfoBySequence();
                carSequence.CarInfoBySequenceRequest = new carInfoBySequenceRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    ownerID = ownerNin,
                    sequenceNumber = vehicleId
                };
                log.ServiceRequest = JsonConvert.SerializeObject(carSequence);
                DateTime dtBeforeCalling = DateTime.Now;
                var response = _client.getCarInfoBySequence(carSequence.CarInfoBySequenceRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.Output = vehicleYakeenInfoDto;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(response);

                vehicleYakeenInfoDto.IsRegistered = true;
                vehicleYakeenInfoDto.Cylinders = response.cylinders;
                vehicleYakeenInfoDto.LicenseExpiryDate = response.licenseExpiryDate;
                vehicleYakeenInfoDto.LogId = response.logId;
                vehicleYakeenInfoDto.MajorColor = response.majorColor;
                vehicleYakeenInfoDto.MinorColor = response.minorColor;
                vehicleYakeenInfoDto.ModelYear = response.modelYear;
                vehicleYakeenInfoDto.PlateTypeCode = response.plateTypeCode;
                vehicleYakeenInfoDto.RegisterationPlace = response.regplace;
                vehicleYakeenInfoDto.BodyCode = response.vehicleBodyCode;
                vehicleYakeenInfoDto.Weight = response.vehicleWeight;
                vehicleYakeenInfoDto.Load = response.vehicleLoad;
                vehicleYakeenInfoDto.MakerCode = response.vehicleMakerCode;
                vehicleYakeenInfoDto.ModelCode = response.vehicleModelCode;
                vehicleYakeenInfoDto.Maker = response.vehicleMaker;
                vehicleYakeenInfoDto.Model = response.vehicleModel;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.Output = vehicleYakeenInfoDto;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                // _logger.Log($"RestfulInsuranceProvider -> ExecuteQuotationRequest - (Provider name: {Configuration.ProviderName})", ex, LogLevel.Error);
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();                  
                    output.Output = vehicleYakeenInfoDto;
                }
                return output;
            }
        }
        private static YakeenOutput GetCarInfoByCustom(string referenceNumber
            , short modelYear, string vehicleId, string channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCarInfoByCustom";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            VehicleYakeenInfoDto res = new VehicleYakeenInfoDto();

            try
            {
                if (string.IsNullOrWhiteSpace(referenceNumber))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "request is null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.Output = res;
                    return output;
                }
                getCarInfoByCustom carCustom = new getCarInfoByCustom();
                carCustom.CarInfoByCustomRequest = new carInfoByCustomRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    modelYear = modelYear,
                    customCardNumber = vehicleId
                };
                log.ServiceRequest = JsonConvert.SerializeObject(carCustom);
                DateTime dtBeforeCalling = DateTime.Now;
                var response = _client.getCarInfoByCustom(carCustom.CarInfoByCustomRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.Output = res;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(response);

                res.IsRegistered = false;
                res.Cylinders = response.cylinders;
                res.LogId = response.logId;
                res.MajorColor = response.majorColor;
                res.MinorColor = response.minorColor;
                res.ModelYear = response.modelYear;
                res.BodyCode = response.vehicleBodyCode;
                res.Weight = response.vehicleWeight;
                res.Load = response.vehicleLoad;
                res.MakerCode = response.vehicleMakerCode;
                res.ModelCode = response.vehicleModelCode;
                res.Maker = response.vehicleMaker;
                res.Model = response.vehicleModel;
                res.PlateTypeCode = null;
                res.ChassisNumber = response.chassisNumber;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.Output = res;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                // _logger.Log($"RestfulInsuranceProvider -> ExecuteQuotationRequest - (Provider name: {Configuration.ProviderName})", ex, LogLevel.Error);
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.Output = res;
                }
                return output;
            }
        }
        private static YakeenOutput GetCitizenNameInfo(string nin
            , string referenceNumber, string DateOfBirth, string Channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = Channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCitizenNameInfo";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            log.DriverNin = nin;
            CustomerIdYakeenInfoDto result = new CustomerIdYakeenInfoDto();

            try
            {
                if (string.IsNullOrWhiteSpace(nin))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "request is null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.CustomerIdYakeenInfoDto = result;

                    return output;
                }
                getCitizenNameInfo citizenName = new getCitizenNameInfo();
                citizenName.CitizenNameInfoRequest = new citizenNameInfoRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    nin = nin,
                    dateOfBirth = DateOfBirth
                };
                log.ServiceRequest = JsonConvert.SerializeObject(citizenName);
                DateTime dtBeforeCalling = DateTime.Now;
                var response = _client.getCitizenNameInfo(citizenName.CitizenNameInfoRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (response == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.CustomerIdYakeenInfoDto = result;
                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(response);

                result.IsCitizen = true;
                result.LogId = response.logId;
                result.FirstName = response.firstName;
                result.SecondName = response.fatherName;
                result.ThirdName = response.grandFatherName;
                result.LastName = response.familyName;
                result.EnglishFirstName = response.englishFirstName;
                result.EnglishSecondName = response.englishSecondName;
                result.EnglishThirdName = response.englishThirdName;
                result.EnglishLastName = response.englishLastName;
                result.SubtribeName = response.subtribeName;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.CustomerIdYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                // _logger.Log($"RestfulInsuranceProvider -> ExecuteQuotationRequest - (Provider name: {Configuration.ProviderName})", ex, LogLevel.Error);
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.CustomerIdYakeenInfoDto = result;
                }

                return output;
            }
        }
        private static YakeenOutput GetAlienNameInfoByIqama(string nin
            , string referenceNumber, string DateOfBirth, string Channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = "Portal";
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getAlienNameInfoByIqama";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            log.DriverNin = nin;
            log.ReferenceId = referenceNumber;
            CustomerIdYakeenInfoDto result = new CustomerIdYakeenInfoDto();

            try
            {
                if (string.IsNullOrWhiteSpace(nin))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullRequest;
                    output.ErrorDescription = "Nin is null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.CustomerIdYakeenInfoDto = result;

                    return output;
                }
                getAlienNameInfoByIqama alienName = new getAlienNameInfoByIqama();
                alienName.AlienNameInfoByIqamaRequest = new alienNameInfoByIqamaRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    iqamaNumber = nin,
                    dateOfBirth = DateOfBirth
                };
                log.ServiceRequest = JsonConvert.SerializeObject(alienName);
                DateTime dtBeforeCalling = DateTime.Now;
                var alianNameInfo = _client.getAlienNameInfoByIqama(alienName.AlienNameInfoByIqamaRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (alianNameInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    output.CustomerIdYakeenInfoDto = result;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(alianNameInfo);
                result.IsCitizen = false;
                result.LogId = alianNameInfo.logId;
                result.FirstName = alianNameInfo.firstName;
                result.SecondName = alianNameInfo.secondName;
                result.ThirdName = alianNameInfo.thirdName;
                result.LastName = alianNameInfo.lastName;
                result.EnglishFirstName = alianNameInfo.englishFirstName;
                result.EnglishSecondName = alianNameInfo.englishSecondName;
                result.EnglishThirdName = alianNameInfo.englishThirdName;
                result.EnglishLastName = alianNameInfo.englishLastName;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.CustomerIdYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                }
                output.CustomerIdYakeenInfoDto = result;
                return output;
            }
        }
        private static YakeenOutput GetCitizenIdInfo(string nin, string referenceNumber, string dateOfBirth, string Channel)
        {
            YakeenOutput output = new YakeenOutput();
            ServiceRequestLog log = new ServiceRequestLog();
            log.Channel = Channel;
            //log.ServiceRequest = JsonConvert.SerializeObject(request);
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCitizenIDInfo";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
            log.DriverNin = nin;
            log.ReferenceId = referenceNumber;
            CustomerIdYakeenInfoDto customerIdYakeenInfoDto = new CustomerIdYakeenInfoDto();
            try
            {
                //if (request == null)
                if (string.IsNullOrEmpty(nin))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NinIsNull;
                    output.ErrorDescription = "Nin Is Null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }
                getCitizenIDInfo citizenId = new getCitizenIDInfo();
                citizenId.CitizenIDInfoRequest = new citizenIDInfoRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    nin = nin,
                    dateOfBirth = dateOfBirth
                };
                log.ServiceRequest = JsonConvert.SerializeObject(citizenId);
                DateTime dtBeforeCalling = DateTime.Now;
                var citizenIdInfo = _client.getCitizenIDInfo(citizenId.CitizenIDInfoRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (citizenIdInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(citizenIdInfo);

                customerIdYakeenInfoDto.IsCitizen = true;
                customerIdYakeenInfoDto.FirstName = citizenIdInfo.firstName;
                customerIdYakeenInfoDto.SecondName = citizenIdInfo.fatherName;
                customerIdYakeenInfoDto.ThirdName = citizenIdInfo.grandFatherName;
                customerIdYakeenInfoDto.LastName = citizenIdInfo.familyName;
                customerIdYakeenInfoDto.EnglishFirstName = citizenIdInfo.englishFirstName;
                customerIdYakeenInfoDto.EnglishSecondName = citizenIdInfo.englishSecondName;
                customerIdYakeenInfoDto.EnglishThirdName = citizenIdInfo.englishThirdName;
                customerIdYakeenInfoDto.EnglishLastName = citizenIdInfo.englishLastName;
                customerIdYakeenInfoDto.SubtribeName = citizenIdInfo.subtribeName;
                customerIdYakeenInfoDto.Gender = ConvertYakeenGenderEnumToTameenkGenderEnum(citizenIdInfo.gender);
                customerIdYakeenInfoDto.LogId = citizenIdInfo.logId;
                customerIdYakeenInfoDto.DateOfBirthG = DateTime.ParseExact(citizenIdInfo.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                customerIdYakeenInfoDto.DateOfBirthH = citizenIdInfo.dateOfBirthH;
                customerIdYakeenInfoDto.IdIssuePlace = citizenIdInfo.idIssuePlace;
                customerIdYakeenInfoDto.IdExpiryDate = citizenIdInfo.idExpiryDate;
                customerIdYakeenInfoDto.NationalityCode = (short) RepositoryConstants.SaudiNationalityCode;
                customerIdYakeenInfoDto.SocialStatus = citizenIdInfo.socialStatusDetailedDesc;
                customerIdYakeenInfoDto.OccupationCode = citizenIdInfo.occupationCode;
                customerIdYakeenInfoDto.licenseListListField = citizenIdInfo.licenseListList;

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.CustomerIdYakeenInfoDto = customerIdYakeenInfoDto;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.CustomerIdYakeenInfoDto = customerIdYakeenInfoDto;
                }
                return output;
            }
        }
        private static YakeenOutput GetAlienInfoByIqamaInfo(string nin, string referenceNumber, string dateOfBirth, string channel)
        {
            YakeenOutput output = new YakeenOutput();
            ServiceRequestLog log = new ServiceRequestLog();
            log.Channel = channel;
            // log.ServiceRequest = JsonConvert.SerializeObject(request);
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getAlienInfoByIqama";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;

            CustomerIdYakeenInfoDto result = new CustomerIdYakeenInfoDto();
            try
            {
                if (string.IsNullOrEmpty(nin))
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NinIsNull;
                    output.ErrorDescription = "Nin Is Null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }
                getAlienInfoByIqama alienId = new getAlienInfoByIqama();
                alienId.AlienInfoByIqamaRequest = new alienInfoByIqamaRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    iqamaNumber = nin,
                    dateOfBirth = dateOfBirth
                };
                log.ServiceRequest = JsonConvert.SerializeObject(alienId);
                DateTime dtBeforeCalling = DateTime.Now;
                var alianInfo = _client.getAlienInfoByIqama(alienId.AlienInfoByIqamaRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (alianInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(alianInfo);

                
                OccupationDataAccess occupationDataAccess = new OccupationDataAccess();
                var occupations = occupationDataAccess.GetAll();
                result.IsCitizen = false;

                result.FirstName = alianInfo.firstName;
                result.SecondName = alianInfo.secondName;
                result.ThirdName = alianInfo.thirdName;
                result.LastName = alianInfo.lastName;
                result.EnglishFirstName = alianInfo.englishFirstName;
                result.EnglishSecondName = alianInfo.englishSecondName;
                result.EnglishThirdName = alianInfo.englishThirdName;
                result.EnglishLastName = alianInfo.englishLastName;
                result.NationalityCode = alianInfo.nationalityCode;
                result.Gender = ConvertYakeenGenderEnumToTameenkGenderEnum(alianInfo.gender);
                result.LogId = alianInfo.logId;
                result.DateOfBirthG = DateTime.ParseExact(alianInfo.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                result.DateOfBirthH = alianInfo.dateOfBirthH;
                result.IdIssuePlace = alianInfo.iqamaIssuePlaceDesc;
                result.IdExpiryDate = alianInfo.iqamaExpiryDateH;
                result.SocialStatus = alianInfo.socialStatus;
                result.OccupationCode = occupations.FirstOrDefault(x => x.NameAr.Trim() == alianInfo.occupationDesc || x.NameEn.Trim().ToUpper() == alianInfo.occupationDesc.ToUpper())?.Code;
                if (string.IsNullOrEmpty(result.OccupationCode))
                    result.OccupationCode = occupations.FirstOrDefault(x => x.NameAr.Trim().Contains(alianInfo.occupationDesc) || x.NameEn.Trim().ToUpper().Contains(alianInfo.occupationDesc.ToUpper()))?.Code;
                var licenseListListField = new List<licenseList>();
                if (alianInfo.licensesListList != null)
                {
                    foreach (var item in alianInfo.licensesListList)
                    {
                        licenseListListField.Add(new licenseList()
                        {
                            licnsTypeDesc = item.licnsTypeDesc,
                            licssExpiryDateH = item.licssExpiryDateH,
                            licssIssueDate = item.licssIssueDate
                        });
                    }
                    result.licenseListListField = licenseListListField.ToArray();
                }

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.CustomerIdYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.CustomerIdYakeenInfoDto = result;
                }
                return output;
            }
        }
        private static YakeenOutput GetCitizenDriverInfo(string referenceNumber,
            string nin, string licenseExpiryDate, string channel)
        {          
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCitizenDriverInfo";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;
                       
            DriverYakeenInfoDto result = new DriverYakeenInfoDto();

            try
            {               
                getCitizenDriverInfo citizenId = new getCitizenDriverInfo();
                citizenId.CitizenDriverInfoRequest = new citizenDriverInfoRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    nin = nin,
                    licExpiryDate = licenseExpiryDate
                };

                log.ServiceRequest = JsonConvert.SerializeObject(citizenId);

                DateTime dtBeforeCalling = DateTime.Now;
                var driverInfo = _client.getCitizenDriverInfo(citizenId.CitizenDriverInfoRequest);

                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (driverInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    output.DriverYakeenInfoDto = result;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(driverInfo);

                result.IsCitizen = true;
                result.Gender = ConvertYakeenGenderEnumToTameenkGenderEnum(driverInfo.gender);
                result.FirstName = driverInfo.firstName;
                result.SecondName = driverInfo.fatherName;
                result.ThirdName = driverInfo.grandFatherName;
                result.LastName = driverInfo.familyName;
                result.SubtribeName = driverInfo.subtribeName;
                result.EnglishFirstName = driverInfo.englishFirstName;
                result.EnglishSecondName = driverInfo.englishSecondName;
                result.EnglishThirdName = driverInfo.englishThirdName;
                result.EnglishLastName = driverInfo.englishLastName;
                result.DateOfBirthG = DateTime.ParseExact(driverInfo.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                result.DateOfBirthH = driverInfo.dateOfBirthH;
                result.NationalityCode = (short)RepositoryConstants.SaudiNationalityCode;

                foreach (var lic in driverInfo.licenseListList)
                {
                    result.Licenses.Add(new DriverLicenseYakeenInfoDto
                    {
                        TypeDesc = lic.licnsTypeCode,
                        ExpiryDateH = lic.licssExpiryDateH
                    });
                }

                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.DriverYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException exp)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = exp.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = exp.ToString();
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = exp.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.DriverYakeenInfoDto = result;
                }
                return output;
            }
        }
        private static YakeenOutput GetAlienDriverInfoByIqama(string referenceNumber,
            string nin, string licenseExpiryDate, string channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();

            YakeenOutput output = new YakeenOutput();
            log.Channel = "Portal";
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getAlienDriverInfoByIqama";
            log.DriverNin = nin;
            log.ReferenceId = referenceNumber;           

            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;

            AlienYakeenInfoDto result = new AlienYakeenInfoDto();

            try
            {               
                getCitizenDriverInfo citizenId = new getCitizenDriverInfo();
                getAlienDriverInfoByIqama alienId = new getAlienDriverInfoByIqama();
                alienId.AlienDriverInfoByIqamaRequest = new alienDriverInfoByIqamaRequest()
                {
                    userName = RepositoryConstants.YakeenUserName,
                    password = RepositoryConstants.YakeenPassword,
                    chargeCode = RepositoryConstants.YakeenChargeCode,
                    referenceNumber = referenceNumber,
                    iqamaNumber = nin,
                    licExpiryDate = licenseExpiryDate
                };
                log.ServiceRequest = JsonConvert.SerializeObject(citizenId);
                DateTime dtBeforeCalling = DateTime.Now;
                var driverInfo = _client.getAlienDriverInfoByIqama(alienId.AlienDriverInfoByIqamaRequest);
                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (driverInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);

                    output.AlienYakeenInfoDto = result;

                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(driverInfo);

                result.IsCitizen = false;
                result.Gender = ConvertYakeenGenderEnumToTameenkGenderEnum(driverInfo.gender);
                result.NationalityCode = driverInfo.nationalityCode;
                result.FirstName = driverInfo.firstName;
                result.SecondName = driverInfo.secondName;
                result.ThirdName = driverInfo.thirdName;
                result.LastName = driverInfo.lastName;
                result.EnglishFirstName = driverInfo.englishFirstName;
                result.EnglishSecondName = driverInfo.englishSecondName;
                result.EnglishThirdName = driverInfo.englishThirdName;
                result.EnglishLastName = driverInfo.englishLastName;
                result.DateOfBirthG = DateTime.ParseExact(driverInfo.dateOfBirthG, "dd-MM-yyyy", new CultureInfo("en-US"));
                result.DateOfBirthH = driverInfo.dateOfBirthH;

                if (driverInfo.licensesListList != null)
                {
                    foreach (var lic in driverInfo.licensesListList)
                    {
                        result.Licenses.Add(new DriverLicenseYakeenInfoDto
                        {
                            TypeDesc = lic.licnsTypeCode,
                            ExpiryDateH = lic.licssExpiryDateH
                        });
                    }
                }
                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.AlienYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();
                    output.AlienYakeenInfoDto = result;
                }
                return output;
            }
        }
        private static EGender ConvertYakeenGenderEnumToTameenkGenderEnum(gender gender)
        {
            switch (gender)
            {
                case gender.M:
                    return EGender.M;
                case gender.F:
                    return EGender.F;
                case gender.U:
                    return EGender.U;
            }
            return EGender.U;
        }
  
        public static YakeenOutput GetCompanyInfo(string chargeCode,string iqamaNumber,
            string password,
            string referenceNumber,
            string sponsorNumber,
            string userName,
            string channel)
        {
            ServiceRequestLog log = new ServiceRequestLog();
            YakeenOutput output = new YakeenOutput();
            log.Channel = channel;
            log.ServerIP = ServicesUtilities.GetServerIP();
            log.Method = "Yakeen-getCompanyInfo";
            log.ServiceURL = _client.Endpoint.ListenUri.AbsoluteUri;

            CompanyYakeenInfoDto result = new CompanyYakeenInfoDto();

            try
            {                
                var request = new companyInfoBySponseredIqamaRequest
                {
                    chargeCode = chargeCode,
                    iqamaNumber = iqamaNumber,                   
                    referenceNumber = referenceNumber,
                    sponsorNumber = sponsorNumber,
                    userName = userName,
                    password = password,
                };

                log.ServiceRequest = JsonConvert.SerializeObject(sponsorNumber);
                DateTime dtBeforeCalling = DateTime.Now;

                companyInfoBySponseredIqamaResult companyInfo = _client.getCompanyInfoBySponseredIqama(request);

                DateTime dtAfterCalling = DateTime.Now;
                log.ServiceResponseTimeInSeconds = dtAfterCalling.Subtract(dtBeforeCalling).TotalSeconds;

                if (companyInfo == null)
                {
                    output.ErrorCode = YakeenOutput.ErrorCodes.NullResponse;
                    output.ErrorDescription = "response return null";
                    log.ErrorCode = (int)output.ErrorCode;
                    log.ErrorDescription = output.ErrorDescription;
                    log.ServiceErrorCode = log.ErrorCode.ToString();
                    log.ServiceErrorDescription = log.ServiceErrorDescription;
                    ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);                   
                    return output;
                }
                log.ServiceResponse = JsonConvert.SerializeObject(companyInfo);

                
                result.LogId = companyInfo.logId;
                result.SponsorName = companyInfo.sponsorName;
                result.TotalNumberOfSponsoredDependents= companyInfo.totalNumberOfSponsoredDependents;
                result.TotalNumberOfSponsoreds = companyInfo.totalNumberOfSponsoreds;                 
                
                output.ErrorCode = YakeenOutput.ErrorCodes.Success;
                output.ErrorDescription = "Success";
                output.CompanyYakeenInfoDto = result;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                log.ServiceErrorCode = log.ErrorCode.ToString();
                log.ServiceErrorDescription = log.ServiceErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                return output;
            }
            catch (System.ServiceModel.FaultException ex)
            {
                output.ErrorCode = YakeenOutput.ErrorCodes.ServiceException;
                output.ErrorDescription = ex.GetBaseException().Message;
                log.ErrorCode = (int)output.ErrorCode;
                log.ErrorDescription = output.ErrorDescription;
                ServiceRequestLogDataAccess.AddtoServiceRequestLogs(log);
                var msgFault = ex.CreateMessageFault();
                if (msgFault.HasDetail)
                {
                    var errorDetail = msgFault.GetDetail<Yakeen4BcareFault>();                   
                    output.CompanyYakeenInfoDto = result;
                }
                return output;
            }
        }
    

    }
}
