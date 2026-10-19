using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    [Table("PhieuDangKy")]
    public class PhieuDangKy
    {
        [Key]
        [StringLength(450)]
        public string MaPhieu { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string MaHoiVien { get; set; } = string.Empty;

        [Required]
        [StringLength(450)]
        public string MaGoiTap { get; set; } = string.Empty;

        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        public DateTime NgayBatDau { get; set; } = DateTime.Now;

        public DateTime NgayKetThuc { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DonGia { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } = "Chờ xác nhận";

        [ForeignKey("MaHoiVien")]
        public virtual HoiVien? HoiVien { get; set; }

        [ForeignKey("MaGoiTap")]
        public virtual GoiTap? GoiTap { get; set; }
    }
}
