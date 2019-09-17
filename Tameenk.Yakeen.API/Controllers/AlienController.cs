using System;
using System.Web.Http;
using YakeenComponent;

namespace Tameenk.Yakeen.API.Controllers
{
    public class AlienController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("GetAlien")]
        public IHttpActionResult GetAlien([FromBody]DriverYakeenInfoRequestModel model)
        {
            var alienObject = AlienServices.GetAlienByOfficialIdAndLicenseExpiryDate(model);

            return Ok(alienObject);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAlienByID")]
        public IHttpActionResult GetAlienByID(Guid id)
        {
            var alienObject = AlienServices.GetAlienByTameenkId(id);

            return Ok(alienObject);
        }
    }
}
