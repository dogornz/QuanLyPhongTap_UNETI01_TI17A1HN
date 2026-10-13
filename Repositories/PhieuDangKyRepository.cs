
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

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

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
