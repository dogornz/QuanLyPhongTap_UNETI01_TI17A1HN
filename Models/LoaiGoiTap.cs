using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class LoaiGoiTap
    {
        [Key]
        public int MaLoaiGoi { get; set; }

        [Required(ErrorMessage = "Tên loại gói tập không được để trống")]
        public string TenLoaiGoi { get; set; } = null!;

        public string MoTa { get; set; } = null!;

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string TrangThai { get; set; } = null!;  // (Active/Inactive)

        // Số gói tập trực thuộc (đồng bộ từ bảng GoiTap)
        public int SoGoiTrucThuoc { get; set; }

        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public ICollection<GoiTap> GoiTap { get; set; } = new List<GoiTap>();
    }
}
