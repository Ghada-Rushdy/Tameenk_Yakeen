using System;

namespace Tameenk.Yakeen.DAL
{ 
    public class VehicleRequestLog
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
        public long ID { get; set; }
        public string Method { get; set; }
        public string SequenceNumber { get; set; }
        public string ServiceURL { get; set; }
        public string ServerIP { get; set; }
        public int Channel { get; set; }
        public int VehicleId { get; set; }
        public long OwnerNin { get; set; }
        public int VehicleIdTypeId { get; set; }
        public bool IsOwnerTransfer { get; set; }
        public string ReferenceNumber { set; get; }
        public short? ModelYear { get; set; }

    }
}
