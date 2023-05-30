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
    /// Interaction logic for dathang.xaml
    /// </summary>
    public partial class dathang : Window
    {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public dathang()
        {
            InitializeComponent();
        }

        private void napdulieu()
        {
            grdthd.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            //Tạo câu lệnh truy vấn dữ liệu

            string sqlStr = "Select  MaHang,TenHang, SoLuong,  DonGia, SoLuong * DonGia AS ThanhTien from tblhoadon"; //(select tblhang.DonGiaBan from tblhang where tblhoadon.DonGia = tblhang.DonGiaBan ) as DonGia from tblhoadon
            //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
            DataSet dataSet = new DataSet();
            //Điền dữ liệu từ SqlDataAdapter vào DataSet
            adapter.Fill(dataSet, "tblhoadon");
            //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
            dataTable = dataSet.Tables["tblhoadon"];
            //Lấy nội dung từ DataTable hiển thị lên DataGrid
            grdthd.ItemsSource = dataTable.DefaultView;

        }

        private void grdthd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStrin = @"Data Source=DESKTOP-RU72BJJ\SQLEXPRESS;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
            
            addmanhanvien();
        }

      



        private void addmanhanvien()
        {
            string sql = "select MaNhanVien from tblnhanvien";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string mnv = reader.GetString(0);
                string s = mnv.ToString();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                nv.Items.Add(item);
            }
            reader.Close();
        }
    }
}
