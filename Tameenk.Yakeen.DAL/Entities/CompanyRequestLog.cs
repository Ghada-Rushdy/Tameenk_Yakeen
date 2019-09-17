
namespace Tameenk.Yakeen.DAL
{
    public class CompanyRequestLog
    {
        public enum ErrorCodes
        {
            Success = 1,
            NullResponse,
            UnspecifiedError,
            ServiceError,
            NullRequest,
            NinIsNull,
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
        public int ID { get; set; }
        public string Method { get; set; }
        public string ServiceURL { get; set; }
        public string ReferenceId { get; set; }
        public string ServerIP { get; set; }
        public int Channel { get; set; }
        public string sponsorNumber { get; set; }

    }
}
