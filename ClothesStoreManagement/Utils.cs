using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Markup;

namespace ClothesStoreManagement {
    public enum Table {
        ChatLieu,
        ChiTietHoaDon,
        HoaDonBan,
        KhachHang,
        NhanVien,
        SanPham
    }
    public class Utils {
        public static DatabaseWindow databaseWindow = null;
        //public static DatabaseWindow databaseWindow = ( (DatabaseWindow) Application.Current.MainWindow ).databaseWindow;
        //static readonly MainWindow databaseWindow = (MainWindow) Application.Current.MainWindow;

        public static void HideAllMenu() {
            foreach (var element in databaseWindow.grid.Children) {
                if (element is Label l) {
                    if (l.Name == "labelChooseTable")
                        continue;
                    l.Visibility = Visibility.Collapsed;
                }
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Visibility = Visibility.Collapsed;
                }
            }
        }
        public static void UnloadAllFields() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is TextBox) {
                    TextBox tb = element as TextBox;
                    tb.Text = string.Empty;
                }
        }
        public static void DisableAllFields() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is TextBox tb)
                    tb.IsReadOnly = true;
        }
        public static void EnableAllFields() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is TextBox tb)
                    tb.IsReadOnly = false;
        }
        public static void HideButtons() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is Button bt)
                    bt.Visibility = Visibility.Collapsed;
        }
        public static void EnableButtons() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is Button bt)
                    bt.IsEnabled = true;
        }
        public static void DisableButtons() {
            foreach (var element in databaseWindow.grid.Children)
                if (element is Button bt)
                    if (bt.Name.Contains("Modify") || bt.Name.Contains("Delete"))
                        bt.IsEnabled = false;
        }
        public static void ChangeButtonState( bool IsEditing ) {
            if (IsEditing) {
                databaseWindow.buttonInsert.Visibility = Visibility.Collapsed;
                databaseWindow.buttonModify.Visibility = Visibility.Collapsed;
                databaseWindow.buttonDelete.Visibility = Visibility.Collapsed;

                databaseWindow.buttonConfirm.Visibility = Visibility.Visible;
                databaseWindow.buttonCancel.Visibility = Visibility.Visible;
            }
            else {
                databaseWindow.buttonInsert.Visibility = Visibility.Visible;
                databaseWindow.buttonModify.Visibility = Visibility.Visible;
                databaseWindow.buttonDelete.Visibility = Visibility.Visible;

                databaseWindow.buttonConfirm.Visibility = Visibility.Collapsed;
                databaseWindow.buttonCancel.Visibility = Visibility.Collapsed;
            }
        }
        public static bool IsUsernameValid( string username, DataRow[] data ) {
            if (username == string.Empty || username == "Username") {
                MessageBox.Show("Tên đăng nhập không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (username.Length > 75) {
                MessageBox.Show("Tên đăng nhập quá dài", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            foreach (var item in data)
                if (username == item[1].ToString()) {
                    MessageBox.Show("Tên này đã có người sử dụng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            return true;
        }
        public static bool IsPasswordValid( string password ) { // for signing up
            if (password == string.Empty) {
                MessageBox.Show("Mật khẩu không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (password.Length < 8) {
                MessageBox.Show("Mật khẩu quá ngắn", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (password.Length > 30) {
                MessageBox.Show("Mật khẩu quá dài", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (password.IndexOfAny(new char[] { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '{', '[', '}', ']', '|', '\\', ':', ';', '\'', '"', '<', ',', '>', '.', '?', '/' }) == -1) {
                MessageBox.Show("Mật khẩu phải chứa ký tự đặc biệt", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            if (password.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }) == -1) {
                MessageBox.Show("Mật khẩu phải chứa số", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }
        private static bool IsKeyPresent( string[] keys ) {
            foreach ( var key in keys )
                if (key == string.Empty)
                    return false;
            return true;
        }
        private static string VerifyDate( string date ) {
            bool error = false;
            bool isCheckingDay = true, isCheckingMonth = false, isCheckingYear = false;
            string dayString = "", monthString = "", yearString = "";
            string formattedDate = "";
            while (!error) {
                foreach (var c in date) {
                    if (int.TryParse(c.ToString(), out int num)) {
                        if (isCheckingDay)
                            dayString += c;
                        if (isCheckingMonth)
                            monthString += c;
                        if (isCheckingYear)
                            yearString += c;
                        formattedDate += c;
                    }
                    else {
                        if (isCheckingDay) { isCheckingDay = false; isCheckingMonth = true; }
                        else if (isCheckingMonth) { isCheckingMonth = false; isCheckingYear = true; }
                        else isCheckingYear = false;
                        formattedDate += '/';
                    }
                }
                // check format
                int countSeparator = 0;
                foreach (var c in formattedDate) {
                    if (c.ToString().CompareTo("/") == 0)
                        countSeparator++;
                }
                if (countSeparator != 2) {
                    error = true;
                    break;
                }
                // check time validity
                int day = int.Parse(dayString), month = int.Parse(monthString), year = int.Parse(yearString);
                if (month < 1 || month > 12 || day < 1 || year < 1 || year > 10000 ||
                    ( month == 2 && ( ( year % 4 == 0 && day > 29 ) || ( year % 4 != 0 && day > 28 ) ) ) ||
                    ( ( ( month < 8 && month % 2 != 0 ) || ( month > 7 && month % 2 == 0 ) ) && day > 31 ) ||
                    ( ( ( month < 8 && month % 2 == 0 ) || ( month > 7 && month % 2 != 0 ) ) && day > 30 )) {
                    error = true;
                    break;
                }
                break;
            }
            if (error)
                throw new DateFormatException("Wrong date format");
            return formattedDate;
        }
        public static void InsertToDatabase( Table? table, string[] data, bool isNew ) {
            string updateStatement = "";
            try {
                if (isNew) {
                    switch (table) {
                        case Table.ChatLieu:
                            if (IsKeyPresent(new string[] { data[0] }))
                                updateStatement += $"(MaChatLieu, TenChatlieu) values " +
                                    $"(N'{data[0]}',N'{data[1]}')";
                            break;
                        case Table.ChiTietHoaDon:
                            if (IsKeyPresent(new string[] { data[0], data[1] }))
                                updateStatement += $"(MaHDBan,MaHang,SoLuong,DonGia,GiamGia,ThanhTien) values " +
                                    $"(N'{data[0]}',N'{data[1]}',{data[2]},{data[3]},{data[4]},{data[5]})";
                            break;
                        case Table.HoaDonBan:
                            if (IsKeyPresent(new string[] { data[0] }))
                                updateStatement += $"(MaHDBan,MaNhanVien,NgayBan,MaKhach,TongTien) values " +
                                    $"(N'{data[0]}',N'{data[1]}',N'{VerifyDate(data[2])}',N'{data[3]}',{data[4]})";
                            break;
                        case Table.KhachHang:
                            if (IsKeyPresent(new string[] { data[0] }))
                                updateStatement += $"(MaKhachHang,TenKhachHang,DiaChi,SDT) values " +
                                $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',N'{data[3]}')";
                            break;
                        case Table.NhanVien:
                            if (IsKeyPresent(new string[] { data[0] }))
                                updateStatement += $"(MaNhanVien,TenNhanVien,DiaChi,SDT,NgaySinh,GioiTinh) values " +
                                    $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',N'{data[3]}',N'{VerifyDate(data[4])}',N'{data[5]}')";

                            break;
                        case Table.SanPham:
                            if (IsKeyPresent(new string[] { data[0] }))
                                updateStatement += $"(MaSanPham,TenSanPham,MaChatLieu,SoLuong,DonGiaNhap,DonGiaBan,Anh,GhiChu) values " + $"(N'{data[0]}',N'{data[1]}',N'{data[2]}',{data[3]},{data[4]},{data[5]},N'Anh',N'{data[6]}')";
                            break;
                    }
                    updateStatement = "insert into " + table.ToString() + " " + updateStatement;
                }
                else {
                    switch (table) {
                        case Table.ChatLieu:
                            updateStatement += $"MaChatLieu=N'{data[0]}',TenChatlieu=N'{data[1]}' where MaChatLieu=N'{data[0]}'";
                            break;
                        case Table.ChiTietHoaDon:
                            updateStatement += $"MaHDBan=N'{data[0]}',MaHang=N'{data[1]}',SoLuong={data[2]},DonGia={data[3]},GiamGia={data[4]},ThanhTien={data[5]} where MaHDBan=N'{data[0]}' and MaHang=N'{data[1]}'";
                            break;
                        case Table.HoaDonBan:
                            updateStatement += $"MaHDBan=N'{data[0]}',MaNhanVien=N'{data[1]}',NgayBan=N'{VerifyDate(data[2])}',MaKhach=N'{data[3]}',TongTien={data[4]} where MaHDBan=N'{data[0]}'";
                            break;
                        case Table.KhachHang:
                            updateStatement += $"MaKhachHang=N'{data[0]}',TenKhachHang=N'{data[1]}',DiaChi=N'{data[2]}',SDT=N'{data[3]}' where MaKhachHang=N'{data[0]}'";
                            break;
                        case Table.NhanVien:
                            updateStatement += $"MaNhanVien=N'{data[0]}',TenNhanVien=N'{data[1]}',DiaChi=N'{data[2]}',SDT=N'{data[3]}',NgaySinh=N'{VerifyDate(data[4])}',GioiTinh=N'{data[5]}' where MaNhanVien=N'{data[0]}'";
                            break;
                        case Table.SanPham:
                            updateStatement += $"MaSanPham=N'{data[0]}',TenSanPham=N'{data[1]}',MaChatLieu=N'{data[2]}',SoLuong={data[3]},DonGiaNhap={data[4]},DonGiaBan={data[5]},Anh=N'Anh',GhiChu=N'{data[6]}' where MaSanPham=N'{data[0]}'";
                            break;
                    }
                    updateStatement = "update " + table.ToString() + " set " + updateStatement;
                }
                new SqlCommand(updateStatement, databaseWindow.connection).ExecuteNonQuery();
            }
            catch (Exception ex) {
                string message = "Đã xảy ra lỗi với dữ liệu.\nXin hãy kiểm tra lại.";
                if (ex is DateFormatException)
                    message = ex.Message;
                MessageBox.Show(message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void DeleteFromDatabase( Table? table, string[] data) {
            string deleteStatement = "delete from " + table.ToString() + " where ";
            switch (table) {
                case Table.ChatLieu:
                    deleteStatement += $"MaChatLieu=N'{data[0]}'";
                    break;
                case Table.ChiTietHoaDon:
                    deleteStatement += $"MaHDBan=N'{data[0]}' and MaHang=N'{data[1]}'";
                    break;
                case Table.HoaDonBan:
                    deleteStatement += $"MaHDBan=N'{data[0]}'";
                    break;
                case Table.KhachHang:
                    deleteStatement += $"MaKhachHang=N'{data[0]}'";
                    break;
                case Table.NhanVien:
                    deleteStatement += $"MaNhanVien=N'{data[0]}'";
                    break;
                case Table.SanPham:
                    deleteStatement += $"MaSanPham=N'{data[0]}'";
                    break;
            }
            try {
                new SqlCommand(deleteStatement, databaseWindow.connection).ExecuteNonQuery();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        public static string[] GetFields( Table? table ) {
            List<string> fieldData = new List<string>();
            switch (table) {
                case Table.ChatLieu:
                    fieldData.Add(databaseWindow.textboxMaChatLieu.Text);
                    fieldData.Add(databaseWindow.textboxTenChatLieu.Text);
                    break;
                case Table.ChiTietHoaDon:
                    fieldData.Add(databaseWindow.textboxMaHDBan.Text);
                    fieldData.Add(databaseWindow.textboxMaHang.Text);
                    fieldData.Add(databaseWindow.textboxSoLuong.Text);
                    fieldData.Add(databaseWindow.textboxDonGia.Text);
                    fieldData.Add(databaseWindow.textboxGiamGia.Text);
                    fieldData.Add(databaseWindow.textboxThanhTien.Text);
                    break;
                case Table.HoaDonBan:
                    fieldData.Add(databaseWindow.textboxMaHDBan.Text);
                    fieldData.Add(databaseWindow.textboxMaNhanVien.Text);
                    fieldData.Add(databaseWindow.textboxNgayBan.Text);
                    fieldData.Add(databaseWindow.textboxMaKhach.Text);
                    fieldData.Add(databaseWindow.textboxTongTien.Text);
                    break;
                case Table.KhachHang:
                    fieldData.Add(databaseWindow.textboxMaKhachHang.Text);
                    fieldData.Add(databaseWindow.textboxTenKhachHang.Text);
                    fieldData.Add(databaseWindow.textboxDiaChiKH.Text);
                    fieldData.Add(databaseWindow.textboxSDTKH.Text);
                    break;
                case Table.NhanVien:
                    fieldData.Add(databaseWindow.textboxMaNhanVien.Text);
                    fieldData.Add(databaseWindow.textboxTenNhanVien.Text);
                    fieldData.Add(databaseWindow.textboxDiaChiNV.Text);
                    fieldData.Add(databaseWindow.textboxSDTNV.Text);
                    fieldData.Add(databaseWindow.textboxNgaySinh.Text);
                    fieldData.Add(databaseWindow.textboxGioiTinh.Text);
                    break;
                case Table.SanPham:
                    fieldData.Add(databaseWindow.textboxMaSanPham.Text);
                    fieldData.Add(databaseWindow.textboxTenSanPham.Text);
                    fieldData.Add(databaseWindow.textboxMaChatLieu.Text);
                    fieldData.Add(databaseWindow.textboxSoLuong.Text);
                    fieldData.Add(databaseWindow.textboxDonGiaNhap.Text);
                    fieldData.Add(databaseWindow.textboxDonGiaBan.Text);
                    fieldData.Add(databaseWindow.textboxGhiChu.Text);
                    break;
            }
            return fieldData.ToArray();
        }
        public static void LoadMenu( Table? table ) {
            if (databaseWindow.connection.State != ConnectionState.Open || table == null)
                return;
            switch (table) {
                case Table.ChatLieu:
                    HideAllMenu();
                    databaseWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    databaseWindow.labelTenChatLieu.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    databaseWindow.textboxTenChatLieu.Visibility = Visibility.Visible;
                    break;
                case Table.ChiTietHoaDon:
                    HideAllMenu();
                    databaseWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    databaseWindow.labelMaHang.Visibility = Visibility.Visible;
                    databaseWindow.labelSoLuong.Visibility = Visibility.Visible;
                    databaseWindow.labelDonGia.Visibility = Visibility.Visible;
                    databaseWindow.labelGiamGia.Visibility = Visibility.Visible;
                    databaseWindow.labelThanhTien.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    databaseWindow.textboxMaHang.Visibility = Visibility.Visible;
                    databaseWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    databaseWindow.textboxDonGia.Visibility = Visibility.Visible;
                    databaseWindow.textboxGiamGia.Visibility = Visibility.Visible;
                    databaseWindow.textboxThanhTien.Visibility = Visibility.Visible;
                    break;
                case Table.HoaDonBan:
                    HideAllMenu();
                    databaseWindow.labelMaHDBan.Visibility = Visibility.Visible;
                    databaseWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.labelNgayBan.Visibility = Visibility.Visible;
                    databaseWindow.labelMaKhach.Visibility = Visibility.Visible;
                    databaseWindow.labelTongTien.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaHDBan.Visibility = Visibility.Visible;
                    databaseWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.textboxNgayBan.Visibility = Visibility.Visible;
                    databaseWindow.textboxMaKhach.Visibility = Visibility.Visible;
                    databaseWindow.textboxTongTien.Visibility = Visibility.Visible;
                    break;
                case Table.KhachHang:
                    HideAllMenu();
                    databaseWindow.labelMaKhachHang.Visibility = Visibility.Visible;
                    databaseWindow.labelTenKhachHang.Visibility = Visibility.Visible;
                    databaseWindow.labelDiaChiKH.Visibility = Visibility.Visible;
                    databaseWindow.labelSDTKH.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaKhachHang.Visibility = Visibility.Visible;
                    databaseWindow.textboxTenKhachHang.Visibility = Visibility.Visible;
                    databaseWindow.textboxDiaChiKH.Visibility = Visibility.Visible;
                    databaseWindow.textboxSDTKH.Visibility = Visibility.Visible;
                    break;
                case Table.NhanVien:
                    HideAllMenu();
                    databaseWindow.labelMaNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.labelTenNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.labelDiaChiNV.Visibility = Visibility.Visible;
                    databaseWindow.labelSDTNV.Visibility = Visibility.Visible;
                    databaseWindow.labelNgaySinh.Visibility = Visibility.Visible;
                    databaseWindow.labelGioiTinh.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.textboxTenNhanVien.Visibility = Visibility.Visible;
                    databaseWindow.textboxDiaChiNV.Visibility = Visibility.Visible;
                    databaseWindow.textboxSDTNV.Visibility = Visibility.Visible;
                    databaseWindow.textboxNgaySinh.Visibility = Visibility.Visible;
                    databaseWindow.textboxGioiTinh.Visibility = Visibility.Visible;
                    break;
                case Table.SanPham:
                    HideAllMenu();
                    databaseWindow.labelMaSanPham.Visibility = Visibility.Visible;
                    databaseWindow.labelTenSanPham.Visibility = Visibility.Visible;
                    databaseWindow.labelMaChatLieu.Visibility = Visibility.Visible;
                    databaseWindow.labelSoLuong.Visibility = Visibility.Visible;
                    databaseWindow.labelDonGiaNhap.Visibility = Visibility.Visible;
                    databaseWindow.labelDonGiaBan.Visibility = Visibility.Visible;
                    databaseWindow.labelGhiChu.Visibility = Visibility.Visible;

                    databaseWindow.textboxMaSanPham.Visibility = Visibility.Visible;
                    databaseWindow.textboxTenSanPham.Visibility = Visibility.Visible;
                    databaseWindow.textboxMaChatLieu.Visibility = Visibility.Visible;
                    databaseWindow.textboxSoLuong.Visibility = Visibility.Visible;
                    databaseWindow.textboxDonGiaNhap.Visibility = Visibility.Visible;
                    databaseWindow.textboxDonGiaBan.Visibility = Visibility.Visible;
                    databaseWindow.textboxGhiChu.Visibility = Visibility.Visible;
                    break;
            }
        }
        public static void LoadFields( Table? table, DataRowView row ) {
            try {
                switch (table) {
                    case Table.ChatLieu:
                        databaseWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        databaseWindow.textboxTenChatLieu.Text = row["TenChatLieu"].ToString();
                        break;
                    case Table.ChiTietHoaDon:
                        databaseWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        databaseWindow.textboxMaHang.Text = row["MaHang"].ToString();
                        databaseWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        databaseWindow.textboxDonGia.Text = row["DonGia"].ToString();
                        databaseWindow.textboxGiamGia.Text = row["GiamGia"].ToString();
                        databaseWindow.textboxThanhTien.Text = row["ThanhTien"].ToString();
                        break;
                    case Table.HoaDonBan:
                        databaseWindow.textboxMaHDBan.Text = row["MaHDBan"].ToString();
                        databaseWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        databaseWindow.textboxNgayBan.Text = row["NgayBan"].ToString().Split(' ')[0];
                        databaseWindow.textboxMaKhach.Text = row["MaKhach"].ToString();
                        databaseWindow.textboxTongTien.Text = row["TongTien"].ToString();
                        break;
                    case Table.KhachHang:
                        databaseWindow.textboxMaKhachHang.Text = row["MaKhachhang"].ToString();
                        databaseWindow.textboxTenKhachHang.Text = row["TenKhachHang"].ToString();
                        databaseWindow.textboxDiaChiKH.Text = row["DiaChi"].ToString();
                        databaseWindow.textboxSDTKH.Text = row["SDT"].ToString();
                        break;
                    case Table.NhanVien:
                        databaseWindow.textboxMaNhanVien.Text = row["MaNhanVien"].ToString();
                        databaseWindow.textboxTenNhanVien.Text = row["TenNhanVien"].ToString();
                        databaseWindow.textboxDiaChiNV.Text = row["DiaChi"].ToString();
                        databaseWindow.textboxSDTNV.Text = row["SDT"].ToString();
                        databaseWindow.textboxNgaySinh.Text = row["NgaySinh"].ToString().Split(' ')[0];
                        databaseWindow.textboxGioiTinh.Text = row["GioiTinh"].ToString();
                        break;
                    case Table.SanPham:
                        databaseWindow.textboxMaSanPham.Text = row["MaSanPham"].ToString();
                        databaseWindow.textboxTenSanPham.Text = row["TenSanPham"].ToString();
                        databaseWindow.textboxMaChatLieu.Text = row["MaChatLieu"].ToString();
                        databaseWindow.textboxSoLuong.Text = row["SoLuong"].ToString();
                        databaseWindow.textboxDonGiaNhap.Text = row["DonGiaNhap"].ToString();
                        databaseWindow.textboxDonGiaBan.Text = row["DonGiaBan"].ToString();
                        databaseWindow.textboxGhiChu.Text = row["GhiChu"].ToString();
                        break;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
    public class DateFormatException : Exception {
        public DateFormatException( string message ) : base(message) { }
    }
}
