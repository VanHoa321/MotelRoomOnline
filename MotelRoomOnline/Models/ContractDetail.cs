using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_contract_detail")]
    public class ContractDetail
    {
        [Key]
        public int ContractDetailId { get; set; }

        public long ContractId { get; set; }

        public long RentalMonthId { get; set; }

        public long RoomServiceId { get; set; }

        public decimal ServicePrice { get; set; }
 
        public int ServiceQuantity { get; set; }

        public decimal Amount { get; set; }

    }
}
