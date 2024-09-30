using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_district")]
    public class District
    {
        [Key]
        public int DistrictId { get; set; }

        public string? DistrictName { get; set; }
    }
}
