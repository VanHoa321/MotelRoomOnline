using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_contact")]
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        public int AccountId { get; set; }

        public string? Message { get; set; }

        public bool? IsRead { get; set; }
    }
}
