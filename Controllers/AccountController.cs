using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TraSuaFireFox.Models;
using TraSuaFireFox.Models.ViewModels;

namespace TraSuaFireFox.Controllers
{
    public class AccountController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public AccountController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập rồi thì đá về trang chủ
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // 1. Tìm nhân viên trong CSDL
                // LƯU Ý: Hiện tại đang so sánh password dạng thô.
                var user = await _context.Nhanviens
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Matkhau == model.Password);

                if (user != null)
                {
                    if (user.Trangthai != "Đang làm")
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa hoặc nghỉ việc.");
                        return View(model);
                    }

                    // 2. Tạo danh sách các Claim (Thông tin hộ chiếu)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Hoten), // Tên hiển thị
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("MaNV", user.Manv), // Lưu mã NV để dùng sau này
                        new Claim(ClaimTypes.Role, user.Chucvu) // QUAN TRỌNG: Để phân quyền (Quản lý/Nhân viên)
                    };

                    // 3. Tạo Identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 4. Sign In (Tạo Cookie)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties { IsPersistent = true } // Giữ đăng nhập ngay cả khi đóng trình duyệt
                    );

                    // 5. Chuyển hướng
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Nếu là Quản lý thì vào trang Admin, ngược lại về trang chủ
                    if (user.Chucvu == "Quản lý")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
            }

            return View(model);
        }

        // Đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Trang thông báo không đủ quyền
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}