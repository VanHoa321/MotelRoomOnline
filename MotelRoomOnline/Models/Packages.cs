using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_packages")]
    public class Packages
    {
        [Key]
        public int PackageId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int DurationDays { get; set; }
    }
}
