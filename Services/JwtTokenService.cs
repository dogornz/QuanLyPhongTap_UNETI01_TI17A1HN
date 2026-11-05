using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(TaiKhoan taiKhoan)
        {
            string key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Key.");
            string issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Issuer.");
            string audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Audience.");

            int expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, taiKhoan.MaTaiKhoan.ToString()),
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                new Claim(ClaimTypes.Role, taiKhoan.VaiTro)
            };

            if (!string.IsNullOrWhiteSpace(taiKhoan.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, taiKhoan.Email));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

