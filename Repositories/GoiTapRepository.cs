using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public class GoiTapRepository : IGoiTapRepository
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public GoiTapRepository(
            QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        public List<GoiTap> GetAll()
        {
            return _context.GoiTap
                .Include(g => g.LoaiGoiTap) // THÊM - cần cho thống kê theo loại
                .OrderBy(g => g.TenGoiTap)
                .ToList();
        }

        public List<GoiTap> GetGoiDangApDung()
        {
            return _context.GoiTap
                .Where(g => g.TrangThai == TrangThaiGoiTap.ApDung)
                .OrderBy(g => g.TenGoiTap)
                .ToList();
        }

        public GoiTap? GetById(string maGoiTap)
        {
            return _context.GoiTap
                .FirstOrDefault(g => g.MaGoiTap == maGoiTap);
        }

        // THÊM - 8.6: Số gói tập theo loại
        public List<(string Ten, int SoLuong)> DemTheoLoai()
        {
            return _context.GoiTap
                .Include(g => g.LoaiGoiTap)
                .GroupBy(g => g.LoaiGoiTap != null ? g.LoaiGoiTap.TenLoaiGoi : "Chưa phân loại")
                .Select(g => new { Ten = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .AsEnumerable()
                .Select(x => (x.Ten ?? "Chưa phân loại", x.SoLuong))
                .ToList();
        }
    }
}