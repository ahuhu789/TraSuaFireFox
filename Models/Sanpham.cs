using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 1. Thêm dòng này

namespace TraSuaFireFox.Models;

public partial class Sanpham
{

    [Key] 
    [Display(Name = "Mã Sản Phẩm")] 
    [Required(ErrorMessage = "Mã sản phẩm không được để trống.")] 
    [StringLength(6, ErrorMessage = "Mã SP tối đa 6 ký tự.")] 

    public string Masp { get; set; } = null!;

    [Display(Name = "Tên Sản Phẩm")]
    [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
    public string? Tensp { get; set; }

    [Display(Name = "Danh mục")]
    [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
    public int? Madm { get; set; }

    [Display(Name = "Nhà cung cấp")]
    [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp.")]
    public int? Mancc { get; set; }

    [Display(Name = "Mô tả")]
    public string? Mota { get; set; }

    [Display(Name = "Đơn giá")]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0.")]
    public decimal? Dongia { get; set; }

    [Display(Name = "Số lượng tồn")]
    [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm.")]
    public int? Soluongton { get; set; }

    public string? Hinhanh { get; set; }

    public string? Trangthai { get; set; }

    public DateOnly? Ngaytao { get; set; }

    public virtual Chitietsanpham? Chitietsanpham { get; set; }

    public virtual ICollection<Ctdonhang> Ctdonhangs { get; set; } = new List<Ctdonhang>();

    public virtual Danhmuc? MadmNavigation { get; set; }

    public virtual Nhacungcap? ManccNavigation { get; set; }
}