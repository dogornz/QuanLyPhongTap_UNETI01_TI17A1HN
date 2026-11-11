using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public interface IPhieuDangKyRepository
    {
        List<PhieuDangKy> GetAll();
        PhieuDangKy? GetById(string maPhieu);
        void Add(PhieuDangKy phieu);
        void Update(PhieuDangKy phieu);
        void Save();

        // THÊM - chuyển lọc/tìm kiếm xuống Repository, lọc tại DB thay vì tải hết vào bộ nhớ
        List<PhieuDangKy> TimKiem(string? maHoiVien, string? maGoiTap, string? trangThai,
                                   DateTime? tuNgay, DateTime? denNgay);

        // THÊM - các hàm thống kê (8.6), đếm/nhóm ngay tại Repository
        List<(string Trang, int SoLuong)> DemTheoTrangThai();
        List<(string Ten, int SoLuong)> DemTheoGoi();
        List<(string Ten, int SoLuong)> DemTheoLoaiGoi();
        List<(int Nam, int Thang, int SoLuot)> DemTheoThang();
        List<(int Nam, int Thang, decimal DoanhThu)> TinhDoanhThuTheoThang();
    }
}
