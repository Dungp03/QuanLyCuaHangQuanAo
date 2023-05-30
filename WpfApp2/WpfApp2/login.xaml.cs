using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        DataTable dataTable = null;
        public login()
        {
            InitializeComponent();
        }



        private void dangnhap_Click(object sender, RoutedEventArgs e)
        {
 
            string sql = "Select * from login Where (MaNhanVien ='" +
                manv.Text + "')and(MatKhau='" +
                mk.Password + "')";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.");
                manv.Focus();
            }

        }

        private void quenmk_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hỏi chủ cửa hàng");
        }



        private void manv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mk.Focus();
            }
        }

        private void mk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dangnhap_Click(sender, e);
            }
        }

        private void frm_login_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStrin = @"Data Source=DESKTOP-RU72BJJ\SQLEXPRESS;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            
        }
    }
}