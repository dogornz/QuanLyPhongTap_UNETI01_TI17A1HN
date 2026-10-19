<<<<<<< Updated upstream
﻿using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
=======
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;
>>>>>>> Stashed changes
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services.QuanLyPhongTap_UNETI01_TI17A1HN.Services;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class PhieuDangKyService
    {
        private readonly PhieuDangKyRepository _repo;

        public PhieuDangKyService(PhieuDangKyRepository repo)
        {
            _repo = repo;
        }

<<<<<<< Updated upstream
        
=======
        // ===== Truy vấn =====

>>>>>>> Stashed changes
        public List<PhieuDangKy> GetAll()
        {
            return _repo.GetAll();
        }

<<<<<<< Updated upstream
        
        public PhieuDangKy GetById(int id)
=======
        public PhieuDangKy? GetById(string maPhieu)
>>>>>>> Stashed changes
        {
            return _repo.GetById(id);
        }

<<<<<<< Updated upstream
        
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
=======
        // 8.1: Lọc theo hội viên, gói tập, trạng thái, khoảng ngày đăng ký
        public List<PhieuDangKy> TimKiem(string? maHoiVien, string? maGoiTap, string? trangThai,
                                         DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _phieuRepository.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(maHoiVien))
                query = query.Where(p => p.MaHoiVien == maHoiVien);

            if (!string.IsNullOrWhiteSpace(maGoiTap))
                query = query.Where(p => p.MaGoiTap == maGoiTap);

            if (!string.IsNullOrWhiteSpace(trangThai))
                query = query.Where(p => p.TrangThai == trangThai);

            if (tuNgay.HasValue)
                query = query.Where(p => p.NgayDangKy >= tuNgay.Value.Date);

            if (denNgay.HasValue)
                query = query.Where(p => p.NgayDangKy < denNgay.Value.Date.AddDays(1));

            return query.OrderByDescending(p => p.NgayDangKy).ToList();
        }

        public List<HoiVien> GetHoiViens()
        {
            return _hoiVienRepository.GetAll()
                .Where(h => h.TrangThai == TrangThaiHoiVien.HoatDong)
                .OrderBy(h => h.HoTen)
                .ToList();
        }

        public List<GoiTap> GetGoiTaps()
        {
            return _goiTapRepository.GetGoiDangApDung();
        }

        // ===== 6. Đăng ký gói tập =====

        public (bool Success, string Message) DangKy(string maHoiVien, string maGoiTap, DateTime ngayBatDau)
        {
            if (string.IsNullOrWhiteSpace(maHoiVien))
                return (false, "Vui lòng chọn hội viên.");

            if (string.IsNullOrWhiteSpace(maGoiTap))
                return (false, "Vui lòng chọn gói tập.");

            if (ngayBatDau == default)
                return (false, "Vui lòng chọn ngày bắt đầu.");

            if (ngayBatDau.Date < DateTime.Today)
                return (false, "Ngày bắt đầu không được ở trong quá khứ.");

            var hoiVien = _hoiVienRepository.GetById(maHoiVien);
            if (hoiVien == null)
                return (false, "Hội viên không tồn tại.");

            if (hoiVien.TrangThai != TrangThaiHoiVien.HoatDong)
                return (false, "Hội viên hiện không hoạt động.");

            var goiTap = _goiTapRepository.GetById(maGoiTap);
            if (goiTap == null)
                return (false, "Gói tập không tồn tại.");

            if (goiTap.TrangThai != TrangThaiGoiTap.ApDung)
                return (false, "Gói tập hiện không được áp dụng.");

            if (!int.TryParse(goiTap.ThoiHanThang, out int soThang) || soThang <= 0)
                return (false, "Thời hạn gói tập không hợp lệ.");

            var phieu = new PhieuDangKy
            {
                MaPhieu = TaoMaPhieu(),
                MaHoiVien = maHoiVien,
                MaGoiTap = maGoiTap,
                NgayDangKy = DateTime.Now,
                NgayBatDau = ngayBatDau.Date,
                NgayKetThuc = ngayBatDau.Date.AddMonths(soThang),
                DonGia = goiTap.DonGia,
                TongTien = goiTap.DonGia,
                TrangThai = TrangThaiPhieu.ChoXacNhan
            };

            try
            {
                _phieuRepository.Add(phieu);
                _phieuRepository.Save();
                return (true, "Đăng ký gói tập thành công. Phiếu đang chờ xác nhận.");
            }
            catch (Exception)
            {
                return (false, "Không thể lưu phiếu đăng ký. Vui lòng thử lại.");
            }
        }

        // ===== 8.2: Xác nhận =====

        public (bool Success, string Message) XacNhan(string maPhieu)
        {
            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai != TrangThaiPhieu.ChoXacNhan)
                return (false, "Chỉ xác nhận được phiếu đang ở trạng thái Chờ xác nhận.");

            var hoiVien = _hoiVienRepository.GetById(phieu.MaHoiVien);
            if (hoiVien == null || hoiVien.TrangThai != TrangThaiHoiVien.HoatDong)
                return (false, "Hội viên không còn hoạt động.");

            var goiTap = _goiTapRepository.GetById(phieu.MaGoiTap);
            if (goiTap == null)
                return (false, "Gói tập không còn tồn tại.");

            if (goiTap.TrangThai != TrangThaiGoiTap.ApDung)
                return (false, "Gói tập không còn được áp dụng.");

            phieu.TrangThai = TrangThaiPhieu.DangHieuLuc;

            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Xác nhận phiếu #{phieu.MaPhieu} thành công.");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi cập nhật trạng thái phiếu.");
            }
        }

        // ===== 8.3: Gia hạn =====

        public (bool Success, string Message) GiaHan(string maPhieu, int soThang)
        {
            if (soThang <= 0)
                return (false, "Số tháng gia hạn phải lớn hơn 0.");

            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai == TrangThaiPhieu.DaHuy)
                return (false, "Không thể gia hạn phiếu đã hủy.");

            if (phieu.TrangThai != TrangThaiPhieu.DangHieuLuc)
                return (false, "Chỉ gia hạn được phiếu đang có hiệu lực.");

            var goiTap = _goiTapRepository.GetById(phieu.MaGoiTap);
            if (goiTap == null)
                return (false, "Gói tập không còn tồn tại.");

            // Còn hạn: cộng tiếp từ ngày kết thúc cũ. Hết hạn: tính từ hôm nay.
            // Quy tắc này nhóm cần thống nhất.
            var ngayGoc = phieu.NgayKetThuc > DateTime.Now ? phieu.NgayKetThuc : DateTime.Now;
            phieu.NgayKetThuc = ngayGoc.AddMonths(soThang);

            // Giá trị thanh toán lấy từ dữ liệu hệ thống, không nhận từ người dùng
            phieu.TongTien += goiTap.DonGia;

            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Gia hạn phiếu #{phieu.MaPhieu} thêm {soThang} tháng. Hết hạn mới: {phieu.NgayKetThuc:dd/MM/yyyy}.");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi gia hạn phiếu.");
            }
        }

        // ===== 8.4: Hủy =====

        public (bool Success, string Message) Huy(string maPhieu)
>>>>>>> Stashed changes
        {
            var phieu = GetById(id);
            if (phieu == null || phieu.TrangThai == "Đã hủy") return false;
            if (thoiHan <= 0 || phieu.GoiTap == null) return false;

<<<<<<< Updated upstream
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
=======
            if (phieu.TrangThai == TrangThaiPhieu.DaHuy)
                return (false, "Phiếu này đã được hủy trước đó.");

            phieu.TrangThai = TrangThaiPhieu.DaHuy;

            try
            {
                _phieuRepository.Update(phieu);
                _phieuRepository.Save();
                return (true, $"Đã hủy phiếu #{phieu.MaPhieu}.");
            }
            catch (Exception)
            {
                return (false, "Lỗi khi hủy phiếu.");
            }
        }

        // ===== Tiện ích =====

        private static string TaoMaPhieu()
        {
            return "PDK" + Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
>>>>>>> Stashed changes
        }
    }
}