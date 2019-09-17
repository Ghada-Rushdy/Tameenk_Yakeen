using System.Web.Http;
using YakeenComponent;

namespace Tameenk.Yakeen.API.Controllers
{
    public class CompanyController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("GetCompanyBySponsorId")]
        public IHttpActionResult GetCompanyBySponsorId([FromBody]CompanyYakeenInfoModel model)
        {
            var companyOutput = CompanyServices.GetCompanyBySponsorId(model);

            return Ok(companyOutput);
        }
    }
}
