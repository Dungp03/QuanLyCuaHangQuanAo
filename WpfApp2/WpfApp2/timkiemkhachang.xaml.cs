using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for timkiemkhachang.xaml
    /// </summary>
    public partial class timkiemkhachang : Window {
        SqlConnection conn = new SqlConnection();
        DataTable dataTable = null;
        public timkiemkhachang() {
            InitializeComponent();
        }

        private void napdulieu() {
            dtgrtkkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;

            string sqlStr = "Select * from tblkhach";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblkhach");
            dataTable = dataSet.Tables["tblkhach"];
            dtgrtkkh.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            conn.ConnectionString = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.Open();

            napdulieu();
        }
        // tìm kiếm
        private void Button_Click( object sender, RoutedEventArgs e ) {
            dtgrtkkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;

            string sql = "SELECT * FROM tblKhach WHERE KH_MaKhach like '%" + makh.Text + "%'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblkhach");
            dataTable = dataSet.Tables["tblkhach"];
            dtgrtkkh.ItemsSource = dataTable.DefaultView;
        }

        private void dong_Click( object sender, RoutedEventArgs e ) {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
                this.Close();
        }

        private void quaylai_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
        }
    }
}
