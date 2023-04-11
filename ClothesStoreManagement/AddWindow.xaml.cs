using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace ClothesStoreManagement {
    /// <summary>
    /// Window for Adding data to database
    /// </summary>
    public partial class AddWindow : Window {
        public AddWindow() {
            InitializeComponent();
        }
        SqlConnection connection = ( (MainWindow) Application.Current.MainWindow ).connection;

        public string mode = "";

        private void Button_Click( object sender, RoutedEventArgs e ) {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=QlyShopQuanAo;Integrated Security=True;";
            try {
                connection.Open();
            }
            catch {
                if (connection.State != ConnectionState.Open) {
                    MessageBox.Show("Couldn't establish a connection to Database");
                    return;
                }
            }
            try {
                string GetTableQuery = "select * from ChatLieu";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                DataTable dataTable = dataSet.Tables[0];
                ( (MainWindow) Application.Current.MainWindow ).dataView.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
