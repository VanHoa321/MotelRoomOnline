using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_post")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string? PostTitle { get; set; }

        public string? Alias { get; set; }

        public string? Abstract { get; set; }

        public string? Content { get; set; }

        public string? PostImage { get; set; }

        public int PostCategoryId { get; set; }

        public int AccountId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
