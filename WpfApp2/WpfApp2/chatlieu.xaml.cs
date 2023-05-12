using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public Window1() {
            InitializeComponent();
        }

        private void napdulieu() {
            grdt.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblchatlieu";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblchatlieu");
            dataTable = dataSet.Tables["tblchatlieu"];
            grdt.ItemsSource = dataTable.DefaultView;
        }

        private void frm_chatlieu_Loaded( object sender, RoutedEventArgs e ) {
            ConnectionStrin = @"Data Source=.;Initial Catalog=qlchn;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }


        private void bt_them_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Insert Into tblchatlieu(ID, tenchatlieu)values('" + txt_machatlieu.Text + "','" + txt_tenchatlieu.Text + "')";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("Mã chất liệu không hợp lệ, nhập lại!!!");
            }
            bt_lammoi_Click(sender, e);

        }


        private void bt_sua_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Update tblchatlieu Set ID ='" + txt_machatlieu.Text + "', tenchatlieu = '" + txt_tenchatlieu.Text + "' where ID = '" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
            bt_lammoi_Click(sender, e);
        }

        private void bt_xoa_Click( object sender, RoutedEventArgs e ) {
            try {
                string sqlStr = "";
                sqlStr = "Delete from tblchatlieu where ID ='" + selectedID + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.ExecuteNonQuery();
                napdulieu();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
            bt_lammoi_Click(sender, e);

        }


        private void bt_boqua_Click( object sender, RoutedEventArgs e ) {

            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }

        }

        private void grdt_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            try {
                if (grdt.CurrentItem == null) { return; }
                DataRowView row = (DataRowView) grdt.CurrentItem;
                selectedID = row[0].ToString();
                txt_machatlieu.Text = row[0].ToString();
                txt_tenchatlieu.Text = row[1].ToString();
            }
            catch (Exception) {
                MessageBox.Show("ERROR!!!");
            }
        }

        private void bt_lammoi_Click( object sender, RoutedEventArgs e ) {
            txt_machatlieu.Text = "";
            txt_tenchatlieu.Text = "";
        }
    }
}
