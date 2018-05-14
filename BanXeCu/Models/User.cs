using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BanXeCu.Models
{
    public class User
    {
        [DisplayName("Tên tài khoản")]
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(50)]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
    }
}