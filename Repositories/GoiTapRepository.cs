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

        public List<GoiTap> GetAll() // THÊM
        {
            return _context.GoiTap
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
    }
}