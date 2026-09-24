using Microsoft.EntityFrameworkCore;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options) : DbContext(options)
{
    public DbSet<QuanLyPhongTap_UNETI01_TI17A1HN.Models.HoiVien> HoiVien { get; set; } = default!;
}
