using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI_TI17A1HN.Models;

public class QuanLyPhongTap_UNETI_TI17A1HNContext(DbContextOptions<QuanLyPhongTap_UNETI_TI17A1HNContext> options) : DbContext(options)
{
    public DbSet<QuanLyPhongTap_UNETI_TI17A1HN.Models.PhieuDangKy> PhieuDangKy { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // HoiVien (1) -- (n) PhieuDangKy
        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.HoiVien)
            .WithMany(h => h.PhieuDangKy)
            .HasForeignKey(p => p.IdHoiVien);

        // GoiTap (1) -- (n) PhieuDangKy
        modelBuilder.Entity<PhieuDangKy>()
            .HasOne(p => p.GoiTap)
            .WithMany(g => g.PhieuDangKy)
            .HasForeignKey(p => p.IdGoiTap);

        // LoaiGoiTap (1) -- (n) GoiTap
        modelBuilder.Entity<GoiTap>()
            .HasOne(g => g.LoaiGoiTap)
            .WithMany(l => l.GoiTap)
            .HasForeignKey(g => g.LoaiGoiTapId);
    }
}
