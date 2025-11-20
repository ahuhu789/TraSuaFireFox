using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TraSuaFireFox.Helpers;
using TraSuaFireFox.Models;
using TraSuaFireFox.Models.ViewModels;

namespace TraSuaFireFox.Controllers
{
    public class ClientAccountController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public ClientAccountController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // ================= ĐĂNG KÝ =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            // 1. Xác định Input là Email hay SĐT
            bool isEmail = model.ContactInfo.Contains("@");
            bool isPhone = Regex.IsMatch(model.ContactInfo, @"^\d{9,11}$");

            if (!isEmail && !isPhone)
            {
                ModelState.AddModelError("ContactInfo", "Định dạng Email hoặc SĐT không hợp lệ.");
                return View(model);
            }

            // 2. Kiểm tra đã tồn tại chưa
            var existingUser = await _context.Khachhangs.FirstOrDefaultAsync(k =>
                (isEmail && k.Email == model.ContactInfo) || (isPhone && k.Sdt == model.ContactInfo));

            if (existingUser != null)
            {
                ModelState.AddModelError("ContactInfo", "Email hoặc SĐT này đã được đăng ký.");
                return View(model);
            }

            // 3. Tạo Khách hàng mới
            // --- BẮT ĐẦU LOGIC SINH MÃ KHÁCH HÀNG TỰ ĐỘNG ---
            // 1. Tìm khách hàng có mã lớn nhất hiện tại trong CSDL
            var lastCustomer = _context.Khachhangs
                .OrderByDescending(k => k.Makh)
                .FirstOrDefault();

            int nextNumber = 1; // Mặc định là 1 nếu chưa có ai

            if (lastCustomer != null)
            {
                // Giả sử mã có dạng "KH0001", "KH1025"...
                // Ta cắt bỏ 2 ký tự đầu ("KH") để lấy phần số
                // Lưu ý: Cần đảm bảo dữ liệu cũ trong DB đúng định dạng KH...
                try
                {
                    string numberPart = lastCustomer.Makh.Substring(2);
                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }
                catch
                {
                    // Nếu mã cũ bị sai định dạng (vd: "TEST"), bỏ qua và reset về 1 hoặc xử lý riêng
                    nextNumber = 1;
                }
            }

            // Tạo mã mới dạng KHxxxx (ví dụ: KH0001, KH0012)
            // "D4" nghĩa là số luôn có 4 chữ số (tự điền số 0 ở đầu)
            string newMakh = "KH" + nextNumber.ToString("D4");
            // -----------------------------------------------

            // 2. Tạo đối tượng Khách hàng với mã vừa sinh
            var khachhang = new Khachhang
            {
                Makh = newMakh, // Sử dụng mã tự tăng vừa tạo
                Hoten = model.Hoten,
                Matkhau = model.Matkhau,
                Trangthai = "Hoạt động",
                Trangthaixacthuc = false,

                // Tạo OTP
                Maxacthuc = new Random().Next(100000, 999999).ToString(),
                Hanhieuluc = DateTime.Now.AddMinutes(10)
            };

            if (isEmail) khachhang.Email = model.ContactInfo;
            if (isPhone) khachhang.Sdt = model.ContactInfo;

            _context.Khachhangs.Add(khachhang);
            await _context.SaveChangesAsync();

            // 4. Gửi OTP
            if (isEmail)
            {
                // Gửi Email thật (Cần cấu hình MailService)
                SendMailService.SendEmail(khachhang.Email, "Mã xác thực FireFox", $"Mã OTP của bạn là: <b>{khachhang.Maxacthuc}</b>");

                // DEBUG: In ra Console nếu chưa cấu hình mail
                Console.WriteLine($"--- OTP GỬI EMAIL ĐẾN {khachhang.Email}: {khachhang.Maxacthuc} ---");
            }
            else
            {
                // Giả lập gửi SMS (In ra cửa sổ Output của Visual Studio)
                Console.WriteLine($"--- OTP GỬI SMS ĐẾN {khachhang.Sdt}: {khachhang.Maxacthuc} ---");
            }

            // Chuyển sang trang xác thực
            return RedirectToAction("Verify", new { contact = model.ContactInfo });
        }

        // ================= XÁC THỰC OTP =================
        [HttpGet]
        public IActionResult Verify(string contact)
        {
            return View(new VerifyVM { ContactInfo = contact });
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyVM model)
        {
            var user = await _context.Khachhangs.FirstOrDefaultAsync(k => k.Email == model.ContactInfo || k.Sdt == model.ContactInfo);

            if (user == null) return NotFound();

            // Kiểm tra OTP và Thời hạn
            if (user.Maxacthuc == model.OtpCode && user.Hanhieuluc > DateTime.Now)
            {
                user.Trangthaixacthuc = true;
                user.Maxacthuc = null; // Xóa OTP sau khi dùng
                await _context.SaveChangesAsync();

                TempData["Success"] = "Xác thực thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("OtpCode", "Mã OTP không đúng hoặc đã hết hạn.");
                return View(model);
            }
        }

        // ================= ĐĂNG NHẬP =================
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        // Trong ClientAccountController.cs

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = "/")
        {
            if (!ModelState.IsValid) return View(model);

            // --- BƯỚC 1: KIỂM TRA BẢNG NHÂN VIÊN (ADMIN/STAFF) ---
            // Tìm nhân viên theo Email HOẶC SĐT và Mật khẩu
            var nhanvien = await _context.Nhanviens
                .FirstOrDefaultAsync(n => (n.Email == model.Input || n.Sdt == model.Input)
                                           && n.Matkhau == model.Password);

            if (nhanvien != null)
            {
                if (nhanvien.Trangthai != "Đang làm")
                {
                    ModelState.AddModelError("", "Tài khoản nhân viên đã bị khóa hoặc nghỉ việc.");
                    return View(model);
                }
                // Tạo Claims cho Nhân viên
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, nhanvien.Hoten ?? "Nhân viên"),
            new Claim("Manv", nhanvien.Manv), // Lưu mã NV
            new Claim(ClaimTypes.Role, nhanvien.Chucvu ?? "Nhân viên"), // QUAN TRỌNG: Role lấy từ DB (Quản lý/Thu ngân...)
            // Thêm Role Admin để chắc chắn được vào các trang yêu cầu "Admin" nếu có
            new Claim(ClaimTypes.Role, "Admin")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                // -> CHUYỂN HƯỚNG VÀO TRANG ADMIN
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            // --- BƯỚC 2: KIỂM TRA BẢNG KHÁCH HÀNG (USER) ---
            // Nếu không tìm thấy trong bảng Nhân viên, tìm tiếp trong bảng Khách hàng
            var khachhang = await _context.Khachhangs
                .FirstOrDefaultAsync(k => (k.Email == model.Input || k.Sdt == model.Input)
                                           && k.Matkhau == model.Password);

            if (khachhang != null)
            {
                if (khachhang.Trangthai == "Khóa") // Kiểm tra trạng thái khách hàng (nếu có logic khóa)
                {
                    ModelState.AddModelError("", "Tài khoản khách hàng của bạn đã bị khóa.");
                    return View(model);
                }

                // Tạo Claims cho Khách hàng
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, khachhang.Hoten ?? "Khách hàng"),
            new Claim("Makh", khachhang.Makh), // Lưu mã KH
            new Claim(ClaimTypes.Role, "Customer") // QUAN TRỌNG: Role cố định là Customer
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                // -> CHUYỂN HƯỚNG VỀ TRANG CHỦ (Hoặc trang trước đó)
                return LocalRedirect(returnUrl);
            }

            // --- BƯỚC 3: ĐĂNG NHẬP THẤT BẠI ---
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
            return View(model);
        }

        // ================= ĐĂNG XUẤT =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ================= HỒ SƠ CÁ NHÂN =================
        [HttpGet]
        // Cần kiểm tra xem đã đăng nhập chưa?
        public async Task<IActionResult> Profile()
        {
            // Lấy Makh từ Cookie
            var makhClaim = User.Claims.FirstOrDefault(c => c.Type == "Makh");
            if (makhClaim == null) return RedirectToAction("Login");

            var user = await _context.Khachhangs.FindAsync(makhClaim.Value);
            if (user == null) return NotFound();

            var model = new ProfileVM
            {
                Makh = user.Makh,
                Hoten = user.Hoten,
                Email = user.Email,
                Sdt = user.Sdt,
                Diachi = user.Diachi
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileVM model)
        {
            var user = await _context.Khachhangs.FindAsync(model.Makh);
            if (user != null)
            {
                user.Hoten = model.Hoten;
                user.Diachi = model.Diachi;
                // Không cho sửa Email/SĐT ở đây vì liên quan đến đăng nhập (cần quy trình riêng)

                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật hồ sơ thành công!";
            }
            return View(model);
        }
    }
}