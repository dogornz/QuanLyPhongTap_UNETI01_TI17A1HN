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
                return new List<PhieuDangKy>();

            return _phieuRepository.GetAll()
                .Where(p => p.MaHoiVien == maHoiVien)
                .OrderByDescending(p => p.NgayDangKy)
                .ToList();
        }

        // Lọc theo hội viên, gói tập, trạng thái, khoảng ngày đăng ký (8.1)
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

        // 4. Lấy danh sách hội viên đang hoạt động
        // GIỮ NGUYÊN - không chuyển xuống repository vì IHoiVienRepository thuộc Module 3
        public List<HoiVien> GetHoiViens()
        {
            return _hoiVienRepository.GetAll()
                .Where(h => h.TrangThai == TrangThaiHoiVien.HoatDong)
                .OrderBy(h => h.HoTen)
                .ToList();
        }

        // 5. Lấy danh sách gói tập đang áp dụng
        public List<GoiTap> GetGoiTaps()
        {
            return _goiTapRepository.GetGoiDangApDung();
        }

        // 6. Đăng ký gói tập (7.3, dùng chung với Module 3)
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
                TongTien = goiTap.DonGia * soThang,
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

        // 7. Xác nhận (8.2)
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

        // 8. Hủy (8.4)
        public (bool Success, string Message) Huy(string maPhieu)
        {
            var phieu = GetById(maPhieu);
            if (phieu == null)
                return (false, "Phiếu đăng ký không tồn tại.");

            if (phieu.TrangThai == TrangThaiPhieu.DaHuy)
                return (false, "Phiếu đã bị hủy trước đó.");

            if (HetHan(phieu)) // THÊM - đề 8.4: "chỉ hủy phiếu phù hợp trạng thái nghiệp vụ"
                return (false, "Phiếu đã hết hạn, không thể hủy.");

            phieu.TrangThai = TrangThaiPhieu.DaHuy;

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

        // 9. Gia hạn phiếu đăng ký (8.3)
        public (bool Success, string Message) GiaHan(string maPhieu, int soThang = 1)
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

            // ĐỔI: DateTime.Now -> DateTime.Today, để quy tắc hết hạn đồng nhất
            // với HetHan()/ConHieuLuc() và với DangKy() (đều dùng .Date)
            var homNay = DateTime.Today;
            var baseDate = phieu.NgayKetThuc.Date > homNay ? phieu.NgayKetThuc.Date : homNay;
            phieu.NgayKetThuc = baseDate.AddMonths(soThang);

            // tăng tiền theo số tháng và đơn giá hiện hành của gói
            phieu.TongTien += goiTap.DonGia * soThang;

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

        // 10. Tạo mã phiếu đăng ký
        private static string TaoMaPhieu()
        {
            return "PDK" + Guid.NewGuid().ToString("N")[..10].ToUpperInvariant();
        }

        // Dùng chung cho 8.4 (Hủy) và 8.5 (Dashboard)
        public static bool HetHan(PhieuDangKy p) =>
            p.TrangThai == TrangThaiPhieu.DangHieuLuc && p.NgayKetThuc.Date < DateTime.Today;

        public static bool ConHieuLuc(PhieuDangKy p) =>
            p.TrangThai == TrangThaiPhieu.DangHieuLuc && p.NgayKetThuc.Date >= DateTime.Today;
    }
}