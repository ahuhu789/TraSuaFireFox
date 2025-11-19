using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TraSuaFireFox.Models;

public partial class QlTrasuaMainContext : DbContext
{
    public QlTrasuaMainContext()
    {
    }

    public QlTrasuaMainContext(DbContextOptions<QlTrasuaMainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietsanpham> Chitietsanphams { get; set; }

    public virtual DbSet<Ctdonhang> Ctdonhangs { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Loaisp> Loaisps { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Thanhtoan> Thanhtoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HUYTHANH;Initial Catalog=QL_TRASUA_MAIN;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitietsanpham>(entity =>
        {
            entity.HasKey(e => e.Masp).HasName("PK__CHITIETS__60228A3222BCBAAA");

            entity.ToTable("CHITIETSANPHAM");

            entity.Property(e => e.Masp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Da)
                .HasMaxLength(10)
                .HasColumnName("DA");
            entity.Property(e => e.Dongiact)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DONGIACT");
            entity.Property(e => e.Duong)
                .HasMaxLength(10)
                .HasColumnName("DUONG");
            entity.Property(e => e.Hinhanhct)
                .HasMaxLength(100)
                .HasColumnName("HINHANHCT");
            entity.Property(e => e.Maloai)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MALOAI");
            entity.Property(e => e.Size)
                .HasMaxLength(10)
                .HasColumnName("SIZE");
            entity.Property(e => e.Soluongtonct).HasColumnName("SOLUONGTONCT");
            entity.Property(e => e.Trangthaict)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAICT");

            entity.HasOne(d => d.MaloaiNavigation).WithMany(p => p.Chitietsanphams)
                .HasForeignKey(d => d.Maloai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETSA__MALOA__44FF419A");

            entity.HasOne(d => d.MaspNavigation).WithOne(p => p.Chitietsanpham)
                .HasForeignKey<Chitietsanpham>(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETSAN__MASP__45F365D3");
        });

        modelBuilder.Entity<Ctdonhang>(entity =>
        {
            entity.HasKey(e => new { e.Madh, e.Masp }).HasName("PK__CTDONHAN__563D28E45745C3E1");

            entity.ToTable("CTDONHANG");

            entity.Property(e => e.Madh)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MADH");
            entity.Property(e => e.Masp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Dongia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DONGIA");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");
            entity.Property(e => e.Thanhtien)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("THANHTIEN");

            entity.HasOne(d => d.MadhNavigation).WithMany(p => p.Ctdonhangs)
                .HasForeignKey(d => d.Madh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTDONHANG__MADH__4CA06362");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.Ctdonhangs)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CTDONHANG__MASP__4D94879B");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.Madm).HasName("PK__DANHMUC__603F005C70B3CCD9");

            entity.ToTable("DANHMUC");

            entity.Property(e => e.Madm)
                .ValueGeneratedNever()
                .HasColumnName("MADM");
            entity.Property(e => e.Mota)
                .HasMaxLength(100)
                .HasColumnName("MOTA");
            entity.Property(e => e.Ngaytao).HasColumnName("NGAYTAO");
            entity.Property(e => e.Tendm)
                .HasMaxLength(35)
                .HasColumnName("TENDM");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.Madh).HasName("PK__DONHANG__603F0047218959DF");

            entity.ToTable("DONHANG");

            entity.Property(e => e.Madh)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MADH");
            entity.Property(e => e.Diachigiaohang)
                .HasMaxLength(100)
                .HasColumnName("DIACHIGIAOHANG");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(100)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Makh)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAKH");
            entity.Property(e => e.Manv)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Ngaydathang).HasColumnName("NGAYDATHANG");
            entity.Property(e => e.Ngaygiaohangdukien).HasColumnName("NGAYGIAOHANGDUKIEN");
            entity.Property(e => e.Sdtgiaohang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDTGIAOHANG");
            entity.Property(e => e.Trangthaidh)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAIDH");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONHANG__MAKH__48CFD27E");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DONHANG__MANV__49C3F6B7");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh).HasName("PK__KHACHHAN__603F592CDC8B40AB");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAKH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(50)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(35)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(20)
                .HasColumnName("TRANGTHAI");
        });

        modelBuilder.Entity<Loaisp>(entity =>
        {
            entity.HasKey(e => e.Maloai).HasName("PK__LOAISP__2F633F23A6A4C96E");

            entity.ToTable("LOAISP");

            entity.Property(e => e.Maloai)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MALOAI");
            entity.Property(e => e.Tenloai)
                .HasMaxLength(30)
                .HasColumnName("TENLOAI");
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.Mancc).HasName("PK__NHACUNGC__7ABEA582E0BDF3BA");

            entity.ToTable("NHACUNGCAP");

            entity.Property(e => e.Mancc)
                .ValueGeneratedNever()
                .HasColumnName("MANCC");
            entity.Property(e => e.Diachi)
                .HasMaxLength(100)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.Tenncc)
                .HasMaxLength(50)
                .HasColumnName("TENNCC");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F511483BEDB99");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MANV");
            entity.Property(e => e.Chucvu)
                .HasMaxLength(20)
                .HasColumnName("CHUCVU");
            entity.Property(e => e.Diachi)
                .HasMaxLength(40)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hoten)
                .HasMaxLength(35)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Luongcoban)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("LUONGCOBAN");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MATKHAU");
            entity.Property(e => e.Ngayvaolam).HasColumnName("NGAYVAOLAM");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(20)
                .HasColumnName("TRANGTHAI");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.Masp).HasName("PK__SANPHAM__60228A32DDAD4231");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.Masp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MASP");
            entity.Property(e => e.Dongia)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("DONGIA");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(50)
                .HasColumnName("HINHANH");
            entity.Property(e => e.Madm).HasColumnName("MADM");
            entity.Property(e => e.Mancc).HasColumnName("MANCC");
            entity.Property(e => e.Mota)
                .HasMaxLength(100)
                .HasColumnName("MOTA");
            entity.Property(e => e.Ngaytao).HasColumnName("NGAYTAO");
            entity.Property(e => e.Soluongton).HasColumnName("SOLUONGTON");
            entity.Property(e => e.Tensp)
                .HasMaxLength(50)
                .HasColumnName("TENSP");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MadmNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Madm)
                .HasConstraintName("FK__SANPHAM__MADM__3F466844");

            entity.HasOne(d => d.ManccNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Mancc)
                .HasConstraintName("FK__SANPHAM__MANCC__403A8C7D");
        });

        modelBuilder.Entity<Thanhtoan>(entity =>
        {
            entity.HasKey(e => e.Magd).HasName("PK__THANHTOA__603F38AF31F4F621");

            entity.ToTable("THANHTOAN");

            entity.Property(e => e.Magd)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MAGD");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(100)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Madh)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MADH");
            entity.Property(e => e.Ngaythanhtoan).HasColumnName("NGAYTHANHTOAN");
            entity.Property(e => e.Phuongthucthanhtoan)
                .HasMaxLength(20)
                .HasColumnName("PHUONGTHUCTHANHTOAN");
            entity.Property(e => e.Tongtien)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("TONGTIEN");
            entity.Property(e => e.Trangthaithanhtoan)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAITHANHTOAN");

            entity.HasOne(d => d.MadhNavigation).WithMany(p => p.Thanhtoans)
                .HasForeignKey(d => d.Madh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__THANHTOAN__MADH__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
