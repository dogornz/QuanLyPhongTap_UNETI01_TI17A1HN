
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

        // 7. Sinh mã phiếu đăng ký
        private static string TaoMaPhieu()
        {
            return "PDK" +
                   Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
        }
    }
}
