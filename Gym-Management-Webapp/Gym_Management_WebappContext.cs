using Gym_Management_Webapp.Models;
using Microsoft.EntityFrameworkCore;

public class Gym_Management_WebappContext(DbContextOptions<Gym_Management_WebappContext> options) : DbContext(options)
{

    public DbSet<Gym_Management_Webapp.Models.TaiKhoan> TaiKhoan { get; set; } = default!;
    public DbSet<Gym_Management_Webapp.Models.LoaiGoiTap> LoaiGoiTap { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLPG;Integrated Security=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TaiKhoan>().HasData(
            new TaiKhoan
            {
                MaTaiKhoan = 1,
                TenDangNhap = "admin",
                MatKhau = "admin123",
                HoTen = "Admin",
                Email = "user1@gmail.com",
                VaiTro = "Admin",
                TrangThai = "Active"
            },
            new TaiKhoan
            {
                MaTaiKhoan = 2,
                TenDangNhap = "user",
                MatKhau = "user123",
                HoTen = "User",
                Email = "user2@gmail.com",
                VaiTro = "User",
                TrangThai = "Active"
            },
            new TaiKhoan
            {
                MaTaiKhoan = 3,
                TenDangNhap = "user3",
                MatKhau = "user123",
                HoTen = "User3",
                Email = "user3@gmail.com",
                VaiTro = "User",
                TrangThai = "Active"
            },
            new TaiKhoan
            {
                MaTaiKhoan = 4,
                TenDangNhap = "user4",
                MatKhau = "user123",
                HoTen = "User4",
                Email = "user4@gmail.com",
                VaiTro = "User",
                TrangThai = "Active"
            }
        );

        modelBuilder.Entity<LoaiGoiTap>().HasData(
            new LoaiGoiTap
            {
                MaLoaiGoi = 1,
                TenLoaiGoi = "GYM & CARDIO TIÊU CHUẨN",
                MoTa = "Truy cập khu tập tạ tự do, máy cardio hiện đại và phòng tắm nóng lạnh tiêu chuẩn quốc tế",
                TrangThai = "Active",
                SoGoiTrucThuoc = 8,
                NgayCapNhat = new DateTime(2026, 3, 12, 8, 30, 0)
            },
            new LoaiGoiTap
            {
                MaLoaiGoi = 2,
                TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)",
                MoTa = "Giáo án chuyên sâu 1 kèm 1 với Master Coach, đo InBody định kỳ và thực đơn dinh dưỡng riêng",
                TrangThai = "Active",
                SoGoiTrucThuoc = 5,
                NgayCapNhat = new DateTime(2026, 3, 15, 11, 15, 0)
            },
            new LoaiGoiTap
            {
                MaLoaiGoi = 3,
                TenLoaiGoi = "YOGA & GROUP X",
                MoTa = "Tham gia không giới hạn các lớp Zumba, Yoga Flow, Les Mills BodyPump, Pilates studio",
                TrangThai = "Active",
                SoGoiTrucThuoc = 4,
                NgayCapNhat = new DateTime(2026, 3, 18, 14, 20, 0)
            },
            new LoaiGoiTap
            {
                MaLoaiGoi = 4,
                TenLoaiGoi = "BƠI LỘI & SAUNA SPA",
                MoTa = "Bể bơi bốn mùa nước mặn, phòng xông hơi đá muối Himalaya cao cấp",
                TrangThai = "Active",
                SoGoiTrucThuoc = 3,
                NgayCapNhat = new DateTime(2026, 3, 20, 9, 45, 0)
            },
            new LoaiGoiTap
            {
                MaLoaiGoi = 5,
                TenLoaiGoi = "COMBO TOÀN NĂNG ALL-IN-ONE",
                MoTa = "Full quyền lợi tập luyện, hồ bơi, sauna và 10 buổi PT định hướng cho người mới bắt đầu",
                TrangThai = "Active",
                SoGoiTrucThuoc = 2,
                NgayCapNhat = new DateTime(2026, 3, 25, 16, 0, 0)
            },
            new LoaiGoiTap
            {
                MaLoaiGoi = 6,
                TenLoaiGoi = "GÓI TRẢI NGHIỆM HÈ (CŨ)",
                MoTa = "Áp dụng chiến dịch hè (Đã dừng tuyển sinh mới)",
                TrangThai = "Inactive",
                SoGoiTrucThuoc = 0,
                NgayCapNhat = new DateTime(2026, 1, 2, 10, 0, 0)
            }
        );
    }
}
