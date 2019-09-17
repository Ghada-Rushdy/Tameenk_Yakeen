
using System;
using System.ComponentModel.DataAnnotations;

namespace Tameenk.Yakeen.DAL
{
    public class AlienRequestLog
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
        public Guid AlienID { get; set; }
        public string Method { get; set; }
        public string ServiceURL { get; set; }
        public string ReferenceId { get; set; }
        public string ServerIP { get; set; }
        public int Channel { get; set; }
        public long NiN { get; set; }

    }
}
