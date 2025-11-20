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

        // 4. CHỈNH SỬA (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham == null) return NotFound();

            // Lấy danh sách thô
            ViewBag.DsDanhmuc = _context.Danhmucs.ToList();
            ViewBag.DsNhacungcap = _context.Nhacungcaps.ToList();

            return View(sanpham);
        }

        // 5. CHỈNH SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Sanpham sanpham, IFormFile? HinhAnhUpload)
        {
            if (id != sanpham.Masp) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Logic xử lý ảnh khi update
                    if (HinhAnhUpload != null)
                    {
                        // 1. Upload ảnh mới (như phần Create)
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhUpload.FileName);
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");

                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await HinhAnhUpload.CopyToAsync(fileStream);
                        }

                        // 2. Cập nhật tên file mới
                        sanpham.Hinhanh = fileName;
                    }
                    else
                    {
                        // Nếu không chọn ảnh mới, giữ nguyên ảnh cũ (Cần cẩn thận: Form sẽ trả về null cho Hinhanh nếu không input hidden)
                        // Cách an toàn nhất là lấy từ DB ra gán lại, hoặc dùng AsNoTracking() ở GET. 
                        // Để đơn giản ở đây tôi giả sử bạn dùng input hidden ở View.
                    }

                    _context.Update(sanpham);
                    await _context.SaveChangesAsync();
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

        // 6. XÓA 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sanpham = await _context.Sanphams.FindAsync(id);
            if (sanpham != null)
            {
                _context.Sanphams.Remove(sanpham);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}