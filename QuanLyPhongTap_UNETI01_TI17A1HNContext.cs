using Microsoft.EntityFrameworkCore;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options) : DbContext(options)
{

    public DbSet<QuanLyPhongTap_UNETI01_TI17A1HN.Models.TaiKhoan> TaiKhoan { get; set; } = default!;

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
    }
}
