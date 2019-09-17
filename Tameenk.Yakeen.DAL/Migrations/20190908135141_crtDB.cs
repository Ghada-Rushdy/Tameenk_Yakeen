using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tameenk.Yakeen.DAL.Migrations
{
    public partial class crtDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlienRequestLogs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<int>(nullable: false),
                    ErrorDescription = table.Column<string>(nullable: true),
                    AlienID = table.Column<Guid>(nullable: false),
                    Method = table.Column<string>(nullable: true),
                    ServiceURL = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    NiN = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlienRequestLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ExpireDateInDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequestLogs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<int>(nullable: false),
                    ErrorDescription = table.Column<string>(nullable: true),
                    CitizenId = table.Column<Guid>(nullable: false),
                    Method = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    NiN = table.Column<long>(nullable: false),
                    licenseExpiryDate = table.Column<string>(nullable: true),
                    IsCitizen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    logId = table.Column<int>(nullable: false),
                    SponsorName = table.Column<string>(nullable: true),
                    SponsorId = table.Column<string>(nullable: true),
                    TotalNumberOfSponsoredDependents = table.Column<int>(nullable: false),
                    TotalNumberOfSponsoreds = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRequestLogs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<int>(nullable: false),
                    ErrorDescription = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    ServiceURL = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    sponsorNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRequestLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LicenseType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<short>(nullable: false),
                    EnglishDescription = table.Column<string>(nullable: true),
                    ArabicDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Occupation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    NameAr = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    IsCitizen = table.Column<bool>(nullable: true),
                    IsMale = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occupation", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UserID = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 255, nullable: true),
                    Method = table.Column<string>(maxLength: 255, nullable: true),
                    CompanyID = table.Column<int>(nullable: true),
                    CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    ServiceURL = table.Column<string>(maxLength: 255, nullable: true),
                    ErrorCode = table.Column<int>(nullable: true),
                    ErrorDescription = table.Column<string>(nullable: true),
                    ServiceRequest = table.Column<string>(nullable: true),
                    ServiceResponse = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(maxLength: 50, nullable: true),
                    RequestId = table.Column<Guid>(nullable: true),
                    ServiceResponseTimeInSeconds = table.Column<double>(nullable: true),
                    Channel = table.Column<string>(maxLength: 255, nullable: true),
                    ServiceErrorCode = table.Column<string>(maxLength: 50, nullable: true),
                    ServiceErrorDescription = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(nullable: true),
                    InsuranceTypeCode = table.Column<int>(nullable: true),
                    DriverNin = table.Column<string>(nullable: true),
                    VehicleId = table.Column<string>(nullable: true),
                    PolicyNo = table.Column<string>(nullable: true),
                    VehicleMaker = table.Column<string>(nullable: true),
                    VehicleMakerCode = table.Column<string>(nullable: true),
                    VehicleModel = table.Column<string>(nullable: true),
                    VehicleModelCode = table.Column<string>(nullable: true),
                    VehicleModelYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SequenceNumber = table.Column<string>(nullable: true),
                    CustomCardNumber = table.Column<string>(nullable: true),
                    Cylinders = table.Column<byte>(nullable: true),
                    LicenseExpiryDate = table.Column<string>(nullable: true),
                    MajorColor = table.Column<string>(nullable: true),
                    MinorColor = table.Column<string>(nullable: true),
                    ModelYear = table.Column<short>(nullable: true),
                    PlateTypeCode = table.Column<byte>(nullable: true),
                    RegisterationPlace = table.Column<string>(nullable: true),
                    VehicleBodyCode = table.Column<byte>(nullable: false),
                    VehicleWeight = table.Column<int>(nullable: false),
                    VehicleLoad = table.Column<int>(nullable: false),
                    VehicleMaker = table.Column<string>(nullable: true),
                    VehicleModel = table.Column<string>(nullable: true),
                    ChassisNumber = table.Column<string>(nullable: true),
                    VehicleMakerCode = table.Column<short>(nullable: true),
                    VehicleModelCode = table.Column<long>(nullable: true),
                    CarPlateText1 = table.Column<string>(nullable: true),
                    CarPlateText2 = table.Column<string>(nullable: true),
                    CarPlateText3 = table.Column<string>(nullable: true),
                    CarPlateNumber = table.Column<short>(nullable: true),
                    CarOwnerNIN = table.Column<string>(nullable: true),
                    CarOwnerName = table.Column<string>(nullable: true),
                    VehicleValue = table.Column<int>(nullable: true),
                    IsUsedCommercially = table.Column<bool>(nullable: true),
                    OwnerTransfer = table.Column<bool>(nullable: true),
                    EngineSizeId = table.Column<int>(nullable: true),
                    VehicleUseId = table.Column<int>(nullable: false),
                    CurrentMileageKM = table.Column<decimal>(nullable: true),
                    TransmissionTypeId = table.Column<int>(nullable: true),
                    MileageExpectedAnnualId = table.Column<int>(nullable: true),
                    AxleWeightId = table.Column<int>(nullable: true),
                    ParkingLocationId = table.Column<int>(nullable: true),
                    HasModifications = table.Column<bool>(nullable: true),
                    HasAntiTheftAlarm = table.Column<bool>(nullable: true),
                    HasFireExtinguisher = table.Column<bool>(nullable: true),
                    ModificationDetails = table.Column<string>(nullable: true),
                    BrakeSystemId = table.Column<int>(nullable: true),
                    CruiseControlTypeId = table.Column<int>(nullable: true),
                    ParkingSensorId = table.Column<int>(nullable: true),
                    CameraTypeId = table.Column<int>(nullable: true),
                    VehicleIdTypeId = table.Column<int>(nullable: true),
                    AxlesWeight = table.Column<int>(nullable: true),
                    MileageExpectedAnnual = table.Column<int>(nullable: true),
                    TransmissionType = table.Column<int>(nullable: true),
                    EngineSize = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ServerIP = table.Column<string>(nullable: true),
                    UserIP = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMaker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<short>(nullable: false),
                    EnglishDescription = table.Column<string>(nullable: true),
                    ArabicDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMaker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRequestLogs",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ErrorCode = table.Column<int>(nullable: false),
                    ErrorDescription = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    SequenceNumber = table.Column<string>(nullable: true),
                    ServiceURL = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    VehicleId = table.Column<int>(nullable: false),
                    OwnerNin = table.Column<long>(nullable: false),
                    VehicleIdTypeId = table.Column<int>(nullable: false),
                    IsOwnerTransfer = table.Column<bool>(nullable: false),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    ModelYear = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRequestLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<long>(nullable: false),
                    EnglishDescription = table.Column<string>(nullable: true),
                    ArabicDescription = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                    table.ForeignKey(
                        name: "FK_City_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<long>(nullable: false),
                    VehicleMakerCode = table.Column<short>(nullable: false),
                    EnglishDescription = table.Column<string>(nullable: true),
                    ArabicDescription = table.Column<string>(nullable: true),
                    VehicleMakerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModel_VehicleMaker_VehicleMakerId",
                        column: x => x.VehicleMakerId,
                        principalTable: "VehicleMaker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Aliens",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverId = table.Column<Guid>(nullable: false),
                    IsCitizen = table.Column<bool>(nullable: false),
                    EnglishFirstName = table.Column<string>(nullable: true),
                    EnglishLastName = table.Column<string>(nullable: true),
                    EnglishSecondName = table.Column<string>(nullable: true),
                    EnglishThirdName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    ThirdName = table.Column<string>(nullable: true),
                    SubtribeName = table.Column<string>(nullable: true),
                    DateOfBirthG = table.Column<DateTime>(nullable: false),
                    NationalityCode = table.Column<short>(nullable: true),
                    DateOfBirthH = table.Column<string>(nullable: true),
                    NIN = table.Column<string>(nullable: true),
                    GenderId = table.Column<int>(nullable: false),
                    NCDFreeYears = table.Column<int>(nullable: true),
                    NCDReference = table.Column<string>(nullable: true),
                    IsSpecialNeed = table.Column<bool>(nullable: true),
                    IdIssuePlace = table.Column<string>(nullable: true),
                    IdExpiryDate = table.Column<string>(nullable: true),
                    DrivingPercentage = table.Column<int>(nullable: true),
                    ChildrenBelow16Years = table.Column<int>(nullable: true),
                    EducationId = table.Column<int>(nullable: false),
                    SocialStatusId = table.Column<int>(nullable: true),
                    OccupationId = table.Column<int>(nullable: true),
                    MedicalConditionId = table.Column<int>(nullable: true),
                    ResidentOccupation = table.Column<string>(nullable: true),
                    CityId = table.Column<long>(nullable: true),
                    WorkCityId = table.Column<long>(nullable: true),
                    NOALast5Years = table.Column<int>(nullable: true),
                    NOCLast5Years = table.Column<int>(nullable: true),
                    Education = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    SocialStatus = table.Column<int>(nullable: false),
                    MedicalCondition = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ServerIP = table.Column<string>(nullable: true),
                    UserIP = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aliens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Aliens_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aliens_Occupation_OccupationId",
                        column: x => x.OccupationId,
                        principalTable: "Occupation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aliens_City_WorkCityId",
                        column: x => x.WorkCityId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverId = table.Column<Guid>(nullable: false),
                    IsCitizen = table.Column<bool>(nullable: false),
                    EnglishFirstName = table.Column<string>(nullable: true),
                    EnglishLastName = table.Column<string>(nullable: true),
                    EnglishSecondName = table.Column<string>(nullable: true),
                    EnglishThirdName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    ThirdName = table.Column<string>(nullable: true),
                    SubtribeName = table.Column<string>(nullable: true),
                    DateOfBirthG = table.Column<DateTime>(nullable: false),
                    NationalityCode = table.Column<short>(nullable: true),
                    DateOfBirthH = table.Column<string>(nullable: true),
                    NIN = table.Column<string>(nullable: true),
                    GenderId = table.Column<int>(nullable: true),
                    NCDFreeYears = table.Column<int>(nullable: true),
                    NCDReference = table.Column<string>(nullable: true),
                    IsSpecialNeed = table.Column<bool>(nullable: true),
                    IdIssuePlace = table.Column<string>(nullable: true),
                    IdExpiryDate = table.Column<string>(nullable: true),
                    DrivingPercentage = table.Column<int>(nullable: true),
                    ChildrenBelow16Years = table.Column<int>(nullable: true),
                    EducationId = table.Column<int>(nullable: true),
                    SocialStatusId = table.Column<int>(nullable: true),
                    OccupationId = table.Column<int>(nullable: true),
                    MedicalConditionId = table.Column<int>(nullable: true),
                    ResidentOccupation = table.Column<string>(nullable: true),
                    CityId = table.Column<long>(nullable: true),
                    WorkCityId = table.Column<long>(nullable: true),
                    NOALast5Years = table.Column<int>(nullable: true),
                    NOCLast5Years = table.Column<int>(nullable: true),
                    Education = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    SocialStatus = table.Column<int>(nullable: false),
                    MedicalCondition = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ServerIP = table.Column<string>(nullable: true),
                    UserIP = table.Column<string>(nullable: true),
                    UserAgent = table.Column<string>(nullable: true),
                    ViolationId = table.Column<int>(nullable: true),
                    MaritalStatusCode = table.Column<int>(nullable: true),
                    NumOfChildsUnder16 = table.Column<int>(nullable: true),
                    DrivingLicenseTypeCode = table.Column<int>(nullable: true),
                    SaudiLicenseHeldYears = table.Column<int>(nullable: true),
                    EligibleForNoClaimsDiscountYears = table.Column<int>(nullable: true),
                    NumOfFaultAccidentInLast5Years = table.Column<int>(nullable: true),
                    NumOfFaultclaimInLast5Years = table.Column<int>(nullable: true),
                    RoadConvictions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Citizens_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citizens_Occupation_OccupationId",
                        column: x => x.OccupationId,
                        principalTable: "Occupation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Citizens_City_WorkCityId",
                        column: x => x.WorkCityId,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    ObjLatLng = table.Column<string>(nullable: true),
                    BuildingNumber = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    AdditionalNumber = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    PolygonString = table.Column<string>(nullable: true),
                    IsPrimaryAddress = table.Column<string>(nullable: true),
                    UnitNumber = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    RegionId = table.Column<string>(nullable: true),
                    Restriction = table.Column<string>(nullable: true),
                    PKAddressID = table.Column<string>(nullable: true),
                    DriverId = table.Column<Guid>(nullable: true),
                    AddressLoction = table.Column<string>(nullable: true),
                    AlienID = table.Column<int>(nullable: true),
                    CitizenID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Address_Aliens_AlienID",
                        column: x => x.AlienID,
                        principalTable: "Aliens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_Citizens_CitizenID",
                        column: x => x.CitizenID,
                        principalTable: "Citizens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverExtraLicense",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverId = table.Column<Guid>(nullable: false),
                    CountryCode = table.Column<short>(nullable: false),
                    LicenseYearsId = table.Column<int>(nullable: false),
                    AlienID = table.Column<int>(nullable: true),
                    CitizenID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverExtraLicense", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DriverExtraLicense_Aliens_AlienID",
                        column: x => x.AlienID,
                        principalTable: "Aliens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverExtraLicense_Citizens_CitizenID",
                        column: x => x.CitizenID,
                        principalTable: "Citizens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverLicense",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LicenseId = table.Column<int>(nullable: false),
                    DriverId = table.Column<Guid>(nullable: false),
                    TypeDesc = table.Column<short>(nullable: true),
                    ExpiryDateH = table.Column<string>(nullable: true),
                    IssueDateH = table.Column<string>(nullable: true),
                    AlienID = table.Column<int>(nullable: true),
                    CitizenID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLicense", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DriverLicense_Aliens_AlienID",
                        column: x => x.AlienID,
                        principalTable: "Aliens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverLicense_Citizens_CitizenID",
                        column: x => x.CitizenID,
                        principalTable: "Citizens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverViolation",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverId = table.Column<Guid>(nullable: false),
                    ViolationId = table.Column<int>(nullable: false),
                    AlienID = table.Column<int>(nullable: true),
                    CitizenID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverViolation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DriverViolation_Aliens_AlienID",
                        column: x => x.AlienID,
                        principalTable: "Aliens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DriverViolation_Citizens_CitizenID",
                        column: x => x.CitizenID,
                        principalTable: "Citizens",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_AlienID",
                table: "Address",
                column: "AlienID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CitizenID",
                table: "Address",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_Aliens_CityId",
                table: "Aliens",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Aliens_OccupationId",
                table: "Aliens",
                column: "OccupationId");

            migrationBuilder.CreateIndex(
                name: "IX_Aliens_WorkCityId",
                table: "Aliens",
                column: "WorkCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_CityId",
                table: "Citizens",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_OccupationId",
                table: "Citizens",
                column: "OccupationId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizens_WorkCityId",
                table: "Citizens",
                column: "WorkCityId");

            migrationBuilder.CreateIndex(
                name: "IX_City_RegionId",
                table: "City",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverExtraLicense_AlienID",
                table: "DriverExtraLicense",
                column: "AlienID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverExtraLicense_CitizenID",
                table: "DriverExtraLicense",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverLicense_AlienID",
                table: "DriverLicense",
                column: "AlienID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverLicense_CitizenID",
                table: "DriverLicense",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverViolation_AlienID",
                table: "DriverViolation",
                column: "AlienID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverViolation_CitizenID",
                table: "DriverViolation",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModel_VehicleMakerId",
                table: "VehicleModel",
                column: "VehicleMakerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AlienRequestLogs");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "CitizenRequestLogs");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyRequestLogs");

            migrationBuilder.DropTable(
                name: "DriverExtraLicense");

            migrationBuilder.DropTable(
                name: "DriverLicense");

            migrationBuilder.DropTable(
                name: "DriverViolation");

            migrationBuilder.DropTable(
                name: "LicenseType");

            migrationBuilder.DropTable(
                name: "ServiceRequestLog");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "VehicleModel");

            migrationBuilder.DropTable(
                name: "VehicleRequestLogs");

            migrationBuilder.DropTable(
                name: "Aliens");

            migrationBuilder.DropTable(
                name: "Citizens");

            migrationBuilder.DropTable(
                name: "VehicleMaker");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Occupation");

            migrationBuilder.DropTable(
                name: "Region");
        }
    }
}
