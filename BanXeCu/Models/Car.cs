using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace BanXeCu.Models
{
    public class Car
    {
        
        [Required]
        [DisplayName("Hãng xe (*)")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [DisplayName("Dáng xe (*)")]
        [StringLength(50)]
        public string CarModel { get; set; }
        [Required]
        [DisplayName("Dòng xe (*)")]
        [StringLength(50)]
        public string Family { get; set; }
        [Required]
        [DisplayName("Giá(triệu VNĐ) (*)")]
        [Range(0,1000000)]
        public int Price { get; set; }
        [Required]
        [DisplayName("Năm SX (*)")]
        public int ManufactureYear { get; set; }
        [Required]
        [DisplayName("Xuất xứ (*)")]
        [StringLength(50)]
        public string ComeFrom { get; set; }
        [DisplayName("Tình trạng")]
        [StringLength(50)]
        public string Used { get; set; }
        [DisplayName("Số km đã đi (*)")]
        [Required]
        public int Distance { get; set; }
        [DisplayName("Dài x Rộng x Cao(mm)")]
        [StringLength(50)]
        public string Size { get; set; }
        [DisplayName("Chiều dài cơ sở(mm)")]
        [StringLength(10)]
        public string Length { get; set; }
        [DisplayName("Trọng lượng không tải(kg)")]
        [StringLength(10)]
        public string Weight { get; set; }
        [DisplayName("Dung tích xy lanh(cc)")]
        [StringLength(10)]
        public string CylinderCapacity  { get; set; }
        [DisplayName("Dung tích bình xăng")]
        [StringLength(10)]
        public string FuelCapacity { get; set; }
        [DisplayName("Thông số lốp")]
        [StringLength(50)]
        public string TireInfo   { get; set; }
        [DisplayName("Vành mâm xe")]
        [StringLength(50)]
        public string WheelBase { get; set; }
        [DisplayName("Số chỗ ngồi")]
        [StringLength(10)]
        public string MaxSeatingCapacity { get; set; }
        [DisplayName("Dẫn động")]
        [StringLength(50)]
        public string DriveType { get; set; }
        [DisplayName("Hộp số")]
        [StringLength(50)]
        public string Transmission { get; set; }
        [DisplayName("Số cửa")]
        [StringLength(10)]
        public string NumDoor { get; set; }


    }
}