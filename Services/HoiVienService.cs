using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class HoiVienService
    {
        private readonly IHoiVienRepository _repo;

        public HoiVienService(IHoiVienRepository repo)
        {
            _repo = repo;
        }

        public List<HoiVien> GetAll() => _repo.GetAll();

        public List<HoiVien> Search(string keyword) => _repo.Search(keyword);

        public HoiVien? GetById(string id) => _repo.GetById(id);

        public (bool success, string message) Add(HoiVien model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.MaHoiVien))
                {
                    model.MaHoiVien = "HV" + DateTime.Now.Ticks;
                }

                _repo.Add(model);
                _repo.Save();
                return (true, "Thêm hội viên thành công.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi thêm hội viên: " + ex.Message);
            }
        }

        public (bool success, string message) Update(HoiVien model)
        {
            try
            {
                var exist = _repo.GetById(model.MaHoiVien);
                if (exist == null) return (false, "Hội viên không tồn tại.");

                _repo.Update(model);
                _repo.Save();
                return (true, "Cập nhật thông tin hội viên thành công.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi cập nhật: " + ex.Message);
            }
        }

        public (bool success, string message) Delete(string maHoiVien)
        {
            try
            {
                var exist = _repo.GetById(maHoiVien);
                if (exist == null) return (false, "Hội viên không tồn tại.");

                _repo.Delete(exist);
                _repo.Save();
                return (true, "Xóa hội viên thành công.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi xóa: " + ex.Message);
            }
        }

        public (bool success, string message) UpdateTrangThai(string maHv, string trangThai)
        {
            try
            {
                var ok = _repo.UpdateTrangThai(maHv, trangThai);
                return ok ? (true, "Cập nhật trạng thái thành công.") : (false, "Không tìm thấy hội viên.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi cập nhật trạng thái: " + ex.Message);
            }
        }
    }
}
