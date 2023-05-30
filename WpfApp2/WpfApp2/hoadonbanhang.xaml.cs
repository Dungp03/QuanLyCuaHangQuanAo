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
// using Microsoft.Office.Interop.Excel;


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for hoadonbanhang.xaml
    /// </summary>
    public partial class hoadonbanhang : Window
    {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public hoadonbanhang()
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
            
            string sqlStr = "Select MaHDBan, MaNhanVien, CONVERT(varchar,NgayBan, 103) AS NgayBan,MaKhachHang, TenKhachHang, DiaChi, SDT, TongTien from tblhoadon"; //(select tblhang.DonGiaBan from tblhang where tblhoadon.DonGia = tblhang.DonGiaBan ) as DonGia from tblhoadon
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

        private void napdulieu1()
        {
            string sqlStr = "Select * from tblchon";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblchon");
            dataTable = dataSet.Tables["tblchon"];
            grdthd.ItemsSource = dataTable.DefaultView;
        }

        private void frm_hoadon_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionStrin = @"Data Source=DESKTOP-RU72BJJ\SQLEXPRESS;Initial Catalog=qlchn;Integrated Security=True";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();
            // napdulieu1();
            napdulieu();
            addamhang();
            addmanhanvien();
        }

       

      

       
        // xóa
        private void hd_in_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sqlStr = "";
                sqlStr = "delete from tblchon where MaHDBan = '" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                sqlStr = "Delete from tblhoadon where MaHDBan ='" + selectedID + "'";
                SqlCommand cmd1 = new SqlCommand(sqlStr, conn);
                cmd1.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR!!!" + ex.Message);
            }
            clear();
        }
       

        private void clear()
        {
            hd_mahd.Text = "";
            hd_nhanvien.Text = "";
            hd_ngay.Text = "";
            hd_makh.Text = "";
            tenkh.Text = "";
            diachi.Text = "";
            sdt.Text = "";
            hd_mahang.Text = "";
            hd_tenhang.Text = "";
            hd_sl.Text = "";
            hd_dongia.Text = "";
            hd_thanhtien.Text = "";
          
        }

        private void grdthd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (grdthd.CurrentItem == null) { return; }
                DataRowView row = (DataRowView)grdthd.CurrentItem;
                selectedID = row[0].ToString();
                hd_mahd.Text = row[0].ToString();
                hd_nhanvien.Text = row[1].ToString();
                hd_ngay.Text = row[2].ToString();
               
                hd_makh.Text = row[3].ToString();
                tenkh.Text = row[4].ToString();
                diachi.Text = row[5].ToString();
                sdt.Text = row[6].ToString();
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR DataGrid tblhanghoa!!!"+ ex.Message);
                
            }
        }

        private void hd_lammoi_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        

        private void hd_dongia_TextChanged(object sender, TextChangedEventArgs e)
        {
           if(hd_sl.Text != "")
            {
                hd_thanhtien.Text = (int.Parse(hd_dongia.Text) * int.Parse(hd_sl.Text)).ToString();
            }
           
        }

       
        private void addamhang()
        {
            string sql = "select MaHang from tblhang";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string machatlieuu = reader.GetString(0);
                string s = machatlieuu.ToString();
                ComboBoxItem item = new ComboBoxItem();
                item.Content = s;
                hd_mahang.Items.Add(item);
            }
            reader.Close();
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
                hd_nhanvien.Items.Add(item);
            }
            reader.Close();
        }

        

  
       


        private void grdthd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Lấy dòng được nhấp đúp trên DataGrid "grdth"
            DataRowView selectedRow = (DataRowView)grdth.SelectedItem;
           
            // Kiểm tra dòng có hợp lệ không
            if (grdthd.SelectedItem != null )
            {
                // Lấy mã hóa đơn từ dòng được nhấp đúp
                string sql = "";
                sql = "select MaHDBan from tblhoadon where MaHDBan = '" + hd_mahd + "'";

                // Lấy thông tin hàng từ mã hóa đơn

                // Hiển thị thông tin hàng lên DataGrid "tblchon"

                
                sql = "Select MaHDBan, MaHang,TenHang, SoLuong, DonGia, SoLuong * DonGia as ThanhTien from tblchon where MaHDBan = '" + hd_mahd.Text + "'";
                SqlDataAdapter adapter1 = new SqlDataAdapter(sql, conn);
                DataSet dataSet1 = new DataSet();
                adapter1.Fill(dataSet1, "tblchon");
                dataTable = dataSet1.Tables["tblchon"];
                grdthd.ItemsSource = dataTable.DefaultView;
                hdm.Visibility = Visibility.Visible;
            }
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            napdulieu();
            hdm.Visibility = Visibility.Hidden; 
        }
    }
}
