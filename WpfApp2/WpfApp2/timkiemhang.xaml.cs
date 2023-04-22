using System.Windows;
using System.Data;
using System.Data.SqlClient;
namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for timkiemhang.xaml
    /// </summary>
    public partial class timkiemhang : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public timkiemhang() {
            InitializeComponent();
        }

        private void napdulieu() {
            grdttkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdttkh.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.\PHUONGNGU;Initial Catalog=qlchh;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void timkiem_Click( object sender, RoutedEventArgs e ) {
            grdttkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sql;
            sql = "SELECT * FROM tblhang WHERE MaHang = '" + mahang.Text + "'";
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
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }
        }
    }
}
