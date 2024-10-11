using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_sliders")]
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }

        public string? SliderName { get; set; }

        public string? Alias { get; set; }

        public string? Image { get; set; }

        public int Position { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public bool? IsActive { get; set; }
    }
}
