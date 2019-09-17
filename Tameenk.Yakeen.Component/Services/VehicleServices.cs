using Tameenk.Yakeen;
using Tameenk.Yakeen.DAL.Enums;
using Tameenk.Yakeen.DAL;
using Tameenk.Yakeen.Component.Extensions;
using Tameenk.Yakeen.Service.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tameenk.Yakeen.Component;
using Tameenk.Yakeen.Service.Utilities;

namespace YakeenComponent
{
    public static class VehicleServices 
    { 
        private static readonly VehicleDataAccess vehicleService = new VehicleDataAccess();
        private static VehicleOutput vehicleOutput = new VehicleOutput();
        private static VehicleRequestLogDataAccess vehicleDataAccess = new VehicleRequestLogDataAccess();
        private static VehicleRequestLog vehicleLog = new VehicleRequestLog();
        private static ChannelDataAccess channelData = new ChannelDataAccess();

        public static VehicleOutput GetVehicleByTameenkId(int vehicleId)
        {
            vehicleLog.Method = "GetVehicleByTameenkId";
            vehicleLog.ServerIP = Utilities.GetServerIP();

            try
            {
                if (vehicleId == 0)
                {
                    vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.NullRequest;
                    vehicleLog.ErrorDescription = "Vehicle Id is null";
                    vehicleLog.VehicleId = vehicleId;
                    vehicleDataAccess.AddToVehicleLog(vehicleLog);

                    vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullRequest;
                    vehicleOutput.ErrorDescription = "GetVehicleByTameenkId vehicleId is null";
                    vehicleOutput.Vehicle = null;

                    return vehicleOutput;
                }

                VehicleYakeenModel vehicle = null;
                Vehicle vehicleData = vehicleService.GetSingleOrDefault(v => v.ID == vehicleId);

                if (vehicleData != null)
                {
                    vehicle = vehicleData.ToModel();

                    vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.Success;
                    vehicleOutput.ErrorDescription = "Success";
                    vehicleOutput.Vehicle = vehicle;
                }
                else
                {
                    vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullResponse;
                    vehicleOutput.ErrorDescription = "Vehicle ID not exist";
                    vehicleOutput.Vehicle = null;
                }

            }
            catch(Exception exp)
            {
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleDataAccess.AddToVehicleLog(vehicleLog);

                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.MethodException;
                vehicleOutput.ErrorDescription = "GetVehicleByTameenkId through exception";
                vehicleOutput.Vehicle = null;
            }
            
            return vehicleOutput;
        }

        public static VehicleOutput GetVehicleByOfficialId(VehicleInfoRequestModel vehicleInfoRequest)
        {
            ServicesUtilities.AcceptCertificate();

            vehicleLog.Method = "GetDriverByOfficialIdAndLicenseExpiryDate";
            vehicleLog.ServerIP = Utilities.GetServerIP();          

            if (vehicleInfoRequest==null)
            {
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.NullRequest;
                vehicleLog.ErrorDescription = "VehicleInfoRequest model is null";
                vehicleDataAccess.AddToVehicleLog(vehicleLog);               

                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullRequest;
                vehicleOutput.ErrorDescription = "VehicleInfoRequest model is null";
                vehicleOutput.Vehicle = null;

                return vehicleOutput;
            }

            vehicleLog.VehicleId = vehicleInfoRequest.VehicleId;
            vehicleLog.VehicleIdTypeId = vehicleInfoRequest.VehicleIdTypeId;
            vehicleLog.OwnerNin = vehicleInfoRequest.OwnerNin;
            vehicleLog.IsOwnerTransfer = vehicleInfoRequest.IsOwnerTransfer;
            vehicleLog.Channel = vehicleInfoRequest.Channel;
            vehicleLog.ReferenceNumber = vehicleInfoRequest.ReferenceNumber;
            vehicleLog.ModelYear = vehicleInfoRequest.ModelYear;

            if (vehicleInfoRequest.VehicleId== 0
                || vehicleInfoRequest.VehicleIdTypeId==0
                || vehicleInfoRequest.OwnerNin==0 
                || vehicleInfoRequest.Channel==0
                || string.IsNullOrEmpty(vehicleInfoRequest.ReferenceNumber)
                || vehicleInfoRequest.ModelYear==0)
            {
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.NullRequest;
                vehicleLog.ErrorDescription = "Request mandatory fields are not exist";              
                vehicleDataAccess.AddToVehicleLog(vehicleLog);

                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullRequest;
                vehicleOutput.ErrorDescription = "Request mandatory fields are not exist";
                vehicleOutput.Vehicle = null;

                return vehicleOutput;
            }

            try
            {
                Vehicle vehicleData = GetVehicleEntity(vehicleInfoRequest.VehicleId,
                vehicleInfoRequest.VehicleIdTypeId, vehicleInfoRequest.IsOwnerTransfer);

                int channelExpireByDays = channelData.GetChannelExpireDateByID(vehicleInfoRequest.Channel);
                VehicleYakeenModel vehicle = null;

                if (vehicleData == null || vehicleData.CreatedDate >= DateTime.Now.AddDays(channelExpireByDays))
                {
                    var YakeenOutput = YakeenClient.GetVehicleInfo(vehicleInfoRequest.ReferenceNumber, vehicleInfoRequest.OwnerNin,
                    Convert.ToInt32(vehicleInfoRequest.VehicleId), vehicleInfoRequest.VehicleIdTypeId, vehicleInfoRequest.ModelYear.Value, vehicleInfoRequest.Channel.ToString());

                    var vehicleInfoFromYakeen = YakeenOutput.Output;

                    if (vehicleInfoFromYakeen != null &&
                        YakeenOutput.ErrorCode == Tameenk.Yakeen.Service.WebClients.YakeenOutput.ErrorCodes.Success)
                    {
                        DriverYakeenInfoDto vehiclePlateInfoFromYakeen = null;
                        if (vehicleInfoRequest.VehicleIdTypeId == (int)VehicleIdType.SequenceNumber)
                        {
                            vehiclePlateInfoFromYakeen = YakeenClient.GetVehiclePlateInfo(vehicleInfoRequest.VehicleIdTypeId,
                               vehicleInfoRequest.ReferenceNumber, vehicleInfoRequest.OwnerNin,
                               Convert.ToInt32(vehicleInfoRequest.VehicleId),
                               vehicleInfoRequest.Channel.ToString()).DriverYakeenInfoDto;

                            if (vehiclePlateInfoFromYakeen == null ||
                                YakeenOutput.ErrorCode != Tameenk.Yakeen.Service.WebClients.YakeenOutput.ErrorCodes.Success)
                            {
                                vehicle = new VehicleYakeenModel();

                                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullResponse;
                                vehicleOutput.ErrorDescription = "Yakeen null response Vehicle Plate";
                                vehicleOutput.Vehicle = null;
                            }
                        }

                        if (vehicle == null)
                        {
                            vehicleData = InsertVehicleInfoIntoDb(vehicleInfoRequest, vehicleInfoFromYakeen, vehiclePlateInfoFromYakeen);

                            if(vehicleData!=null)
                            {
                                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.Success;
                                vehicleOutput.ErrorDescription = "Success";
                                vehicleOutput.Vehicle = vehicle;
                            }
                            else
                            {
                                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.UnspecifiedError;
                                vehicleOutput.ErrorDescription = "Failed to insert vehicle";
                                vehicleOutput.Vehicle = null;
                            }                          
                        }
                    }
                    else
                    {                       
                        vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.NullResponse;
                        vehicleOutput.ErrorDescription = YakeenOutput.ErrorDescription;
                        vehicleOutput.Vehicle = null;
                    }
                }
                else
                {
                    vehicleData.OwnerTransfer = vehicleInfoRequest.IsOwnerTransfer;
                    vehicleData.VehicleValue = vehicleInfoRequest.VehicleValue;
                    vehicleData.IsUsedCommercially = vehicleInfoRequest.IsUsedCommercially;
                    vehicleData.TransmissionTypeId = vehicleInfoRequest.TransmissionTypeId;
                    vehicleData.ParkingLocationId = vehicleInfoRequest.ParkingLocationId;
                    vehicleData.HasModifications = vehicleInfoRequest.HasModification;
                    vehicleData.ModificationDetails = vehicleInfoRequest.Modification;

                    vehicleService.Update(vehicleData);

                    vehicle = vehicleData.ToModel();

                    vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.Success;
                    vehicleOutput.ErrorDescription = "Success";
                    vehicleOutput.Vehicle = vehicle;
                }
            }
            catch(Exception exp)
            {
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleLog.VehicleId = vehicleInfoRequest.VehicleId;
                vehicleLog.VehicleIdTypeId = vehicleInfoRequest.VehicleIdTypeId;
                vehicleLog.OwnerNin = vehicleInfoRequest.OwnerNin;
                vehicleLog.IsOwnerTransfer = vehicleInfoRequest.IsOwnerTransfer;
                vehicleLog.Channel = vehicleInfoRequest.Channel;
                vehicleLog.ReferenceNumber = vehicleInfoRequest.ReferenceNumber;
                vehicleLog.ModelYear = vehicleInfoRequest.ModelYear;
                vehicleDataAccess.AddToVehicleLog(vehicleLog);

                vehicleOutput.ErrorCode = VehicleOutput.ErrorCodes.MethodException;
                vehicleOutput.ErrorDescription = "GetVehicleByOfficialId through exception";
                vehicleOutput.Vehicle = null;
            }

            return vehicleOutput;
        }

        private static Vehicle InsertVehicleInfoIntoDb(VehicleInfoRequestModel vehicleInitialData, VehicleYakeenInfoDto vehicleInfo, DriverYakeenInfoDto vehiclePlateInfo)
        {
            var vehicleData = new Vehicle();

            try
            {
                vehicleData = new Vehicle()
                {
                    //ID = Guid.NewGuid(),
                    VehicleIdTypeId = vehicleInitialData.VehicleIdTypeId,
                    Cylinders = byte.Parse(vehicleInfo.Cylinders.ToString()),
                    LicenseExpiryDate = vehicleInfo.LicenseExpiryDate,
                    MajorColor = vehicleInfo.MajorColor,
                    MinorColor = vehicleInfo.MinorColor,
                    ModelYear = vehicleInfo.ModelYear,
                    PlateTypeCode = (byte?)vehicleInfo.PlateTypeCode,
                    RegisterationPlace = vehicleInfo.RegisterationPlace,
                    VehicleBodyCode = byte.Parse(vehicleInfo.BodyCode.ToString()),
                    VehicleWeight = vehicleInfo.Weight,
                    VehicleLoad = vehicleInfo.Load,
                    VehicleMaker = vehicleInfo.Maker,
                    VehicleModel = vehicleInfo.Model,
                    VehicleMakerCode = (short)vehicleInfo.MakerCode,
                    VehicleModelCode = (short)vehicleInfo.ModelCode,
                    CarOwnerNIN = vehicleInitialData.OwnerNin.ToString(),
                    VehicleValue = vehicleInitialData.VehicleValue,
                    IsUsedCommercially = vehicleInitialData.IsUsedCommercially,
                    OwnerTransfer = vehicleInitialData.IsOwnerTransfer,
                    HasModifications = vehicleInitialData.HasModification,
                    ModificationDetails = vehicleInitialData.Modification
                };
                vehicleData.BrakeSystemId = (BrakingSystem?)vehicleInitialData.BrakeSystemId;
                vehicleData.CruiseControlTypeId = (CruiseControlType?)vehicleInitialData.CruiseControlTypeId;
                vehicleData.ParkingSensorId = (ParkingSensors?)vehicleInitialData.ParkingSensorId;
                vehicleData.CameraTypeId = (VehicleCameraType?)vehicleInitialData.CameraTypeId;
                vehicleData.CurrentMileageKM = vehicleInitialData.CurrentMileageKM;
                vehicleData.HasAntiTheftAlarm = vehicleInitialData.HasAntiTheftAlarm;
                vehicleData.HasFireExtinguisher = vehicleInitialData.HasFireExtinguisher;

                if (vehicleInitialData.VehicleIdTypeId == (int)VehicleIdType.SequenceNumber)
                {
                    //in sequence number type, plate info should not be null
                    if (vehiclePlateInfo == null)
                        throw new TameenkNullReferenceException("Car plate info is null and vehcile type is sequence number");

                    vehicleData.ChassisNumber = vehiclePlateInfo.ChassisNumber;
                    vehicleData.SequenceNumber = vehicleInitialData.VehicleId.ToString();
                    vehicleData.CustomCardNumber = null;
                    vehicleData.CarPlateText1 = vehiclePlateInfo.PlateText1;
                    vehicleData.CarPlateText2 = vehiclePlateInfo.PlateText2;
                    vehicleData.CarPlateText3 = vehiclePlateInfo.PlateText3;
                    vehicleData.CarPlateNumber = vehiclePlateInfo.PlateNumber;
                    vehicleData.CarOwnerName = vehiclePlateInfo.OwnerName;
                }
                //custom card
                else
                {
                    vehicleData.ChassisNumber = vehicleInfo.ChassisNumber;
                    vehicleData.SequenceNumber = null;
                    vehicleData.CustomCardNumber = vehicleInitialData.VehicleId.ToString();
                    vehicleData.CarPlateText1 = vehicleData.CarPlateText2 = vehicleData.CarPlateText3 = null;
                    vehicleData.CarPlateNumber = null;
                    //change request that make the model empty in custom card case if yakeen send it as " غير معرف"
                    if (vehicleData.VehicleModel.Contains("غير معرف") || vehicleData.VehicleModel.Contains("غير متوفر"))
                        vehicleData.VehicleModel = "";
                }

                if (!vehicleData.VehicleModelCode.HasValue || vehicleData.VehicleModelCode.Value == 0)
                {
                    var vehicleModel = GetVehicleModelByName(vehicleData.VehicleModel, (int)vehicleData.VehicleMakerCode);
                    if (vehicleModel != null)
                    {
                        vehicleData.VehicleModelCode = (short)vehicleModel.Code;
                    }
                    else
                    {
                        vehicleData.VehicleModelCode = GetVehicleCode(vehicleData.VehicleModel, (short)vehicleData.VehicleMakerCode);
                    }
                }

                vehicleService.Add(vehicleData);

            }
            catch(Exception exp)
            {
                vehicleLog.Method = "InsertVehicleInfoIntoDb";
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleDataAccess.AddToVehicleLog(vehicleLog);                       

            }    

            return vehicleData;
        }

        private static VehicleModel GetVehicleModelByName(string vehicleModelName, int? vehicleMakerId)
        {
            VehicleModel vehicleModel= new VehicleModel();

            try
            {
                if (vehicleMakerId == null)
                    return null;

                if (string.IsNullOrWhiteSpace(vehicleModelName))
                    return null;

                var res = vehicleService.VehicleModels(vehicleMakerId.Value);

                vehicleModel = res.FirstOrDefault(e => e.ArabicDescription.Trim() == vehicleModelName); ;

            }
            catch(Exception exp)
            {
                vehicleLog.Method = "GetVehicleModelByName";
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleDataAccess.AddToVehicleLog(vehicleLog);          
           
            }
           
            return vehicleModel;

        }

        private static short? GetVehicleCode(string VehicleModel, short VehicleMakerCode)
        {
            try
            {
                string[] numbers = Regex.Split(VehicleModel, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        char[] charArray = value.ToCharArray();
                        Array.Reverse(charArray);
                        VehicleModel = VehicleModel.Replace(value, new string(charArray));
                        var model = GetVehicleModelByName(VehicleModel, (int)VehicleMakerCode);
                        if (model != null)
                            return (short)model.Code;
                    }
                }

            }
            catch(Exception exp)
            {
                vehicleLog.Method = "GetVehicleCode";
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleDataAccess.AddToVehicleLog(vehicleLog);

            }           

            return null;
        }
        private static Vehicle GetVehicleEntity(int vehicleId, int VehicleIdTypeId, bool isOwershipTransfere)
        {
            Vehicle vehicleData = null;

            try
            {
                if (VehicleIdTypeId == (int)VehicleIdType.SequenceNumber)
                    vehicleData = vehicleService.Find(v => !v.IsDeleted && v.ID == vehicleId, e =>
                     e.OrderBy(x => x.CreatedDate)).FirstOrDefault();
                else
                    vehicleData = vehicleService.Find(v => !v.IsDeleted && v.CustomCardNumber == vehicleId.ToString()
                    , e => e.OrderBy(x => x.CreatedDate)).FirstOrDefault();

                if (vehicleData == null)
                {
                    return null;
                }

            }
            catch(Exception exp)
            {
                vehicleLog.Method = "GetVehicleCode";
                vehicleLog.ErrorCode = VehicleRequestLog.ErrorCodes.MethodException;
                vehicleLog.ErrorDescription = exp.ToString();
                vehicleDataAccess.AddToVehicleLog(vehicleLog);

            }
            
            return vehicleData;
        }

    }
}