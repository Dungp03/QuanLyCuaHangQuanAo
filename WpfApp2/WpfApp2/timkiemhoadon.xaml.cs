using System.Windows;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for timkiemhoadon.xaml
    /// </summary>
    public partial class timkiemhoadon : Window {

        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        DataTable dataTable = null;
        public timkiemhoadon() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblhoadon";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdttkhd.ItemsSource = dataTable.DefaultView;
        }

        private void frm_tkhd_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.\PHUONGNGU;Initial Catalog=qlchh;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void timkiem_Click( object sender, RoutedEventArgs e ) {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sql;
            sql = "SELECT * FROM tblhoadon WHERE MaHDBan = '" + mahoadon.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdttkhd.ItemsSource = dataTable.DefaultView;


        }

        private void thoattk_Click( object sender, RoutedEventArgs e ) {
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
