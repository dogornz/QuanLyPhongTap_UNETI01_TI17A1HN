using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class PhieuDangKy
    {
        [Key]
        public string MaPhieu { get; set; }
        [ForeignKey("HoiVien")]
        public string MaHoiVien { get; set; }
        [ForeignKey("GoiTap")]
        public string MaGoiTap { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal DonGia { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public HoiVien? HoiVien { get; set; }
        public GoiTap? GoiTap { get; set; }
    }
}
