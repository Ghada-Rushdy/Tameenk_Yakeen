
using System.Linq;

namespace Tameenk.Yakeen.DAL
{
    public class VehicleDataAccess : BaseDataAccess<Vehicle, int>
    {
        private const string VEHICLE_Model_ALL = "tameenk.vehiclMaker.all.{0}.{1}.{2}";
        private readonly MemoryCacheManager _cacheManager;
        private readonly VehicleModelDataAccess vehicleModelService;

        public VehicleDataAccess()
        { }


        public PagedList<VehicleModel> VehicleModels(int vehicleMakerId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            string vehicleMakerCode = vehicleMakerId.ToString();
            return _cacheManager.Get(string.Format(VEHICLE_Model_ALL, vehicleMakerId, pageIndex, pageSize), () =>
            {
                return new PagedList<VehicleModel>(vehicleModelService.Find
                    (e => e.VehicleMakerCode == vehicleMakerId, x => x.OrderBy(e => e.Code)), pageIndex, pageSize);
            });
        }
    }
}
