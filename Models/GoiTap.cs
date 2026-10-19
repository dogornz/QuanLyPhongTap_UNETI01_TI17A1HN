using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class GoiTap
    {
        [Key]
        public string MaGoiTap { get; set; } = null!;

        [Required(ErrorMessage = "Tên gói tập không được để trống")]
        public string TenGoiTap { get; set; } = null!;

        [Required(ErrorMessage = "Giá gói tập không được để trống")]
        public decimal DonGia { get; set; }

        [Required(ErrorMessage = "Thời hạn không được để trống")]
        public string ThoiHanThang { get; set; } = null!;

        [Required]
        public string TrangThai { get; set; } = null!;

        [Required]
        public string MoTa { get; set; } = null!;

        public int SoBuoiToiDa { get; set; }

        public int MaLoaiGoi { get; set; }
        public LoaiGoiTap LoaiGoiTap { get; set; } = null!;

        public ICollection<PhieuDangKy> PhieuDangKy { get; set; } = new List<PhieuDangKy>();
    }
}
