using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_room_status")]
    public class RoomStatus
    {
        [Key]
        public int RoomStatusId { get; set; }

        public string? RoomStatusName { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
