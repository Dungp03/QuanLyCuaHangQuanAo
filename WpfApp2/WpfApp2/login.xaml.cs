using System;
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

        public login() {
            InitializeComponent();
        }

        private void dangnhap_Click( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            if (conn.State == ConnectionState.Closed) {
                conn.Open();
            }
            string query = "SELECT COUNT(1) FROM login WHERE MaNhanVien = @manv AND MatKhau = @matkhau";
            SqlCommand sqlCmd = new SqlCommand(query, conn);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Parameters.AddWithValue("@manv", manv.Text);
            sqlCmd.Parameters.AddWithValue("@matkhau", mk.Password);
            int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
            if (count == 1) {

                MainWindow main = new MainWindow();
                main.ShowDialog();
                this.Close();
            }
            else {
                MessageBox.Show("Incorect");
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
    }
}