using System.ComponentModel.DataAnnotations;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Models
{
    public class GoiTap
    {
        [Key]
        public string MaGoiTap { get; set; }
        public string TenGoiTap { get; set; }
        public string ThoiHanThang { get; set; }
        public decimal DonGia { get; set; }
        public int SoBuoiToiDa { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
    }
}
