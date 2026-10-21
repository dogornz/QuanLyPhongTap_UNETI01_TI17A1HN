using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;
using QuanLyPhongTap_UNETI01_TI17A1HN.ViewModels;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class DashboardService
    {
        private readonly IPhieuDangKyRepository _phieu;
        private readonly IGoiTapRepository _goi;
        private readonly IHoiVienRepository _hv;
        private readonly ILoaiGoiTapRepository _loai;

        public DashboardService(IPhieuDangKyRepository phieu, IGoiTapRepository goi,
                                IHoiVienRepository hv, ILoaiGoiTapRepository loai)
        {
            _phieu = phieu;
            _goi = goi;
            _hv = hv;
            _loai = loai;
        }

        public DashboardViewModel Get()
        {
            var phieus = _phieu.GetAll();
            var goiTaps = _goi.GetAll();

            return new DashboardViewModel
            {
                TongLoaiGoi = _loai.GetAll().Count,
                TongGoiTap = goiTaps.Count,
                TongHoiVien = _hv.GetAll().Count,
                TongPhieu = phieus.Count,
                PhieuChoXacNhan = phieus.Count(p => p.TrangThai == TrangThaiPhieu.ChoXacNhan),
                DangHieuLuc = phieus.Count(PhieuDangKyService.ConHieuLuc),
                HetHan = phieus.Count(PhieuDangKyService.HetHan),
                GoiDangApDung = goiTaps.Count(g => g.TrangThai == TrangThaiGoiTap.ApDung)
            };
        }
    }
}