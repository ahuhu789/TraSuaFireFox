using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaFireFox.Models;

namespace TraSuaFireFox.Controllers
{
    public class MenuController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public MenuController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // Action hiển thị danh sách sản phẩm theo danh mục
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách tất cả các Danh mục (bao gồm cả sản phẩm của danh mục đó)
            var categories = await _context.Danhmucs
                .Include(d => d.Sanphams.Where(s => s.Trangthai == "Còn hàng")) // Lấy các sản phẩm đang bán
                .ToListAsync();

            // Lọc ra danh mục không có sản phẩm để tránh hiển thị menu trống
            var activeCategories = categories.Where(c => c.Sanphams.Any()).ToList();

            return View(activeCategories);
        }
    }
}