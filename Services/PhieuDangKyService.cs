
using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class PhieuDangKyService
    {
        private readonly IPhieuDangKyRepository _phieuRepository;
        private readonly IGoiTapRepository _goiTapRepository;
        private readonly IHoiVienRepository _hoiVienRepository;

        public PhieuDangKyService(
            IPhieuDangKyRepository phieuRepository,
            IGoiTapRepository goiTapRepository,
            IHoiVienRepository hoiVienRepository)
        {
            _phieuRepository = phieuRepository;
            _goiTapRepository = goiTapRepository;
            _hoiVienRepository = hoiVienRepository;
        }

        // 1. Lấy toàn bộ phiếu đăng ký
        public List<PhieuDangKy> GetAll()
        {
            return _phieuRepository.GetAll();
        }

        // 2. Lấy phiếu đăng ký theo mã phiếu
        public PhieuDangKy? GetById(string maPhieu)
        {
            if (string.IsNullOrWhiteSpace(maPhieu))
                return null;

            return _phieuRepository.GetById(maPhieu);
        }

        // 3. Lọc phiếu đăng ký theo hội viên
        public List<PhieuDangKy> GetByHoiVien(string maHoiVien)
        {
            if (string.IsNullOrWhiteSpace(maHoiVien))
                return GetAll();

            return GetAll()
                .Where(p => p.MaHoiVien == maHoiVien)
                .ToList();
        }

        // 4. Lấy danh sách hội viên đang hoạt động
        public List<HoiVien> GetHoiViens()
        {
            return _hoiVienRepository.GetAll()
                .Where(h => h.TrangThai == "Đang hoạt động")
                .OrderBy(h => h.HoTen)
                .ToList();
        }

        // 5. Lấy danh sách gói tập đang áp dụng
        public List<GoiTap> GetGoiTaps()
        {
            return _goiTapRepository.GetGoiDangApDung();
        }

        // 6. Đăng ký gói tập
        public (bool Success, string Message) DangKy(
            string maHoiVien,
            string maGoiTap,
            DateTime ngayBatDau)
        {
            // Kiểm tra mã hội viên
            if (string.IsNullOrWhiteSpace(maHoiVien))
            {
                return (false, "Vui lòng chọn hội viên.");
            }

            // Kiểm tra mã gói tập
            if (string.IsNullOrWhiteSpace(maGoiTap))
            {
                return (false, "Vui lòng chọn gói tập.");
            }

            // Kiểm tra ngày bắt đầu
            if (ngayBatDau == default)
            {
                return (false, "Vui lòng chọn ngày bắt đầu.");
            }

            if (ngayBatDau.Date < DateTime.Today)
            {
                return (false, "Ngày bắt đầu không được ở trong quá khứ.");
            }

            // Kiểm tra hội viên tồn tại
            var hoiVien = _hoiVienRepository.GetById(maHoiVien);

            if (hoiVien == null)
            {
                return (false, "Hội viên không tồn tại.");
            }

            if (hoiVien.TrangThai != "Đang hoạt động")
            {
                return (false, "Hội viên hiện không hoạt động.");
            }

            // Kiểm tra gói tập tồn tại
            var goiTap = _goiTapRepository.GetById(maGoiTap);

            if (goiTap == null)
            {
                return (false, "Gói tập không tồn tại.");
            }

            if (goiTap.TrangThai != "Đang áp dụng")
            {
                return (false, "Gói tập hiện không được áp dụng.");
            }

            // Model GoiTap hiện tại sử dụng string ThoiHanThang
            if (!int.TryParse(goiTap.ThoiHanThang, out int soThang)
                || soThang <= 0)
            {
                return (false, "Thời hạn gói tập không hợp lệ.");
            }

            // Tính ngày kết thúc
            DateTime ngayKetThuc;

            try
            {
                ngayKetThuc = ngayBatDau.Date.AddMonths(soThang);
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, "Không thể tính ngày kết thúc của gói tập.");
            }

            // Tạo phiếu đăng ký mới
            var phieu = new PhieuDangKy
            {
                MaPhieu = TaoMaPhieu(),
                MaHoiVien = maHoiVien,
                MaGoiTap = maGoiTap,
                NgayDangKy = DateTime.Now,
                NgayBatDau = ngayBatDau.Date,
                NgayKetThuc = ngayKetThuc,
                DonGia = goiTap.DonGia,
                TongTien = goiTap.DonGia,
                TrangThai = "Chờ xác nhận"
            };

            try
            {
                _phieuRepository.Add(phieu);
                _phieuRepository.Save();

                return (
                    true,
                    "Đăng ký gói tập thành công. Phiếu đang chờ xác nhận."
                );
            }
            catch (Exception)
            {
                return (
                    false,
                    "Không thể lưu phiếu đăng ký. Vui lòng kiểm tra dữ liệu và thử lại."
                );
            }
        }

        // 7. Xác nhận phiếu đăng ký
        public (bool Success, string Message) XacNhan(string maPhieu)
        {
            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai != "Chờ xác nhận")
                return (false, "Chỉ có thể xác nhận phiếu đang ở trạng thái Chờ xác nhận.");

            phieu.TrangThai = "Đang hiệu lực";
            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Xác nhận kích hoạt phiếu #{phieu.MaPhieu} thành công.");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi cập nhật trạng thái phiếu đăng ký.");
            }
        }

        // 8. Hủy phiếu đăng ký
        public (bool Success, string Message) Huy(string maPhieu)
        {
            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai == "Đã hủy")
                return (false, "Phiếu này đã được hủy trước đó.");

            phieu.TrangThai = "Đã hủy";
            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Đã hủy phiếu #{phieu.MaPhieu} thành công.");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi hủy phiếu đăng ký.");
            }
        }

        // 9. Gia hạn phiếu đăng ký
        public (bool Success, string Message) GiaHan(string maPhieu, int soThang = 1)
        {
            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai != "Đang hiệu lực")
                return (false, "Chỉ có thể gia hạn cho phiếu đang có hiệu lực.");

            if (soThang <= 0)
                return (false, "Số tháng gia hạn phải lớn hơn 0.");

            var baseDate = phieu.NgayKetThuc > DateTime.Now ? phieu.NgayKetThuc : DateTime.Now;
            phieu.NgayKetThuc = baseDate.AddMonths(soThang);
            if (phieu.GoiTap != null)
            {
                phieu.TongTien += phieu.GoiTap.DonGia;
            }

            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Gia hạn phiếu #{phieu.MaPhieu} thêm {soThang} tháng thành công (Hết hạn mới: {phieu.NgayKetThuc:dd/MM/yyyy}).");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi gia hạn phiếu đăng ký.");
            }
        }

        // 10. Sinh mã phiếu đăng ký
        private static string TaoMaPhieu()
        {
            return "PDK" +
                   Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
        }
    }
}
