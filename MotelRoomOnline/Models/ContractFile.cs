using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotelRoomOnline.Models
{
    [Table("tb_contract_file")]
    public class ContractFile
    {
        [Key]
        public long Id { get; set; }

        public long ContractId { get; set; }

        public string? FileName { get; set; }

        public string? FilePath { get; set; }

        public string? FileType { get; set; }

        public DateTime? UploadedAt { get; set; }
    }
}
