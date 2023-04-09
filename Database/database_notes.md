# Notes for the database

- ChatLieu
  - MaChatLieu &rarr; nvarchar
  - TenChatLieu &rarr; nvarchar

- ChiTietHoaDon
  - MaHDBan &rarr; nvarchar
  - MaHang &rarr; nvarchar
  - SoLuong &rarr; float ?maybe int
  - DonGia &rarr; float
  - GiamGia &rarr; float
  - ThanhTien &rarr; float

- HoaDonBan
  - MaHDBan &rarr; nvarchar
  - MaNhanVien &rarr; nvarchar
  - NgayBan &rarr; datetime
  - MaKhach &rarr; nvarchar
  - TongTien &rarr; float

- KhachHang
  - MaKhachHang &rarr; nvarchar
  - TenKhachHang &rarr; nvarchar
  - DiaChi &rarr; nvarchar
  - SDT &rarr; nvarchar

- NhanVien
  - MaNhanVien &rarr; nvarchar
  - TenNhanVien &rarr; nvarchar
  - GioiTinh &rarr; nvarchar
  - NgaySinh &rarr; datetime
  - DiaChi &rarr; nvarchar
  - SDT &rarr; nvarchar

- SanPham
  - MaSanPham &rarr; nvarchar
  - TenSanPham &rarr; nvarchar
  - MaChatLieu &rarr; nvarchar
  - SoLuong &rarr; float ?maybe int
  - DonGiaNhap &rarr; float
  - DonGiaBan &rarr; float
  - Anh &rarr; nvarchar
  - GhiChu &rarr; nvarchar