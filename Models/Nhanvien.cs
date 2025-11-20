using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 1. BẮT BUỘC CÓ DÒNG NÀY

namespace TraSuaFireFox.Models;

public partial class Nhanvien
{
    [Key]
    [Display(Name = "Mã Nhân Viên")]
    [Required(ErrorMessage = "Mã nhân viên không được để trống.")]
    [StringLength(6, ErrorMessage = "Mã NV tối đa 6 ký tự.")]
    public string Manv { get; set; } = null!;

    [Display(Name = "Họ và Tên")]
    [Required(ErrorMessage = "Họ tên không được để trống.")]
    [StringLength(35, ErrorMessage = "Họ tên quá dài.")]
    public string? Hoten { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email không được để trống.")]
    [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
    public string? Email { get; set; }

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 - 20 ký tự.")]
    public string? Matkhau { get; set; }

    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Số điện thoại không được để trống.")]
    [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
    public string? Sdt { get; set; }

    [Display(Name = "Địa chỉ")]
    public string? Diachi { get; set; }

    [Display(Name = "Chức vụ")]
    public string? Chucvu { get; set; }

    [Display(Name = "Lương cơ bản")]
    [Range(0, double.MaxValue, ErrorMessage = "Lương không được âm.")]
    public decimal? Luongcoban { get; set; }

    [Display(Name = "Ngày vào làm")]
    public DateOnly? Ngayvaolam { get; set; }

    [Display(Name = "Trạng thái")]
    public string? Trangthai { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}