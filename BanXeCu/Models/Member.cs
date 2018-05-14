using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace BanXeCu.Models
{
    public class Member : User
    {
        [Key]
        [DisplayName("Mã thành viên")]
        public int ID { get; set; }
        [StringLength(20)]
        [DisplayName("Số điện thoại")]
        [RegularExpression("[0-9]+")]
        public string PhoneNumber { get; set; }
        [StringLength(20)]
        [DisplayName("Thành phố")]
        public string City { set; get; }
        [StringLength(50)]
        [DisplayName("Họ và tên")]
        public string Name { get; set; }
        public bool IsSalon { get; set; }
    }
}