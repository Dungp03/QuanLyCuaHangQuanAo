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
    /// Interaction logic for timkiemkhachang.xaml
    /// </summary>
    public partial class timkiemkhachang : Window
    {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public timkiemkhachang()
        {
            InitializeComponent();
        }

        private void napdulieu()
        {
            dtgrtkkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            string sqlStr = "Select * from tblkhach";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblkhach");
            dataTable = dataSet.Tables["tblkhach"];
            dtgrtkkh.ItemsSource = dataTable.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStrin = @"Data Source=.\DESKTOP-RU72BJJ\SQLEXPRESS;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }
        // tìm kiếm
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dtgrtkkh.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            string sql;
            sql = "SELECT * FROM tblKhach WHERE KH_MaKhach = '" + makh.Text + "'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblkhach");
            dataTable = dataSet.Tables["tblkhach"];
           dtgrtkkh.ItemsSource = dataTable.DefaultView;
        }

        private void dong_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else
            {
                // Thực hiện hành động khi người dùng chọn No
            }
        }

        private void quaylai_Click(object sender, RoutedEventArgs e)
        {
            napdulieu();
        }
    }
}
