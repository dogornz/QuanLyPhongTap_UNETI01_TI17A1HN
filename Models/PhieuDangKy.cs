using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class PhieuDangKy
    {
        [Key]
        public string MaPhieu { get; set; } = null!;

        [Required]
        public string MaHoiVien { get; set; } = null!;

        [Required]
        public string MaGoiTap { get; set; } = null!;

        [Required]
        public DateTime NgayDangKy { get; set; }

        [Required]
        public DateTime NgayBatDau { get; set; }

        [Required]
        public DateTime NgayKetThuc { get; set; }

        [Required]
        public decimal DonGia { get; set; }

        [Required]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; } = null!;

        public HoiVien HoiVien { get; set; } = null!;
        public GoiTap GoiTap { get; set; } = null!;
    }
}
