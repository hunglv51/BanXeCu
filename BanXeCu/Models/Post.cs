using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace BanXeCu.Models
{
    public class Post : Car
    {
        [Key]
        public int PostID { get; set; }
        [DisplayName("Tiêu đề")]
        [StringLength(50)]
        public string Title { get; set; }
        [Required]
        [StringLength(1000)]
        [DisplayName("Nội dung (*)")]
        public string Content { get; set; }
        public int MemID { get; set; }
        [DisplayName("Ngày hết hạn")]
        [DataType(DataType.Date)]
        public DateTime ExpiredDate { get; set; }
        
        [DisplayName("Link Video giới thiệu")]
        [StringLength(100)]
        [Url]
        public string VideoLink { get; set; }
        [StringLength(20)]
        [DisplayName("Loại tin")]
        public string  PostType { get; set; }
        [DisplayName("Ngày đăng")]
        [DataType(DataType.Date)]
        public DateTime TimeStart { get; set; }
        [DisplayName("Đã bán")]
        public bool Sold { get; set; }

    }
}