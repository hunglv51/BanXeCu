using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BanXeCu.Models
{
    public class Salon
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Mã thành viên")]
        public int MemID { get; set; }
        [StringLength(50)]
        [DisplayName("Địa chỉ")]
        public string SalonAddress { get; set; }
        [StringLength(50)]
        [DisplayName("Giới thiệu")]
        public string Introduction { get; set; }
        [StringLength(30)]
        [DisplayName("Tên Salon")]
        public string SalonName { get; set; }
        
    }
}