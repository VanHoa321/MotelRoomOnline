using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_contracts")]
    public class Contract
    {
        [Key]
        public long ContractId { get; set; }

        public int RoomId { get; set; }

        public long CustomerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal PriceRoom { get; set; }

        public decimal DepositAmount { get; set; }

        public int Status { get; set; }
    }
}
