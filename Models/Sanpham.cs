using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Sanpham
{
    public string Masp { get; set; } = null!;

    public string? Tensp { get; set; }

    public int? Madm { get; set; }

    public int? Mancc { get; set; }

    public string? Mota { get; set; }

    public decimal? Dongia { get; set; }

    public int? Soluongton { get; set; }

    public string? Hinhanh { get; set; }

    public string? Trangthai { get; set; }

    public DateOnly? Ngaytao { get; set; }

    public virtual Chitietsanpham? Chitietsanpham { get; set; }

    public virtual ICollection<Ctdonhang> Ctdonhangs { get; set; } = new List<Ctdonhang>();

    public virtual Danhmuc? MadmNavigation { get; set; }

    public virtual Nhacungcap? ManccNavigation { get; set; }
}
