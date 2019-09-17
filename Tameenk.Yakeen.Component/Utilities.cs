using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Tameenk.Yakeen.Component
{
    public class Utilities
    {
        public static int GetSocialStatusId(string socialStatus)
        {
            if (socialStatus == "مطلقة" || socialStatus == "Divorced Female")
            {
                return 5;
            }
            if (socialStatus == "متزوجة" || socialStatus == "Married Female")
            {
                return 4;
            }
            if (socialStatus == "متزوج" || socialStatus == "Married Male")
            {
                return 2;
            }
            if (socialStatus == "غير متاح" || socialStatus == "Not Available")
            {
                return 0;
            }
            if (socialStatus == "غير ذلك" || socialStatus == "Other")
            {
                return 7;
            }
            if (socialStatus == "غير متزوجة" || socialStatus == "Single Female")
            {
                return 3;
            }
            if (socialStatus == "أعزب" || socialStatus == "Single Male")
            {
                return 1;
            }
            if (socialStatus == "ارملة" || socialStatus == "Widowed Female")
            {
                return 6;
            }
            return 1;
        }

        public static string GetServerIP()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                return (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).FirstOrDefault();
            }
            catch (Exception exp)
            {
                // ErrorLogger.LogError(exp.Message, exp, false);
                return string.Empty;
            }
        }

    }
}
