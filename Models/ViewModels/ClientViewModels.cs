using System.ComponentModel.DataAnnotations;

namespace TraSuaFireFox.Models.ViewModels
{
    // ViewModel cho Đăng ký
    public class RegisterVM
    {
        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        public string Hoten { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email hoặc SĐT")]
        public string ContactInfo { get; set; } // Nhập chung SĐT hoặc Email

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")]
        public string Matkhau { get; set; }

        [Compare("Matkhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatkhau { get; set; }
    }

    // ViewModel cho Xác thực OTP
    public class VerifyVM
    {
        public string ContactInfo { get; set; } // Email hoặc SĐT cần xác thực

        [Required(ErrorMessage = "Nhập mã OTP")]
        public string OtpCode { get; set; }
    }

    // ViewModel cho Đăng nhập
    public class LoginVM
    {
        [Required(ErrorMessage = "Nhập Email hoặc SĐT")]
        public string Input { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string Password { get; set; }
    }

    // ViewModel cập nhật Profile
    public class ProfileVM
    {
        public string Makh { get; set; }
        public string Hoten { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }
        public string Diachi { get; set; }
    }
}