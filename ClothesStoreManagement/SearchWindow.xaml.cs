using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace ClothesStoreManagement {
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window {
        public SearchWindow() {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection();

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
        }
        private void Window_ContentRendered( object sender, EventArgs e ) {
            ConnectToDatabase();
            comboBoxSelectSearch.Items.Add("Sản phẩm");
            comboBoxSelectSearch.Items.Add("Hóa đơn");
            comboBoxSelectSearch.Items.Add("Nhân viên");
            comboBoxSelectSearch.Items.Add("Khách hàng");
            //LoadData();
        }
        private void ConnectToDatabase() {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=QlyShopQuanAo;Integrated Security=True;";
            try {
                connection.Open();
                if (connection.State == ConnectionState.Open) {
                    buttonConnect.IsEnabled = false;
                    buttonDisconnect.IsEnabled = true;
                    comboBoxSelectSearch.IsEnabled = true;
                    LoadData();
                }
            }
            catch (Exception) {
                if (connection.State != ConnectionState.Open) {
                    MessageBox.Show("Không thể kết nối với Cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);
                    comboBoxSelectSearch.IsEnabled = false;
                }
            }
        }
        private void LoadData() {
            if (connection.State != ConnectionState.Open)
                return;
            Query("");
        }
        private void GetTable( string query ) {
            if (query == string.Empty || query.Length == 0)
                return;
            string GetTableQuery = query;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            DataTable dataTable = dataSet.Tables[0];
            //MessageBox.Show(dataTable.Rows[0][0].ToString());
            dataView.ItemsSource = dataTable.DefaultView;
            // stretch table to fit datagrid
            foreach (var column in dataView.Columns) {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
        private void Query( string condition ) {
            switch (comboBoxSelectSearch.SelectedIndex) {
                case 0:
                    condition = " and cl.MaChatLieu=N'MaCL1'";
                    GetTable(@"select sp.MaSanPham, sp.TenSanPham, cl.*, sp.SoLuong, sp.DonGiaNhap, sp.DonGiaBan, sp.Anh, sp.GhiChu
                               from[SanPham] sp, [ChatLieu] cl where sp.MaChatLieu = cl.MaChatLieu" + condition);
                    break;
                case 1:
                    condition = " where cthd.MaHDBan = hd.MaHDBan";
                    GetTable(@"select cthd.MaHDBan, cthd.MaSanPham,
                               hd.MaNhanVien, hd.MaKhach, hd.NgayBan,
                               cthd.SoLuong, cthd.DonGia, cthd.GiamGia, cthd.ThanhTien
                               from [ChiTietHoaDon] cthd, [HoaDonBan] hd" + condition);
                    break;
                case 2:
                    GetTable(@"select sp.MaSanPham, sp.TenSanPham, cl.*, sp.SoLuong, sp.DonGiaNhap, sp.DonGiaBan, sp.Anh, sp.GhiChu
                               from[SanPham] sp, [ChatLieu] cl
                               where sp.MaChatLieu = cl.MaChatLieu");
                    break;
                case 3:
                    GetTable(@"select sp.MaSanPham, sp.TenSanPham, cl.*, sp.SoLuong, sp.DonGiaNhap, sp.DonGiaBan, sp.Anh, sp.GhiChu
                               from[SanPham] sp, [ChatLieu] cl
                               where sp.MaChatLieu = cl.MaChatLieu");
                    break;
            }

        }
        private void comboBoxSelectSearch_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            Query("");
        }
        private void buttonConnect_Click( object sender, RoutedEventArgs e ) {
            ConnectToDatabase();
        }
        private void buttonDisconnect_Click( object sender, RoutedEventArgs e ) {
            connection.Close();
            buttonConnect.IsEnabled = true;
            buttonDisconnect.IsEnabled = false;
            comboBoxSelectSearch.IsEnabled = false;
            comboBoxSelectSearch.SelectedIndex = -1;
            dataView.ItemsSource = null;
            dataView.Items.Refresh();
        }
    }
}
