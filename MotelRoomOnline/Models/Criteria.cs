using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_criterias")]
    public class Criteria
    {
        [Key]
        public int CriteriaId { get; set; }

        public string? CriteriaName { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
