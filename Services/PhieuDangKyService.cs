using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class PhieuDangKyService
    {
        private readonly PhieuDangKyRepository _repo;

        public PhieuDangKyService(PhieuDangKyRepository repo)
        {
            _repo = repo;
        }

        
        public List<PhieuDangKy> GetAll()
        {
            return _repo.GetAll();
        }

        
        public PhieuDangKy GetById(int id)
        {
            return _repo.GetById(id);
        }

        
        public bool Add(PhieuDangKy phieu)
        {
            if (phieu == null) return false;
            phieu.TrangThai = "Chờ xác nhận";
            _repo.Add(phieu);
            return true;
        }

       
        public bool XacNhan(int id)
        {
            var phieu = GetById(id);
            if (phieu == null || phieu.TrangThai != "Chờ xác nhận") return false;
            if (phieu.HoiVien == null || phieu.HoiVien.TrangThai != "active") return false;
            if (phieu.GoiTap == null || !phieu.GoiTap.ConApDung) return false;

            phieu.TrangThai = "Đang hiệu lực";
            _repo.Update(phieu);
            return true;
        }

       
        public bool GiaHan(int id, int thoiHan)
        {
            var phieu = GetById(id);
            if (phieu == null || phieu.TrangThai == "Đã hủy") return false;
            if (thoiHan <= 0 || phieu.GoiTap == null) return false;

            phieu.NgayKetThuc = phieu.NgayKetThuc.AddDays(thoiHan);
            phieu.DonGia = phieu.GoiTap.GiaGoiTap;
            _repo.Update(phieu);
            return true;
        }

       
        public bool Huy(int id)
        {
            var phieu = GetById(id);
            if (phieu == null) return false;

            phieu.TrangThai = "Đã hủy";
            _repo.Update(phieu);
            return true;
        }
    }
}
