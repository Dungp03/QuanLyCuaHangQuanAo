using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ClothesStoreManagement {
    public enum Table {
        ChatLieu,
        ChiTietHoaDon,
        HoaDonBan,
        KhachHang,
        NhanVien,
        SanPham
    }
    public class MenuUtils {

        static readonly MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;

        public static void HideAllMenu() {
            foreach (var element in mainWindow.grid.Children) {
                if (element is Label l) {
                    if (l.Name == "labelChooseTable")
                        continue;
                    l.Visibility = Visibility.Hidden;
                }
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Visibility = Visibility.Hidden;
                }
            }
        }
        public static void UnloadAllFields() {
            foreach (var element in mainWindow.grid.Children) {
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Text = string.Empty;
                }
            }
        }
        public static void LoadMenu( Table? table ) {
            if (mainWindow.connection.State != ConnectionState.Open || table == null)
                return;
            switch (table) {
                case Table.ChatLieu:
                    HideAllMenu();
                    mainWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.labelTenChatLieu.Visibility = Visibility.Visible;

                    mainWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.textboxTenChatLieu.Visibility = Visibility.Visible;
                    break;
                case Table.ChiTietHoaDon:
                    HideAllMenu();
                    mainWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaHang.Visibility = Visibility.Visible;
                    mainWindow.labelSoLuong.Visibility = Visibility.Visible;
                    mainWindow.labelDonGia.Visibility = Visibility.Visible;
                    mainWindow.labelGiamGia.Visibility = Visibility.Visible;
                    mainWindow.labelThanhTien.Visibility = Visibility.Visible;

                    mainWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaHang.Visibility = Visibility.Visible;
                    mainWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGia.Visibility = Visibility.Visible;
                    mainWindow.textboxGiamGia.Visibility = Visibility.Visible;
                    mainWindow.textboxThanhTien.Visibility = Visibility.Visible;
                    break;
                case Table.HoaDonBan:
                    HideAllMenu();
                    mainWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelNgayBan.Visibility = Visibility.Visible;
                    mainWindow.labelMaKhach.Visibility = Visibility.Visible;
                    mainWindow.labelTongTien.Visibility = Visibility.Visible;

                    mainWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxNgayBan.Visibility = Visibility.Visible;
                    mainWindow.textboxMaKhach.Visibility = Visibility.Visible;
                    mainWindow.textboxTongTien.Visibility = Visibility.Visible;
                    break;
                case Table.KhachHang:
                    HideAllMenu();
                    mainWindow.labelMaKhachHang.Visibility = Visibility.Visible;
                    mainWindow.labelTenKhachHang.Visibility = Visibility.Visible;
                    mainWindow.labelDiaChiKH.Visibility = Visibility.Visible;
                    mainWindow.labelSDTKH.Visibility = Visibility.Visible;

                    mainWindow.textboxMaKhachHang.Visibility = Visibility.Visible;
                    mainWindow.textboxTenKhachHang.Visibility = Visibility.Visible;
                    mainWindow.textboxDiaChiKH.Visibility = Visibility.Visible;
                    mainWindow.textboxSDTKH.Visibility = Visibility.Visible;
                    break;
                case Table.NhanVien:
                    HideAllMenu();
                    mainWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelTenNhanVien.Visibility = Visibility.Visible;
                    mainWindow.labelDiaChiNV.Visibility = Visibility.Visible;
                    mainWindow.labelSDTNV.Visibility = Visibility.Visible;
                    mainWindow.labelNgaySinh.Visibility = Visibility.Visible;
                    mainWindow.labelGioiTinh.Visibility = Visibility.Visible;

                    mainWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxTenNhanVien.Visibility = Visibility.Visible;
                    mainWindow.textboxDiaChiNV.Visibility = Visibility.Visible;
                    mainWindow.textboxSDTNV.Visibility = Visibility.Visible;
                    mainWindow.textboxNgaySinh.Visibility = Visibility.Visible;
                    mainWindow.textboxGioiTinh.Visibility = Visibility.Visible;
                    break;
                case Table.SanPham:
                    HideAllMenu();
                    mainWindow.labelMaSanPham.Visibility = Visibility.Visible;
                    mainWindow.labelTenSanPham.Visibility = Visibility.Visible;
                    mainWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.labelSoLuong.Visibility = Visibility.Visible;
                    mainWindow.labelDonGiaNhap.Visibility = Visibility.Visible;
                    mainWindow.labelDonGiaBan.Visibility = Visibility.Visible;
                    mainWindow.labelGhiChu.Visibility = Visibility.Visible;

                    mainWindow.textboxMaSanPham.Visibility = Visibility.Visible;
                    mainWindow.textboxTenSanPham.Visibility = Visibility.Visible;
                    mainWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    mainWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGiaNhap.Visibility = Visibility.Visible;
                    mainWindow.textboxDonGiaBan.Visibility = Visibility.Visible;
                    mainWindow.textboxGhiChu.Visibility = Visibility.Visible;
                    break;
            }
        }

        public static void LoadFields( Table? table, DataRowView row ) {
            try {
                switch (table) {
                    case Table.ChatLieu:
                        mainWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        mainWindow.textboxTenChatLieu.Text = row["TenChatLieu"].ToString();
                        break;
                    case Table.ChiTietHoaDon:
                        mainWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        mainWindow.textboxMaHang.Text = row["MaHang"].ToString();
                        mainWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        mainWindow.textboxDonGia.Text = row["DonGia"].ToString();
                        mainWindow.textboxGiamGia.Text = row["GiamGia"].ToString();
                        mainWindow.textboxThanhTien.Text = row["ThanhTien"].ToString();
                        break;
                    case Table.HoaDonBan:
                        mainWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        mainWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        mainWindow.textboxNgayBan.Text = row["NgayBan"].ToString();
                        mainWindow.textboxMaKhach.Text = row["MaKhach"].ToString();
                        mainWindow.textboxTongTien.Text = row["TongTien"].ToString();
                        break;
                    case Table.KhachHang:
                        mainWindow.textboxMaKhachHang.Text = row["MaKhachhang"].ToString();
                        mainWindow.textboxTenKhachHang.Text = row["TenKhachHang"].ToString();
                        mainWindow.textboxDiaChiKH.Text = row["DiaChi"].ToString();
                        mainWindow.textboxSDTKH.Text = row["SDT"].ToString();
                        break;
                    case Table.NhanVien:
                        mainWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        mainWindow.textboxTenNhanVien.Text = row["TenNhanVien"].ToString();
                        mainWindow.textboxDiaChiNV.Text = row["DiaChi"].ToString();
                        mainWindow.textboxSDTNV.Text = row["SDT"].ToString();
                        mainWindow.textboxNgaySinh.Text = row["NgaySinh"].ToString();
                        mainWindow.textboxGioiTinh.Text = row["GioiTinh"].ToString();
                        break;
                    case Table.SanPham:
                        mainWindow.textboxMaSanPham.Text = row["MaSanPham"].ToString();
                        mainWindow.textboxTenSanPham.Text = row["TenSanPham"].ToString();
                        mainWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        mainWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        mainWindow.textboxDonGiaNhap.Text = row["DonGiaNhap"].ToString();
                        mainWindow.textboxDonGiaBan.Text = row["DonGiaBan"].ToString();
                        mainWindow.textboxGhiChu.Text = row["GhiChu"].ToString();
                        break;
                }
            }
            catch (System.Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
