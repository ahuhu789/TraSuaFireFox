using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraSuaFireFox.Models;
using Microsoft.AspNetCore.Authorization;

namespace TraSuaFireFox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản lý")]
    public class EmployeesController : Controller
    {
        private readonly QlTrasuaMainContext _context;

        public EmployeesController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // 1. DANH SÁCH NHÂN VIÊN
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nhanviens.ToListAsync());
        }

        // 2. TẠO MỚI (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 3. TẠO MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Nhanvien nhanvien)
        {
            ModelState.Remove("Donhangs");

            // 1. Kiểm tra trùng Mã Nhân Viên
            if (_context.Nhanviens.Any(x => x.Manv == nhanvien.Manv))
            {
                ModelState.AddModelError("Manv", "Mã nhân viên này đã tồn tại.");
            }

            //Kiểm tra trùng email
            if (_context.Nhanviens.Any(x => x.Email == nhanvien.Email))
            {
                ModelState.AddModelError("Email", "Trùng Email với nhân viên khác.");
            }

            // 2. Xử lý dữ liệu mặc định nếu thiếu
            if (nhanvien.Ngayvaolam == null) nhanvien.Ngayvaolam = DateOnly.FromDateTime(DateTime.Now);
            if (string.IsNullOrEmpty(nhanvien.Trangthai)) nhanvien.Trangthai = "Đang làm";

            if (ModelState.IsValid)
            {
                _context.Add(nhanvien);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã thêm nhân viên '{nhanvien.Hoten}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            return View(nhanvien);
        }

        // 4. CHỈNH SỬA (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var nhanvien = await _context.Nhanviens.FindAsync(id);
            if (nhanvien == null) return NotFound();

            return View(nhanvien);
        }

        // 5. CHỈNH SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Nhanvien nhanvien)
        {
            if (id != nhanvien.Manv) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanvien);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Cập nhật nhân viên '{nhanvien.Hoten}' thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Nhanviens.Any(e => e.Manv == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nhanvien);
        }

        // 6. XÓA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nhanvien = await _context.Nhanviens.FindAsync(id);
            if (nhanvien != null)
            {
                _context.Nhanviens.Remove(nhanvien);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa nhân viên thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}