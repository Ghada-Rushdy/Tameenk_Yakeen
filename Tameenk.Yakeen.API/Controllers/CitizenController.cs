using System;
using System.Web.Http;
using YakeenComponent;

namespace Tameenk.Yakeen.API.Controllers
{
    public class CitizenController : ApiController
    {
        [HttpPost]
        [AllowAnonymous] 
        [Route("GetCitizen")]
        public IHttpActionResult GetCitizen([FromBody]DriverYakeenInfoRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var citizenObject = CitizenServices.GetCitizenByOfficialIdAndLicenseExpiryDate(model);

                return Ok(citizenObject);
            }
            else
            {
                return BadRequest(ModelState);
            }            
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCitizenByID")]
        public IHttpActionResult GetCitizenByID(string id)
        {
            var citizenObject = CitizenServices.GetCitizenByTameenkId(id);

            return Ok(citizenObject);
        }

    }
}
