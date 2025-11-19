using System;
using System.Collections.Generic;

namespace TraSuaFireFox.Models;

public partial class Nhanvien
{
    public string Manv { get; set; } = null!;

    public string? Hoten { get; set; }

    public string? Email { get; set; }

    public string? Matkhau { get; set; }

    public string? Sdt { get; set; }

    public string? Diachi { get; set; }

    public string? Chucvu { get; set; }

    public decimal? Luongcoban { get; set; }

    public DateOnly? Ngayvaolam { get; set; }

    public string? Trangthai { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
}
