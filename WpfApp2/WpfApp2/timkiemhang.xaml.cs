using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for timkiemhang.xaml
    /// </summary>
    public partial class timkiemhang : Window {
        readonly SqlConnection conn = new SqlConnection();
        DataTable dataTable = null;
        public timkiemhang() {
            InitializeComponent();
        }

        private void napdulieu() {
            grdttkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;
            string sqlStr = "Select MaHang, TenHang,MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, GhiChu,CONVERT(varchar, ngaynhap, 103) AS ngaynhap from tblhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdttkh.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            conn.ConnectionString = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.Open();

            napdulieu();
        }

        private void timkiem_Click( object sender, RoutedEventArgs e ) {
            grdttkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;
            string sql = "SELECT * FROM tblhang WHERE MaHang like '%" + mahang.Text + "%'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdttkh.ItemsSource = dataTable.DefaultView;
        }

        private void quaylai_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
        }

        private void dong_Click( object sender, RoutedEventArgs e ) {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
