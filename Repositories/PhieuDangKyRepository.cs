using Microsoft.EntityFrameworkCore;

using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
    public class PhieuDangKyRepository
    {
        private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

       
        public PhieuDangKyRepository(QuanLyPhongTap_UNETI01_TI17A1HNContext context)
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
      
        public List<PhieuDangKy> GetAll(string tenHoiVien = "", int? goiTapId = null,
                                         string trangThai = "", DateTime? ngayTu = null)
        {
            var query = _context.PhieuDangKy.AsQueryable();

            if (!string.IsNullOrEmpty(tenHoiVien))
                query = query.Where(p => p.HoiVien.HoTen.Contains(tenHoiVien));

            if (goiTapId.HasValue)
                query = query.Where(p => p.IdGoiTap == goiTapId);

            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(p => p.TrangThai == trangThai);

            if (ngayTu.HasValue)
                query = query.Where(p => p.NgayDangKy >= ngayTu);

            return query.Include(p => p.HoiVien)
                       .Include(p => p.GoiTap)
                       .ToList();
        }
    }
}




        
