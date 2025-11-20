using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaFireFox.Models;
using Microsoft.AspNetCore.Authorization;

namespace TraSuaFireFox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản lý")]
    public class OrdersController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public OrdersController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH ĐƠN HÀNG
        public async Task<IActionResult> Index()
        {
            var donhangs = _context.Donhangs
                .Include(d => d.MakhNavigation) // Lấy tên khách
                .Include(d => d.ManvNavigation) // Lấy tên nhân viên
                .OrderByDescending(d => d.Ngaydathang); // Mới nhất lên đầu
            return View(await donhangs.ToListAsync());
        }

        // 2. XEM CHI TIẾT ĐƠN HÀNG
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var donhang = await _context.Donhangs
                .Include(d => d.MakhNavigation)
                .Include(d => d.ManvNavigation)
                .Include(d => d.Ctdonhangs) // Lấy chi tiết đơn hàng
                    .ThenInclude(ct => ct.MaspNavigation) // Lấy tên sản phẩm trong chi tiết
                .FirstOrDefaultAsync(m => m.Madh == id);

            if (donhang == null) return NotFound();

            return View(donhang);
        }

        // 3. CẬP NHẬT TRẠNG THÁI (GET)
        // Chúng ta chỉ cho sửa Trạng thái, Ghi chú và Ngày giao dự kiến
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var donhang = await _context.Donhangs.FindAsync(id);
            if (donhang == null) return NotFound();
            return View(donhang);
        }

        // 4. CẬP NHẬT TRẠNG THÁI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string Trangthaidh, DateTime? Ngaygiaohangdukien, string Ghichu)
        {
            // Lấy đơn hàng gốc từ CSDL
            var donhangDb = await _context.Donhangs.FindAsync(id);

            if (donhangDb == null)
            {
                return NotFound();
            }
            try
            {
                // Chỉ cập nhật 3 thông tin cho phép sửa
                donhangDb.Trangthaidh = Trangthaidh;
                donhangDb.Ngaygiaohangdukien = Ngaygiaohangdukien;
                donhangDb.Ghichu = Ghichu;

                // Lưu thay đổi
                _context.Update(donhangDb);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Cập nhật đơn hàng {id} thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
            }

            // Nếu lỗi, trả về lại View với dữ liệu cũ
            return View(donhangDb);
        }
    }

        // Không làm chức năng Xóa đơn hàng vì lý do lịch sử kế toán/thống kê.
        // Thay vào đó chỉ chuyển trạng thái sang "Đã hủy".
    }
