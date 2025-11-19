using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Nhacungcap
{
    public int Mancc { get; set; }

    public string? Tenncc { get; set; }

    public string? Sdt { get; set; }

    public string? Diachi { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
