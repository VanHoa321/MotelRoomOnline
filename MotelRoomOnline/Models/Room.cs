using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace MotelRoomOnline.Models
{
    [Table("tb_room")]
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        public string? RoomName { get; set; }

        public int RoomCategoryId { get; set; }

        public int RoomStatusId { get; set; }

        public int WardId { get; set; }

        public int AccountId { get; set; }

        public string? Abstract {  get; set; }

        public string? Content { get; set; }

        public string? Image { get; set; }

        public int Acreage { get; set; }

        public decimal PriceRoom {  get; set; }

        public int MaxPeople { get; set; }

        public string? Address { get; set; }

        public long ViewCount { get; set; }

        public string? Alias { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

    }
}
