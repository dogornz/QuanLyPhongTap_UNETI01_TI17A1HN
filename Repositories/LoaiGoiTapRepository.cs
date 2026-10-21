using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public class LoaiGoiTapRepository : ILoaiGoiTapRepository
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

        public LoaiGoiTapRepository(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
        {
            _context = context;
        }

        public List<LoaiGoiTap> GetAll()
        {
            return _context.LoaiGoiTap.ToList();
        }
    }
}
    