using System.ComponentModel.DataAnnotations;

namespace BanXeCu.Models
{
    public class Admin : User
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}