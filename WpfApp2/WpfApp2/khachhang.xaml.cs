using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for khachhang.xaml
    /// </summary>
    public partial class khachhang : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStr = "";
        string ID = "";
        DataTable dataTable = null;
        public khachhang() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdtkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblkhach";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblkhach");
            dataTable = dataSet.Tables["tblkhach"];
            grdtkh.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStr = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStr;
            conn.Open();

            napdulieu();
        }

        private void kh_them_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Insert Into tblkhach(KH_MaKhach,KH_TenKhach,Kh_DiaChi,KH_DienThoai)values('" + makh.Text + "','" + tenkh.Text + "','" + kh_dc.Text + "','" + kh_sdt.Text + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("Không hợp lệ, nhập lại!!!");
            }
            kh_lammoi_Click(sender, e);
        }

        private void kh_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Update tblkhach Set KH_MaKhach ='" + makh.Text + "', KH_TenKhach = '" + tenkh.Text + "', KH_DiaChi = '" + kh_dc.Text + "', KH_DienThoai = '" + kh_sdt.Text + "' where KH_MaKhach = '" + ID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
            kh_lammoi_Click(sender, e);
        }

        private void kh_xoa_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblkhach where KH_MaKhach ='" + ID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void kh_lammoi_Click( object sender, RoutedEventArgs e ) {
            makh.Text = "";
            tenkh.Text = "";
            kh_dc.Text = "";
            kh_sdt.Text = "";
        }

        private void kh_dong_Click( object sender, RoutedEventArgs e ) {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }
        }


        private void grdtkh_SelectionChanged_1( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdtkh.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdtkh.CurrentItem;
                ID = row[0].ToString();
                makh.Text = row[0].ToString();
                tenkh.Text = row[1].ToString();
                kh_dc.Text = row[2].ToString();
                kh_sdt.Text = row[3].ToString();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }
    }
}
