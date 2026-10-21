using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;
using QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class ThongKeService
    {
        private readonly IPhieuDangKyRepository _phieu;
        private readonly IGoiTapRepository _goi;
        private readonly IHoiVienRepository _hv;

        public ThongKeService(IPhieuDangKyRepository phieu, IGoiTapRepository goi,
                              IHoiVienRepository hv)
        {
            _phieu = phieu;
            _goi = goi;
            _hv = hv;
        }

        // Hàm dùng chung: nhóm theo khóa rồi đếm
        private static List<ThongKeItemViewModel> NhomDem<T>(
            IEnumerable<T> ds, Func<T, string?> khoa) =>
            ds.GroupBy(x => khoa(x) ?? "(Không rõ)")
              .Select(g => new ThongKeItemViewModel { Ten = g.Key, SoLuong = g.Count() })
              .OrderByDescending(x => x.SoLuong)
              .ToList();

        public List<ThongKeItemViewModel> GoiTheoLoai() =>
            NhomDem(_goi.GetAll(), g => g.LoaiGoiTap?.TenLoaiGoi ?? "Chưa phân loại");                 // KIỂM TRA

        public List<ThongKeItemViewModel> HoiVienTheoTrangThai() =>
            NhomDem(_hv.GetAll(), h => h.TrangThai);

        public List<ThongKeItemViewModel> PhieuTheoTrangThai() =>
            NhomDem(_phieu.GetAll(), p => p.TrangThai);

        public List<ThongKeItemViewModel> LuotTheoGoi() =>
            NhomDem(_phieu.GetAll(), p => p.GoiTap?.TenGoiTap ?? p.MaGoiTap);

        public List<ThongKeItemViewModel> LuotTheoLoai() =>
            NhomDem(_phieu.GetAll(), p => p.GoiTap?.LoaiGoiTap?.TenLoaiGoi ?? "Chưa phân loại");        // KIỂM TRA

        public ThongKeItemViewModel? GoiNhieuNhat() =>
            LuotTheoGoi().FirstOrDefault();   // đã OrderByDescending nên phần tử đầu là nhiều nhất

        public List<LuotDangKyThangViewModel> LuotTheoThang() =>
            _phieu.GetAll()
                .GroupBy(p => new { p.NgayDangKy.Year, p.NgayDangKy.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new LuotDangKyThangViewModel
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    SoLuot = g.Count()
                }).ToList();

        public List<DoanhThuThangViewModel> DoanhThuTheoThang() =>
            _phieu.GetAll()
                .Where(p => p.TrangThai == TrangThaiPhieu.DangHieuLuc)  // chỉ đăng ký hợp lệ
                .GroupBy(p => new { p.NgayDangKy.Year, p.NgayDangKy.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new DoanhThuThangViewModel
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    DoanhThu = g.Sum(p => p.TongTien)                   // KIỂM TRA kiểu
                }).ToList();

        // Gom 8 thống kê cho Controller
        public ThongKeViewModel Get() => new ThongKeViewModel
        {
            GoiTheoLoai = GoiTheoLoai(),
            HoiVienTheoTrangThai = HoiVienTheoTrangThai(),
            PhieuTheoTrangThai = PhieuTheoTrangThai(),
            LuotTheoGoi = LuotTheoGoi(),
            LuotTheoLoai = LuotTheoLoai(),
            GoiNhieuNhat = GoiNhieuNhat(),
            LuotTheoThang = LuotTheoThang(),
            DoanhThuTheoThang = DoanhThuTheoThang()
        };
    }
}