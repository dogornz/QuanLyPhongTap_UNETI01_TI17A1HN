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

        modelBuilder.Entity<GoiTap>()
            .HasOne(g => g.LoaiGoiTap)
            .WithMany()
            .HasForeignKey(g => g.MaLoaiGoi)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GoiTap>(entity =>
        {
            entity.Property(e => e.TenGoiTap)
                  .HasMaxLength(100)
                  .IsUnicode(true);

            entity.Property(e => e.MoTa)
                  .IsUnicode(true);

            entity.Property(e => e.TrangThai)
                  .HasMaxLength(50)
                  .IsUnicode(true);
        });

        modelBuilder.Entity<GoiTap>()
            .Property(g => g.DonGia)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PhieuDangKy>()
            .Property(p => p.DonGia)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PhieuDangKy>()
            .Property(p => p.TongTien)
            .HasPrecision(18, 2);

        // Danh sách thông tin 20 hội viên thực tế
        var memberData = new (string HoTen, string GioiTinh, DateTime NgaySinh, string Phone, string Email, string TrangThai)[]
        {
            ("Nguyễn Văn An", "Nam", new DateTime(1998, 5, 15), "0912345001", "an.nguyen@gym.com", "Unlock"),
            ("Trần Thị Bích", "Nữ", new DateTime(1999, 8, 20), "0912345002", "bich.tran@gym.com", "Unlock"),
            ("Lê Hoàng Cường", "Nam", new DateTime(1995, 11, 10), "0912345003", "cuong.le@gym.com", "Unlock"),
            ("Phạm Minh Đức", "Nam", new DateTime(2001, 3, 25), "0912345004", "duc.pham@gym.com", "Unlock"),
            ("Vũ Phương Dung", "Nữ", new DateTime(2000, 9, 12), "0912345005", "dung.vu@gym.com", "Unlock"),
            ("Đặng Quốc Hưng", "Nam", new DateTime(1997, 7, 8), "0912345006", "hung.dang@gym.com", "Unlock"),
            ("Bùi Thu Hà", "Nữ", new DateTime(2002, 1, 30), "0912345007", "ha.bui@gym.com", "Unlock"),
            ("Đỗ Tuấn Kiệt", "Nam", new DateTime(1996, 12, 4), "0912345008", "kiet.do@gym.com", "Unlock"),
            ("Hồ Mai Linh", "Nữ", new DateTime(2003, 4, 18), "0912345009", "linh.ho@gym.com", "Unlock"),
            ("Ngô Thành Nam", "Nam", new DateTime(1994, 6, 22), "0912345010", "nam.ngo@gym.com", "Lock"),
            ("Dương Diệu Oanh", "Nữ", new DateTime(1999, 10, 14), "0912345011", "oanh.duong@gym.com", "Unlock"),
            ("Lý Gia Phúc", "Nam", new DateTime(2001, 8, 5), "0912345012", "phuc.ly@gym.com", "Unlock"),
            ("Phan Thanh Quang", "Nam", new DateTime(1998, 2, 28), "0912345013", "quang.phan@gym.com", "Unlock"),
            ("Trịnh Bảo Sơn", "Nam", new DateTime(1997, 11, 19), "0912345014", "son.trinh@gym.com", "Unlock"),
            ("Hoàng Thảo Tiên", "Nữ", new DateTime(2002, 7, 27), "0912345015", "tien.hoang@gym.com", "Unlock"),
            ("Mai Quốc Uy", "Nam", new DateTime(1993, 3, 16), "0912345016", "uy.mai@gym.com", "Unlock"),
            ("Đoàn Tố Uyên", "Nữ", new DateTime(2000, 12, 9), "0912345017", "uyen.doan@gym.com", "Unlock"),
            ("Cao Đăng Việt", "Nam", new DateTime(1996, 9, 3), "0912345018", "viet.cao@gym.com", "Unlock"),
            ("Nguyễn Hải Yến", "Nữ", new DateTime(2004, 5, 21), "0912345019", "yen.nguyen@gym.com", "Unlock"),
            ("Lê Văn Tùng", "Nam", new DateTime(1995, 4, 11), "0912345020", "tung.le@gym.com", "Lock")
        };

        // 1. Seed TaiKhoan (2 Admin + 20 Hội viên liên kết 1-1)
        modelBuilder.Entity<TaiKhoan>().HasIndex(tdn => tdn.TenDangNhap).IsUnique();
        var taiKhoans = new List<TaiKhoan>
        {
            new TaiKhoan { MaTaiKhoan = 1, TenDangNhap = "admin01", MatKhau = "admin123", HoTen = "Admin 01", Email = "admin01@gym.com", VaiTro = "admin", TrangThai = "Unlock" },
            new TaiKhoan { MaTaiKhoan = 2, TenDangNhap = "admin02", MatKhau = "admin123", HoTen = "Admin 02", Email = "admin02@gym.com", VaiTro = "admin", TrangThai = "Unlock" }
        };

        for (int i = 0; i < memberData.Length; i++)
        {
            int maTk = i + 3;
            var m = memberData[i];
            taiKhoans.Add(new TaiKhoan
            {
                MaTaiKhoan = maTk,
                TenDangNhap = $"hoivien{i + 1:D2}",
                MatKhau = "hoivien123",
                HoTen = m.HoTen,
                Email = m.Email,
                VaiTro = "hoivien",
                TrangThai = m.TrangThai
            });
        }
        modelBuilder.Entity<TaiKhoan>().HasData(taiKhoans);

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
            new GoiTap { MaGoiTap = "GT01", TenGoiTap = "GYM 1 Tháng", ThoiHanThang = "1", DonGia = 150000m, SoBuoiToiDa = 30, MoTa = "Tập gym 1 tháng không giới hạn khung giờ", TrangThai = "Đang áp dụng", MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT02", TenGoiTap = "GYM 3 Tháng", ThoiHanThang = "3", DonGia = 350000m, SoBuoiToiDa = 90, MoTa = "Tập gym 3 tháng kèm 1 buổi định hướng HLV", TrangThai = "Đang áp dụng", MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT03", TenGoiTap = "GYM 6 Tháng", ThoiHanThang = "6", DonGia = 600000m, SoBuoiToiDa = 180, MoTa = "Gói 6 tháng, miễn phí tủ locker cá nhân và nước uống", TrangThai = "Đang áp dụng", MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT04", TenGoiTap = "GYM 12 Tháng", ThoiHanThang = "12", DonGia = 1000000m, SoBuoiToiDa = 365, MoTa = "Tập luyện cả năm, kèm dịch vụ phòng xông hơi Sauna", TrangThai = "Đang áp dụng", MaLoaiGoi = 1 },
            new GoiTap { MaGoiTap = "GT05", TenGoiTap = "PT 1-1 10 Buổi", ThoiHanThang = "2", DonGia = 2000000m, SoBuoiToiDa = 10, MoTa = "10 buổi tập kèm riêng HLV Master, kèm thực đơn", TrangThai = "Đang áp dụng", MaLoaiGoi = 2 },
            new GoiTap { MaGoiTap = "GT06", TenGoiTap = "PT 1-1 20 Buổi", ThoiHanThang = "3", DonGia = 3500000m, SoBuoiToiDa = 20, MoTa = "20 buổi tập kèm riêng HLV Master, cam kết tiến độ", TrangThai = "Đang áp dụng", MaLoaiGoi = 2 },
            new GoiTap { MaGoiTap = "GT07", TenGoiTap = "Yoga Tháng", ThoiHanThang = "1", DonGia = 250000m, SoBuoiToiDa = 30, MoTa = "Tham gia không giới hạn các lớp Yoga cơ bản và nâng cao", TrangThai = "Đang áp dụng", MaLoaiGoi = 3 },
            new GoiTap { MaGoiTap = "GT08", TenGoiTap = "Yoga 3 Tháng", ThoiHanThang = "3", DonGia = 600000m, SoBuoiToiDa = 90, MoTa = "Khóa Yoga trị liệu, thiền định và dẻo dai toàn diện", TrangThai = "Đang áp dụng", MaLoaiGoi = 3 },
            new GoiTap { MaGoiTap = "GT09", TenGoiTap = "Bơi 1 Tháng", ThoiHanThang = "1", DonGia = 200000m, SoBuoiToiDa = 30, MoTa = "Bơi tự do tại hồ bơi nước mặn 4 mùa tiêu chuẩn Olympic", TrangThai = "Đang áp dụng", MaLoaiGoi = 4 },
            new GoiTap { MaGoiTap = "GT10", TenGoiTap = "Bơi 6 Tháng", ThoiHanThang = "6", DonGia = 900000m, SoBuoiToiDa = 180, MoTa = "Thẻ bơi 6 tháng kèm tiện ích xông hơi đá muối thảo dược", TrangThai = "Đang áp dụng", MaLoaiGoi = 4 },
            new GoiTap { MaGoiTap = "GT11", TenGoiTap = "Combo 3 Tháng", ThoiHanThang = "3", DonGia = 1200000m, SoBuoiToiDa = 90, MoTa = "Gym + Yoga + Bơi lội trọn gói trong 3 tháng", TrangThai = "Đang áp dụng", MaLoaiGoi = 5 },
            new GoiTap { MaGoiTap = "GT12", TenGoiTap = "Combo 6 Tháng", ThoiHanThang = "6", DonGia = 2000000m, SoBuoiToiDa = 180, MoTa = "Gym + Yoga + Bơi + Sauna không giới hạn 6 tháng", TrangThai = "Đang áp dụng", MaLoaiGoi = 5 },
            new GoiTap { MaGoiTap = "GT13", TenGoiTap = "Combo 12 Tháng", ThoiHanThang = "12", DonGia = 3500000m, SoBuoiToiDa = 365, MoTa = "Đặc quyền cao cấp nhất, toàn bộ dịch vụ và tiện ích", TrangThai = "Đang áp dụng", MaLoaiGoi = 5 },
            new GoiTap { MaGoiTap = "GT14", TenGoiTap = "Combo Hè Giới Hạn", ThoiHanThang = "3", DonGia = 800000m, SoBuoiToiDa = 90, MoTa = "Gói ưu đãi chiến dịch hè (đã ngừng áp dụng)", TrangThai = "Ngừng áp dụng", MaLoaiGoi = 6 },
            new GoiTap { MaGoiTap = "GT15", TenGoiTap = "Bơi + Yoga", ThoiHanThang = "2", DonGia = 700000m, SoBuoiToiDa = 60, MoTa = "Kết hợp Yoga thanh lọc cơ thể và bơi lội rèn luyện sức bền", TrangThai = "Đang áp dụng", MaLoaiGoi = 3 }
        );

        // 4. Seed HoiVien (20 members liên kết 1-1 với TaiKhoan)
        var hoiViens = new List<HoiVien>();
        for (int i = 0; i < memberData.Length; i++)
        {
            int maTk = i + 3;
            var m = memberData[i];
            hoiViens.Add(new HoiVien
            {
                MaHoiVien = $"HV{i + 1:D2}",
                MaTaiKhoan = maTk.ToString(),
                HoTen = m.HoTen,
                NgaySinh = m.NgaySinh,
                GioiTinh = m.GioiTinh,
                SoDienThoai = m.Phone,
                Email = m.Email,
                NgayThamGia = new DateTime(2026, 1, 10).AddDays(i * 5),
                TrangThai = m.TrangThai == "Unlock" ? "Đang hoạt động" : "Ngừng hoạt động"
            });
        }
        modelBuilder.Entity<HoiVien>().HasData(hoiViens);

        // 5. Seed PhieuDangKy (28 tickets liên kết chính xác với HoiVien và GoiTap)
        var phieus = new List<PhieuDangKy>
        {
            new PhieuDangKy { MaPhieu = "PDK01", MaHoiVien = "HV01", MaGoiTap = "GT02", NgayDangKy = new DateTime(2026, 9, 15), NgayBatDau = new DateTime(2026, 9, 15), NgayKetThuc = new DateTime(2026, 12, 15), DonGia = 350000m, TongTien = 350000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK02", MaHoiVien = "HV01", MaGoiTap = "GT01", NgayDangKy = new DateTime(2026, 5, 10), NgayBatDau = new DateTime(2026, 5, 10), NgayKetThuc = new DateTime(2026, 6, 10), DonGia = 150000m, TongTien = 150000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK03", MaHoiVien = "HV02", MaGoiTap = "GT07", NgayDangKy = new DateTime(2026, 10, 5), NgayBatDau = new DateTime(2026, 10, 5), NgayKetThuc = new DateTime(2026, 11, 5), DonGia = 250000m, TongTien = 250000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK04", MaHoiVien = "HV02", MaGoiTap = "GT08", NgayDangKy = new DateTime(2026, 4, 1), NgayBatDau = new DateTime(2026, 4, 1), NgayKetThuc = new DateTime(2026, 7, 1), DonGia = 600000m, TongTien = 600000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK05", MaHoiVien = "HV03", MaGoiTap = "GT05", NgayDangKy = new DateTime(2026, 9, 20), NgayBatDau = new DateTime(2026, 9, 20), NgayKetThuc = new DateTime(2026, 11, 20), DonGia = 2000000m, TongTien = 2000000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK06", MaHoiVien = "HV04", MaGoiTap = "GT03", NgayDangKy = new DateTime(2026, 7, 1), NgayBatDau = new DateTime(2026, 7, 1), NgayKetThuc = new DateTime(2027, 1, 1), DonGia = 600000m, TongTien = 600000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK07", MaHoiVien = "HV05", MaGoiTap = "GT11", NgayDangKy = new DateTime(2026, 8, 15), NgayBatDau = new DateTime(2026, 8, 15), NgayKetThuc = new DateTime(2026, 11, 15), DonGia = 1200000m, TongTien = 1200000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK08", MaHoiVien = "HV06", MaGoiTap = "GT04", NgayDangKy = new DateTime(2026, 1, 15), NgayBatDau = new DateTime(2026, 1, 15), NgayKetThuc = new DateTime(2027, 1, 15), DonGia = 1000000m, TongTien = 1000000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK09", MaHoiVien = "HV07", MaGoiTap = "GT09", NgayDangKy = new DateTime(2026, 10, 10), NgayBatDau = new DateTime(2026, 10, 10), NgayKetThuc = new DateTime(2026, 11, 10), DonGia = 200000m, TongTien = 200000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK10", MaHoiVien = "HV08", MaGoiTap = "GT06", NgayDangKy = new DateTime(2026, 8, 1), NgayBatDau = new DateTime(2026, 8, 1), NgayKetThuc = new DateTime(2026, 11, 1), DonGia = 3500000m, TongTien = 3500000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK11", MaHoiVien = "HV09", MaGoiTap = "GT15", NgayDangKy = new DateTime(2026, 9, 1), NgayBatDau = new DateTime(2026, 9, 1), NgayKetThuc = new DateTime(2026, 11, 1), DonGia = 700000m, TongTien = 700000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK12", MaHoiVien = "HV10", MaGoiTap = "GT01", NgayDangKy = new DateTime(2026, 2, 1), NgayBatDau = new DateTime(2026, 2, 1), NgayKetThuc = new DateTime(2026, 3, 1), DonGia = 150000m, TongTien = 150000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK13", MaHoiVien = "HV11", MaGoiTap = "GT10", NgayDangKy = new DateTime(2026, 6, 15), NgayBatDau = new DateTime(2026, 6, 15), NgayKetThuc = new DateTime(2026, 12, 15), DonGia = 900000m, TongTien = 900000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK14", MaHoiVien = "HV12", MaGoiTap = "GT02", NgayDangKy = new DateTime(2026, 8, 20), NgayBatDau = new DateTime(2026, 8, 20), NgayKetThuc = new DateTime(2026, 11, 20), DonGia = 350000m, TongTien = 350000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK15", MaHoiVien = "HV13", MaGoiTap = "GT12", NgayDangKy = new DateTime(2026, 7, 10), NgayBatDau = new DateTime(2026, 7, 10), NgayKetThuc = new DateTime(2027, 1, 10), DonGia = 2000000m, TongTien = 2000000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK16", MaHoiVien = "HV14", MaGoiTap = "GT05", NgayDangKy = new DateTime(2026, 9, 10), NgayBatDau = new DateTime(2026, 9, 10), NgayKetThuc = new DateTime(2026, 11, 10), DonGia = 2000000m, TongTien = 2000000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK17", MaHoiVien = "HV15", MaGoiTap = "GT07", NgayDangKy = new DateTime(2026, 10, 12), NgayBatDau = new DateTime(2026, 10, 12), NgayKetThuc = new DateTime(2026, 11, 12), DonGia = 250000m, TongTien = 250000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK18", MaHoiVien = "HV16", MaGoiTap = "GT03", NgayDangKy = new DateTime(2026, 3, 1), NgayBatDau = new DateTime(2026, 3, 1), NgayKetThuc = new DateTime(2026, 9, 1), DonGia = 600000m, TongTien = 600000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK19", MaHoiVien = "HV17", MaGoiTap = "GT09", NgayDangKy = new DateTime(2026, 10, 15), NgayBatDau = new DateTime(2026, 10, 15), NgayKetThuc = new DateTime(2026, 11, 15), DonGia = 200000m, TongTien = 200000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK20", MaHoiVien = "HV18", MaGoiTap = "GT13", NgayDangKy = new DateTime(2026, 5, 1), NgayBatDau = new DateTime(2026, 5, 1), NgayKetThuc = new DateTime(2027, 5, 1), DonGia = 3500000m, TongTien = 3500000m, TrangThai = "Đang hiệu lực" },
            new PhieuDangKy { MaPhieu = "PDK21", MaHoiVien = "HV19", MaGoiTap = "GT08", NgayDangKy = new DateTime(2026, 10, 18), NgayBatDau = new DateTime(2026, 10, 18), NgayKetThuc = new DateTime(2027, 1, 18), DonGia = 600000m, TongTien = 600000m, TrangThai = "Chờ xác nhận" },
            new PhieuDangKy { MaPhieu = "PDK22", MaHoiVien = "HV20", MaGoiTap = "GT01", NgayDangKy = new DateTime(2026, 1, 10), NgayBatDau = new DateTime(2026, 1, 10), NgayKetThuc = new DateTime(2026, 2, 10), DonGia = 150000m, TongTien = 150000m, TrangThai = "Đã hủy" },
            new PhieuDangKy { MaPhieu = "PDK23", MaHoiVien = "HV03", MaGoiTap = "GT02", NgayDangKy = new DateTime(2026, 10, 20), NgayBatDau = new DateTime(2026, 10, 20), NgayKetThuc = new DateTime(2027, 1, 20), DonGia = 350000m, TongTien = 350000m, TrangThai = "Chờ xác nhận" },
            new PhieuDangKy { MaPhieu = "PDK24", MaHoiVien = "HV06", MaGoiTap = "GT09", NgayDangKy = new DateTime(2026, 10, 19), NgayBatDau = new DateTime(2026, 10, 19), NgayKetThuc = new DateTime(2026, 11, 19), DonGia = 200000m, TongTien = 200000m, TrangThai = "Chờ xác nhận" },
            new PhieuDangKy { MaPhieu = "PDK25", MaHoiVien = "HV08", MaGoiTap = "GT15", NgayDangKy = new DateTime(2026, 10, 21), NgayBatDau = new DateTime(2026, 10, 21), NgayKetThuc = new DateTime(2026, 12, 21), DonGia = 700000m, TongTien = 700000m, TrangThai = "Chờ xác nhận" },
            new PhieuDangKy { MaPhieu = "PDK26", MaHoiVien = "HV11", MaGoiTap = "GT11", NgayDangKy = new DateTime(2026, 10, 21), NgayBatDau = new DateTime(2026, 10, 21), NgayKetThuc = new DateTime(2027, 1, 21), DonGia = 1200000m, TongTien = 1200000m, TrangThai = "Chờ xác nhận" },
            new PhieuDangKy { MaPhieu = "PDK27", MaHoiVien = "HV14", MaGoiTap = "GT14", NgayDangKy = new DateTime(2026, 6, 1), NgayBatDau = new DateTime(2026, 6, 1), NgayKetThuc = new DateTime(2026, 9, 1), DonGia = 800000m, TongTien = 800000m, TrangThai = "Đã hủy" },
            new PhieuDangKy { MaPhieu = "PDK28", MaHoiVien = "HV04", MaGoiTap = "GT01", NgayDangKy = new DateTime(2026, 3, 15), NgayBatDau = new DateTime(2026, 3, 15), NgayKetThuc = new DateTime(2026, 4, 15), DonGia = 150000m, TongTien = 150000m, TrangThai = "Đã hủy" }
        };
        modelBuilder.Entity<PhieuDangKy>().HasData(phieus);
    }
}