using Microsoft.AspNetCore.Identity;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            return _hasher.HashPassword(new object(), password);
        }

        public bool VerifyPassword(string storedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(storedPassword) || string.IsNullOrEmpty(providedPassword))
            {
                return false;
            }

            // Nếu mật khẩu lưu trữ là chuỗi hash PBKDF2 của ASP.NET Core Identity
            if (IsHashed(storedPassword))
            {
                try
                {
                    var result = _hasher.VerifyHashedPassword(new object(), storedPassword, providedPassword);
                    return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
                }
                catch
                {
                    return false;
                }
            }

            // Fallback an toàn: So sánh trực tiếp nếu là tài khoản cũ chưa kịp migrate
            return storedPassword == providedPassword;
        }

        public bool IsHashed(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 30)
            {
                return false;
            }

            try
            {
                var bytes = Convert.FromBase64String(password);
                // Định dạng hash ASP.NET Core Identity v2 (0x00) hoặc v3 (0x01)
                return bytes.Length > 0 && (bytes[0] == 0x00 || bytes[0] == 0x01);
            }
            catch
            {
                return false;
            }
        }
    }
}

