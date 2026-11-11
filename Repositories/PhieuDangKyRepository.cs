using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services; // THÊM - dùng hằng TrangThaiPhieu

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public class PhieuDangKyRepository : IPhieuDangKyRepository
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public PhieuDangKyRepository(
            QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        public List<PhieuDangKy> GetAll()
        {
            return _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .OrderByDescending(p => p.NgayDangKy)
                .ToList();
        }

        public PhieuDangKy? GetById(string maPhieu)
        {
            return _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .FirstOrDefault(p => p.MaPhieu == maPhieu);
        }

        public void Add(PhieuDangKy phieu)
        {
            _context.PhieuDangKy.Add(phieu);
        }

        public void Update(PhieuDangKy phieu)
        {
            _context.PhieuDangKy.Update(phieu);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        // THÊM - lọc ngay tại DB (IQueryable), không tải hết bảng vào bộ nhớ như bản cũ
        public List<PhieuDangKy> TimKiem(string? maHoiVien, string? maGoiTap, string? trangThai,
                                          DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(maHoiVien))
                query = query.Where(p => p.MaHoiVien == maHoiVien);

            if (!string.IsNullOrWhiteSpace(maGoiTap))
                query = query.Where(p => p.MaGoiTap == maGoiTap);

            if (!string.IsNullOrWhiteSpace(trangThai))
                query = query.Where(p => p.TrangThai == trangThai);

            if (tuNgay.HasValue)
                query = query.Where(p => p.NgayDangKy >= tuNgay.Value.Date);

            if (denNgay.HasValue)
                query = query.Where(p => p.NgayDangKy < denNgay.Value.Date.AddDays(1));

            return query.OrderByDescending(p => p.NgayDangKy).ToList();
        }

        // THÊM - 8.6: Số phiếu theo trạng thái
        public List<(string Trang, int SoLuong)> DemTheoTrangThai()
        {
            return _context.PhieuDangKy
                .GroupBy(p => p.TrangThai)
                .Select(g => new { Trang = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .AsEnumerable()
                .Select(x => (x.Trang, x.SoLuong))
                .ToList();
        }

        // THÊM - 8.6: Số lượt đăng ký theo gói
        public List<(string Ten, int SoLuong)> DemTheoGoi()
        {
            return _context.PhieuDangKy
                .Include(p => p.GoiTap)
                .GroupBy(p => p.GoiTap != null ? p.GoiTap.TenGoiTap : p.MaGoiTap)
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .AsEnumerable()
                .Select(x => (x.Ten ?? "(Không rõ)", x.SoLuong))
                .ToList();
        }

        // THÊM - 8.6: Số lượt đăng ký theo loại gói
        public List<(string Ten, int SoLuong)> DemTheoLoaiGoi()
        {
            return _context.PhieuDangKy
                .Include(p => p.GoiTap)
                    .ThenInclude(g => g!.LoaiGoiTap) // ⚠ KIỂM TRA: tên navigation LoaiGoiTap trong GoiTap.cs
                .GroupBy(p => p.GoiTap != null && p.GoiTap.LoaiGoiTap != null
                    ? p.GoiTap.LoaiGoiTap.TenLoaiGoi
                    : "Chưa phân loại")
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .AsEnumerable()
                .Select(x => (x.Ten ?? "Chưa phân loại", x.SoLuong))
                .ToList();
        }

        // THÊM - 8.6: Số lượt đăng ký theo tháng
        public List<(int Nam, int Thang, int SoLuot)> DemTheoThang()
        {
            return _context.PhieuDangKy
                .GroupBy(p => new { p.NgayDangKy.Year, p.NgayDangKy.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, SoLuot = g.Count() })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .AsEnumerable()
                .Select(x => (x.Year, x.Month, x.SoLuot))
                .ToList();
        }

        // THÊM - 8.6: Doanh thu theo tháng (chỉ phiếu Đang hiệu lực)
        public List<(int Nam, int Thang, decimal DoanhThu)> TinhDoanhThuTheoThang()
        {
            return _context.PhieuDangKy
                .Where(p => p.TrangThai == TrangThaiPhieu.DangHieuLuc)
                .GroupBy(p => new { p.NgayDangKy.Year, p.NgayDangKy.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, DoanhThu = g.Sum(p => p.TongTien) })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .AsEnumerable()
                .Select(x => (x.Year, x.Month, x.DoanhThu))
                .ToList();
        }
    }
}