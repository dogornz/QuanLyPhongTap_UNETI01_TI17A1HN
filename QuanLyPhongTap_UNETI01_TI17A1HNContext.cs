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
            .HasForeignKey(g => g.MaLoaiGoi);

        modelBuilder.Entity<GoiTap>()
        .Property(g => g.DonGia)
        .HasPrecision(18, 2);

        modelBuilder.Entity<PhieuDangKy>()
            .Property(p => p.DonGia)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PhieuDangKy>()
            .Property(p => p.TongTien)
            .HasPrecision(18, 2);

        // 1. Seed TaiKhoan
        modelBuilder.Entity<TaiKhoan>().HasIndex(tdn => tdn.TenDangNhap).IsUnique();

        modelBuilder.Entity<TaiKhoan>().HasData(
               new TaiKhoan { MaTaiKhoan = 1, TenDangNhap = "admin01", MatKhau = "admin123", HoTen = "Admin 01", Email = "admin01@gym.com", VaiTro = "admin", TrangThai = "Lock" },
               new TaiKhoan { MaTaiKhoan = 2, TenDangNhap = "admin02", MatKhau = "admin123", HoTen = "Admin 02", Email = "admin02@gym.com", VaiTro = "admin", TrangThai = "Unlock" },
               new TaiKhoan { MaTaiKhoan = 3, TenDangNhap = "hoivien01", MatKhau = "hoivien123", HoTen = "Nguyễn Văn A", Email = "vana@gym.com", VaiTro = "hoivien", TrangThai = "Lock" },
               new TaiKhoan { MaTaiKhoan = 4, TenDangNhap = "hoivien02", MatKhau = "hoivien123", HoTen = "Trần Thị B", Email = "thib@gym.com", VaiTro = "hoivien", TrangThai = "Unlock" }
           );
        modelBuilder.Entity<LoaiGoiTap>().HasData(
    new LoaiGoiTap { MaLoaiGoi = 1, TenLoaiGoi = "GYM & CARDIO TIÊU CHUẨN", TrangThai = "Active" },
    new LoaiGoiTap { MaLoaiGoi = 2, TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)", TrangThai = "Active" },
    new LoaiGoiTap { MaLoaiGoi = 3, TenLoaiGoi = "YOGA & GROUP X", TrangThai = "Active" },
    new LoaiGoiTap { MaLoaiGoi = 4, TenLoaiGoi = "BƠI LỘI & SAUNA SPA", TrangThai = "Active" },
    new LoaiGoiTap { MaLoaiGoi = 5, TenLoaiGoi = "COMBO TOÀN NĂNG", TrangThai = "Active" },
    new LoaiGoiTap { MaLoaiGoi = 6, TenLoaiGoi = "GÓI TRẢI NGHIỆM HÈ (CŨ)", TrangThai = "Inactive" }
);
        modelBuilder.Entity<GoiTap>().HasData(
            new GoiTap { MaGoiTap = "GT01", DonGia = 150000m, ThoiHanThang = "1", TenGoiTap = "GYM 1 Tháng", TrangThai = "Đang áp dụng", MoTa = "Tập gym 1 tháng không giới hạn khung giờ", SoBuoiToiDa = 30, MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT02", DonGia = 350000m, ThoiHanThang = "3", TenGoiTap = "GYM 3 Tháng", TrangThai = "Đang áp dụng", MoTa = "Tập gym 3 tháng kèm 1 buổi định hướng HLV", SoBuoiToiDa = 90, MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT03", DonGia = 600000m, ThoiHanThang = "6", TenGoiTap = "GYM 6 Tháng", TrangThai = "Đang áp dụng", MoTa = "Gói 6 tháng, miễn phí tủ locker cá nhân và nước uống", SoBuoiToiDa = 180, MaLoaiGoi = 1 }
        );

        // Seed HoiVien (20 hội viên)
        var hoiViens = new List<HoiVien>();
        for (int i = 1; i <= 20; i++)
        {
            hoiViens.Add(new HoiVien
            {
                MaHoiVien = $"HV{i:00}",
                HoTen = $"Hội viên {i}",
                SoDienThoai = $"091234567{i % 10}",
                Email = $"hoivien{i}@email.com",
                DiaChi = i % 2 == 0 ? "Hà Nội" : "TP HCM",
                TrangThai = i % 10 == 0 ? "inactive" : "active",
                NgayThamGia = DateTime.Now.AddDays(-i * 10)
            });
        }
        modelBuilder.Entity<HoiVien>().HasData(hoiViens);

        // Seed PhieuDangKy (30 phiếu)
        var phieus = new List<PhieuDangKy>();
        for (int i = 1; i <= 30; i++)
        {
            var hoiVienId = (i % 20) + 1;
            var goiTapId = (i % 3) + 1; // only seeded 3 goiTap above
            var ngayDangKy = DateTime.Now.AddDays(-i * 7);
            var trangThai = "Chờ xác nhận";

            if (i % 5 == 0) trangThai = "Đã hủy";
            else if (i % 3 == 0) trangThai = "Đang hiệu lực";

            phieus.Add(new PhieuDangKy
            {
                MaPhieu = $"PDK{i:00}",
                MaHoiVien = $"HV{hoiVienId:00}",
                MaGoiTap = $"GT{goiTapId:00}",
                NgayDangKy = ngayDangKy,
                NgayBatDau = ngayDangKy,
                NgayKetThuc = ngayDangKy.AddDays(90),
                DonGia = i < 15 ? 350000m : 1000000m,
                TongTien = i < 15 ? 350000m : 1000000m,
                TrangThai = trangThai
            });
        }
        modelBuilder.Entity<PhieuDangKy>().HasData(phieus);
    }
}
