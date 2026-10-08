using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI_TI17A1HN.Models;
namespace QuanLyPhongTap_UNETI_TI17A1HN.Repositories
{
    public class PhieuDangKyRepository
    {
        private readonly QuanLyPhongTap_UNETI_TI17A1HNContext _context;

       
        public PhieuDangKyRepository(QuanLyPhongTap_UNETI_TI17A1HNContext context)
        {
            _context = context;
        }

        public List<PhieuDangKy> GetAll()
        {
            return _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .ToList();
        }

        
        public PhieuDangKy GetById(int id)
        {
            return _context.PhieuDangKy
                .Include(p => p.HoiVien)
                .Include(p => p.GoiTap)
                .FirstOrDefault(p => p.Id == id);
        }

      
        public void Add(PhieuDangKy phieu)
        {
            _context.PhieuDangKy.Add(phieu);
            _context.SaveChanges();
        }

      
        public void Update(PhieuDangKy phieu)
        {
            _context.PhieuDangKy.Update(phieu);
            _context.SaveChanges();
        }

       
        public void Delete(int id)
        {
            var phieu = GetById(id);
            if (phieu != null)
            {
                _context.PhieuDangKy.Remove(phieu);
                _context.SaveChanges();
            }
        }
    }
}




        
