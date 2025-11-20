using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraSuaFireFox.Models; // Namespace Models của bạn
using System.Linq;

namespace TraSuaFireFox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản lý")]
    public class HomeController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public HomeController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy số liệu thống kê thực tế từ DB

            // 1. Tổng doanh thu (từ bảng THANHTOAN)
            var tongDoanhThu = _context.Thanhtoans.Sum(t => t.Tongtien);

            // 2. Số đơn hàng
            var soDonHang = _context.Donhangs.Count();

            // 3. Số sản phẩm đang bán
            var soSanPham = _context.Sanphams.Count(s => s.Trangthai == "Còn hàng");

            // 4. Số nhân viên
            var soNhanVien = _context.Nhanviens.Count(n => n.Trangthai == "Đang làm");

            // Truyền qua ViewBag
            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.SoDonHang = soDonHang;
            ViewBag.SoSanPham = soSanPham;
            ViewBag.SoNhanVien = soNhanVien;

            return View();
        }
    }
}