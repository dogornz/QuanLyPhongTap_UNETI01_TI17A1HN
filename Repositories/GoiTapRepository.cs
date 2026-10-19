
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

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

        public List<GoiTap> GetGoiDangApDung()
        {
            return _context.GoiTap
                .Where(g => g.TrangThai == "Đang áp dụng")
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
