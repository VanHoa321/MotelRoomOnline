using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_menu")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        public string? MenuName { get; set; }

        public string? Alias { get; set; }

        public int Levels { get; set; }

        public int? ParentId { get; set; }

        public int? Position { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
