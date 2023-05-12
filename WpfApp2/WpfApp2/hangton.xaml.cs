using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for hangton.xaml
    /// </summary>
    public partial class hangton : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public hangton() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdtpl.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select MaHang, TenHang,MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, GhiChu,CONVERT(varchar, ngaynhap, 103) AS ngaynhap from tblhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdtpl.ItemsSource = dataTable.DefaultView;
        }
        private void DataGrid_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdtpl.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdtpl.CurrentItem;
                selectedID = row[0].ToString();
                phanloai.Text = row[0].ToString();
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

        private void phanloai_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            ComboBox comboBox = (ComboBox) sender;
            ComboBoxItem selectedItem = (ComboBoxItem) comboBox.SelectedItem;
            string ghiChu = selectedItem.Content.ToString();

            grdtpl.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }

            string sql;
            if (ghiChu == "Còn Hàng") {
                sql = "SELECT * FROM tblhang WHERE GhiChu = 'Còn Hàng'";
            }
            else {
                sql = "SELECT * FROM tblhang WHERE GhiChu = 'Het hàng'";
            }

            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdtpl.ItemsSource = dataTable.DefaultView;
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {
            napdulieu();
        }
    }
}
