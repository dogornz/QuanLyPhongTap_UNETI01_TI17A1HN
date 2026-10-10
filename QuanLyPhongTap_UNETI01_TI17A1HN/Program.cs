using Microsoft.EntityFrameworkCore;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuanLyPhongTap_UNETI01_TI17A1HNContext") ?? throw new InvalidOperationException("Connection string 'QuanLyPhongTap_UNETI01_TI17A1HNContext' not found.");

builder.Services.AddDbContext<QuanLyPhongTap_UNETI01_TI17A1HNContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPhieuDangKyRepository, PhieuDangKyRepository>();
builder.Services.AddScoped<IGoiTapRepository, GoiTapRepository>();
builder.Services.AddScoped<IHoiVienRepository, HoiVienRepository>();

builder.Services.AddScoped<PhieuDangKyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
