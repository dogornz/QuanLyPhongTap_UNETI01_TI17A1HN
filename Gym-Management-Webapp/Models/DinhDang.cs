// Họ và tên: [Điền họ và tên của bạn]
// Mã sinh viên: [Điền mã sinh viên của bạn]
// Nội dung thực hiện: Module 2 - Định dạng tiền tệ, ngày tháng và màu trạng thái gói tập
using System.Globalization;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Helpers
{
    public static class DinhDang
    {
        private static readonly CultureInfo ViVN = new("vi-VN");

        /// <summary>1500000 -> "1.500.000 ₫"</summary>
        public static string Tien(this decimal soTien) => soTien.ToString("N0", ViVN) + " ₫";

        /// <summary>Lớp CSS badge của Bootstrap theo trạng thái gói tập.</summary>
        public static string LopTrangThai(this string trangThai) => trangThai switch
        {
            TrangThaiGoiTap.DangApDung => "bg-success",
            TrangThaiGoiTap.TamNgung => "bg-warning text-dark",
            _ => "bg-secondary"
        };
    }
}
