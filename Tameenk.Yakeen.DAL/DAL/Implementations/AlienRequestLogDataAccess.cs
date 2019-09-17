

namespace Tameenk.Yakeen.DAL
{
   public class AlienRequestLogDataAccess : BaseDataAccess<AlienRequestLog,int>
    {
        public AlienRequestLogDataAccess(): base()
        { }

        public int AddToAlienLog(AlienRequestLog entity)
        {
            return Add(entity);
        }

    }
}
