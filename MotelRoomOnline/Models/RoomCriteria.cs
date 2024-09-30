using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_room_criterias")]
    public class RoomCriteria
    {
        [Key]
        public long RoomCriteriaId { get; set; }

        public long RoomId { get; set; }

        public int CriteriaId { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

    }
}
