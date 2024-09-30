using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_room_images")]
    public class RoomImage
    {
        [Key]
        public int RoomImageId { get; set; }

        public long RoomId { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public bool? IsDefault { get; set; }
    }
}
