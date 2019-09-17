
using System;
using System.Linq.Expressions;

namespace Tameenk.Yakeen.DAL
{   

    public class CitizenDataAccess : BaseDataAccess<Citizen, int>
    {
        public CitizenDataAccess() : base()
        { }
         public virtual Citizen GetSingleOrDefault(Expression<Func<Citizen, bool>> predicate)
        => GetSingleOrDefault(predicate);

    }
}
