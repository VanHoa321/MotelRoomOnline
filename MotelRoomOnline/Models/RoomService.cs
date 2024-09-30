using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_room_services")]
    public class RoomService
    {
        [Key]
        public long RoomServiceId { get; set; }

        public long RoomId { get; set; }

        public int ServiceId { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
