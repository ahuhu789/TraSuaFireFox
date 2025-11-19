using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Khachhang
{
    public string Makh { get; set; } = null!;

    public string? Hoten { get; set; }

    public string? Email { get; set; }

    public string? Sdt { get; set; }

    public string? Diachi { get; set; }

    public string? Trangthai { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}
