using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_ward")]
    public class Ward
    {
        [Key]
        public int WardId { get; set; }

        public string? WardName { get; set; }

        public int DistrictId { get; set; }
    }
}
