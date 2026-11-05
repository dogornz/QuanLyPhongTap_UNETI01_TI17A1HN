namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public interface IPasswordService
    {
        /// <summary>
        /// Băm mật khẩu sử dụng thuật toán chuẩn PBKDF2 HMAC-SHA256 (100.000 iterations).
        /// </summary>
        string HashPassword(string password);

        /// <summary>
        /// Xác thực mật khẩu cung cấp với mật khẩu đã lưu (hỗ trợ cả hash PBKDF2 và legacy plain text).
        /// </summary>
        bool VerifyPassword(string storedPassword, string providedPassword);

        /// <summary>
        /// Kiểm tra xem chuỗi mật khẩu trong DB đã được băm theo chuẩn hay vẫn là plain text.
        /// </summary>
        bool IsHashed(string password);
    }
}

