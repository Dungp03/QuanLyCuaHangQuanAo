using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for timkiemhoadon.xaml
    /// </summary>
    public partial class timkiemhoadon : Window {

        SqlConnection conn = new SqlConnection();
        DataTable dataTable = null;
        public timkiemhoadon() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;

            string sqlStr = "Select MaHDBan, MaNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan,MaKhachHang, DiaChi, SDT, TongTien from tblhoadon";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdttkhd.ItemsSource = dataTable.DefaultView;
        }

        private void napdulieu2() {
            grdthd.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open)
                return;
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

        private void frm_tkhd_Loaded( object sender, RoutedEventArgs e ) {
            conn.ConnectionString = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.Open();
            napdulieu2();
            napdulieu();
        }

        private void timkiem_Click( object sender, RoutedEventArgs e ) {
            grdttkhd.ItemsSource = null;
            if (conn.State != ConnectionState.Open)
                return;

            string sql = "Select MaHDBan, MaNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan,MaKhachHang, TenKhachHang,DiaChi,SDT,TongTien from tblhoadon where MaHDBan like '%" + mahoadon.Text + "%'";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            dataTable = dataSet.Tables["tblhoadon"];
            grdttkhd.ItemsSource = dataTable.DefaultView;

            string sqlstr = "Select MaHDBan, MaHang,TenHang, SoLuong, DonGia, SoLuong * DonGia as ThanhTien from tblchon where MaHDBan like '%" + mahoadon.Text + "%'";
            SqlDataAdapter adapter1 = new SqlDataAdapter(sqlstr, conn);
            DataSet dataSet1 = new DataSet();
            adapter1.Fill(dataSet1, "tblchon");
            dataTable = dataSet1.Tables["tblchon"];
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        private void thoattk_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
            napdulieu2();
        }

        private void dong_Click( object sender, RoutedEventArgs e ) {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
