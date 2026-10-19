using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Repositories
{
        public class HoiVienRepository : IHoiVienRepository
        {
            private readonly QuanLyPhongTap_UNETI01_TI17A1HNContext _context;

            public HoiVienRepository(
                QuanLyPhongTap_UNETI01_TI17A1HNContext context)
            {
                _context = context;
            }

            //lấy hv
            public List<HoiVien> GetAll()
            {
                return _context.HoiVien
                    
                    .ToList();
            }

            //lấy hv theo id
            public HoiVien? GetById(object id)
            {
                return _context.HoiVien
                    .Find(id);
            }

            
            public void Add(HoiVien entity)
            {
                _context.HoiVien.Add(entity);
            }

           
            public void Update(HoiVien entity)
            {
                _context.HoiVien.Update(entity);
            }

            public void Delete(HoiVien entity)
            {
                _context.HoiVien.Remove(entity);
            }

            
            public void Save()
            {
                _context.SaveChanges();
            }

            
            public List<HoiVien> Search(string keyword)
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return GetAll();
                }

                keyword = keyword.Trim().ToLower();

                return _context.HoiVien
                    .Where(h =>
                        h.MaHoiVien.ToLower().Contains(keyword) ||
                        h.HoTen.ToLower().Contains(keyword) ||
                        h.Email.ToLower().Contains(keyword) ||
                        h.SoDienThoai.Contains(keyword))
                    .ToList();
            }

            
            public bool UpdateTrangThai(
                string maHv,
                string trangThai)
            {
                var hoiVien = _context.HoiVien
                    .FirstOrDefault(h => h.MaHoiVien == maHv);

                if (hoiVien == null)
                {
                    return false;
                }

                hoiVien.TrangThai = trangThai;

                _context.SaveChanges();

                return true;
            }
        }
    }

