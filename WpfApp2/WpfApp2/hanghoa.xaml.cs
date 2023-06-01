using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for hanghoa.xaml
    /// </summary>
    public partial class hanghoa : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public hanghoa() {
            InitializeComponent();
        }
        private void napdulieu() {
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

        private void h_them_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Insert Into tblhang(MaHang, TenHang,MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, GhiChu,ngaynhap)values" +
                    "('" + mahang.Text + "','" + tenhang.Text + "', '" + machatlieu.Text + "'" +
                    ",'" + soluong.Text + "','" + dongianhap.Text + "','" + dongia.Text + "','" + "Còn hàng" + "','" + ngaynhap.SelectedDate + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("Không hợp lệ, nhập lại!!!");
            }
            h_lammoi_Click(sender, e);
        }

        private void h_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Update tblhang Set MaHang ='" + mahang.Text + "', TenHang = '" + tenhang.Text + "', MaChatLieu = '" + machatlieu.Text + "', SoLuong = '" + soluong.Text + "', DonGiaNhap = '" + dongianhap.Text + "', DonGiaBan = '" + dongia.Text + "',GhiChu = '" + ghichu.Text + "',ngaynhap = '" + ngaynhap.SelectedDate + "' where MaHang = '" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void h_xoa_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblhang where MaHang ='" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception ex) {
                MessageBox.Show("ERROR!!!" + ex.Message);
            }
        }

        private void h_lammoi_Click( object sender, RoutedEventArgs e ) {
            mahang.Text = "";
            tenhang.Text = "";
            machatlieu.Text = "";
            soluong.Text = "";
            dongianhap.Text = "";
            dongia.Text = "";
            ghichu.Text = "";
            ngaynhap.Text = "";

        }

        private void h_dong_Click( object sender, RoutedEventArgs e ) {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }
        }

        private void dtgrh_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdth.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdth.CurrentItem;
                selectedID = row[0].ToString();
                mahang.Text = row[0].ToString();
                tenhang.Text = row[1].ToString();
                machatlieu.Text = row[2].ToString();
                soluong.Text = row[3].ToString();
                dongianhap.Text = row[4].ToString();
                dongia.Text = row[5].ToString();
                ghichu.Text = row[6].ToString();
                ngaynhap.Text = row[7].ToString();

            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void addmachatlieu() {
            string sql = "select ID from tblchatlieu";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                string machatlieuu = reader.GetString(0);
                string s = machatlieuu.ToString();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                machatlieu.Items.Add(item);
            }
        }

        private void frm_hanghoa_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
            addmachatlieu();
        }
    }
}
