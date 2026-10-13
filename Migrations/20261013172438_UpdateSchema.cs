using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyPhongTap_UNETI01_TI17A1HN.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
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
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "PhieuDangKy",
                columns: table => new
                {
                    MaPhieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaHoiVien = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaGoiTap = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                table: "LoaiGoiTap",
                columns: new[] { "MaLoaiGoi", "MoTa", "NgayCapNhat", "SoGoiTrucThuoc", "TenLoaiGoi", "TrangThai" },
                values: new object[,]
                {
                    { 1, "Truy cập khu tập tạ tự do, máy cardio hiện đại và phòng tắm nóng lạnh tiêu chuẩn quốc tế", new DateTime(2026, 3, 12, 8, 30, 0, 0, DateTimeKind.Unspecified), 8, "GYM & CARDIO TIÊU CHUẨN", "Active" },
                    { 2, "Giáo án chuyên sâu 1 kèm 1 với Master Coach, đo InBody định kỳ và thực đơn dinh dưỡng riêng", new DateTime(2026, 3, 15, 11, 15, 0, 0, DateTimeKind.Unspecified), 5, "HUẤN LUYỆN VIÊN CÁ NHÂN (VIP PT)", "Active" },
                    { 3, "Tham gia không giới hạn các lớp Zumba, Yoga Flow, Les Mills BodyPump, Pilates studio", new DateTime(2026, 3, 18, 14, 20, 0, 0, DateTimeKind.Unspecified), 4, "YOGA & GROUP X", "Active" },
                    { 4, "Bể bơi bốn mùa nước mặn, phòng xông hơi đá muối Himalaya cao cấp", new DateTime(2026, 3, 20, 9, 45, 0, 0, DateTimeKind.Unspecified), 3, "BƠI LỘI & SAUNA SPA", "Active" },
                    { 5, "Full quyền lợi tập luyện, hồ bơi, sauna và 10 buổi PT định hướng cho người mới bắt đầu", new DateTime(2026, 3, 25, 16, 0, 0, 0, DateTimeKind.Unspecified), 2, "COMBO TOÀN NĂNG ALL-IN-ONE", "Active" },
                    { 6, "Áp dụng chiến dịch hè (Đã dừng tuyển sinh mới)", new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 0, "GÓI TRẢI NGHIỆM HÈ (CŨ)", "Inactive" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangKy_MaGoiTap",
                table: "PhieuDangKy",
                column: "MaGoiTap");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuDangKy_MaHoiVien",
                table: "PhieuDangKy",
                column: "MaHoiVien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoaiGoiTap");

            migrationBuilder.DropTable(
                name: "PhieuDangKy");

            migrationBuilder.DropTable(
                name: "GoiTap");

            migrationBuilder.DropTable(
                name: "HoiVien");
        }
    }
}
