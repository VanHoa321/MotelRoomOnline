using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_account")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? DOB { get; set; }

        public bool? Gender { get; set; }

        public string? Avatar { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Status { get; set; }

        public int RoleID { get; set; }

        public int? PremiumId { get; set; }

        public DateTime? ExpiryDate { get; set; }

    }
}
