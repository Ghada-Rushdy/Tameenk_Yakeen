using System;
using System.ComponentModel.DataAnnotations;

namespace Tameenk.Yakeen.DAL
{
    public class CitizenRequestLog
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

        [Key]
        public int ID { get; set; }
        public string ErrorDescription
        {
            get;
            set;
        }
        public Guid CitizenId { get; set; }
        public string Method { get; set; }       
        public string ReferenceNumber { get; set; }
        public string ServerIP { get; set; }
        public int Channel { get; set; }
        public long NiN { get; set; }
        public string licenseExpiryDate { get; set; }
        public bool IsCitizen { get; set; }

    }
}
