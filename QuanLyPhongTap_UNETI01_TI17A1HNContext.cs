using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options) : DbContext(options)
{
    public DbSet<QuanLyPhongTap_UNETI01_TI17A1HN.Models.PhieuDangKy> PhieuDangKy { get; set; } = default!;
    public DbSet<TaiKhoan> TaiKhoan { get; set; } = default!;
    public DbSet<HoiVien> HoiVien { get; set; } = default!;
    public DbSet<LoaiGoiTap> LoaiGoiTap { get; set; } = default!;
    public DbSet<GoiTap> GoiTap { get; set; } = default!;
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // HoiVien (1) -- (n) PhieuDangKy
        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.HoiVien)
            .WithMany(h => h.PhieuDangKy)
            .HasForeignKey(p => p.MaHoiVien);

        // GoiTap (1) -- (n) PhieuDangKy
        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.GoiTap)
            .WithMany(g => g.PhieuDangKy)
            .HasForeignKey(p => p.MaGoiTap);

        // LoaiGoiTap (1) -- (n) GoiTap
        modelBuilder.Entity<GoiTap>()
            .HasOne(g => g.LoaiGoiTap)
            .WithMany(l => l.GoiTap)
            .HasForeignKey(g => g.LoaiGoiTapId);
        modelBuilder.Entity<TaiKhoan>().HasData(
               new TaiKhoan { MaTaiKhoan = 1, TenDangNhap = "admin01", MatKhau = "admin123", HoTen = "Admin 01", Email = "admin01@gym.com", VaiTro = "admin", TrangThai = "Lock" },
               new TaiKhoan { MaTaiKhoan = 2, TenDangNhap = "admin02", MatKhau = "admin123", HoTen = "Admin 02", Email = "admin02@gym.com", VaiTro = "admin", TrangThai = "Unlock" },
               new TaiKhoan { MaTaiKhoan = 3, TenDangNhap = "hoivien01", MatKhau = "hoivien123", HoTen = "Nguyễn Văn A", Email = "vana@gym.com", VaiTro = "hoivien", TrangThai = "Lock" },
               new TaiKhoan { MaTaiKhoan = 4, TenDangNhap = "hoivien02", MatKhau = "hoivien123", HoTen = "Trần Thị B", Email = "thib@gym.com", VaiTro = "hoivien", TrangThai = "Unlock" }
           );
        modelBuilder.Entity<LoaiGoiTap>().HasData(
    new LoaiGoiTap { LoaiGoiTapId = 1, TenLoaiGoi = "GYM & CARDIO TIÊU CHUẨN", TrangThai = "Active" },
    new LoaiGoiTap { LoaiGoiTapId = 2, TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)", TrangThai = "Active" },
    new LoaiGoiTap { LoaiGoiTapId = 3, TenLoaiGoi = "YOGA & GROUP X", TrangThai = "Active" },
    new LoaiGoiTap { LoaiGoiTapId = 4, TenLoaiGoi = "BƠI LỘI & SAUNA SPA", TrangThai = "Active" },
    new LoaiGoiTap { LoaiGoiTapId = 5, TenLoaiGoi = "COMBO TOÀN NĂNG", TrangThai = "Active" },
    new LoaiGoiTap { LoaiGoiTapId = 6, TenLoaiGoi = "GÓI TRẢI NGHIỆM HÈ (CŨ)", TrangThai = "Inactive" }
);
        modelBuilder.Entity<GoiTap>().HasData(
                new GoiTap { IdGoiTap = 1, TenGoiTap = "GYM 1 Tháng", GiaGoiTap = 150000, ThoiHan = 30, ConApDung = true, LoaiGoiTapId = 1 },
                new GoiTap { IdGoiTap = 2, TenGoiTap = "GYM 3 Tháng", GiaGoiTap = 350000, ThoiHan = 90, ConApDung = true, LoaiGoiTapId = 1 },
                new GoiTap { IdGoiTap = 3, TenGoiTap = "GYM 6 Tháng", GiaGoiTap = 600000, ThoiHan = 180, ConApDung = true, LoaiGoiTapId = 1 },
                new GoiTap { IdGoiTap = 4, TenGoiTap = "GYM 12 Tháng", GiaGoiTap = 1000000, ThoiHan = 365, ConApDung = true, LoaiGoiTapId = 1 },
                new GoiTap { IdGoiTap = 5, TenGoiTap = "PT 1-1 10 Buổi", GiaGoiTap = 2000000, ThoiHan = 60, ConApDung = true, LoaiGoiTapId = 2 },
                new GoiTap { IdGoiTap = 6, TenGoiTap = "PT 1-1 20 Buổi", GiaGoiTap = 3500000, ThoiHan = 90, ConApDung = true, LoaiGoiTapId = 2 },
                new GoiTap { IdGoiTap = 7, TenGoiTap = "Yoga Tháng", GiaGoiTap = 250000, ThoiHan = 30, ConApDung = true, LoaiGoiTapId = 3 },
                new GoiTap { IdGoiTap = 8, TenGoiTap = "Yoga 3 Tháng", GiaGoiTap = 600000, ThoiHan = 90, ConApDung = true, LoaiGoiTapId = 3 },
                new GoiTap { IdGoiTap = 9, TenGoiTap = "Bơi 1 Tháng", GiaGoiTap = 200000, ThoiHan = 30, ConApDung = true, LoaiGoiTapId = 4 },
                new GoiTap { IdGoiTap = 10, TenGoiTap = "Bơi 6 Tháng", GiaGoiTap = 900000, ThoiHan = 180, ConApDung = true, LoaiGoiTapId = 4 },
                new GoiTap { IdGoiTap = 11, TenGoiTap = "Combo 3 Tháng", GiaGoiTap = 1200000, ThoiHan = 90, ConApDung = true, LoaiGoiTapId = 5 },
                new GoiTap { IdGoiTap = 12, TenGoiTap = "Combo 6 Tháng", GiaGoiTap = 2000000, ThoiHan = 180, ConApDung = true, LoaiGoiTapId = 5 },
                new GoiTap { IdGoiTap = 13, TenGoiTap = "Combo 12 Tháng", GiaGoiTap = 3500000, ThoiHan = 365, ConApDung = true, LoaiGoiTapId = 5 },
                new GoiTap { IdGoiTap = 14, TenGoiTap = "Combo Hè Giới Hạn", GiaGoiTap = 800000, ThoiHan = 90, ConApDung = false, LoaiGoiTapId = 6 },
                new GoiTap { IdGoiTap = 15, TenGoiTap = "Bơi + Yoga", GiaGoiTap = 700000, ThoiHan = 60, ConApDung = true, LoaiGoiTapId = 3 }
            );
        // Seed HoiVien (20 hội viên)
        var hoiViens = new List<HoiVien>();
        for (int i = 1; i <= 20; i++)
        {
            hoiViens.Add(new HoiVien
            {
                MaHoiVien = i,
                HoTen = $"Hội viên {i}",
                SoDienThoai = $"091234567{i % 10}",
                Email = $"hoivien{i}@email.com",
                DiaChi = i % 2 == 0 ? "Hà Nội" : "TP HCM",
                
                TrangThai = i % 10 == 0 ? "inactive" : "active",
                NgayThamGia = DateTime.Now.AddDays(-i * 10),
                
            });
        }
        modelBuilder.Entity<HoiVien>().HasData(hoiViens);

        // Seed PhieuDangKy (30 phiếu)
        var phieus = new List<PhieuDangKy>();
        for (int i = 1; i <= 30; i++)
        {
            var hoiVienId = (i % 20) + 1;
            var goiTapId = (i % 15) + 1;
            var ngayDangKy = DateTime.Now.AddDays(-i * 7);
            var trangThai = "Chờ xác nhận";

            if (i % 5 == 0) trangThai = "Đã hủy";
            else if (i % 3 == 0) trangThai = "Đang hiệu lực";

            phieus.Add(new PhieuDangKy
            {
                MaPhieu = i,
                MaHoiVien = hoiVienId,
                MaGoiTap = goiTapId,
                NgayDangKy = ngayDangKy,
                NgayBatDau = ngayDangKy,
                NgayKetThuc = ngayDangKy.AddDays(90),
                DonGia = i < 15 ? 350000 : 1000000,
                TrangThai = trangThai
            });
        }
        modelBuilder.Entity<PhieuDangKy>().HasData(phieus);
    }
}
