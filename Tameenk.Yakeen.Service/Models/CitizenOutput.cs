
namespace Tameenk.Yakeen.Service.Models
{
    public class CitizenOutput
    {
        public enum ErrorCodes
        {
            Success = 1,
            NullResponse,
            UnspecifiedError,
            ServiceError,
            NullRequest,     
            ServiceException,
            MethodException
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
        public DriverYakeenInfoModel Citizen
        {
            get;
            set;
        }

    }
}
