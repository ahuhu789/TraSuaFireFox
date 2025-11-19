using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Chitietsanpham
{
    public string Masp { get; set; } = null!;

    public string? Size { get; set; }

    public string? Duong { get; set; }

    public string? Da { get; set; }

    public string Maloai { get; set; } = null!;

    public int? Soluongtonct { get; set; }

    public decimal? Dongiact { get; set; }

    public string? Hinhanhct { get; set; }

    public string? Trangthaict { get; set; }

    public virtual Loaisp MaloaiNavigation { get; set; } = null!;

    public virtual Sanpham MaspNavigation { get; set; } = null!;
}
