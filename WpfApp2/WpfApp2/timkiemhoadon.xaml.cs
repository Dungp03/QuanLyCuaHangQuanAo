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
    /// Interaction logic for timkiemhoadon.xaml
    /// </summary>
    public partial class timkiemhoadon : Window
    {

        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        DataTable dataTable = null;
        public timkiemhoadon()
        {
            InitializeComponent();
        }
        private void napdulieu()
        {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            string sqlStr = "Select MaHDBan, MaNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan,MaKhachHang, DiaChi, SDT, TongTien from tblhoadon";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdttkhd.ItemsSource = dataTable.DefaultView;
        }

        private void napdulieu2()
        {
            grdthd.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            //Tạo câu lệnh truy vấn dữ liệu

            string sqlStr = "Select MaHDBan, MaHang,TenHang, SoLuong,Dongia, SoLuong*DonGia as ThanhTien from tblchon";
            //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
            DataSet dataSet = new DataSet();
            //Điền dữ liệu từ SqlDataAdapter vào DataSet
            adapter.Fill(dataSet, "tblchon");
            //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
            dataTable = dataSet.Tables["tblchon"];
            //Lấy nội dung từ DataTable hiển thị lên DataGrid
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        private void frm_tkhd_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStrin = @"Data Source=.\DESKTOP-RU72BJJ\SQLEXPRESS;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();
            napdulieu2();
            napdulieu();
        }

        private void timkiem_Click(object sender, RoutedEventArgs e)
        {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
            {
                return;
            }
            string maHDBan = mahoadon.Text.ToUpper();

             string sql;
             sql = "Select MaHDBan, MaNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan,MaKhachHang, TenKhachHang,DiaChi,SDT,TongTien from tblhoadon where MaHDBan = '"+ mahoadon.Text + "'";
             SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
             DataSet dataSet = new DataSet();
             adapter.Fill(dataSet, "tblhoadon");
             dataTable = dataSet.Tables["tblhoadon"];
             grdttkhd.ItemsSource = dataTable.DefaultView;

            string sqlstr;
            sqlstr = "Select MaHDBan, MaHang,TenHang, SoLuong, DonGia, SoLuong * DonGia as ThanhTien from tblchon where MaHDBan = '" + mahoadon.Text + "'";
            SqlDataAdapter adapter1 = new SqlDataAdapter(sqlstr, conn);
            DataSet dataSet1 = new DataSet();
            adapter1.Fill(dataSet1, "tblchon");
            dataTable = dataSet1.Tables["tblchon"];
            grdthd.ItemsSource = dataTable.DefaultView;
            


        }

        private void thoattk_Click(object sender, RoutedEventArgs e)
        {
            napdulieu();
            napdulieu2();
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
    }
}
