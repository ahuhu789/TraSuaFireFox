using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Danhmuc
{
    public int Madm { get; set; }

    public string? Tendm { get; set; }

    public string? Mota { get; set; }

    public DateOnly? Ngaytao { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
