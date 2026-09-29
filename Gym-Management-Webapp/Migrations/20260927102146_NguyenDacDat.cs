using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym_Management_Webapp.Migrations
{
    /// <inheritdoc />
    public partial class NguyenDacDat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoan", x => x.MaTaiKhoan);
                });

            migrationBuilder.InsertData(
                table: "TaiKhoan",
                columns: new[] { "MaTaiKhoan", "Email", "HoTen", "MatKhau", "TenDangNhap", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { 1, "user1@gmail.com", "Admin", "admin123", "admin", "Active", "Admin" },
                    { 2, "user2@gmail.com", "User", "user123", "user", "Active", "User" },
                    { 3, "user3@gmail.com", "User3", "user123", "user3", "Active", "User" },
                    { 4, "user4@gmail.com", "User4", "user123", "user4", "Active", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
