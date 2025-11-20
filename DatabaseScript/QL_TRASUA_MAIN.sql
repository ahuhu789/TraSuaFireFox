-- ================================================
-- TẠO CSDL QUẢN LÝ TRÀ SỮA
-- ================================================
CREATE DATABASE QL_TRASUA_MAIN
GO
USE QL_TRASUA_MAIN
GO

-- ================================================
-- BẢNG NHÀ CUNG CẤP
-- ================================================
CREATE TABLE NHACUNGCAP
(
    MANCC INT NOT NULL PRIMARY KEY,
    TENNCC NVARCHAR(50),
    SDT CHAR(10),
    DIACHI NVARCHAR(100)
)
INSERT INTO NHACUNGCAP VALUES
(1, N'Trà Sữa TocoToco', '0901111222', N'123 Nguyễn Trãi, Hà Nội'),
(2, N'DingTea Việt Nam', '0902222333', N'25 Nguyễn Huệ, TP.HCM'),
(3, N'GongCha Việt Nam', '0903333444', N'88 Lê Lợi, Đà Nẵng'),
(4, N'Bobapop', '0904444555', N'45 Trần Hưng Đạo, Hà Nội'),
(5, N'RoyalTea', '0905555666', N'72 Hai Bà Trưng, TP.HCM'),
(6, N'Koi Thé', '0906666777', N'11 Phan Chu Trinh, Đà Nẵng'),
(7, N'MilkTeaZone', '0907777888', N'60 Nguyễn Văn Cừ, Cần Thơ'),
(8, N'Trà Sữa House', '0908888999', N'25 Hùng Vương, Huế'),
(9, N'ShakeForYou', '0911111222', N'10 Quang Trung, Nha Trang'),
(10, N'Hot&Cold', '0912222333', N'90 Võ Thị Sáu, Bình Dương');

-- ================================================
-- BẢNG DANH MỤC
-- ================================================
CREATE TABLE DANHMUC
(
    MADM INT NOT NULL PRIMARY KEY,
    TENDM NVARCHAR(35),
    MOTA NVARCHAR(100),
    NGAYTAO DATE
)
INSERT INTO DANHMUC VALUES
(1, N'Trà sữa', N'Các loại trà sữa cổ điển, kem cheese, đặc biệt', '2024-01-01'),
(2, N'Trà', N'Trà nguyên chất, trà ô long, trà xanh, trà đen', '2024-01-10'),
(3, N'Trà trái cây', N'Trà kết hợp trái cây tươi mát, thanh nhiệt', '2024-02-01'),
(4, N'Topping', N'Trân châu, thạch, pudding, kem cheese, v.v.', '2024-03-01'),
(5, N'Combo khuyến mãi', N'Combo giảm giá theo set hoặc theo nhóm', '2024-04-01'),
(6, N'Trà sữa đặc biệt', N'Hương vị cao cấp và độc quyền của quán', '2024-05-01');

-- ================================================
-- BẢNG KHÁCH HÀNG
-- ================================================
CREATE TABLE KHACHHANG
(
    MAKH CHAR(6) NOT NULL PRIMARY KEY,
    HOTEN NVARCHAR(35),
    EMAIL VARCHAR(50),
    SDT CHAR(10),
    DIACHI NVARCHAR(50),
    TRANGTHAI NVARCHAR(20)
)
INSERT INTO KHACHHANG VALUES
('KH0001', N'Nguyễn Văn An', 'an@gmail.com', '0901111222', N'Số 25 Nguyễn Trãi, Quận Thanh Xuân, Hà Nội', N'Hoạt động'),
('KH0002', N'Trần Thị Bình', 'binh@gmail.com', '0902222333', N'120 Lê Lợi, Quận 1, TP.HCM', N'Hoạt động'),
('KH0003', N'Lê Văn Cường', 'cuong@gmail.com', '0903333444', N'45 Trần Phú, Quận Hải Châu, Đà Nẵng', N'Khóa'),
('KH0004', N'Phạm Thị Duyên', 'duyen@gmail.com', '0904444555', N'89 Nguyễn Huệ, TP. Huế, Thừa Thiên Huế', N'Hoạt động'),
('KH0005', N'Hoàng Minh', 'minh@gmail.com', '0905555666', N'56 Mậu Thân, Quận Ninh Kiều, Cần Thơ', N'Hoạt động');

-- ================================================
-- BẢNG NHÂN VIÊN
-- ================================================
CREATE TABLE NHANVIEN
(
    MANV CHAR(6) NOT NULL PRIMARY KEY,
    HOTEN NVARCHAR(35),
    EMAIL VARCHAR(40),
    MATKHAU CHAR(20),
    SDT CHAR(10),
    DIACHI NVARCHAR(40),
    CHUCVU NVARCHAR(20),
    LUONGCOBAN DECIMAL(12,2) CHECK (LUONGCOBAN > 0),
    NGAYVAOLAM DATE,
    TRANGTHAI NVARCHAR(20)
)
INSERT INTO NHANVIEN VALUES
('NV0001', N'Nguyễn Thị Mai', 'mai@trasua.vn', '123456', '0911111111', N'Hà Nội', N'Quản lý', 12000000, '2023-01-05', N'Đang làm'),
('NV0002', N'Lê Văn Nam', 'nam@trasua.vn', '123456', '0922222222', N'TP.HCM', N'Nhân viên', 8000000, '2023-02-10', N'Đang làm'),
('NV0003', N'Phạm Thị Hoa', 'hoa@trasua.vn', '123456', '0933333333', N'Đà Nẵng', N'Thu ngân', 8500000, '2023-03-01', N'Đang làm'),
('NV0004', N'Võ Văn Hưng', 'hung@trasua.vn', '123456', '0944444444', N'Huế', N'Pha chế', 9000000, '2023-03-15', N'Đang làm');

-- ================================================
-- BẢNG SẢN PHẨM (liên kết DANHMUC & NHACUNGCAP)
-- ================================================
CREATE TABLE SANPHAM
(
    MASP CHAR(6) NOT NULL PRIMARY KEY,
    TENSP NVARCHAR(50),
    MADM INT,
    MANCC INT,
    MOTA NVARCHAR(100),
    DONGIA DECIMAL(10,2) CHECK (DONGIA > 0),
    SOLUONGTON INT CHECK (SOLUONGTON >= 0),
    HINHANH NVARCHAR(50),
    TRANGTHAI NVARCHAR(30),
    NGAYTAO DATE,
    FOREIGN KEY (MADM) REFERENCES DANHMUC(MADM),
    FOREIGN KEY (MANCC) REFERENCES NHACUNGCAP(MANCC)
)
INSERT INTO SANPHAM VALUES
-- Trà sữa
('SP0001', N'Trà sữa truyền thống', 1, 1, N'Trà sữa vị cổ điển, hương thơm tự nhiên', 30000, 100, 'h1.jpg', N'Còn hàng', '2024-06-01'),
('SP0002', N'Trà sữa matcha', 1, 2, N'Hương vị matcha Nhật Bản thanh mát', 40000, 80, 'h2.jpg', N'Còn hàng', '2024-06-02'),
('SP0003', N'Trà sữa caramel', 1, 3, N'Hòa quyện vị caramel ngọt ngào và béo ngậy', 46000, 70, 'h3.jpg', N'Còn hàng', '2024-06-03'),
('SP0004', N'Trà sữa khoai môn', 1, 4, N'Màu tím đẹp mắt, vị ngọt thanh', 42000, 65, 'h4.jpg', N'Còn hàng', '2024-06-04'),
('SP0005', N'Trà sữa phô mai kem cheese', 1, 5, N'Béo ngậy với lớp kem cheese mềm mịn', 50000, 90, 'h5.jpg', N'Còn hàng', '2024-06-05'),

-- Trà
('SP0006', N'Trà đào', 2, 6, N'Trà đào tươi mát, hương thơm tự nhiên', 38000, 70, 'h6.jpg', N'Còn hàng', '2024-06-06'),
('SP0007', N'Trà dâu', 2, 7, N'Trà dâu thơm ngọt, giải nhiệt ngày hè', 37000, 60, 'h7.jpg', N'Còn hàng', '2024-06-07'),
('SP0008', N'Trà chanh', 2, 8, N'Trà chanh tươi mát, vị thanh dịu', 35000, 75, 'h8.jpg', N'Còn hàng', '2024-06-08'),
('SP0009', N'Hồng trà', 2, 9, N'Hồng trà đậm vị, hương thơm quyến rũ', 39000, 85, 'h9.jpg', N'Còn hàng', '2024-06-09'),
('SP0010', N'Trà ô long hương nhài', 2, 10, N'Trà ô long kết hợp hương nhài dịu nhẹ', 42000, 70, 'h10.jpg', N'Còn hàng', '2024-06-10'),

-- Trà trái cây
('SP0011', N'Trà xoài nhiệt đới', 3, 1, N'Trà kết hợp xoài tươi, vị ngọt mát', 45000, 60, 'h11.jpg', N'Còn hàng', '2024-06-11'),
('SP0012', N'Trà cam sả', 3, 2, N'Trà cam sả thanh nhiệt, giải khát mùa hè', 43000, 65, 'h12.jpg', N'Còn hàng', '2024-06-12'),
('SP0013', N'Trà vải', 3, 3, N'Trà vải ngọt dịu, hương thơm nhẹ nhàng', 44000, 70, 'h13.jpg', N'Còn hàng', '2024-06-13'),
('SP0014', N'Trà dưa lưới', 3, 4, N'Trà dưa lưới thơm mát, vị thanh nhẹ', 45000, 60, 'h14.jpg', N'Còn hàng', '2024-06-14'),
('SP0015', N'Trà tắc mật ong', 3, 5, N'Trà kết hợp vị tắc và mật ong tươi', 42000, 65, 'h15.jpg', N'Còn hàng', '2024-06-15'),

-- Topping
('SP0016', N'Trân châu đen', 4, 6, N'Trân châu dai ngon truyền thống', 10000, 150, 'h16.jpg', N'Còn hàng', '2024-06-16'),
('SP0017', N'Thạch trái cây', 4, 7, N'Thạch nhiều màu, vị trái cây tự nhiên', 12000, 180, 'h17.jpg', N'Còn hàng', '2024-06-17'),
('SP0018', N'Pudding trứng', 4, 8, N'Béo thơm mềm mịn, dùng kèm trà sữa', 15000, 160, 'h18.jpg', N'Còn hàng', '2024-06-18'),

-- Combo khuyến mãi
('SP0019', N'Combo 2 ly trà sữa bất kỳ', 5, 9, N'Giảm 10% khi mua 2 ly', 72000, 50, 'h19.jpg', N'Còn hàng', '2024-06-19'),

-- Trà sữa đặc biệt
('SP0020', N'Trà sữa hoàng gia', 6, 10, N'Hương vị cao cấp, công thức độc quyền', 55000, 60, 'h20.jpg', N'Còn hàng', '2024-06-20');

-- ================================================
-- CHI TIẾT SẢN PHẨM (liên kết DANHMUC)
-- ================================================
CREATE TABLE CHITIETSANPHAM
(
    MASP CHAR(6) NOT NULL PRIMARY KEY,
    SIZE NVARCHAR(10),
    DUONG NVARCHAR(10),
    DA NVARCHAR(10),
    MADM INT NOT NULL,
    SOLUONGTONCT INT CHECK (SOLUONGTONCT >= 0),
    DONGIACT DECIMAL(10,2) CHECK (DONGIACT > 0),
    HINHANHCT NVARCHAR(100),
    TRANGTHAICT NVARCHAR(30),
    FOREIGN KEY (MADM) REFERENCES DANHMUC(MADM),
    FOREIGN KEY (MASP) REFERENCES SANPHAM(MASP)
)
INSERT INTO CHITIETSANPHAM VALUES
('SP0001', N'Lớn', N'70%', N'50%', 1, 50, 30000, 'h1.jpg', N'Còn hàng'),
('SP0002', N'Lớn', N'50%', N'50%', 1, 45, 40000, 'h2.jpg', N'Còn hàng'),
('SP0003', N'Lớn', N'50%', N'50%', 1, 40, 46000, 'h3.jpg', N'Còn hàng'),
('SP0004', N'Vừa', N'50%', N'50%', 1, 35, 42000, 'h4.jpg', N'Còn hàng'),
('SP0005', N'Lớn', N'50%', N'50%', 1, 60, 50000, 'h5.jpg', N'Còn hàng'),
('SP0006', N'Lớn', N'0%', N'50%', 2, 45, 38000, 'h6.jpg', N'Còn hàng'),
('SP0007', N'Lớn', N'0%', N'50%', 2, 50, 37000, 'h7.jpg', N'Còn hàng'),
('SP0008', N'Lớn', N'0%', N'50%', 2, 55, 35000, 'h8.jpg', N'Còn hàng'),
('SP0009', N'Lớn', N'0%', N'50%', 2, 60, 39000, 'h9.jpg', N'Còn hàng'),
('SP0010', N'Lớn', N'0%', N'50%', 2, 50, 42000, 'h10.jpg', N'Còn hàng'),
('SP0011', N'Lớn', N'50%', N'50%', 3, 45, 45000, 'h11.jpg', N'Còn hàng'),
('SP0012', N'Lớn', N'50%', N'50%', 3, 40, 43000, 'h12.jpg', N'Còn hàng'),
('SP0013', N'Lớn', N'50%', N'50%', 3, 35, 44000, 'h13.jpg', N'Còn hàng'),
('SP0014', N'Lớn', N'50%', N'50%', 3, 30, 45000, 'h14.jpg', N'Còn hàng'),
('SP0015', N'Lớn', N'50%', N'50%', 3, 35, 42000, 'h15.jpg', N'Còn hàng'),
('SP0016', N'Nhỏ', N'0%', N'0%', 4, 120, 10000, 'h16.jpg', N'Còn hàng'),
('SP0017', N'Nhỏ', N'0%', N'0%', 4, 130, 12000, 'h17.jpg', N'Còn hàng'),
('SP0018', N'Nhỏ', N'0%', N'0%', 4, 125, 15000, 'h18.jpg', N'Còn hàng'),
('SP0019', N'Lớn', N'50%', N'50%', 5, 40, 72000, 'h19.jpg', N'Còn hàng'),
('SP0020', N'Lớn', N'50%', N'50%', 6, 30, 55000, 'h20.jpg', N'Còn hàng');

-- ================================================
-- BẢNG ĐƠN HÀNG
-- ================================================
CREATE TABLE DONHANG
(
    MADH CHAR(6) NOT NULL PRIMARY KEY,
    MAKH CHAR(6) NOT NULL,
    MANV CHAR(6) NOT NULL,
    NGAYDATHANG DATE,
    TRANGTHAIDH NVARCHAR(30),
    GHICHU NVARCHAR(100),
    DIACHIGIAOHANG NVARCHAR(100),
    SDTGIAOHANG CHAR(10),
    NGAYGIAOHANGDUKIEN DATE,
    FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH),
    FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV)
)
INSERT INTO DONHANG VALUES
('DH0001', 'KH0001', 'NV0002', '2024-07-01', N'Đã giao', N'Giao tận nơi', N'Số 25 Nguyễn Trãi, Quận Thanh Xuân, Hà Nội', '0901111222', '2024-07-02'),
('DH0002', 'KH0002', 'NV0003', '2024-07-02', N'Đang giao', N'Giao nhanh', N'Số 120 Lê Lợi, Quận 1, TP. Hồ Chí Minh', '0902222333', '2024-07-03'),
('DH0003', 'KH0003', 'NV0001', '2024-07-03', N'Đã giao', N'Khách quen', N'Số 45 Trần Phú, Quận Hải Châu, Đà Nẵng', '0903333444', '2024-07-04');

-- ================================================
-- CHI TIẾT ĐƠN HÀNG
-- ================================================
CREATE TABLE CTDONHANG
(
    MADH CHAR(6) NOT NULL,
    MASP CHAR(6) NOT NULL,
    SOLUONG INT CHECK (SOLUONG > 0),
    DONGIA DECIMAL(10,2) CHECK (DONGIA > 0),
    THANHTIEN DECIMAL(12,2),
    PRIMARY KEY (MADH, MASP),
    FOREIGN KEY (MADH) REFERENCES DONHANG(MADH),
    FOREIGN KEY (MASP) REFERENCES SANPHAM(MASP)
)
INSERT INTO CTDONHANG VALUES
('DH0001', 'SP0001', 2, 30000, 60000),
('DH0002', 'SP0002', 1, 40000, 40000),
('DH0003', 'SP0003', 3, 45000, 135000);

-- ================================================
-- BẢNG THANH TOÁN
-- ================================================
CREATE TABLE THANHTOAN
(
    MAGD CHAR(6) NOT NULL PRIMARY KEY,
    MADH CHAR(6) NOT NULL,
    PHUONGTHUCTHANHTOAN NVARCHAR(20),
    TONGTIEN DECIMAL(12,2),
    NGAYTHANHTOAN DATE,
    TRANGTHAITHANHTOAN NVARCHAR(30),
    GHICHU NVARCHAR(100),
    FOREIGN KEY (MADH) REFERENCES DONHANG(MADH)
)
INSERT INTO THANHTOAN VALUES
('TT0001', 'DH0001', N'Tiền mặt', 60000, '2024-07-02', N'Hoàn tất', N'Cảm ơn quý khách'),
('TT0002', 'DH0002', N'Chuyển khoản', 40000, '2024-07-03', N'Hoàn tất', N'Đã thanh toán online'),
('TT0003', 'DH0003', N'Tiền mặt', 135000, '2024-07-04', N'Hoàn tất', N'Đã giao hàng');
GO
