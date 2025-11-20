using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TraSuaFireFox.Models;

public partial class Ctdonhang
{
    [Key]
    public string Madh { get; set; } = null!;

    public string Masp { get; set; } = null!;

    public int? Soluong { get; set; }

    public decimal? Dongia { get; set; }

    public decimal? Thanhtien { get; set; }

    public virtual Donhang MadhNavigation { get; set; } = null!;

    public virtual Sanpham MaspNavigation { get; set; } = null!;
}
