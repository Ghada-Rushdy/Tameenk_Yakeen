using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tameenk.Yakeen.DAL
{
    public class Channel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ExpireDateInDays { get; set; }

    }}
