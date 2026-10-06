using System.ComponentModel.DataAnnotations.Schema;

namespace Quanlyphongtap.Models
{
    public class PhieuDangKy
    {
        public int Id { get; set; }
        [ForeignKey("IdHoiVien")] 
        public int IdHoiVien { get; set; }
        [ForeignKey("IdGoiTap")]
        public int IdGoiTap { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayHetHan { get; set; }
        public decimal GiaTriThanhToan { get; set; } //wait accept, existing, canceled 
        public decimal GiaTriGiaHan { get; set; }
        public string TrangThai { get; set; } // "Chờ xác nhận", "Đang hiệu lực", "Đã hủy"
        public HoiVien HoiVien { get; set; }
        public GoiTap GoiTap { get; set; }
    }
}
