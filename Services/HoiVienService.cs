using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class HoiVienService
    {
        private readonly IHoiVienRepository _repository;

        public HoiVienService(
            IHoiVienRepository repository)
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
        public (bool success, string message)
            UpdateTrangThai(
                string maHv,
                string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maHv))
            {
                return (
                    false,
                    "Mã hội viên không được để trống."
                );
            }

            if (trangThai != "Đang hoạt động" &&
                trangThai != "Ngừng hoạt động")
            {
                return (
                    false,
                    "Trạng thái không hợp lệ."
                );
            }

            var hoiVien = _repository.GetById(maHv);

            if (hoiVien == null)
            {
                return (
                    false,
                    "Không tìm thấy hội viên."
                );
            }

            bool result = _repository.UpdateTrangThai(
                maHv,
                trangThai);

            if (result)
            {
                return (
                    true,
                    "Cập nhật trạng thái thành công."
                );
            }

            return (
                false,
                "Cập nhật trạng thái thất bại."
            );
        }
    }
    
}
