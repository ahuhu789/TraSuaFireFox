using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraSuaFireFox.Models;

public partial class Thanhtoan
{
    [Key]
    public string Magd { get; set; } = null!;

    public string Madh { get; set; } = null!;

    public string? Phuongthucthanhtoan { get; set; }

    public decimal? Tongtien { get; set; }

    public DateOnly? Ngaythanhtoan { get; set; }

    public string? Trangthaithanhtoan { get; set; }

    public string? Ghichu { get; set; }

    public virtual Donhang MadhNavigation { get; set; } = null!;
}
