using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Migrations
{
    /// <inheritdoc />
    public partial class Nhom1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoiVien",
                columns: table => new
                {
                    MaHoiVien = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayThamGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiVien", x => x.MaHoiVien);
                });

            migrationBuilder.CreateTable(
                name: "LoaiGoiTap",
                columns: table => new
                {
                    MaLoaiGoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiGoi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoGoiTrucThuoc = table.Column<int>(type: "int", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiGoiTap", x => x.MaLoaiGoi);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "GoiTap",
                columns: table => new
                {
                    MaGoiTap = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TenGoiTap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThoiHanThang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SoBuoiToiDa = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaLoaiGoi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiTap", x => x.MaGoiTap);
                    table.ForeignKey(
                        name: "FK_GoiTap_LoaiGoiTap_MaLoaiGoi",
                        column: x => x.MaLoaiGoi,
                        principalTable: "LoaiGoiTap",
                        principalColumn: "MaLoaiGoi",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhieuDangKy",
                columns: table => new
                {
                    MaPhieu = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaHoiVien = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    MaGoiTap = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDangKy", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PhieuDangKy_GoiTap_MaGoiTap",
                        column: x => x.MaGoiTap,
                        principalTable: "GoiTap",
                        principalColumn: "MaGoiTap",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhieuDangKy_HoiVien_MaHoiVien",
                        column: x => x.MaHoiVien,
                        principalTable: "HoiVien",
                        principalColumn: "MaHoiVien",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "HoiVien",
                columns: new[] { "MaHoiVien", "Email", "GioiTinh", "HoTen", "MaTaiKhoan", "NgaySinh", "NgayThamGia", "SoDienThoai", "TrangThai" },
                values: new object[,]
                {
                    { "HV01", "an.nguyen@gym.com", "Nam", "Nguyễn Văn An", "3", new DateTime(1998, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345001", "Đang hoạt động" },
                    { "HV02", "bich.tran@gym.com", "Nữ", "Trần Thị Bích", "4", new DateTime(1999, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345002", "Đang hoạt động" },
                    { "HV03", "cuong.le@gym.com", "Nam", "Lê Hoàng Cường", "5", new DateTime(1995, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345003", "Đang hoạt động" },
                    { "HV04", "duc.pham@gym.com", "Nam", "Phạm Minh Đức", "6", new DateTime(2001, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345004", "Đang hoạt động" },
                    { "HV05", "dung.vu@gym.com", "Nữ", "Vũ Phương Dung", "7", new DateTime(2000, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345005", "Đang hoạt động" },
                    { "HV06", "hung.dang@gym.com", "Nam", "Đặng Quốc Hưng", "8", new DateTime(1997, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345006", "Đang hoạt động" },
                    { "HV07", "ha.bui@gym.com", "Nữ", "Bùi Thu Hà", "9", new DateTime(2002, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345007", "Đang hoạt động" },
                    { "HV08", "kiet.do@gym.com", "Nam", "Đỗ Tuấn Kiệt", "10", new DateTime(1996, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345008", "Đang hoạt động" },
                    { "HV09", "linh.ho@gym.com", "Nữ", "Hồ Mai Linh", "11", new DateTime(2003, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345009", "Đang hoạt động" },
                    { "HV10", "nam.ngo@gym.com", "Nam", "Ngô Thành Nam", "12", new DateTime(1994, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345010", "Ngừng hoạt động" },
                    { "HV11", "oanh.duong@gym.com", "Nữ", "Dương Diệu Oanh", "13", new DateTime(1999, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345011", "Đang hoạt động" },
                    { "HV12", "phuc.ly@gym.com", "Nam", "Lý Gia Phúc", "14", new DateTime(2001, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345012", "Đang hoạt động" },
                    { "HV13", "quang.phan@gym.com", "Nam", "Phan Thanh Quang", "15", new DateTime(1998, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345013", "Đang hoạt động" },
                    { "HV14", "son.trinh@gym.com", "Nam", "Trịnh Bảo Sơn", "16", new DateTime(1997, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345014", "Đang hoạt động" },
                    { "HV15", "tien.hoang@gym.com", "Nữ", "Hoàng Thảo Tiên", "17", new DateTime(2002, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345015", "Đang hoạt động" },
                    { "HV16", "uy.mai@gym.com", "Nam", "Mai Quốc Uy", "18", new DateTime(1993, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345016", "Đang hoạt động" },
                    { "HV17", "uyen.doan@gym.com", "Nữ", "Đoàn Tố Uyên", "19", new DateTime(2000, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345017", "Đang hoạt động" },
                    { "HV18", "viet.cao@gym.com", "Nam", "Cao Đăng Việt", "20", new DateTime(1996, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345018", "Đang hoạt động" },
                    { "HV19", "yen.nguyen@gym.com", "Nữ", "Nguyễn Hải Yến", "21", new DateTime(2004, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345019", "Đang hoạt động" },
                    { "HV20", "tung.le@gym.com", "Nam", "Lê Văn Tùng", "22", new DateTime(1995, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345020", "Ngừng hoạt động" }
                });

            migrationBuilder.InsertData(
                table: "LoaiGoiTap",
                columns: new[] { "MaLoaiGoi", "MoTa", "NgayCapNhat", "SoGoiTrucThuoc", "TenLoaiGoi", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Khu tập tạ và máy cardio", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "GYM & CARDIO TIÊU CHUẨN", "Active" },
                    { 2, "1 kèm 1 với HLV chuyên nghiệp", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)", "Active" },
                    { 3, "Lớp Yoga, Zumba, Pilates", new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "YOGA & GROUP X", "Active" },
                    { 4, "Hồ bơi 4 mùa và xông hơi đá muối", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "BƠI LỘI & SAUNA SPA", "Active" },
                    { 5, "Full dịch vụ Gym, Yoga, Bơi", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "COMBO TOÀN NĂNG", "Active" },
                    { 6, "Chiến dịch hè cũ", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "GÓI TRẢI NGHIỆM HÈ (CŨ)", "Inactive" }
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "MaTaiKhoan", "Email", "HoTen", "MatKhau", "TenDangNhap", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { 1, "admin01@gym.com", "Admin 01", "admin123", "admin01", "Unlock", "admin" },
                    { 2, "admin02@gym.com", "Admin 02", "admin123", "admin02", "Unlock", "admin" },
                    { 3, "an.nguyen@gym.com", "Nguyễn Văn An", "hoivien123", "hoivien01", "Unlock", "hoivien" },
                    { 4, "bich.tran@gym.com", "Trần Thị Bích", "hoivien123", "hoivien02", "Unlock", "hoivien" },
                    { 5, "cuong.le@gym.com", "Lê Hoàng Cường", "hoivien123", "hoivien03", "Unlock", "hoivien" },
                    { 6, "duc.pham@gym.com", "Phạm Minh Đức", "hoivien123", "hoivien04", "Unlock", "hoivien" },
                    { 7, "dung.vu@gym.com", "Vũ Phương Dung", "hoivien123", "hoivien05", "Unlock", "hoivien" },
                    { 8, "hung.dang@gym.com", "Đặng Quốc Hưng", "hoivien123", "hoivien06", "Unlock", "hoivien" },
                    { 9, "ha.bui@gym.com", "Bùi Thu Hà", "hoivien123", "hoivien07", "Unlock", "hoivien" },
                    { 10, "kiet.do@gym.com", "Đỗ Tuấn Kiệt", "hoivien123", "hoivien08", "Unlock", "hoivien" },
                    { 11, "linh.ho@gym.com", "Hồ Mai Linh", "hoivien123", "hoivien09", "Unlock", "hoivien" },
                    { 12, "nam.ngo@gym.com", "Ngô Thành Nam", "hoivien123", "hoivien10", "Lock", "hoivien" },
                    { 13, "oanh.duong@gym.com", "Dương Diệu Oanh", "hoivien123", "hoivien11", "Unlock", "hoivien" },
                    { 14, "phuc.ly@gym.com", "Lý Gia Phúc", "hoivien123", "hoivien12", "Unlock", "hoivien" },
                    { 15, "quang.phan@gym.com", "Phan Thanh Quang", "hoivien123", "hoivien13", "Unlock", "hoivien" },
                    { 16, "son.trinh@gym.com", "Trịnh Bảo Sơn", "hoivien123", "hoivien14", "Unlock", "hoivien" },
                    { 17, "tien.hoang@gym.com", "Hoàng Thảo Tiên", "hoivien123", "hoivien15", "Unlock", "hoivien" },
                    { 18, "uy.mai@gym.com", "Mai Quốc Uy", "hoivien123", "hoivien16", "Unlock", "hoivien" },
                    { 19, "uyen.doan@gym.com", "Đoàn Tố Uyên", "hoivien123", "hoivien17", "Unlock", "hoivien" },
                    { 20, "viet.cao@gym.com", "Cao Đăng Việt", "hoivien123", "hoivien18", "Unlock", "hoivien" },
                    { 21, "yen.nguyen@gym.com", "Nguyễn Hải Yến", "hoivien123", "hoivien19", "Unlock", "hoivien" },
                    { 22, "tung.le@gym.com", "Lê Văn Tùng", "hoivien123", "hoivien20", "Lock", "hoivien" }
                });

            migrationBuilder.InsertData(
                table: "GoiTap",
                columns: new[] { "MaGoiTap", "DonGia", "MaLoaiGoi", "MoTa", "SoBuoiToiDa", "TenGoiTap", "ThoiHanThang", "TrangThai" },
                values: new object[,]
                {
                    { "GT01", 150000m, 1, "Tập gym 1 tháng không giới hạn khung giờ", 30, "GYM 1 Tháng", "1", "Đang áp dụng" },
                    { "GT02", 350000m, 1, "Tập gym 3 tháng kèm 1 buổi định hướng HLV", 90, "GYM 3 Tháng", "3", "Đang áp dụng" },
                    { "GT03", 600000m, 1, "Gói 6 tháng, miễn phí tủ locker cá nhân và nước uống", 180, "GYM 6 Tháng", "6", "Đang áp dụng" },
                    { "GT04", 1000000m, 1, "Tập luyện cả năm, kèm dịch vụ phòng xông hơi Sauna", 365, "GYM 12 Tháng", "12", "Đang áp dụng" },
                    { "GT05", 2000000m, 2, "10 buổi tập kèm riêng HLV Master, kèm thực đơn", 10, "PT 1-1 10 Buổi", "2", "Đang áp dụng" },
                    { "GT06", 3500000m, 2, "20 buổi tập kèm riêng HLV Master, cam kết tiến độ", 20, "PT 1-1 20 Buổi", "3", "Đang áp dụng" },
                    { "GT07", 250000m, 3, "Tham gia không giới hạn các lớp Yoga cơ bản và nâng cao", 30, "Yoga Tháng", "1", "Đang áp dụng" },
                    { "GT08", 600000m, 3, "Khóa Yoga trị liệu, thiền định và dẻo dai toàn diện", 90, "Yoga 3 Tháng", "3", "Đang áp dụng" },
                    { "GT09", 200000m, 4, "Bơi tự do tại hồ bơi nước mặn 4 mùa tiêu chuẩn Olympic", 30, "Bơi 1 Tháng", "1", "Đang áp dụng" },
                    { "GT10", 900000m, 4, "Thẻ bơi 6 tháng kèm tiện ích xông hơi đá muối thảo dược", 180, "Bơi 6 Tháng", "6", "Đang áp dụng" },
                    { "GT11", 1200000m, 5, "Gym + Yoga + Bơi lội trọn gói trong 3 tháng", 90, "Combo 3 Tháng", "3", "Đang áp dụng" },
                    { "GT12", 2000000m, 5, "Gym + Yoga + Bơi + Sauna không giới hạn 6 tháng", 180, "Combo 6 Tháng", "6", "Đang áp dụng" },
                    { "GT13", 3500000m, 5, "Đặc quyền cao cấp nhất, toàn bộ dịch vụ và tiện ích", 365, "Combo 12 Tháng", "12", "Đang áp dụng" },
                    { "GT14", 800000m, 6, "Gói ưu đãi chiến dịch hè (đã ngừng áp dụng)", 90, "Combo Hè Giới Hạn", "3", "Ngừng áp dụng" },
                    { "GT15", 700000m, 3, "Kết hợp Yoga thanh lọc cơ thể và bơi lội rèn luyện sức bền", 60, "Bơi + Yoga", "2", "Đang áp dụng" }
                });

            migrationBuilder.InsertData(
                table: "PhieuDangKy",
                columns: new[] { "MaPhieu", "DonGia", "MaGoiTap", "MaHoiVien", "NgayBatDau", "NgayDangKy", "NgayKetThuc", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { "PDK01", 350000m, "GT02", "HV01", new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK02", 150000m, "GT01", "HV01", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 150000m, "Đang hiệu lực" },
                    { "PDK03", 250000m, "GT07", "HV02", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 250000m, "Đang hiệu lực" },
                    { "PDK04", 600000m, "GT08", "HV02", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 600000m, "Đang hiệu lực" },
                    { "PDK05", 2000000m, "GT05", "HV03", new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000000m, "Đang hiệu lực" },
                    { "PDK06", 600000m, "GT03", "HV04", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 600000m, "Đang hiệu lực" },
                    { "PDK07", 1200000m, "GT11", "HV05", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200000m, "Đang hiệu lực" },
                    { "PDK08", 1000000m, "GT04", "HV06", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đang hiệu lực" },
                    { "PDK09", 200000m, "GT09", "HV07", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 200000m, "Đang hiệu lực" },
                    { "PDK10", 3500000m, "GT06", "HV08", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3500000m, "Đang hiệu lực" },
                    { "PDK11", 700000m, "GT15", "HV09", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 700000m, "Đang hiệu lực" },
                    { "PDK12", 150000m, "GT01", "HV10", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 150000m, "Đang hiệu lực" },
                    { "PDK13", 900000m, "GT10", "HV11", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 900000m, "Đang hiệu lực" },
                    { "PDK14", 350000m, "GT02", "HV12", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK15", 2000000m, "GT12", "HV13", new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000000m, "Đang hiệu lực" },
                    { "PDK16", 2000000m, "GT05", "HV14", new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000000m, "Đang hiệu lực" },
                    { "PDK17", 250000m, "GT07", "HV15", new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 250000m, "Đang hiệu lực" },
                    { "PDK18", 600000m, "GT03", "HV16", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 600000m, "Đang hiệu lực" },
                    { "PDK19", 200000m, "GT09", "HV17", new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 200000m, "Đang hiệu lực" },
                    { "PDK20", 3500000m, "GT13", "HV18", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3500000m, "Đang hiệu lực" },
                    { "PDK21", 600000m, "GT08", "HV19", new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 600000m, "Chờ xác nhận" },
                    { "PDK22", 150000m, "GT01", "HV20", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 150000m, "Đã hủy" },
                    { "PDK23", 350000m, "GT02", "HV03", new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK24", 200000m, "GT09", "HV06", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 200000m, "Chờ xác nhận" },
                    { "PDK25", 700000m, "GT15", "HV08", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 700000m, "Chờ xác nhận" },
                    { "PDK26", 1200000m, "GT11", "HV11", new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200000m, "Chờ xác nhận" },
                    { "PDK27", 800000m, "GT14", "HV14", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 800000m, "Đã hủy" },
                    { "PDK28", 150000m, "GT01", "HV04", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 150000m, "Đã hủy" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoiTap_MaLoaiGoi",
                table: "GoiTap",
                column: "MaLoaiGoi");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangKy_MaGoiTap",
                table: "PhieuDangKy",
                column: "MaGoiTap");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangKy_MaHoiVien",
                table: "PhieuDangKy",
                column: "MaHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoan_TenDangNhap",
                table: "TaiKhoan",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuDangKy");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "GoiTap");

            migrationBuilder.DropTable(
                name: "HoiVien");

            migrationBuilder.DropTable(
                name: "LoaiGoiTap");
        }
    }
}
