using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;



namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        public Window2() {
            InitializeComponent();
        }
        private void napdulieu() {
            grdtpl.ItemsSource = null;
            if (conn.State != ConnectionState.Open) {
                return;
            }
            string sqlStr = "Select * from tblhang";
            SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "tblhang");
            dataTable = dataSet.Tables["tblhang"];
            grdtpl.ItemsSource = dataTable.DefaultView;
        }
        private void phanloai_SelectionChanged( object sender, SelectionChangedEventArgs e ) {

        }

        private void Window_Closed( object sender, EventArgs e ) {
            ConnectionStrin = @"Data Source=.\PHUONGNGU;Initial Catalog=qlch;Integrated Security=True;";
            conn.ConnectionString = ConnectionStrin;
            conn.Open();

            napdulieu();
        }
    }
}
