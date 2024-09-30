using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_rental_month")]
    public class RentalMonth
    {
        [Key]
        public long RentalMonthId { get; set; }

        public long ContractId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PriceRoom { get; set; }

        public int Status { get; set; }
    }
}
