using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public static class DbInitializer
    {
        /// <summary>
        /// Tự động quét và mã hóa lại tất cả mật khẩu đang lưu dạng plain-text sang PBKDF2
        /// </summary>
        public static async Task MigrateLegacyPasswordsAsync(QuanLyPhongTap_UNETI01_TI17A1HNContext context, IPasswordService passwordService)
        {
            try
            {
                // Kiểm tra kết nối DB trước
                if (!await context.Database.CanConnectAsync())
                {
                    return;
                }

                var accounts = await context.TaiKhoan.ToListAsync();
                bool hasChanges = false;

                foreach (var account in accounts)
                {
                    if (!string.IsNullOrWhiteSpace(account.MatKhau) && !passwordService.IsHashed(account.MatKhau))
                    {
                        account.MatKhau = passwordService.HashPassword(account.MatKhau);
                        hasChanges = true;
                    }
                }

                if (hasChanges)
                {
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbInitializer] Lỗi khi migrate mật khẩu cũ: {ex.Message}");
            }
        }
    }
}

