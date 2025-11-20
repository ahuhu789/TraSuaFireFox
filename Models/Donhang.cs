using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraSuaFireFox.Models;

public partial class Donhang
{
    [Key]

    public string Madh { get; set; } = null!;

    public string Makh { get; set; } = null!;

    public string Manv { get; set; } = null!;

    public DateOnly? Ngaydathang { get; set; }

    public string? Trangthaidh { get; set; }

    public string? Ghichu { get; set; }

    public string? Diachigiaohang { get; set; }

    public string? Sdtgiaohang { get; set; }

    public DateOnly? Ngaygiaohangdukien { get; set; }

    public virtual ICollection<Ctdonhang> Ctdonhangs { get; set; } = new List<Ctdonhang>();

    public virtual Khachhang MakhNavigation { get; set; } = null!;

    public virtual Nhanvien ManvNavigation { get; set; } = null!;

    public virtual ICollection<Thanhtoan> Thanhtoans { get; set; } = new List<Thanhtoan>();
}
