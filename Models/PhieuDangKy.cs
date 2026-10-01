namespace qlphongtap.Models
{
    public class PhieuDangKy
    {
        public int Id { get; set; }
        public int IdHoiVien { get; set; }
        public int IdGoiTap { get; set; }
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayHetHan { get; set; }
        public decimal GiaTriThanhToan { get; set; } //wait accept, existing, canceled 
        public decimal GiaTriGiaHan { get; set; }
        
    }
}
