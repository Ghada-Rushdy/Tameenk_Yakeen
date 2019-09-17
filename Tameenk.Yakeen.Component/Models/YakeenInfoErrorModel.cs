using Tameenk.Yakeen.DAL.Enums;


namespace YakeenComponent
{
    public class YakeenInfoErrorModel
    {
        public EErrorType Type { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }
    }
}
