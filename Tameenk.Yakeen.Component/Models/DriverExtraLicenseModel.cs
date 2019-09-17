using Newtonsoft.Json;

namespace YakeenComponent
{
    [JsonObject("DriverExtraLicense")]
    public class DriverExtraLicenseModel
    {
        // [Required]
        [JsonProperty("countryId")]
        public short CountryId { get; set; }

        [JsonProperty("licenseYearsId")]
        //[Required]
        public int LicenseYearsId { get; set; }
    }
}
