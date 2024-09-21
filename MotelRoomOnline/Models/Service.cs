using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_services")]
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        public string? ServiceName { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
