using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_room_categories")]
    public class RoomCategories
    {
        [Key]
        public int RoomCategoryId { get; set; }

        public string? RoomCategoryName { get; set; }

        public string? Alias { get; set; }

        public string? Image { get; set; }

        public int Position { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
