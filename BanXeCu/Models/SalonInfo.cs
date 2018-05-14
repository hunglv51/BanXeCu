using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanXeCu.Models
{
    public class SalonInfo
    {
        public int MemID { get; set; }
        public string SalonName { get; set; }
        public string SalonAddress { get; set; }
        public string SalonIntro { get; set; }
        public int CountNumber { get; set; }
    }
}