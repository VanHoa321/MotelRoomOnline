using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_account_upgrades")]
    public class AccountUpgrade
    {
        [Key]
        public int UpgradeId { get; set; }

        public int AccountId { get; set; }

        public int PackageId { get; set; }

        public double Amount { get; set; }

        public DateTime UpgradeDate { get; set; }
    }
}
