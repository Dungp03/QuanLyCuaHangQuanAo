using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for hoadon.xaml
    /// </summary>
    public partial class hoadon : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public hoadon() {
            InitializeComponent();
        }

        void napdulieu() {
            grdthd.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open) {
                return;
            }
            //Tạo câu lệnh truy vấn dữ liệu

            string sqlStr = "Select MaHang,TenHang, SoLuong,Dongia, SoLuong*DonGia as ThanhTien from tblchon";
            //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
            DataSet dataSet = new DataSet();
            //Điền dữ liệu từ SqlDataAdapter vào DataSet
            adapter.Fill(dataSet, "tblchon");
            //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
            dataTable = dataSet.Tables["tblchon"];
            //Lấy nội dung từ DataTable hiển thị lên DataGrid
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        void napdulieu2() {
            grdth.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select MaHang, TenHang,MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, GhiChu,CONVERT(varchar, ngaynhap, 103) AS ngaynhap from tblhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdth.ItemsSource = dataTable.DefaultView;
        }

        private void grdthd_SelectionChanged( object sender, SelectionChangedEventArgs e ) {

            try {
                if (grdthd.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdthd.CurrentItem;
                selectedID = row[0].ToString();
                hd_mahang.Text = row[0].ToString();
                hd_tenhang.Text = row[1].ToString();
                hd_sl.Text = row[2].ToString();
                hd_dongia.Text = row[3].ToString();
                hd_thanhtien.Text = row[4].ToString();


            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!" + ex.Message);

            }



        }
        private void addamhang() {
            string sql = "select MaHang from tblhang";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                string machatlieuu = reader.GetString(0);
                string s = machatlieuu.ToString();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                hd_mahang.Items.Add(item);
            }
            reader.Close();
        }




        private void addmanhanvien() {
            string sql = "select MaNhanVien from tblnhanvien";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                string mnv = reader.GetString(0);
                string s = mnv.ToString();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                hd_nhanvien.Items.Add(item);
            }
            reader.Close();
        }
        private void frm_hoadon_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();
            napdulieu2();
            // napdulieu();
            addmanhanvien();
            addamhang();

        }

        private void naphoadonban() {
        }

        private string TaoMaHoaDon() {
            string maHoaDon = "";
            Random rand = new Random();
            bool trungMa;
            do {
                trungMa = false;
                maHoaDon = "HD" + rand.Next(0, 99999).ToString();
                string sqlStr = "SELECT MaHDBan FROM tblhoadon WHERE MaHDBan = '" + maHoaDon + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataReader dta = cmd.ExecuteReader();
                if (dta.Read()) {
                    trungMa = true;
                }
                dta.Close();
            } while (trungMa);
            return maHoaDon;
        }

        private void clear1() {
            hd_mahd.Text = "";
            hd_nhanvien.Text = "";
            hd_ngay.Text = "";
            hd_makh.Text = "";
            tenkh.Text = "";
            diachi.Text = "";
            sdt.Text = "";
            hd_mahang.Text = "";
            hd_tenhang.Text = "";
            hd_sl.Text = "";
            hd_dongia.Text = "";
            hd_thanhtien.Text = "";
            tong.Content = " ";

        }

        private void clear() {

            hd_mahang.Text = "";
            hd_tenhang.Text = "";
            hd_sl.Text = "";
            hd_dongia.Text = "";
            hd_thanhtien.Text = "";
        }

        private void hd_chon_Click( object sender, RoutedEventArgs e ) {
            string sqlStr;
            string mahd = mahoadon;
            try {
                /*string sql = "insert into tblhoadon (MaHDBan) values ('" + mahd + "')";
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.ExecuteNonQuery();*/
                sqlStr = "insert into tblchon(MaHDBan,MaHang,TenHang,SoLuong,DonGia,ThanhTien) values" +
                        "('" + mahd + "','" + hd_mahang.Text + "', '"
                        + hd_tenhang.Text + "'," + int.Parse(hd_sl.Text) + ", " +
                        int.Parse(hd_dongia.Text) + ", " + int.Parse(hd_thanhtien.Text) + ")";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();

                if (conn.State != ConnectionState.Open) {
                    return;
                }
                TruSoLuongHang();
                napdulieu2();
                // cập nhật hiển thị của dtgr
                //Tạo câu lệnh truy vấn dữ liệu
                sqlStr = "SELECT MaHang, TenHang, SoLuong, Dongia, SoLuong * Dongia AS ThanhTien FROM tblchon WHERE MaHDBan = '" + mahd + "'";

                //sqlStr = "Select MaHang,TenHang, SoLuong,Dongia, SoLuong*Dongia as ThanhTien from tblchon where (MaHDBan = '" + hd_mahd.Text + "') and (MaHang = '" + hd_mahang.Text + "')";
                //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
                SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@mahd", mahoadon);
                //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
                DataSet dataSet = new DataSet();
                //Điền dữ liệu từ SqlDataAdapter vào DataSet
                adapter.Fill(dataSet, "tblchon");
                //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
                dataTable = dataSet.Tables["tblchon"];
                //Lấy nội dung từ DataTable hiển thị lên DataGrid
                grdthd.ItemsSource = dataTable.DefaultView;

                // Xóa nội dung các TextBox sau khi thêm



                TinhTongThanhTien();
                clear();

                mahd = "";
            }
            catch (Exception ex) {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void hd_them_Click( object sender, RoutedEventArgs e ) {
            string sqlStr;
            sqlStr = "select MaHDBan from tblHDBan where MaHDBan=N'" + hd_mahd.Text + "'";
            sqlStr = "insert into tblkhach(KH_MaKhach,KH_TenKhach,KH_DiaChi,KH_DienThoai) values" + "('" + hd_makh.Text + "','" + tenkh.Text + "','" + diachi.Text + "','" + sdt.Text + "');" + "insert into tblhoadon(MaHDBan, MaNhanVien,NgayBan,MaKhachHang,TenKhachHang,DiaChi) values" +
                   "('" + hd_mahd.Text + "','" + hd_nhanvien.Text + "', '" + hd_ngay.Text + "', '" + hd_makh.Text + "','" + tenkh.Text + "','" + diachi.Text + "', '" + sdt.Text + "')";
            sqlStr = "insert into tblchon(MaHDBan,MaHang,TenHang,SoLuong,DonGia,ThanhTien) " +
                "values(N'" + hd_mahang.Text + "', '" + hd_tenhang.Text + "', " + hd_sl.Text + "," + hd_dongia.Text + "," + hd_thanhtien.Text + ")";

        }



        private void DataGrid_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdth.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdth.CurrentItem;
                selectedID = row[0].ToString();
                hd_mahang.Text = row[0].ToString();
                hd_tenhang.Text = row[1].ToString();

                hd_dongia.Text = row[5].ToString();


            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!" + ex.Message);
            }

        }

        private void grdth_MouseDoubleClick( object sender, MouseButtonEventArgs e ) {
            try {
                if (grdth.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdth.CurrentItem;
                selectedID = row[0].ToString();
                hd_mahang.Text = row[0].ToString();
                hd_tenhang.Text = row[1].ToString();
                hd_sl.Text = row[3].ToString();
                hd_dongia.Text = row[5].ToString();


            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }

        }

        string mahoadon = "";


        // thêm mã hóa đơn
        private void luuhd_Click( object sender, RoutedEventArgs e ) {
            /*  mahoadon = TaoMaHoaDon();
              hd_mahd.Text = mahoadon;
              string sql = "insert into tblhoadon (MaHDBan) values ('" + mahoadon + "')";
              SqlCommand cmd1 = new SqlCommand(sql, conn);
              cmd1.ExecuteNonQuery();*/
        }

        private void hd_sl_TextChanged( object sender, TextChangedEventArgs e ) {
            hd_chon.IsEnabled = true;
            if (hd_dongia.Text != "" && hd_sl.Text != "") {
                hd_thanhtien.Text = ( int.Parse(hd_sl.Text) * int.Parse(hd_dongia.Text) ).ToString();

            }
            ComboBoxItem item = hd_mahang.SelectedItem as ComboBoxItem;
            if (hd_mahang.SelectedItem != null) {
                string sqlStr = "select SoLuong from tblhang where MaHang = '" + item.Content.ToString() + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) {
                    if (reader.GetInt32(0) == 0) {
                        lbtb.Content = "Hết hàng, vui lòng chọn sản phẩm khác";
                        hd_chon.IsEnabled = false;
                    }
                    else if (hd_sl.Text != "") {
                        if (reader.GetInt32(0) < int.Parse(hd_sl.Text)) {
                            lbtb.Content = "Trong kho chỉ còn " + reader.GetInt32(0).ToString() + " sản phẩm";
                            hd_chon.IsEnabled = false;
                        }
                    }
                }
                reader.Close();
            }
            if (hd_sl.Text == "") {
                lbtb.Content = "";
            }
        }

        int tongThanhTien = 0;
        private void TinhTongThanhTien() {

            tongThanhTien += int.Parse(hd_thanhtien.Text);

            // Hiển thị tổng thành tiền
            tong.Content = "Tổng thành tiền: " + tongThanhTien.ToString("N0") + " VND";
            tongtt = tongThanhTien;

            //return tongThanhTien;
        }
        private void TruSoLuongHang() {
            try {
                // lấy nội dung bên trong
                ComboBoxItem item = hd_mahang.SelectedItem as ComboBoxItem;
                string sqlStr = "select SoLuong from tblhang where MaHang = '" + item.Content.ToString() + "'";

                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) {
                    sqlStr = "Update tblhang set SoLuong  = " + ( reader.GetInt32(0) - int.Parse(hd_sl.Text) ) + " where MaHang = '" + item.Content.ToString() + "'";
                }
                reader.Close();

                cmd = new SqlCommand(sqlStr, conn); // update bảng hàng
                cmd.ExecuteNonQuery();

                sqlStr = "select SoLuong from tblhang where MaHang = '" + item.Content.ToString() + "'";

                cmd = new SqlCommand(sqlStr, conn);
                SqlDataReader reader1 = cmd.ExecuteReader();
                reader1.Close();
            }

            catch (Exception ex) {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        int tongtt = 0;

        private void tk_SelectionChanged( object sender, SelectionChangedEventArgs e ) {

        }

        private void setbuttonmua( bool set ) {
            boqua.Visibility = set ? Visibility.Visible : Visibility.Hidden; // nếu đúng thì hiển thị, còn false thì ẩn đi
            hd_chon.Visibility = set ? Visibility.Visible : Visibility.Hidden;
            hd_sua.Visibility = set ? Visibility.Visible : Visibility.Hidden;
            hd_tt.Visibility = set ? Visibility.Visible : Visibility.Hidden;

            hd_xoa.Visibility = set ? Visibility.Visible : Visibility.Hidden;
            hd_lammoi.Visibility = set ? Visibility.Visible : Visibility.Hidden;
            mua.Visibility = !set ? Visibility.Visible : Visibility.Hidden;
        }

        private void mua_Click( object sender, RoutedEventArgs e ) {
            setbuttonmua(true);
            mahoadon = TaoMaHoaDon();
            hd_mahd.Text = mahoadon;
            string sql = "insert into tblhoadon (MaHDBan) values ('" + mahoadon + "')";
            SqlCommand cmd1 = new SqlCommand(sql, conn);
            cmd1.ExecuteNonQuery();
        }

        private void boqua_Click( object sender, RoutedEventArgs e ) {
            setbuttonmua(false);
            string sql = "delete from tblchon where MaHDBan = '" + mahoadon + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            sql = "delete from tblhoadon where MaHDBan = '" + mahoadon + "'";
            grdthd.ItemsSource = null;
            try {
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                cmd1.ExecuteNonQuery();
                clear1();
                tongtt = 0;
                tongThanhTien = 0;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        // thanh toán
        private void hd_sua_Click( object sender, RoutedEventArgs e ) {

        }

        private void hd_tt_Click( object sender, RoutedEventArgs e ) {

            try {
                if (hd_makh.Text != "") {
                    string sqlStr = "insert into tblkhach(KH_MaKhach,KH_TenKhach,KH_DiaChi,KH_DienThoai) values" + "('" + hd_makh.Text + "','" + tenkh.Text + "','" + diachi.Text + "','" + sdt.Text + "')";
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);
                    cmd.ExecuteNonQuery();

                    sqlStr = "update tblhoadon set MaNhanVien = '" + hd_nhanvien.Text + "' , NgayBan = '" + hd_ngay.Text + "',MaKhachHang = '" + hd_makh.Text + "',TenKhachHang ='" + tenkh.Text + "',DiaChi = '" + diachi.Text + "',SDT = '" + sdt.Text + "', TongTien = " + tongtt + " where MaHDBan = '" + mahoadon + "'";
                    cmd = new SqlCommand(sqlStr, conn);
                    cmd.ExecuteNonQuery();
                    setbuttonmua(false);
                    clear1();
                    tongtt = 0;
                    grdthd.ItemsSource = null;
                    MessageBox.Show("Thanh toán thành công");
                }
                else
                    MessageBox.Show("Lỗi");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        // xóa
        private void grdthd_MouseDoubleClick( object sender, MouseButtonEventArgs e ) {
            try {

                if (grdthd.SelectedItem != null) {

                    string sqlStr = "";
                    // Xóa dòng trong DataGrid
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes) {
                        // Thực hiện hành động khi người dùng chọn Yes

                        sqlStr = "Delete from tblchon where MaHang ='" + hd_mahang.Text + "'";
                        SqlCommand cmd = new SqlCommand(sqlStr, conn);
                        cmd.ExecuteNonQuery();
                    }
                    else {
                        // Thực hiện hành động khi người dùng chọn No
                    }


                    // Cộng lại số lượng trong tblhang
                    sqlStr = "select SoLuong from tblhang WHERE MaHang = '" + hd_mahang.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(sqlStr, conn);
                    SqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.Read()) {
                        sqlStr = "Update tblhang set SoLuong  = " + ( reader.GetInt32(0) + int.Parse(hd_sl.Text) ) + " where MaHang = '" + hd_mahang.Text + "'";
                        reader.Close();
                        cmd1 = new SqlCommand(sqlStr, conn);
                        cmd1.ExecuteNonQuery();
                    }

                    DataRowView row = (DataRowView) grdthd.CurrentItem;

                    // Trừ tổng tiền
                    int a = tongThanhTien;
                    string b = row[4].ToString();
                    tongThanhTien -= int.Parse(row[4].ToString());


                    // Hiển thị tổng tiền
                    tong.Content = "Tổng thành tiền: " + tongThanhTien.ToString("N0") + " VND"; // Để định dạng số nguyên thành chuỗi có dấu phân cách hàng nghìn
                    tongtt = tongThanhTien;
                    // cập nhật lại datagrid
                    string selectSql = "SELECT MaHang, TenHang, SoLuong, Dongia, SoLuong * Dongia AS ThanhTien FROM tblchon WHERE MaHDBan = '" + mahoadon + "'";
                    SqlDataAdapter adapter = new SqlDataAdapter(selectSql, conn);
                    DataSet newDataSet = new DataSet();
                    adapter.Fill(newDataSet, "tblchon");
                    DataTable newDataTable = newDataSet.Tables["tblchon"];
                    grdthd.ItemsSource = newDataTable.DefaultView;
                    napdulieu2();

                }


            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!" + ex.Message);
            }
            clear();
        }


    }
}
