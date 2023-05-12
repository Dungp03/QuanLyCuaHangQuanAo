using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for hoadonbanhang.xaml
    /// </summary>
    public partial class hoadonbanhang : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public hoadonbanhang() {
            InitializeComponent();
        }

        private void napdulieu() {
            grdthd.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open) {
                return;
            }
            //Tạo câu lệnh truy vấn dữ liệu

            string sqlStr = "Select MaHDBan, TenNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan, MaHang,TenHang,TenKhachHang,DiaChi,SDT, SoLuong, DonGia, SoLuong * Dongia AS ThanhTien from tblhoadon";
            //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
            DataSet dataSet = new DataSet();
            //Điền dữ liệu từ SqlDataAdapter vào DataSet
            adapter.Fill(dataSet, "tblhoadon");
            //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
            dataTable = dataSet.Tables["tblhoadon"];
            //Lấy nội dung từ DataTable hiển thị lên DataGrid
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        private void frm_hoadon_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void hd_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Update tblhoadon Set MaHDBan ='" + hd_mahd.Text + "', " +
                    "TenNhanVien = '" + hd_nhanvien.Text + "'" +
                    ", NgayBan = '" + hd_ngay.Text + "', MaHang = '" + hd_mahang.Text + "', TenKhachHang = '" + hd_tenkh.Text + "'" + "', TenHang = '" + hd_tenhang.Text + "'" +
                    ", DiaChi = '" + hd_diachi.Text + "'" +
                    ", SDT = '" + sdt.Text + "', SoLuong = '" + hd_sl.Text + "', DonGia = '" + hd_dongia.Text + "'," +
                    " ThanhTien = '" + hd_thanhtien.Text + "' where MaHDBan = '" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
            clear();
        }

        private void hd_them_Click( object sender, RoutedEventArgs e ) {
            try {
                /*string sqlStr = "";
                sqlStr = "Insert Into tblhoadon(MaHDBan, TenNhanVien, NgayBan, MaHang,TenHang,TenKhachHang,DiaChi,SDT, SoLuong, DonGia, ThanhTien)values" +
                    "('" + hd_mahd.Text + "','" + hd_nhanvien.Text + "', '" + hd_ngay.Text + "'" +
                    ",'" + hd_tenkh.Text + "','" + hd_mahang.Text + "', '" + hd_tenhang.Text + "','" + hd_diachi.Text + "','" + sdt.Text + "', '" + hd_sl.Text + "'," +
                    "'" + hd_dongia.Text + "','" + hd_thanhtien.Text + "')";*/
                string sqlStr = " ";
                sqlStr = "insert into tblhoadon(MaHDBan, TenNhanVien,NgayBan,MaHang,TenHang,TenKhachHang,DiaChi,SDT,SoLuong,DonGia,ThanhTien) values" +
                    "('" + hd_mahd.Text + "','" + hd_nhanvien.Text + "', '" + hd_ngay.Text + "','" + hd_mahang.Text + "', '"
                    + hd_tenhang.Text + "', '" + hd_tenkh.Text + "','" + hd_diachi.Text + "','" + sdt.Text + "', '" + hd_sl.Text + "', '" +
                    hd_dongia.Text + "', '" + hd_thanhtien.Text + "')";

                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("không hợp lệ, nhập lại!!!");
            }
            clear();
        }

        private void hd_dong_Click( object sender, RoutedEventArgs e ) {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }
        }

        // xóa
        private void hd_in_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblhoadon where MaHDBan ='" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
            clear();
        }


        private void clear() {
            hd_mahd.Text = "";
            hd_nhanvien.Text = "";
            hd_ngay.Text = "";
            hd_tenkh.Text = "";
            hd_mahang.Text = "";
            hd_tenhang.Text = "";
            hd_diachi.Text = "";
            sdt.Text = "";
            hd_sl.Text = "";
            hd_dongia.Text = "";
            hd_thanhtien.Text = "";

        }

        private void grdthd_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdthd.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdthd.CurrentItem;
                selectedID = row[0].ToString();
                hd_mahd.Text = row[0].ToString();
                hd_nhanvien.Text = row[1].ToString();
                hd_ngay.Text = row[2].ToString();
                hd_tenkh.Text = row[3].ToString();
                hd_mahang.Text = row[4].ToString();
                hd_tenhang.Text = row[5].ToString();
                hd_diachi.Text = row[6].ToString();
                sdt.Text = row[7].ToString();
                hd_sl.Text = row[8].ToString();
                hd_dongia.Text = row[9].ToString();
                hd_thanhtien.Text = row[10].ToString();


            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void hd_lammoi_Click( object sender, RoutedEventArgs e ) {
            clear();
        }

        private void hd_ngay_SelectedDateChanged( object sender, SelectionChangedEventArgs e ) {

        }

        private void hd_dongia_TextChanged( object sender, TextChangedEventArgs e ) {
            if (hd_sl.Text == "") {
                return;
            }
            hd_thanhtien.Text = ( int.Parse(hd_dongia.Text) * int.Parse(hd_sl.Text) ).ToString();
        }



        /* private void hd_sl_TextChanged(object sender, TextChangedEventArgs e)
         {
             if (hd_dongia.Text == "")
             {
                 return;
             }
             hd_thanhtien.Text = (int.Parse(hd_dongia.Text) * int.Parse(hd_sl.Text)).ToString();

         } */
    }
}
