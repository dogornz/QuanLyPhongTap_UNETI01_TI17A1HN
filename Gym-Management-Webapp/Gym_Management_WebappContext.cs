using Microsoft.EntityFrameworkCore;

public class Gym_Management_WebappContext(DbContextOptions<Gym_Management_WebappContext> options) : DbContext(options)
{
    public DbSet<Gym_Management_Webapp.Models.TaiKhoan> TaiKhoan { get; set; } = default!;
}
