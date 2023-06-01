using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp2 {
    /// <summary>fv=
    /// Interaction logic for nhanvien.xaml
    /// </summary>
    public partial class nhanvien : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStr = "";
        string ID = "";
        DataTable dataTable = null;
        public nhanvien() {
            InitializeComponent();
        }

        private void napdulieu() {
            grdtnv.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblnhanvien";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblnhanvien");
            dataTable = dataSet.Tables["tblnhanvien"];
            grdtnv.ItemsSource = dataTable.DefaultView;
        }

        private void nv_them_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";


                sqlStr = "Insert Into tblnhanvien(MaNhanVien,TenNhanVien,GioiTinh,DiaChi,DienThoai,NgaySinh)values('" + manv.Text + "','" +
                tennv.Text + "','" + gt.Text + "','" + diachi.Text + "','" + sodt.Text + "','" + ngaysinh.Text + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("Không hợp lệ, nhập lại!!!");
            }
            lammoi_Click(sender, e);
        }

        /* private void nv_sua_Click(object sender, RoutedEventArgs e)
         {
             string sqlStr = "";
             string selectedContent = (gt.SelectedItem as ComboBoxItem).Content.ToString();

             sqlStr = "update tblnhanvien set MaNhanVien = '" + manv.Text + "',TenNhanVien = '" +
             tennv.Text + "',GioiTinh = '" + selectedContent + "', DiaChi = '" + diachi.Text + "', DienThoai = '" +
             "'" + sodt + "', NgaySinh = '" + ngaysinh.Text + "' where MaNhanVien = '" + ID + "'";
             SqlCommand cmd = new SqlCommand(sqlStr, conn);
             cmd.ExecuteNonQuery();
             napdulieu();
         }*/
        private void nv_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string selectedContent = "";
                if (gt.SelectedItem is ComboBoxItem comboBoxItem) {
                    selectedContent = comboBoxItem.Content.ToString();
                }

                // Sử dụng tham số truy vấn để tạo câu lệnh SQL an toàn
                string sqlStr = "update tblnhanvien set MaNhanVien = @MaNhanVien, TenNhanVien = @TenNhanVien, GioiTinh = @GioiTinh, DiaChi = @DiaChi, DienThoai = @DienThoai, NgaySinh = @NgaySinh where MaNhanVien = @ID";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);

                // Thêm các tham số truy vấn
                cmd.Parameters.AddWithValue("@MaNhanVien", manv.Text);
                cmd.Parameters.AddWithValue("@TenNhanVien", tennv.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", selectedContent);
                cmd.Parameters.AddWithValue("@DiaChi", diachi.Text);
                cmd.Parameters.AddWithValue("@DienThoai", sodt.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", ngaysinh.Text);
                cmd.Parameters.AddWithValue("@ID", ID);

                // Thực thi câu lệnh SQL
                cmd.ExecuteNonQuery();

                // Nạp lại dữ liệu
                napdulieu();
            }
            catch (Exception ex) {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            lammoi_Click(sender, e);
        }

        private void grdtnv_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdtnv.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdtnv.CurrentItem;
                ID = row[0].ToString();
                manv.Text = row[0].ToString();
                tennv.Text = row[1].ToString();
                /*ComboBoxItem selected = gt.SelectedItem as ComboBoxItem;
                if (selected != null)
                {
                    // Thiết lập giá trị của ComboBoxItem
                    selected.Content = row[2].ToString();
                }*/
                gt.Text = row[2].ToString();
                diachi.Text = row[3].ToString();
                sodt.Text = row[4].ToString();
                ngaysinh.Text = row[5].ToString();
            }
            catch (Exception) {
                MessageBox.Show("ERROR");
            }
        }

        private void nv_dong_Click( object sender, RoutedEventArgs e ) {
            this.Close();
        }

        private void nv_xoa_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblnhanvien where MaNhanVien ='" + ID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR");
            }
        }

        private void lammoi_Click( object sender, RoutedEventArgs e ) {
            // string selectedContent = "";
            /*   if (gt.SelectedItem is ComboBoxItem comboBoxItem)
               {
                   selectedContent = comboBoxItem.Content.ToString();
               } */
            manv.Text = "";
            tennv.Text = "";
            gt.Text = "";
            sodt.Text = "";
            diachi.Text = "";
            ngaysinh.Text = "";
        }

        private void frm_nhanvien_Loaded_1( object sender, RoutedEventArgs e ) {
            ConnectionStr = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStr;
            conn.Open();

            napdulieu();
        }
    }
}

