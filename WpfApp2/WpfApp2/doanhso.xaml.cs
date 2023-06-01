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
        private object cmd;

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

            string sqlStr = "Select MaHDBan, CONVERT(varchar,NgayBan, 103) AS NgayBan, SoLuong, MaKhachHang,TenKhachHang, DiaChi, SDT, TongTien from tblhoadon";
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
                MessageBox.Show("Lỗi!!!");
            }
        }

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }

        private void ngay_SelectedDatesChanged( object sender, SelectionChangedEventArgs e ) {
            Calendar calendar = sender as Calendar;


            // Tính toán và hiển thị tổng doanh số theo tháng và tổng doanh số của cả năm

            if (calendar != null && ( calendar.DisplayMode == CalendarMode.Month || calendar.DisplayMode == CalendarMode.Year )) {

                int year = calendar.DisplayDate.Year;
                int month = calendar.DisplayDate.Month;

                double monthlyRevenue = CalculateMonthlyRevenue(year, month);
                double annualRevenue = CalculateAnnualRevenue(year);
                int totalQuantity = CalculateTotalQuantity(year, month);

                // Hiển thị doanh thu tháng
                tongDS.Text = "Doanh thu tháng " + month + "/" + year + ": " + monthlyRevenue.ToString();

                // Hiển thị doanh thu năm
                namds.Text = "Doanh thu năm " + year + ": " + annualRevenue.ToString();

                //Hiển thị số lượng hàng đã bán ra
                tongSL.Text = "Tổng số lượng hàng đã bán: " + totalQuantity.ToString();

            }
        }

        private double CalculateMonthlyRevenue( int year, int month ) {
            string sql = "SELECT ISNULL(SUM(TongTien),0) AS TongTien FROM tblhoadon WHERE YEAR(NgayBan) = @Year  AND MONTH(NgayBan) = @Month";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.Parameters.AddWithValue("@Month", month);
            double monthlyRevenue = 0;
            using (SqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read() && !reader.IsDBNull(0)) {
                    monthlyRevenue = Convert.ToDouble(reader["TongTien"]);
                }
            }
            return monthlyRevenue;
        }
        private double CalculateAnnualRevenue( int year ) {
            string sql = "SELECT ISNULL(SUM(TongTien),0 ) AS TongTien FROM tblhoadon WHERE YEAR(NgayBan) = @Year";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            double annualRevenue = 0;
            using (SqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read() && !reader.IsDBNull(0)) {
                    annualRevenue = Convert.ToDouble(reader["TongTien"]);
                }
            }

            return annualRevenue;
        }
        private int CalculateTotalQuantity( int year, int month ) {
            string sql = "SELECT ISNULL(SUM(tblchon.SoLuong), 0) AS TongSoLuong " +
                  "FROM tblhoadon INNER JOIN tblchon ON tblhoadon.MaHDBan = tblchon.MaHDBan " +
                  "WHERE YEAR(tblhoadon.NgayBan) = @Year AND MONTH(tblhoadon.NgayBan) = @Month";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Year", year);
            cmd.Parameters.AddWithValue("@Month", month);

            int totalQuantity = 0;

            using (SqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read() && !reader.IsDBNull(0)) {
                    totalQuantity = Convert.ToInt32(reader["TongSoLuong"]);
                }
            }

            return totalQuantity;
        }


        // thoát lọc 
        private void Button_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
            clear();
        }
        private void clear() {
            tongDS.Text = "";
            namds.Text = "";
        }
    }
}