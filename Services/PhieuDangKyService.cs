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
                .OrderByDescending(p => p.NgayDangKy)
                .ToList();
        }

        // Lấy phiếu đăng ký còn thời gian tập (chưa hết hạn) của hội viên
        public PhieuDangKy? GetActiveTicket(string maHoiVien)
        {
            if (string.IsNullOrWhiteSpace(maHoiVien))
                return null;

            var today = DateTime.Today;
            return _phieuRepository.GetAll()
                .Where(p => p.MaHoiVien == maHoiVien
                            && p.TrangThai != "Đã hủy"
                            && p.TrangThai != "Da huy"
                            && p.NgayKetThuc.Date >= today)
                .OrderByDescending(p => p.NgayKetThuc)
                .FirstOrDefault();
        }

        // 4. Lấy danh sách hội viên đang hoạt động
        public List<HoiVien> GetHoiViens()
        {
            return _hoiVienRepository.GetAll()
                .Where(h => h.TrangThai != "Ngừng hoạt động" && h.TrangThai != "Lock")
                .OrderBy(h => h.HoTen)
                .ToList();
        }

        // 5. Lấy danh sách gói tập đang áp dụng
        public List<GoiTap> GetGoiTaps()
        {
            return _goiTapRepository.GetGoiDangApDung();
        }

        // 6. Đăng ký gói tập
        public (bool Success, string Message, bool RequireConfirm, string? TenGoiHienTai) DangKy(
            string maHoiVien,
            string maGoiTap,
            DateTime ngayBatDau,
            bool xacNhanGoiKhac = false)
        {
            // Kiểm tra mã hội viên
            if (string.IsNullOrWhiteSpace(maHoiVien))
            {
                return (false, "Vui lòng chọn hội viên.", false, null);
            }

            // Kiểm tra mã gói tập
            if (string.IsNullOrWhiteSpace(maGoiTap))
            {
                return (false, "Vui lòng chọn gói tập.", false, null);
            }

            // Kiểm tra ngày bắt đầu
            if (ngayBatDau == default)
            {
                return (false, "Vui lòng chọn ngày bắt đầu.", false, null);
            }

            if (ngayBatDau.Date < DateTime.Today)
            {
                return (false, "Ngày bắt đầu không được ở trong quá khứ.", false, null);
            }

            // Kiểm tra hội viên tồn tại
            var hoiVien = _hoiVienRepository.GetById(maHoiVien);
            if (hoiVien == null)
            {
                return (false, "Hội viên không tồn tại.", false, null);
            }

            if (hoiVien.TrangThai == "Ngừng hoạt động" || hoiVien.TrangThai == "Lock")
            {
                return (false, "Hội viên hiện không hoạt động.", false, null);
            }

            // Kiểm tra gói tập tồn tại
            var goiTap = _goiTapRepository.GetById(maGoiTap);
            if (goiTap == null)
            {
                return (false, "Gói tập không tồn tại.", false, null);
            }

            if (goiTap.TrangThai == "Ngừng áp dụng" || goiTap.TrangThai == "Inactive")
            {
                return (false, "Gói tập hiện không được áp dụng.", false, null);
            }

            // Model GoiTap hiện tại sử dụng string ThoiHanThang
            if (!int.TryParse(goiTap.ThoiHanThang, out int soThang) || soThang <= 0)
            {
                return (false, "Thời hạn gói tập không hợp lệ.", false, null);
            }

            // Kiểm tra xem hội viên có còn thời gian tập của gói tập nào không
            var activeTicket = GetActiveTicket(maHoiVien);

            DateTime actualNgayBatDau = ngayBatDau.Date;
            DateTime actualNgayKetThuc;
            string successMessage;

            if (activeTicket != null)
            {
                string tenGoiHienTai = activeTicket.GoiTap?.TenGoiTap ?? activeTicket.MaGoiTap;

                if (activeTicket.MaGoiTap == maGoiTap)
                {
                    // KHÔNG CHO PHÉP ĐĂNG KÝ THÊM GÓI ĐANG HOẠT ĐỘNG (CHỈ CÓ QUYỀN GIA HẠN THÊM)
                    return (false,
                        $"Hội viên đang trong thời gian sử dụng gói tập '{tenGoiHienTai}' (Thời gian hoạt động: từ {activeTicket.NgayBatDau:dd/MM/yyyy} đến {activeTicket.NgayKetThuc:dd/MM/yyyy}). Hệ thống không cho phép đăng ký thêm gói đang hoạt động, bạn chỉ có thể gia hạn thêm gói tập này!",
                        false,
                        null);
                }
                else
                {
                    // KHÁC GÓI: Cảnh báo "bạn đang trong thời gian sử dụng của gói tập...(tên gói đang sử dụng và thời gian hoạt động), bạn có xác nhận đăng ký gói khác không?"
                    if (!xacNhanGoiKhac)
                    {
                        string canhBao = $"Cảnh báo: Bạn đang trong thời gian sử dụng của gói tập {tenGoiHienTai} (Thời gian hoạt động: từ {activeTicket.NgayBatDau:dd/MM/yyyy} đến {activeTicket.NgayKetThuc:dd/MM/yyyy}), bạn có xác nhận đăng ký gói khác không?";
                        return (false, canhBao, true, tenGoiHienTai);
                    }

                    // Nếu người dùng đã xác nhận muốn đăng ký gói khác:
                    try
                    {
                        actualNgayKetThuc = actualNgayBatDau.AddMonths(soThang);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return (false, "Không thể tính ngày kết thúc của gói tập.", false, null);
                    }

                    var phieuGoiKhac = new PhieuDangKy
                    {
                        MaPhieu = TaoMaPhieu(),
                        MaHoiVien = maHoiVien,
                        MaGoiTap = maGoiTap,
                        NgayDangKy = DateTime.Now,
                        NgayBatDau = actualNgayBatDau,
                        NgayKetThuc = actualNgayKetThuc,
                        DonGia = goiTap.DonGia,
                        TongTien = goiTap.DonGia,
                        TrangThai = "Chờ xác nhận"
                    };

                    try
                    {
                        _phieuRepository.Add(phieuGoiKhac);
                        _phieuRepository.Save();

                        successMessage = $"Đăng ký gói tập '{goiTap.TenGoiTap}' thành công khi đang sử dụng gói '{tenGoiHienTai}' (Hiệu lực: {actualNgayBatDau:dd/MM/yyyy} đến {actualNgayKetThuc:dd/MM/yyyy}). Phiếu đang chờ xác nhận.";
                        return (true, successMessage, false, null);
                    }
                    catch (Exception)
                    {
                        return (false, "Không thể lưu phiếu đăng ký. Vui lòng thử lại.", false, null);
                    }
                }
            }

            // KHÔNG CÓ GÓI CÒN HẠN: Đăng ký bình thường
            try
            {
                actualNgayKetThuc = actualNgayBatDau.AddMonths(soThang);
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, "Không thể tính ngày kết thúc của gói tập.", false, null);
            }

            var phieuMoi = new PhieuDangKy
            {
                MaPhieu = TaoMaPhieu(),
                MaHoiVien = maHoiVien,
                MaGoiTap = maGoiTap,
                NgayDangKy = DateTime.Now,
                NgayBatDau = actualNgayBatDau,
                NgayKetThuc = actualNgayKetThuc,
                DonGia = goiTap.DonGia,
                TongTien = goiTap.DonGia,
                TrangThai = "Chờ xác nhận"
            };

            try
            {
                _phieuRepository.Add(phieuMoi);
                _phieuRepository.Save();

                return (true, "Đăng ký gói tập thành công. Phiếu đang chờ xác nhận.", false, null);
            }
            catch (Exception)
            {
                return (false, "Không thể lưu phiếu đăng ký. Vui lòng kiểm tra dữ liệu và thử lại.", false, null);
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
                return (true, $"Đã hủy phiếu #{phieu.MaPhieu} thành công. (Lưu ý: Hệ thống không hoàn lại tiền cho gói tập đã hủy).");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi hủy phiếu đăng ký.");
            }
        }

        // 9. Gia hạn phiếu đăng ký
        public (bool Success, string Message) GiaHan(string maPhieu, int soThang = 1, bool isHoiVien = false)
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

            if (isHoiVien)
            {
                phieu.TrangThai = "Chờ xác nhận";
            }

            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();

                if (isHoiVien)
                {
                    return (true, $"Yêu cầu gia hạn phiếu #{phieu.MaPhieu} thêm {soThang} tháng đã gửi thành công và chuyển sang trạng thái chờ duyệt (Hạn dự kiến: {phieu.NgayKetThuc:dd/MM/yyyy}).");
                }

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
