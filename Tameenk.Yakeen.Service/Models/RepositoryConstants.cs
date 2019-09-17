using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;

namespace Tameenk.Yakeen.Service.Models
{
    public class RepositoryConstants
    {
        public static readonly string YakeenUserName;
        public static readonly string YakeenPassword;
        public static readonly string YakeenChargeCode;
        public static readonly string YakeenToken;
        public static readonly int YakeenDataThresholdNumberOfDaysToInvalidate;
        public static readonly int SaudiNationalityCode;
        public static bool ShowLocalErrorDetailsInResponse;

        static RepositoryConstants()
        {
            YakeenUserName = ConfigurationManager.AppSettings["YakeenUserName"]; 
            YakeenPassword = ConfigurationManager.AppSettings["YakeenPassword"];
            YakeenChargeCode = ConfigurationManager.AppSettings["YakeenChargeCode"];
            YakeenToken = ConfigurationManager.AppSettings["YakeenToken"];
            YakeenDataThresholdNumberOfDaysToInvalidate = int.Parse(ConfigurationManager.AppSettings["YakeenDataThresholdNumberOfDaysToInvalidate"]);

#if DEBUG
            ShowLocalErrorDetailsInResponse = bool.Parse(ConfigurationManager.AppSettings["ShowLocalErrorDetailsInResponse"]);
#else
            if (ConfigurationManager.AppSettings["ShowLocalErrorDetailsInResponse"] != null)
            {
                if (!bool.TryParse(
                    ConfigurationManager.AppSettings["ShowLocalErrorDetailsInResponse"],
                    out ShowLocalErrorDetailsInResponse))
                {
                    ShowLocalErrorDetailsInResponse = false;
                }
            }
            else
            {
                ShowLocalErrorDetailsInResponse = false;
            }
#endif
        }
    }
}
