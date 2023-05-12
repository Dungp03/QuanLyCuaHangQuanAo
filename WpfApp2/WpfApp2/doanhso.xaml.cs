using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for doanhso.xaml
    /// </summary>
    public partial class doanhso : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;

        public doanhso() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdtds.ItemsSource = null;
            //Kiểm tra xem kết nối đã thực hiện được chưa
            if (conn.State != ConnectionState.Open) {
                return;
            }
            //Tạo câu lệnh truy vấn dữ liệu

            string sqlStr = "Select MaHDBan, CONVERT(varchar,NgayBan, 103) AS NgayBan, MaHang,TenHang, SoLuong, DonGia, SoLuong * Dongia AS ThanhTien from tblhoadon";
            //Khai báo biến kiểu SqlDataAdapter để thực hiện truy vấn dữ liệu
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            //Khai báo DataSet để chứa các dữ liệu lấy được về qua SqlDataAdapter
            DataSet dataSet = new DataSet();
            //Điền dữ liệu từ SqlDataAdapter vào DataSet
            adapter.Fill(dataSet, "tblhoadon");
            //Lấy bảng đầu tiên trong tập dữ liệu chứa vào DataTable
            dataTable = dataSet.Tables["tblhoadon"];
            //Lấy nội dung từ DataTable hiển thị lên DataGrid
            grdtds.ItemsSource = dataTable.DefaultView;
        }
        private void grdtds_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdtds.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdtds.CurrentItem;
                selectedID = row[0].ToString();



            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void ngay_SelectedDatesChanged( object sender, SelectionChangedEventArgs e ) {
            Calendar calendar = sender as Calendar;
            string sql = "";
            double totalRevenue = 0;

            if (calendar != null && calendar.DisplayMode == CalendarMode.Year) {
                int year = calendar.DisplayDate.Year;
                // Lấy năm được chọn
                sql = "SELECT MaHDBan, CONVERT(varchar, NgayBan, 103) AS NgayBan, MaHang, TenHang, SoLuong, DonGia, SoLuong * Dongia AS ThanhTien FROM tblhoadon WHERE YEAR(NgayBan) = " + year;
            }
            else if (calendar != null && calendar.DisplayMode == CalendarMode.Month) {
                int year = calendar.DisplayDate.Year;
                int month = calendar.DisplayDate.Month;
                // Lấy tháng và năm được chọn
                sql = "SELECT MaHDBan, CONVERT(varchar, NgayBan, 103) AS NgayBan, MaHang, TenHang, SoLuong, DonGia, SoLuong * Dongia AS ThanhTien FROM tblhoadon WHERE YEAR(NgayBan) = " + year + " AND MONTH(NgayBan) = " + month;
            }

            // Execute the SQL query and calculate the total revenue
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhoadon");
            DataTable dataTable = dataSet.Tables["tblhoadon"];
            grdtds.ItemsSource = dataTable.DefaultView;

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) {
                totalRevenue += Convert.ToDouble(reader["ThanhTien"]);
            }

            // Display the total revenue
            tongDS.Text = totalRevenue.ToString();
        }

        // thoát lọc 
        private void Button_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
        }
    }
}
