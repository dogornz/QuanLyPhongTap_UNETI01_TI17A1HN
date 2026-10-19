using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Migrations
{
    /// <inheritdoc />
    public partial class NguyenAnhDuong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoiTap",
                columns: table => new
                {
                    MaGoiTap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenGoiTap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiHanThang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SoBuoiToiDa = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiTap", x => x.MaGoiTap);
                });

            migrationBuilder.CreateTable(
                name: "HoiVien",
                columns: table => new
                {
                    MaHoiVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    MatKhau = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "PhieuDangKy",
                columns: table => new
                {
                    MaPhieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaHoiVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGoiTap = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                table: "GoiTap",
                columns: new[] { "MaGoiTap", "DonGia", "MoTa", "SoBuoiToiDa", "TenGoiTap", "ThoiHanThang", "TrangThai" },
                values: new object[,]
                {
                    { "GT01", 150000m, "Tập gym 1 tháng không giới hạn khung giờ", 30, "GYM 1 Tháng", "1", "Đang áp dụng" },
                    { "GT02", 350000m, "Tập gym 3 tháng kèm 1 buổi định hướng HLV", 90, "GYM 3 Tháng", "3", "Đang áp dụng" },
                    { "GT03", 600000m, "Gói 6 tháng, miễn phí tủ locker cá nhân và nước uống", 180, "GYM 6 Tháng", "6", "Đang áp dụng" },
                    { "GT04", 1000000m, "Tập luyện cả năm, kèm dịch vụ phòng xông hơi Sauna", 365, "GYM 12 Tháng", "12", "Đang áp dụng" },
                    { "GT05", 2000000m, "10 buổi tập kèm riêng HLV Master, kèm thực đơn", 10, "PT 1-1 10 Buổi", "2", "Đang áp dụng" },
                    { "GT06", 3500000m, "20 buổi tập kèm riêng HLV Master, cam kết tiến độ", 20, "PT 1-1 20 Buổi", "3", "Đang áp dụng" },
                    { "GT07", 250000m, "Tham gia không giới hạn các lớp Yoga cơ bản và nâng cao", 30, "Yoga Tháng", "1", "Đang áp dụng" },
                    { "GT08", 600000m, "Khóa Yoga trị liệu, thiền định và dẻo dai toàn diện", 90, "Yoga 3 Tháng", "3", "Đang áp dụng" },
                    { "GT09", 200000m, "Bơi tự do tại hồ bơi nước mặn 4 mùa tiêu chuẩn Olympic", 30, "Bơi 1 Tháng", "1", "Đang áp dụng" },
                    { "GT10", 900000m, "Thẻ bơi 6 tháng kèm tiện ích xông hơi đá muối thảo dược", 180, "Bơi 6 Tháng", "6", "Đang áp dụng" },
                    { "GT11", 1200000m, "Gym + Yoga + Bơi lội trọn gói trong 3 tháng", 90, "Combo 3 Tháng", "3", "Đang áp dụng" },
                    { "GT12", 2000000m, "Gym + Yoga + Bơi + Sauna không giới hạn 6 tháng", 180, "Combo 6 Tháng", "6", "Đang áp dụng" },
                    { "GT13", 3500000m, "Đặc quyền cao cấp nhất, toàn bộ dịch vụ và tiện ích", 365, "Combo 12 Tháng", "12", "Đang áp dụng" },
                    { "GT14", 800000m, "Gói ưu đãi chiến dịch hè (đã ngừng áp dụng)", 90, "Combo Hè Giới Hạn", "3", "Ngừng áp dụng" },
                    { "GT15", 700000m, "Kết hợp Yoga thanh lọc cơ thể và bơi lội rèn luyện sức bền", 60, "Bơi + Yoga", "2", "Đang áp dụng" }
                });

            migrationBuilder.InsertData(
                table: "HoiVien",
                columns: new[] { "MaHoiVien", "Email", "GioiTinh", "HoTen", "MaTaiKhoan", "NgaySinh", "NgayThamGia", "SoDienThoai", "TrangThai" },
                values: new object[,]
                {
                    { "HV01", "hoivien1@email.com", "Nữ", "Hội viên 1", "3", new DateTime(1991, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345671", "Đang hoạt động" },
                    { "HV02", "hoivien2@email.com", "Nam", "Hội viên 2", "4", new DateTime(1992, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345672", "Đang hoạt động" },
                    { "HV03", "hoivien3@email.com", "Nữ", "Hội viên 3", "3", new DateTime(1993, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345673", "Đang hoạt động" },
                    { "HV04", "hoivien4@email.com", "Nam", "Hội viên 4", "4", new DateTime(1994, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345674", "Đang hoạt động" },
                    { "HV05", "hoivien5@email.com", "Nữ", "Hội viên 5", "3", new DateTime(1995, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345675", "Đang hoạt động" },
                    { "HV06", "hoivien6@email.com", "Nam", "Hội viên 6", "4", new DateTime(1996, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345676", "Đang hoạt động" },
                    { "HV07", "hoivien7@email.com", "Nữ", "Hội viên 7", "3", new DateTime(1997, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345677", "Đang hoạt động" },
                    { "HV08", "hoivien8@email.com", "Nam", "Hội viên 8", "4", new DateTime(1998, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345678", "Đang hoạt động" },
                    { "HV09", "hoivien9@email.com", "Nữ", "Hội viên 9", "3", new DateTime(1999, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345679", "Đang hoạt động" },
                    { "HV10", "hoivien10@email.com", "Nam", "Hội viên 10", "4", new DateTime(1990, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345670", "Ngừng hoạt động" },
                    { "HV11", "hoivien11@email.com", "Nữ", "Hội viên 11", "3", new DateTime(1991, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345671", "Đang hoạt động" },
                    { "HV12", "hoivien12@email.com", "Nam", "Hội viên 12", "4", new DateTime(1992, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345672", "Đang hoạt động" },
                    { "HV13", "hoivien13@email.com", "Nữ", "Hội viên 13", "3", new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345673", "Đang hoạt động" },
                    { "HV14", "hoivien14@email.com", "Nam", "Hội viên 14", "4", new DateTime(1994, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345674", "Đang hoạt động" },
                    { "HV15", "hoivien15@email.com", "Nữ", "Hội viên 15", "3", new DateTime(1995, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345675", "Đang hoạt động" },
                    { "HV16", "hoivien16@email.com", "Nam", "Hội viên 16", "4", new DateTime(1996, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345676", "Đang hoạt động" },
                    { "HV17", "hoivien17@email.com", "Nữ", "Hội viên 17", "3", new DateTime(1997, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345677", "Đang hoạt động" },
                    { "HV18", "hoivien18@email.com", "Nam", "Hội viên 18", "4", new DateTime(1998, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345678", "Đang hoạt động" },
                    { "HV19", "hoivien19@email.com", "Nữ", "Hội viên 19", "3", new DateTime(1999, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345679", "Đang hoạt động" },
                    { "HV20", "hoivien20@email.com", "Nam", "Hội viên 20", "4", new DateTime(1990, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "0912345670", "Ngừng hoạt động" }
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
                    { 1, "admin01@gym.com", "Admin 01", "admin123", "admin01", "Lock", "admin" },
                    { 2, "admin02@gym.com", "Admin 02", "admin123", "admin02", "Unlock", "admin" },
                    { 3, "vana@gym.com", "Nguyễn Văn A", "hoivien123", "hoivien01", "Lock", "hoivien" },
                    { 4, "thib@gym.com", "Trần Thị B", "hoivien123", "hoivien02", "Unlock", "hoivien" }
                });

            migrationBuilder.InsertData(
                table: "PhieuDangKy",
                columns: new[] { "MaPhieu", "DonGia", "MaGoiTap", "MaHoiVien", "NgayBatDau", "NgayDangKy", "NgayKetThuc", "TongTien", "TrangThai" },
                values: new object[,]
                {
                    { "PDK01", 350000m, "GT02", "HV02", new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK02", 350000m, "GT03", "HV03", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK03", 350000m, "GT04", "HV04", new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK04", 350000m, "GT05", "HV05", new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK05", 350000m, "GT06", "HV06", new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đã hủy" },
                    { "PDK06", 350000m, "GT07", "HV07", new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK07", 350000m, "GT08", "HV08", new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK08", 350000m, "GT09", "HV09", new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK09", 350000m, "GT10", "HV10", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK10", 350000m, "GT11", "HV11", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đã hủy" },
                    { "PDK11", 350000m, "GT12", "HV12", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK12", 350000m, "GT13", "HV13", new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Đang hiệu lực" },
                    { "PDK13", 350000m, "GT14", "HV14", new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK14", 350000m, "GT15", "HV15", new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 350000m, "Chờ xác nhận" },
                    { "PDK15", 1000000m, "GT01", "HV16", new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đã hủy" },
                    { "PDK16", 1000000m, "GT02", "HV17", new DateTime(2026, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK17", 1000000m, "GT03", "HV18", new DateTime(2026, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK18", 1000000m, "GT04", "HV19", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đang hiệu lực" },
                    { "PDK19", 1000000m, "GT05", "HV20", new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK20", 1000000m, "GT06", "HV01", new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đã hủy" },
                    { "PDK21", 1000000m, "GT07", "HV02", new DateTime(2026, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đang hiệu lực" },
                    { "PDK22", 1000000m, "GT08", "HV03", new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK23", 1000000m, "GT09", "HV04", new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK24", 1000000m, "GT10", "HV05", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đang hiệu lực" },
                    { "PDK25", 1000000m, "GT11", "HV06", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đã hủy" },
                    { "PDK26", 1000000m, "GT12", "HV07", new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK27", 1000000m, "GT13", "HV08", new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đang hiệu lực" },
                    { "PDK28", 1000000m, "GT14", "HV09", new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK29", 1000000m, "GT15", "HV10", new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Chờ xác nhận" },
                    { "PDK30", 1000000m, "GT01", "HV11", new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000000m, "Đã hủy" }
                });

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
                name: "LoaiGoiTap");

            migrationBuilder.DropTable(
                name: "PhieuDangKy");

            migrationBuilder.DropTable(
                name: "TaiKhoan");

            migrationBuilder.DropTable(
                name: "GoiTap");

            migrationBuilder.DropTable(
                name: "HoiVien");
        }
    }
}
