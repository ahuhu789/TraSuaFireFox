using Microsoft.AspNetCore.Mvc;
using TraSuaFireFox.Models;
using TraSuaFireFox.Helpers;

namespace TraSuaFireFox.Controllers
{
    public class CartController : Controller
    {
        private readonly QlTrasuaMainContext _context;
        const string CART_KEY = "MY_CART"; // Tên Key lưu trong Session

        public CartController(QlTrasuaMainContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách giỏ hàng từ Session
        public List<CartItem> GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY);
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }

        // 2. HIỂN THỊ GIỎ HÀNG
        public IActionResult Index()
        {
            var cart = GetCartItems();
            // Tính tổng tiền
            ViewBag.TongTien = cart.Sum(x => x.ThanhTien);
            return View(cart);
        }

        // 3. THÊM VÀO GIỎ
        public IActionResult AddToCart(string id, int quantity = 1)
        {
            var cart = GetCartItems();

            // Kiểm tra xem món này đã có trong giỏ chưa
            var item = cart.FirstOrDefault(p => p.Masp == id);

            if (item != null)
            {
                // Nếu có rồi thì tăng số lượng
                item.Soluong += quantity;
            }
            else
            {
                // Nếu chưa có thì lấy từ DB và thêm mới
                var sanpham = _context.Sanphams.Find(id);
                if (sanpham == null) return NotFound();

                cart.Add(new CartItem
                {
                    Masp = sanpham.Masp,
                    Tensp = sanpham.Tensp,
                    Dongia = sanpham.Dongia ?? 0,
                    Hinhanh = sanpham.Hinhanh ?? "default.jpg",
                    Soluong = quantity
                });
            }

            // Lưu lại vào Session
            HttpContext.Session.Set(CART_KEY, cart);

            // Quay lại trang trước đó hoặc trang Menu
            return RedirectToAction("Index", "Menu");
        }

        // 4. XÓA KHỎI GIỎ
        public IActionResult RemoveCart(string id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.Masp == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        // 5. CẬP NHẬT SỐ LƯỢNG (Dùng cho trang giỏ hàng)
        [HttpPost]
        public IActionResult UpdateCart(string id, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.Masp == id);
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Soluong = quantity;
                }
                else
                {
                    // Nếu chỉnh số lượng về 0 hoặc âm thì xóa luôn
                    cart.Remove(item);
                }
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }
        // ... (Giữ nguyên các hàm cũ)

        // 6. [API] LẤY DỮ LIỆU GIỎ HÀNG (Trả về JSON cho Ajax)
        [HttpGet]
        public IActionResult GetCartJson()
        {
            var cart = GetCartItems();
            var total = cart.Sum(x => x.ThanhTien);
            var count = cart.Sum(x => x.Soluong);

            return Json(new
            {
                items = cart,
                total = total,
                count = count
            });
        }

        // 7. [API] THÊM VÀO GIỎ BẰNG AJAX
        [HttpPost]
        public IActionResult AddToCartAjax(string id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.Masp == id);

            if (item != null)
            {
                item.Soluong++;
            }
            else
            {
                var sanpham = _context.Sanphams.Find(id);
                if (sanpham == null) return Json(new { success = false, message = "Sản phẩm không tồn tại" });

                cart.Add(new CartItem
                {
                    Masp = sanpham.Masp,
                    Tensp = sanpham.Tensp,
                    Dongia = sanpham.Dongia ?? 0,
                    Hinhanh = sanpham.Hinhanh ?? "default.jpg",
                    Soluong = 1
                });
            }

            HttpContext.Session.Set(CART_KEY, cart);

            // Trả về tổng số lượng mới để cập nhật icon ngay lập tức
            return Json(new { success = true, count = cart.Sum(x => x.Soluong) });
        }

        // 8. [API] XÓA KHỎI GIỎ AJAX (Dùng cho nút xóa trong Mini-cart)
        [HttpPost]
        public IActionResult RemoveCartAjax(string id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.Masp == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return Json(new { success = true });
        }
        //9. [API] CẬP NHẬT SỐ LƯỢNG AJAX (Dùng cho trang giỏ hàng)
        [HttpPost]
        public IActionResult UpdateQuantityAjax(string id, int quantity)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(p => p.Masp == id);

            if (item != null)
            {
                // Logic: Nếu số lượng <= 0 thì coi như không đổi (hoặc xóa tùy logic, ở đây ta giữ tối thiểu là 1)
                if (quantity > 0)
                {
                    item.Soluong = quantity;
                }
                HttpContext.Session.Set(CART_KEY, cart);

                // Tính toán lại các con số
                var itemTotal = item.ThanhTien; // Thành tiền của riêng món này
                var cartTotal = cart.Sum(x => x.ThanhTien); // Tổng tiền cả giỏ
                var cartCount = cart.Sum(x => x.Soluong); // Tổng số lượng

                return Json(new
                {
                    success = true,
                    itemTotal = itemTotal,
                    cartTotal = cartTotal,
                    cartCount = cartCount
                });
            }

            return Json(new { success = false });
        }
    }
}