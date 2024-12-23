using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_customer")]
    public class Customer
    {
        [Key]
        public long CustomerId { get; set; }

        public string? Code { get; set; }

        public string? FullName { get; set; }

        public string? Avatar { get; set; }

        public DateTime? DOB { get; set; }

        public bool? Gender {  get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Description { get; set; }

        public int AccountId { get; set; }
    }
}
