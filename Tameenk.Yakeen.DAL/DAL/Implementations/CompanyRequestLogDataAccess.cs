
namespace Tameenk.Yakeen.DAL
{
  public class CompanyRequestLogDataAccess: BaseDataAccess<CompanyRequestLog,int>
    {
       public CompanyRequestLogDataAccess(): base()
        { }

        public int AddToCompanyLog(CompanyRequestLog entity)
        {
            return Add(entity);
        }

    }
}
