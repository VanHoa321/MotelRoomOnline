using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_post_categories")]
    public class PostCategories
    {
        [Key]
        public int PostCategoryId { get; set; }

        public string? PostCategoryName { get; set; }

        public string? Alias { get; set; }

        public int Position { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public bool? IsActive { get; set; }

    }
}
