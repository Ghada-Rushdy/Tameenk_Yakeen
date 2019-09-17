using System;
using System.Web.Http;
using YakeenComponent;

namespace Tameenk.Yakeen.API.Controllers
{
    public class VehicleController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("GetVehicleByOfficialId")]
        public IHttpActionResult GetVehicleByOfficialId([FromBody]VehicleInfoRequestModel model)
        {
            var vehicleObject = VehicleServices.GetVehicleByOfficialId(model);

            return Ok(vehicleObject);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetVehicleByID")]
        public IHttpActionResult GetVehicleByID(int ID)
        {
            var vehicleObject = VehicleServices.GetVehicleByTameenkId(ID);

            return Ok(vehicleObject);
        }

    }
}
