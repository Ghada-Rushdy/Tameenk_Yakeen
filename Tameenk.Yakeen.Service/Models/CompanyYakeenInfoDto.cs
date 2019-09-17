using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tameenk.Yakeen.Service.Models
{
    public class CompanyYakeenInfoDto
    {
        public int LogId { get; set; }

        public string SponsorName { get; set; }

        public int TotalNumberOfSponsoredDependents { get; set; }

        public int TotalNumberOfSponsoreds { get; set; }
    }
}
