using Tameenk.Yakeen.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class Vehicle 
    {
        public Vehicle()
        {   
            VehicleUseId = (int)VehicleUse.Private;
            ParkingLocationId = (int)ParkingLocation.RoadSide;
            AxleWeightId = null;
            MileageExpectedAnnualId = null;
            TransmissionTypeId = null;
            EngineSizeId = null;
            CreatedDate = DateTime.Now;
        }

        public int ID { get; set; }

        public string SequenceNumber { get; set; }

        public string CustomCardNumber { get; set; }     

        public byte? Cylinders { get; set; }

        public string LicenseExpiryDate { get; set; }
        public string MajorColor { get; set; }

        public string MinorColor { get; set; }

        public short? ModelYear { get; set; }

        public byte? PlateTypeCode { get; set; }

        public string RegisterationPlace { get; set; }

        public byte VehicleBodyCode { get; set; }

        public int VehicleWeight { get; set; }

        public int VehicleLoad { get; set; }

        public string VehicleMaker { get; set; }

        public string VehicleModel { get; set; }

        public string ChassisNumber { get; set; }

        public short? VehicleMakerCode { get; set; }

        public long? VehicleModelCode { get; set; }
        public string CarPlateText1 { get; set; }
        public string CarPlateText2 { get; set; }
        public string CarPlateText3 { get; set; }
        public short? CarPlateNumber { get; set; }
        public string CarOwnerNIN { get; set; }
        public string CarOwnerName { get; set; }
        public int? VehicleValue { get; set; }
        public bool? IsUsedCommercially { get; set; }
        public bool? OwnerTransfer { get; set; }
        public int? EngineSizeId { get; set; }
        public int VehicleUseId { get; set; }
        public decimal? CurrentMileageKM { get; set; }
        public int? TransmissionTypeId { get; set; }
        public int? MileageExpectedAnnualId { get; set; }
        public int? AxleWeightId { get; set; }
        public int? ParkingLocationId { get; set; }
        public bool? HasModifications { get; set; }
        public bool? HasAntiTheftAlarm { get; set; }
        public bool? HasFireExtinguisher { get; set; }
        public string ModificationDetails { get; set; }
        public BrakingSystem? BrakeSystemId { get; set; }
        public CruiseControlType? CruiseControlTypeId { get; set; }
        public ParkingSensors? ParkingSensorId { get; set; }
        public VehicleCameraType? CameraTypeId { get; set; }
        public int? VehicleIdTypeId { get; set; }     

        public AxlesWeight? AxlesWeight
        {
            get { return (AxlesWeight)AxleWeightId.GetValueOrDefault(); }
            set { AxleWeightId = null; }
        }

        /// <summary>
        /// Expected mileage usage of this vehicle in kilometers.
        /// </summary>
        public Mileage? MileageExpectedAnnual
        {
            get { return (Mileage)MileageExpectedAnnualId.GetValueOrDefault(); }
            set { MileageExpectedAnnualId = null; }
        }

        /// <summary>
        /// Transmission type.
        /// </summary>
        public TransmissionType? TransmissionType
        {
            get { return (TransmissionType)TransmissionTypeId.GetValueOrDefault(); }
            set { TransmissionTypeId = null; }
        }
        /// <summary>
        /// Vehicle usage.
        /// </summary>
        //public VehicleUse? VehicleUse
        //{
        //    get { return (VehicleUse)VehicleUseId; }
        //    set { VehicleUseId = (int)VehicleUse.Private; }
        //}
        /// <summary>
        /// The engine size.
        /// </summary>
        public EngineSize? EngineSize
        {
            get { return (EngineSize)EngineSizeId.GetValueOrDefault(); }
            set { EngineSizeId = null; }
        }

        /// <summary>
        /// The vehicle identity type.
        /// </summary>
        //public VehicleIdType VehicleIdType
        //{
        //    get { return (VehicleIdType)VehicleIdTypeId; }
        //    set { VehicleIdTypeId = (int)value; }
        //}
       // public ICollection<CheckoutDetail> CheckoutDetails { get; set; }

        //public ICollection<QuotationRequest> QuotationRequests { get; set; }

        //public VehicleBodyType VehicleBodyType { get; set; }

       // public VehiclePlateType VehiclePlateType { get; set; }

       // public ICollection<VehicleSpecification> VehicleSpecifications { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string ServerIP { set; get; }
        public string UserIP { set; get; }
        public string UserAgent { set; get; }
    }
}
