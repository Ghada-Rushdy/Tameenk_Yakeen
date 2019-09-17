using Tameenk.Yakeen.Service.Models;

namespace Tameenk.Yakeen.Service.WebClients
{
    public class YakeenOutput
    {
        public enum ErrorCodes
        {
            Success = 1,
            NullResponse,
            UnspecifiedError,
            ServiceError,
            NullRequest,
            NinIsNull,
            ServiceException
        }
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

        public VehicleYakeenInfoDto Output
        {
            get;
            set;
        }//show that
        public CustomerIdYakeenInfoDto CustomerIdYakeenInfoDto
        {
            get;
            set;
        }
        public DriverYakeenInfoDto DriverYakeenInfoDto
        {
            get;
            set;
        }
        public CompanyYakeenInfoDto CompanyYakeenInfoDto
        {
            get;
            set;
        }
        public AlienYakeenInfoDto AlienYakeenInfoDto { get; set; }
    }
}
