using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;
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
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblhoadon";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        private void frm_hoadon_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.\PHUONGNGU;Initial Catalog=qlchh;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void hd_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Update tblhoadon Set MaHDBan ='" + hd_mahd.Text + "', " +
                    "TenNhanVien = '" + hd_nhanvien.Text + "'" +
                    ", NgayBan = '" + hd_ngay.Text + "', TenKhachHang = '" + hd_tenkh.Text + "'" +
                    ", MaHang = '" + hd_mahang.Text + "', TenHang = '" + hd_tenhang.Text + "'" +
                    ", DiaChi = '" + hd_diachi.Text + "'" +
                    ", SDT = '" + sdt.Text + "', SoLuong = '" + hd_sl.Text + "', DonGia = '" + hd_dongia.Text + "'," +
                    " ThanhTien = '" + hd_thanhtien.Text + "', Tong = '" + hd_tong.Text + "' where MaHDBan = '" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!");
            }
            clear();
        }

        private void hd_them_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Insert Into tblhoadon(MaHDBan, TenNhanVien, NgayBan, TenKhachHang,MaHang,TenHang,DiaChi,SDT, SoLuong, DonGia, ThanhTien,Tong)values" +
                    "('" + hd_mahd.Text + "','" + hd_nhanvien.Text + "', '" + hd_ngay.Text + "'" +
                    ",'" + hd_tenkh.Text + "','" + hd_mahang.Text + "', '" + hd_tenhang.Text + "','" + hd_diachi.Text + "','" + sdt.Text + "', '" + hd_sl.Text + "'," +
                    "'" + hd_dongia.Text + "','" + hd_thanhtien.Text + "','" + hd_tong.Text + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception ex) {
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

        private void hd_in_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblhoadon where MaHoaDon ='" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!");
            }
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
            hd_tong.Text = "";
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
                hd_tong.Text = row[11].ToString();

            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void hd_lammoi_Click( object sender, RoutedEventArgs e ) {
            clear();
        }

    }
}
