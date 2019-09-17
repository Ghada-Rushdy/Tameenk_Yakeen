
namespace Tameenk.Yakeen.DAL
{
   public class VehicleRequestLogDataAccess: BaseDataAccess<VehicleRequestLog,int>
    {
        public VehicleRequestLogDataAccess()
        { }

        public int AddToVehicleLog(VehicleRequestLog entity)
        {
            return Add(entity);
        }
     
    }
}
