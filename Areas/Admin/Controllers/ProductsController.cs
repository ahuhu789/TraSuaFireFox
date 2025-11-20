using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraSuaFireFox.Models; // Nhớ đổi namespace đúng project của bạn
using Microsoft.AspNetCore.Authorization;

namespace TraSuaFireFox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Quản lý")]
    public class ProductsController : Controller
    {
        private readonly QlTrasuaMainContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Để lấy đường dẫn lưu ảnh

        public ProductsController(QlTrasuaMainContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 1. DANH SÁCH SẢN PHẨM
        public async Task<IActionResult> Index()
        {
            // Include để lấy tên Danh mục và Nhà cung cấp thay vì chỉ lấy ID
            var products = _context.Sanphams
                .Include(s => s.MadmNavigation)
                .Include(s => s.ManccNavigation);
            return View(await products.ToListAsync());
        }

        // 2. TẠO MỚI (GET)
        public IActionResult Create()
        {
            ViewBag.DsDanhmuc = _context.Danhmucs.ToList();
            ViewBag.DsNhacungcap = _context.Nhacungcaps.ToList();
            return View();
        }

        // 3. TẠO MỚI (POST) 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sanpham sanpham, IFormFile? HinhAnhUpload)
        {

            if (_context.Sanphams.Any(s => s.Masp == sanpham.Masp))
            {
                ModelState.AddModelError("Masp", "Mã sản phẩm này đã tồn tại!");
            }
            if (ModelState.IsValid)
            {
                // Xử lý upload ảnh
                if (HinhAnhUpload != null)
                {
                    // Tạo tên file độc nhất để tránh trùng lặp (VD: tra-sua-uuid.jpg)
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhUpload.FileName);
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    // Copy ảnh vào thư mục
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await HinhAnhUpload.CopyToAsync(fileStream);
                    }

                    // Lưu tên file vào Model
                    sanpham.Hinhanh = fileName;
                }
                else
                {
                    sanpham.Hinhanh = "default.jpg"; // Ảnh mặc định nếu không upload
                }

                sanpham.Ngaytao = DateOnly.FromDateTime(DateTime.Now); // Set ngày tạo
                _context.Sanphams.Add(sanpham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã thêm sản phẩm '{sanpham.Tensp}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi form thì load lại dropdown
            ViewBag.DsDanhmuc = _context.Danhmucs.ToList();
            ViewBag.DsNhacungcap = _context.Nhacungcaps.ToList();
            return View(sanpham);
        }

        // ---------------------------------------------------------
        // 4. CHỈNH SỬA (GET) - Hiển thị form
        // ---------------------------------------------------------
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null) return NotFound();

            // Lấy danh sách cho Dropdown
            ViewBag.DsDanhmuc = _context.Danhmucs.ToList();
            ViewBag.DsNhacungcap = _context.Nhacungcaps.ToList();

            return View(sanpham);
        }

        // ---------------------------------------------------------
        // 5. CHỈNH SỬA (POST) - Xử lý cập nhật
        // ---------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Sanpham sanpham, IFormFile? HinhAnhUpload)
        {
            if (id != sanpham.Masp) return NotFound();

            // Bỏ qua validate các bảng liên kết (giống phần Create)
            ModelState.Remove("MadmNavigation");
            ModelState.Remove("ManccNavigation");
            ModelState.Remove("Ctdonhangs");
            ModelState.Remove("Chitietsanpham");

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin sản phẩm cũ từ DB (để lấy tên ảnh cũ)
                    // AsNoTracking() quan trọng để tránh lỗi conflict khi Update
                    var sanphamCu = await _context.Sanphams.AsNoTracking().FirstOrDefaultAsync(x => x.Masp == id);

                    if (HinhAnhUpload != null)
                    {
                        // A. UPLOAD ẢNH MỚI
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhUpload.FileName);
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await HinhAnhUpload.CopyToAsync(fileStream);
                        }
                        sanpham.Hinhanh = fileName;

                        // B. XÓA ẢNH CŨ (Nếu có và không phải default)
                        if (sanphamCu != null && !string.IsNullOrEmpty(sanphamCu.Hinhanh) && sanphamCu.Hinhanh != "default.jpg")
                        {
                            string oldPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", sanphamCu.Hinhanh);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }
                    }
                    else
                    {
                        // Nếu không upload ảnh mới, GIỮ NGUYÊN ẢNH CŨ
                        sanpham.Hinhanh = sanphamCu?.Hinhanh;
                    }

                    // Giữ nguyên ngày tạo cũ (hoặc logic khác tùy bạn)
                    sanpham.Ngaytao = sanphamCu?.Ngaytao;

                    _context.Update(sanpham);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Cập nhật thành công: {sanpham.Tensp}";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Sanphams.Any(e => e.Masp == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.DsDanhmuc = _context.Danhmucs.ToList();
            ViewBag.DsNhacungcap = _context.Nhacungcaps.ToList();
            return View(sanpham);
        }

        // ---------------------------------------------------------
        // 6. XÓA SẢN PHẨM (POST)
        // ---------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                // Xóa file ảnh khỏi server
                if (!string.IsNullOrEmpty(sanpham.Hinhanh) && sanpham.Hinhanh != "default.jpg")
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products", sanpham.Hinhanh);
                    if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
                }

                _context.Sanphams.Remove(sanpham);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa sản phẩm thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}