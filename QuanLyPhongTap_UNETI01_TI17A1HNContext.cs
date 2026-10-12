
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Models;

public class QuanLyPhongTap_UNETI01_TI17A1HNContext(
    DbContextOptions<QuanLyPhongTap_UNETI01_TI17A1HNContext> options)
    : DbContext(options)
{
    public DbSet<HoiVien> HoiVien { get; set; } = default!;

    public DbSet<GoiTap> GoiTap { get; set; } = default!;

    public DbSet<PhieuDangKy> PhieuDangKy { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}
