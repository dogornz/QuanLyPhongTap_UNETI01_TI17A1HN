// Họ và tên: [Điền họ và tên của bạn]
// Mã sinh viên: [Điền mã sinh viên của bạn]
// Nội dung thực hiện: Module 2 - Kiểm tra đăng nhập/phân quyền Admin tại Controller bằng Session
//
// THỐNG NHẤT VỚI MODULE 1: khi đăng nhập thành công, TaiKhoansController phải lưu vào Session
// đúng 3 khóa bên dưới (KeyMaTaiKhoan, KeyHoTen, KeyVaiTro). Vai trò lưu là "admin" hoặc "hoivien"
// (đúng như dữ liệu mẫu trong DbContext). Nếu Module 1 dùng tên khác thì sửa các hằng số ở đây.
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Filters
{
    public static class PhienDangNhap
    {
        public const string KeyMaTaiKhoan = "MaTaiKhoan";
        public const string KeyHoTen = "HoTen";
        public const string KeyVaiTro = "VaiTro";

        public const string VaiTroAdmin = "admin";
        public const string VaiTroHoiVien = "hoivien";

        // Đường dẫn trang đăng nhập (Module 1). Dùng đường dẫn chuỗi để Module 2 không bị lỗi khi Module 1 chưa xong.
        public const string TrangDangNhap = "/TaiKhoans/DangNhap";

        // Chỉ kiểm tra khóa có tồn tại, không phụ thuộc Module 1 lưu MaTaiKhoan bằng SetInt32 hay SetString.
        public static bool DaDangNhap(this ISession session)
            => session.TryGetValue(KeyMaTaiKhoan, out var giaTri) && giaTri.Length > 0;

        public static bool LaAdmin(this ISession session)
            => session.DaDangNhap()
               && string.Equals(session.GetString(KeyVaiTro), VaiTroAdmin, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Yêu cầu đã đăng nhập (Admin hoặc Hội viên). Chưa đăng nhập -> chuyển về trang đăng nhập.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class YeuCauDangNhapAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Session.DaDangNhap())
            {
                context.Result = new RedirectResult(PhienDangNhap.TrangDangNhap);
            }
        }
    }

    /// <summary>Chỉ Admin. Chạy trước model binding nên người không đủ quyền không thể gọi action bằng cách gõ URL.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class YeuCauAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;

            if (!session.DaDangNhap())
            {
                context.Result = new RedirectResult(PhienDangNhap.TrangDangNhap);
                return;
            }

            if (!session.LaAdmin())
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    ContentType = "text/html; charset=utf-8",
                    Content = "<h3>403 - Bạn không có quyền truy cập chức năng này.</h3><p><a href=\"/\">Về trang chủ</a></p>"
                };
            }
        }
    }
}
