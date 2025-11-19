using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TraSuaFireFox.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- BẮT ĐẦU CẤU HÌNH ---
// 2. Đăng ký dịch vụ Authentication bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Nếu chưa đăng nhập thì chuyển hướng về đây
        options.AccessDeniedPath = "/Account/AccessDenied"; // Nếu không đủ quyền thì chuyển về đây
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie tồn tại 60 phút
    });
// --- KẾT THÚC CẤU HÌNH ---

// Đăng ký DbContext
builder.Services.AddDbContext<QlTrasuaMainContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QlTrasuaMainContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"); // Route cho Admin Area

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
