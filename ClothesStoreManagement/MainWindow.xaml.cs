using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace ClothesStoreManagement {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public SqlConnection connection = new SqlConnection();
        private Table? CurrentTable = null;

        private void Window_Loaded( object sender, RoutedEventArgs e ) {
            Utils.HideAllMenu();
            Utils.HideButtons();
            Utils.DisableButtons();
        }

        private void Window_ContentRendered( object sender, EventArgs e ) {
            ConnectToDatabase();
            foreach (Table table in Enum.GetValues(typeof(Table)))
                comboBoxSelectTable.Items.Add(table);
            LoadData();
            Utils.DisableAllFields();
        }
        private void ConnectToDatabase() {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=QlyShopQuanAo;Integrated Security=True;";
            try {
                connection.Open();
            }
            catch (Exception) {
                if (connection.State != ConnectionState.Open)
                    MessageBox.Show("Couldn't connect to Database");
            }
        }
        private void LoadData() {
            if (connection.State != ConnectionState.Open || CurrentTable == null)
                return;
            GetTable(CurrentTable.ToString());
        }
        private void GetTable( string tableName ) {
            string GetTableQuery = "select * from " + tableName;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            DataTable dataTable = dataSet.Tables[0];
            dataView.ItemsSource = dataTable.DefaultView;
        }
        private void comboBoxSelectTable_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            //MessageBox.Show(Table.KhachHang.ToString() );
            Utils.ChangeButtonState(false);
            CurrentTable = (Table) Enum.ToObject(typeof(Table), comboBoxSelectTable.SelectedIndex);
            GetTable(CurrentTable.ToString());
            Utils.UnloadAllFields();
            Utils.LoadMenu(CurrentTable);
            foreach (var column in dataView.Columns) {
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }

        private void dataView_SelectionChanged( object sender, SelectionChangedEventArgs e ) {
            DataRowView row = (DataRowView) dataView.CurrentItem;
            if ( row == null ) {
                Utils.DisableButtons();
                return;
            }
            Utils.LoadFields(CurrentTable, row);
            Utils.EnableButtons();
        }

        private void InsertData() {
            Utils.EnableAllFields();
            Utils.ChangeButtonState(true);
            string[] data = Utils.GetFields(CurrentTable);
            string dataString = string.Empty;
            foreach (string field in data)
                dataString+= field + "\n";
            MessageBox.Show(dataString);
        }
        private void ModifyData() {
            Utils.EnableAllFields();
            Utils.ChangeButtonState(true);
            string[] data = Utils.GetFields(CurrentTable);
            string dataString = string.Empty;
            foreach (string field in data)
                dataString+= field + "\n";
            MessageBox.Show(dataString);
        }
        private void DeleteData() {

        }

        private void ActionButton_Click( object sender, RoutedEventArgs e ) {
            Button actionButton = (Button) sender;
            switch (actionButton.Name) {
                case "buttonInsert":
                    InsertData();
                    break;
                case "buttonModify":
                    ModifyData();
                    break;
                case "buttonDelete":
                    DeleteData();
                    break;
            }
        }

        private void EditingButton_Click( object sender, RoutedEventArgs e ) {
            if (((Button) sender).Name == "buttonConfirm") {
                string[] data = Utils.GetFields(CurrentTable);
                string dataString = string.Empty;
                foreach (string field in data)
                    dataString += field + "\n";
                MessageBox.Show(dataString); 
            }
            Utils.ChangeButtonState(false);
        }

        // currently this is for reference only
        //private void Insert_Click( object sender, RoutedEventArgs e ) {
        //    try {
        //        string InsertString = "insert into ChiTietHoaDon (MaHDBan,MaHang,SoLuong,DonGia,GiamGia,ThanhTien)" +
        //            "values (N'mahd1',N'mahang1',2,34000,0.1,30600);";
        //        new SqlCommand(InsertString, connection).ExecuteNonQuery();
        //    }
        //    catch (SqlException ex) {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    catch (Exception) {
        //        MessageBox.Show("other err");
        //    }

        //    if (connection.State != ConnectionState.Open) {
        //        MessageBox.Show("Couldn't establish a connection to Database");
        //        return;
        //    }
        //    try {
        //        string GetTableQuery = "select * from ChiTietHoaDon";
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
        //        DataSet dataSet = new DataSet();
        //        sqlDataAdapter.Fill(dataSet);
        //        DataTable dataTable = dataSet.Tables[0];
        //        ( (MainWindow) Application.Current.MainWindow ).dataView.ItemsSource = dataTable.DefaultView;
        //    }
        //    catch (Exception ex) {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
    }
}
