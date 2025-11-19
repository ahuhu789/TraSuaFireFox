using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Loaisp
{
    public string Maloai { get; set; } = null!;

    public string? Tenloai { get; set; }

    public virtual ICollection<Chitietsanpham> Chitietsanphams { get; set; } = new List<Chitietsanpham>();
}
