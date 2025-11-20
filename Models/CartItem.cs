namespace TraSuaFireFox.Models
{
    public class CartItem
    {
        public string Masp { get; set; }
        public string Tensp { get; set; }
        public string Hinhanh { get; set; }
        public decimal Dongia { get; set; }
        public int Soluong { get; set; }

        // Tính thành tiền = Giá * Số lượng
        public decimal ThanhTien => Dongia * Soluong;
    }
}