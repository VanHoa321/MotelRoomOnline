using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_invoice")]
    public class Invoice
    {
        [Key]
        public long InvoiceId { get; set; }

        public long ContractId { get; set; }

        public long RentalMonthId { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal TotalAmount { get; set; }

        public int Status { get; set; }
    }
}
