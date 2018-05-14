using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BanXeCu.Models
{
    public class Notification
    {
        public int ID { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        public int MemID { get; set; }
        [StringLength(10)]
        public string PostID { get; set; }
        [StringLength(2000)]
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
        public bool ForAdmin { get; set; }
        public bool IsRead { get; set; }

    }
}