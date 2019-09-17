
namespace Tameenk.Yakeen.DAL
{
    public class CitizenRequestLogDataAccess: BaseDataAccess<CitizenRequestLog,int>
    {

        public CitizenRequestLogDataAccess():base()
        { }

        public int AddToCistizenLog(CitizenRequestLog entity)
        {
            return Add(entity);
        }       

    }
}
