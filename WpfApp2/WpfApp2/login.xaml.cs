using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        DataTable dataTable = null;
        public login() {
            InitializeComponent();
        }



        private void dangnhap_Click( object sender, RoutedEventArgs e ) {

            string sql = "Select * from login Where (MaNhanVien ='" +
                manv.Text + "')and(MatKhau='" +
                mk.Password + "')";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows.Count > 0) {

                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.");
                manv.Focus();
            }

        }

        private void quenmk_Click( object sender, RoutedEventArgs e ) {
            MessageBox.Show("Hỏi chủ cửa hàng");
        }



        private void manv_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.Enter) {
                mk.Focus();
            }
        }

        private void mk_KeyDown( object sender, KeyEventArgs e ) {
            if (e.Key == Key.Enter) {
                dangnhap_Click(sender, e);
            }
        }

        private void frm_login_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();


        }
    }
}