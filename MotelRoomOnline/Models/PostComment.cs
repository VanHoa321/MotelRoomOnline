using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_post_comment")]
    public class PostComment
    {
        [Key]
        public int PostCommentId { get; set; }

        public int AccountId { get; set; }

        public int PostId { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set;}

        public bool IsActive { get; set; }
    }
}
