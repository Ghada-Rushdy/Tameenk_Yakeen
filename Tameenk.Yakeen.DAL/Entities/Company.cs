using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL
{
    public class Company
    {
        [Key]
        public int ID { get; set; }
        public int logId { get; set; }
        public string SponsorName { get; set; }
        public string SponsorId { get; set; }
        public int TotalNumberOfSponsoredDependents { get; set; }
        public int TotalNumberOfSponsoreds { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
