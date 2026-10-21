namespace QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels
{
    public class ThongKeViewModel
    {
        public List<ThongKeItemViewModel> GoiTheoLoai { get; set; } = new();
        public List<ThongKeItemViewModel> HoiVienTheoTrangThai { get; set; } = new();
        public List<ThongKeItemViewModel> PhieuTheoTrangThai { get; set; } = new();
        public List<ThongKeItemViewModel> LuotTheoGoi { get; set; } = new();
        public List<ThongKeItemViewModel> LuotTheoLoai { get; set; } = new();
        public ThongKeItemViewModel? GoiNhieuNhat { get; set; }
        public List<LuotDangKyThangViewModel> LuotTheoThang { get; set; } = new();
        public List<DoanhThuThangViewModel> DoanhThuTheoThang { get; set; } = new();
    }
}