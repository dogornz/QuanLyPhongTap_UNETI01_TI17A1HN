using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    [Table("GoiTap")]
    public class GoiTap
    {
        [Key]
        [StringLength(450)]
        public string MaGoiTap { get; set; } = string.Empty;

        [Required]
        public string TenGoiTap { get; set; } = string.Empty;

        public string ThoiHanThang { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DonGia { get; set; }

        public int SoBuoiToiDa { get; set; }

        public string MoTa { get; set; } = string.Empty;

        public string TrangThai { get; set; } = "Đang áp dụng";

        [Display(Name = "Mã loại gói")]
        public int? MaLoaiGoi { get; set; }

        [ForeignKey("MaLoaiGoi")]
        public virtual LoaiGoiTap? LoaiGoiTap { get; set; }
    }
}
