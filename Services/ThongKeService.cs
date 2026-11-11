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

        
        public List<ThongKeItemViewModel> GoiTheoLoai() =>
            _goi.DemTheoLoai()
                .Select(x => new ThongKeItemViewModel { Ten = x.Ten, SoLuong = x.SoLuong })
                .ToList();

        public List<ThongKeItemViewModel> PhieuTheoTrangThai() =>
            _phieu.DemTheoTrangThai()
                  .Select(x => new ThongKeItemViewModel { Ten = x.Trang, SoLuong = x.SoLuong })
                  .ToList();

        public List<ThongKeItemViewModel> LuotTheoGoi() =>
            _phieu.DemTheoGoi()
                  .Select(x => new ThongKeItemViewModel { Ten = x.Ten, SoLuong = x.SoLuong })
                  .ToList();

        public List<ThongKeItemViewModel> LuotTheoLoai() =>
            _phieu.DemTheoLoaiGoi()
                  .Select(x => new ThongKeItemViewModel { Ten = x.Ten, SoLuong = x.SoLuong })
                  .ToList();

        public ThongKeItemViewModel? GoiNhieuNhat() =>
            LuotTheoGoi().FirstOrDefault();

        // GIỮ NGUYÊN lọc ở Service - IHoiVienRepository thuộc Module 3, không được sửa
        public List<ThongKeItemViewModel> HoiVienTheoTrangThai() =>
            _hv.GetAll()
               .GroupBy(h => h.TrangThai)
               .Select(g => new ThongKeItemViewModel { Ten = g.Key, SoLuong = g.Count() })
               .OrderByDescending(x => x.SoLuong)
               .ToList();

        public List<LuotDangKyThangViewModel> LuotTheoThang() =>
            _phieu.DemTheoThang()
                  .Select(x => new LuotDangKyThangViewModel { Nam = x.Nam, Thang = x.Thang, SoLuot = x.SoLuot })
                  .ToList();

        public List<DoanhThuThangViewModel> DoanhThuTheoThang() =>
            _phieu.TinhDoanhThuTheoThang()
                  .Select(x => new DoanhThuThangViewModel { Nam = x.Nam, Thang = x.Thang, DoanhThu = x.DoanhThu })
                  .ToList();
        
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