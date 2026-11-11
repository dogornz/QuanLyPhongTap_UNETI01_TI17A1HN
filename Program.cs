using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLyPhongTap_UNETI01_TI17A1HN.Repositories;
using QuanLyPhongTap_UNETI01_TI17A1HN.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuanLyPhongTap_UNETI01_TI17A1HNContext")
    ?? builder.Configuration.GetConnectionString("QuanlyphongtapContext")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<QuanLyPhongTap_UNETI01_TI17A1HNContext>(options => options.UseSqlServer(connectionString));

// Add services to the container (Controllers và Views cho cả MVC và Web API)
builder.Services.AddControllersWithViews();

// Hỗ trợ hiển thị đầy đủ ký tự tiếng Việt Unicode không bị mã hóa HTML entity
builder.Services.AddSingleton<System.Text.Encodings.Web.HtmlEncoder>(
    System.Text.Encodings.Web.HtmlEncoder.Create(System.Text.Unicode.UnicodeRanges.All));

// 3. Đọc cấu hình JWT từ appsettings.json theo chuẩn bài giảng (Slide 17, 18)
var jwtSection = builder.Configuration.GetSection("Jwt");
string jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Key.");
string jwtIssuer = jwtSection["Issuer"]
    ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Issuer.");
string jwtAudience = jwtSection["Audience"]
    ?? throw new InvalidOperationException("Chưa cấu hình Jwt:Audience.");

// Kiểm tra khóa JWT tối thiểu 32 byte theo slide 18
if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("Khóa JWT phải có ít nhất 32 byte.");
}

// 4. Cấu hình Authentication kết hợp Cookie (Web MVC) và JWT Bearer (Web API) (Slide 18, 19)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// 5. Đăng ký Authorization hỗ trợ cả Cookie và JWT Bearer (Slide 19)
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(
        CookieAuthenticationDefaults.AuthenticationScheme,
        JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

// 6. Đăng ký dịch vụ JWT Token Service theo chuẩn bài giảng (Slide 19)
builder.Services.AddScoped<JwtTokenService>();

// 7. Cấu hình Swagger hỗ trợ nhập JWT Bearer Token theo chuẩn bài giảng (Slide 20, 21)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QuanLyPhongTapAPI",
        Version = "v1",
        Description = "Hệ thống Quản lý phòng tập - API xác thực JWT Bearer & Phân quyền theo chuẩn bài giảng 3.10"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập JWT Bearer Token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Đăng ký các Repositories & Services nghiệp vụ
builder.Services.AddScoped<IPhieuDangKyRepository, PhieuDangKyRepository>();
builder.Services.AddScoped<IGoiTapRepository, GoiTapRepository>();
builder.Services.AddScoped<IHoiVienRepository, HoiVienRepository>();
builder.Services.AddScoped<ILoaiGoiTapRepository, LoaiGoiTapRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<PhieuDangKyService>();
builder.Services.AddScoped<HoiVienService>();
builder.Services.AddScoped<ThongKeService>();
builder.Services.AddScoped<QuanLyGoiTapService>();

var app = builder.Build();

// Tự động nâng cấp mã hóa bảo mật cho các tài khoản cũ trong cơ sở dữ liệu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<QuanLyPhongTap_UNETI01_TI17A1HNContext>();
        var passwordService = services.GetRequiredService<IPasswordService>();
        await DbInitializer.MigrateLegacyPasswordsAsync(context, passwordService);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi xảy ra trong quá trình khởi tạo bảo mật DB.");
    }
}

// 8. Cấu hình Middleware theo đúng thứ tự (Slide 22)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

// UseAuthentication() phải đặt TRƯỚC UseAuthorization() (Slide 22)
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers(); // Hỗ trợ Web API Routes (api/Auth/...)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
