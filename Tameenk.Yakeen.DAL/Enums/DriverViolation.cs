using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.Yakeen.DAL.Enums
{
   public class DriverViolation
    {
        /// <summary>
        /// Table id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// Driver Id that has the violation
        /// </summary>
        public Guid DriverId { get; set; }
        /// <summary>
        /// Violation Id
        /// </summary>
        public int ViolationId { get; set; }

        /// <summary>
        /// Driver navigation property
        /// </summary>
       
    }
}
