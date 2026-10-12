using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options) : DbContext(options)
{

    public DbSet<QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan> TaiKhoan { get; set; } = default!;
    public DbSet<QuanLyPhongTap_UNETI01_TI17A1HN.Models.LoaiGoiTap> LoaiGoiTap { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QLPG;Integrated Security=True");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan>().HasIndex(tdn => tdn.TenDangNhap).IsUnique();
        modelBuilder.Entity<QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan>().HasData(
            new QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan
            {
                MaTaiKhoan = 1,
                TenDangNhap = "admin01",
                MatKhau = "admin123",
                HoTen = "Admin 01",
                Email = "admingym01@gmail.com",
                VaiTro = "admin",
                TrangThai = "Lock"
            },
            new QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan
            {
                MaTaiKhoan = 2,
                TenDangNhap = "admin02",
                MatKhau = "admin123",
                HoTen = "Admin 02",
                Email = "admingym02@gmail.com",
                VaiTro = "admin",
                TrangThai = "Unlock"
            },
            new QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan
            {
                MaTaiKhoan = 3,
                TenDangNhap = "hoivien01",
                MatKhau = "hoivien123",
                HoTen = "Nguyễn Viết Nguy",
                Email = "nguyenvietnguy@gmail.com",
                VaiTro = "hoivien",
                TrangThai = "Lock"
            },
            new QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan
            {
                MaTaiKhoan = 4,
                TenDangNhap = "hoivien02",
                MatKhau = "hoivien123",
                HoTen = "Nguyễn Văn Lập",
                Email = "nguyenvanlap@gmail.com",
                VaiTro = "hoivien",
                TrangThai = "Unlock"
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
