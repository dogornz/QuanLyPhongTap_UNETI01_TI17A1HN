using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options) : DbContext(options)
{
    public DbSet<PhieuDangKy> PhieuDangKy { get; set; } = default!;
    public DbSet<TaiKhoan> TaiKhoan { get; set; } = default!;
    public DbSet<HoiVien> HoiVien { get; set; } = default!;
    public DbSet<LoaiGoiTap> LoaiGoiTap { get; set; } = default!;
    public DbSet<GoiTap> GoiTap { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=QLPG;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Foreign keys configuration
        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.HoiVien)
            .WithMany()
            .HasForeignKey(p => p.MaHoiVien)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.GoiTap)
            .WithMany()
            .HasForeignKey(p => p.MaGoiTap)
            .OnDelete(DeleteBehavior.Restrict);

        // 1. Seed TaiKhoan
        modelBuilder.Entity<TaiKhoan>().HasIndex(tdn => tdn.TenDangNhap).IsUnique();
        modelBuilder.Entity<TaiKhoan>().HasData(
            new TaiKhoan { MaTaiKhoan = 1, TenDangNhap = "admin01", MatKhau = "admin123", HoTen = "Admin 01", Email = "admin01@gym.com", VaiTro = "admin", TrangThai = "Lock" },
            new TaiKhoan { MaTaiKhoan = 2, TenDangNhap = "admin02", MatKhau = "admin123", HoTen = "Admin 02", Email = "admin02@gym.com", VaiTro = "admin", TrangThai = "Unlock" },
            new TaiKhoan { MaTaiKhoan = 3, TenDangNhap = "hoivien01", MatKhau = "hoivien123", HoTen = "Nguyễn Văn A", Email = "vana@gym.com", VaiTro = "hoivien", TrangThai = "Lock" },
            new TaiKhoan { MaTaiKhoan = 4, TenDangNhap = "hoivien02", MatKhau = "hoivien123", HoTen = "Trần Thị B", Email = "thib@gym.com", VaiTro = "hoivien", TrangThai = "Unlock" }
        );

        // 2. Seed LoaiGoiTap (6 categories)
        modelBuilder.Entity<LoaiGoiTap>().HasData(
            new LoaiGoiTap { MaLoaiGoi = 1, TenLoaiGoi = "GYM & CARDIO TIÊU CHUẨN", MoTa = "Khu tập tạ và máy cardio", TrangThai = "Active", SoGoiTrucThuoc = 8, NgayCapNhat = new DateTime(2026, 3, 12) },
            new LoaiGoiTap { MaLoaiGoi = 2, TenLoaiGoi = "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)", MoTa = "1 kèm 1 với HLV chuyên nghiệp", TrangThai = "Active", SoGoiTrucThuoc = 5, NgayCapNhat = new DateTime(2026, 3, 15) },
            new LoaiGoiTap { MaLoaiGoi = 3, TenLoaiGoi = "YOGA & GROUP X", MoTa = "Lớp Yoga, Zumba, Pilates", TrangThai = "Active", SoGoiTrucThuoc = 4, NgayCapNhat = new DateTime(2026, 3, 18) },
            new LoaiGoiTap { MaLoaiGoi = 4, TenLoaiGoi = "BƠI LỘI & SAUNA SPA", MoTa = "Hồ bơi 4 mùa và xông hơi đá muối", TrangThai = "Active", SoGoiTrucThuoc = 3, NgayCapNhat = new DateTime(2026, 3, 20) },
            new LoaiGoiTap { MaLoaiGoi = 5, TenLoaiGoi = "COMBO TOÀN NĂNG", MoTa = "Full dịch vụ Gym, Yoga, Bơi", TrangThai = "Active", SoGoiTrucThuoc = 2, NgayCapNhat = new DateTime(2026, 3, 25) },
            new LoaiGoiTap { MaLoaiGoi = 6, TenLoaiGoi = "GÓI TRẢI NGHIỆM HÈ (CŨ)", MoTa = "Chiến dịch hè cũ", TrangThai = "Inactive", SoGoiTrucThuoc = 0, NgayCapNhat = new DateTime(2026, 1, 2) }
        );

        // 3. Seed GoiTap (15 packages)
        modelBuilder.Entity<GoiTap>().HasData(
            new GoiTap { MaGoiTap = "GT01", TenGoiTap = "GYM 1 Tháng", ThoiHanThang = "1", DonGia = 150000m, SoBuoiToiDa = 30, MoTa = "Tập gym 1 tháng không giới hạn khung giờ", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT02", TenGoiTap = "GYM 3 Tháng", ThoiHanThang = "3", DonGia = 350000m, SoBuoiToiDa = 90, MoTa = "Tập gym 3 tháng kèm 1 buổi định hướng HLV", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT03", TenGoiTap = "GYM 6 Tháng", ThoiHanThang = "6", DonGia = 600000m, SoBuoiToiDa = 180, MoTa = "Gói 6 tháng, miễn phí tủ locker cá nhân và nước uống", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT04", TenGoiTap = "GYM 12 Tháng", ThoiHanThang = "12", DonGia = 1000000m, SoBuoiToiDa = 365, MoTa = "Tập luyện cả năm, kèm dịch vụ phòng xông hơi Sauna", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT05", TenGoiTap = "PT 1-1 10 Buổi", ThoiHanThang = "2", DonGia = 2000000m, SoBuoiToiDa = 10, MoTa = "10 buổi tập kèm riêng HLV Master, kèm thực đơn", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT06", TenGoiTap = "PT 1-1 20 Buổi", ThoiHanThang = "3", DonGia = 3500000m, SoBuoiToiDa = 20, MoTa = "20 buổi tập kèm riêng HLV Master, cam kết tiến độ", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT07", TenGoiTap = "Yoga Tháng", ThoiHanThang = "1", DonGia = 250000m, SoBuoiToiDa = 30, MoTa = "Tham gia không giới hạn các lớp Yoga cơ bản và nâng cao", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT08", TenGoiTap = "Yoga 3 Tháng", ThoiHanThang = "3", DonGia = 600000m, SoBuoiToiDa = 90, MoTa = "Khóa Yoga trị liệu, thiền định và dẻo dai toàn diện", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT09", TenGoiTap = "Bơi 1 Tháng", ThoiHanThang = "1", DonGia = 200000m, SoBuoiToiDa = 30, MoTa = "Bơi tự do tại hồ bơi nước mặn 4 mùa tiêu chuẩn Olympic", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT10", TenGoiTap = "Bơi 6 Tháng", ThoiHanThang = "6", DonGia = 900000m, SoBuoiToiDa = 180, MoTa = "Thẻ bơi 6 tháng kèm tiện ích xông hơi đá muối thảo dược", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT11", TenGoiTap = "Combo 3 Tháng", ThoiHanThang = "3", DonGia = 1200000m, SoBuoiToiDa = 90, MoTa = "Gym + Yoga + Bơi lội trọn gói trong 3 tháng", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT12", TenGoiTap = "Combo 6 Tháng", ThoiHanThang = "6", DonGia = 2000000m, SoBuoiToiDa = 180, MoTa = "Gym + Yoga + Bơi + Sauna không giới hạn 6 tháng", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT13", TenGoiTap = "Combo 12 Tháng", ThoiHanThang = "12", DonGia = 3500000m, SoBuoiToiDa = 365, MoTa = "Đặc quyền cao cấp nhất, toàn bộ dịch vụ và tiện ích", TrangThai = "Đang áp dụng" },
            new GoiTap { MaGoiTap = "GT14", TenGoiTap = "Combo Hè Giới Hạn", ThoiHanThang = "3", DonGia = 800000m, SoBuoiToiDa = 90, MoTa = "Gói ưu đãi chiến dịch hè (đã ngừng áp dụng)", TrangThai = "Ngừng áp dụng" },
            new GoiTap { MaGoiTap = "GT15", TenGoiTap = "Bơi + Yoga", ThoiHanThang = "2", DonGia = 700000m, SoBuoiToiDa = 60, MoTa = "Kết hợp Yoga thanh lọc cơ thể và bơi lội rèn luyện sức bền", TrangThai = "Đang áp dụng" }
        );

        // 4. Seed HoiVien (20 members)
        var hoiViens = new List<HoiVien>();
        for (int i = 1; i <= 20; i++)
        {
            hoiViens.Add(new HoiVien
            {
                MaHoiVien = $"HV{i:D2}",
                MaTaiKhoan = (i % 2 == 0 ? 4 : 3).ToString(),
                HoTen = $"Hội viên {i}",
                NgaySinh = new DateTime(1990 + (i % 10), (i % 12) + 1, (i % 28) + 1),
                GioiTinh = i % 2 == 0 ? "Nam" : "Nữ",
                SoDienThoai = $"091234567{i % 10}",
                Email = $"hoivien{i}@email.com",
                NgayThamGia = new DateTime(2026, 1, 1).AddDays(-i * 10),
                TrangThai = i % 10 == 0 ? "Ngừng hoạt động" : "Đang hoạt động"
            });
        }
        modelBuilder.Entity<HoiVien>().HasData(hoiViens);

        // 5. Seed PhieuDangKy (30 tickets)
        var phieus = new List<PhieuDangKy>();
        for (int i = 1; i <= 30; i++)
        {
            var hoiVienId = (i % 20) + 1;
            var goiTapId = (i % 15) + 1;
            var ngayDangKy = new DateTime(2026, 1, 1).AddDays(i * 7);
            var trangThai = "Chờ xác nhận";

            if (i % 5 == 0) trangThai = "Đã hủy";
            else if (i % 3 == 0) trangThai = "Đang hiệu lực";

            var donGia = i < 15 ? 350000m : 1000000m;

            phieus.Add(new PhieuDangKy
            {
                MaPhieu = $"PDK{i:D2}",
                MaHoiVien = $"HV{hoiVienId:D2}",
                MaGoiTap = $"GT{goiTapId:D2}",
                NgayDangKy = ngayDangKy,
                NgayBatDau = ngayDangKy,
                NgayKetThuc = ngayDangKy.AddDays(90),
                DonGia = donGia,
                TongTien = donGia,
                TrangThai = trangThai
            });
        }
        modelBuilder.Entity<PhieuDangKy>().HasData(phieus);
    }
}
