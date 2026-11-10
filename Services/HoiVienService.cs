using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class HoiVienService
    {
        private readonly IHoiVienRepository _repository;
        private readonly IPhieuDangKyRepository _phieuRepository;

        public HoiVienService(
            IHoiVienRepository repository,
            IPhieuDangKyRepository phieuRepository)
        {
            _repository = repository;
            _phieuRepository = phieuRepository;
        }

        // ================================
        // KIỂM TRA GÓI TẬP CÒN HẠN
        // ================================
        public List<PhieuDangKy> GetActivePackages(string maHv)
        {
            if (string.IsNullOrWhiteSpace(maHv))
            {
                return new List<PhieuDangKy>();
            }

            var today = DateTime.Today;
            return _phieuRepository.GetAll()
                .Where(p => p.MaHoiVien == maHv
                            && p.TrangThai != "Đã hủy"
                            && p.NgayKetThuc.Date >= today)
                .OrderBy(p => p.NgayKetThuc)
                .ToList();
        }

        public bool HasActivePackage(string maHv)
        {
            return GetActivePackages(maHv).Any();
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

                if (HasActivePackage(maHv))
                {
                    return (false, "Hội viên còn thời gian tập! Không thể xóa hội viên này.");
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
