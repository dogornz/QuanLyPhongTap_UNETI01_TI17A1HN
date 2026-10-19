using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class HoiVienService
    {
        private readonly IHoiVienRepository _repository;

        public HoiVienService(IHoiVienRepository repository)
        {
            _repository = repository;
        }

        // ================================
        // LẤY DANH SÁCH
        // ================================
        public List<HoiVien> GetAll()
        {
            return _repository.GetAll();
        }

        // ================================
        // TÌM KIẾM
        // ================================
        public List<HoiVien> Search(string keyword)
        {
            return _repository.Search(keyword);
        }

        // ================================
        // CẬP NHẬT TRẠNG THÁI
        // ================================
        public (bool success, string message) UpdateTrangThai(string maHv, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maHv))
            {
                return (false, "Mã hội viên không được để trống.");
            }

            if (trangThai != "Đang hoạt động" && trangThai != "Ngừng hoạt động")
            {
                return (false, "Trạng thái không hợp lệ.");
            }

            var hoiVien = _repository.GetById(maHv);

            if (hoiVien == null)
            {
                return (false, "Không tìm thấy hội viên.");
            }

            bool result = _repository.UpdateTrangThai(maHv, trangThai);

            if (result)
            {
                return (true, "Cập nhật trạng thái thành công.");
            }

            return (false, "Cập nhật trạng thái thất bại.");
        }

        public HoiVien? GetById(string maHv)
        {
            return _repository.GetById(maHv);
        }

        public (bool success, string message) Add(HoiVien model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.MaHoiVien))
                {
                    return (false, "Mã hội viên không được để trống.");
                }

                var existing = _repository.GetById(model.MaHoiVien);
                if (existing != null)
                {
                    return (false, "Mã hội viên đã tồn tại.");
                }

                _repository.Add(model);
                _repository.Save();
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
                _repository.Update(model);
                _repository.Save();
                return (true, "Cập nhật thông tin hội viên thành công.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi cập nhật hội viên: " + ex.Message);
            }
        }

        public (bool success, string message) Delete(string maHv)
        {
            try
            {
                var item = _repository.GetById(maHv);
                if (item == null)
                {
                    return (false, "Không tìm thấy hội viên cần xóa.");
                }

                _repository.Delete(item);
                _repository.Save();
                return (true, "Xóa hội viên thành công.");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi khi xóa hội viên (có thể do đã có phiếu đăng ký liên quan): " + ex.Message);
            }
        }
    }
}
