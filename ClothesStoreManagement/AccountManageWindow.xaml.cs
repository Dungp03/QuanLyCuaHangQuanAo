using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClothesStoreManagement {
    /// <summary>
    /// Interaction logic for AccountDetailsWindow.xaml
    /// </summary>
    public partial class AccountManageWindow : Window {
        public AccountManageWindow() {
            InitializeComponent();
        }

        private MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
        private string currentUser = string.Empty;
        private SqlConnection connection = new SqlConnection();
        private DataRow userData = null;
        DataRow[] data = null;
        private bool isModifyingUsername = false;

        private void accountManageWindow_Loaded( object sender, RoutedEventArgs e ) {
            labelAccountInfo.Content = currentUser = mainWindow.currentUsername;
            textboxUsername.Visibility = Visibility.Collapsed;
            passwordboxPassword.Visibility = Visibility.Collapsed;
            buttonConfirm.Visibility = Visibility.Collapsed;
            buttonCancel.Visibility = Visibility.Collapsed;
            textboxHelp.Visibility = Visibility.Collapsed;
        }
        private void accountManageWindow_ContentRendered( object sender, EventArgs e ) {
            ConnectToDatabase();
        }
        private void ConnectToDatabase() {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=DatabaseUsers;Integrated Security=True;";
            try {
                connection.Open();
                if (connection.State == ConnectionState.Open) {
                    GetUserData();
                }
            }
            catch (Exception) {
                if (connection.State != ConnectionState.Open) {
                    MessageBox.Show("Không thể kết nối với Cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void GetUserData() {
            string GetTableQuery = "select * from Users";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            DataTable dataTable = dataSet.Tables[0];

            userData = dataTable.Select("ID like '" + currentUser + "%'")[0];

            data = new DataRow[dataTable.Rows.Count];
            dataTable.Rows.CopyTo(data, 0);

            labelAccountInfo.Content = currentUser = mainWindow.currentUsername;
        }
        private void ModifyMode( bool isModifying ) {
            foreach (var element in grid.Children)
                if (element is Button bt) {
                    if (isModifying)
                        bt.Visibility = Visibility.Collapsed;
                    else
                        bt.Visibility = Visibility.Visible;
                }
            if (isModifying) {
                if (isModifyingUsername) {
                    textboxUsername.Visibility = Visibility.Visible;
                    textboxUsername.Focus();
                }
                else {
                    passwordboxPassword.Visibility = Visibility.Visible;
                    passwordboxPassword.Focus();
                }
                buttonConfirm.Visibility = Visibility.Visible;
                buttonCancel.Visibility = Visibility.Visible;
                textboxHelp.Visibility = Visibility.Visible;
            }
            else {
                textboxUsername.Visibility = Visibility.Collapsed;
                passwordboxPassword.Visibility = Visibility.Collapsed;
                buttonConfirm.Visibility = Visibility.Collapsed;
                buttonCancel.Visibility = Visibility.Collapsed;
                textboxHelp.Visibility = Visibility.Collapsed;
            }
        }

        private void buttonModifyUsername_Click( object sender, RoutedEventArgs e ) {
            isModifyingUsername = true;
            ModifyMode(true);
        }

        private void buttonModifyPassword_Click( object sender, RoutedEventArgs e ) {
            isModifyingUsername = false;
            ModifyMode(true);
        }
        private void buttonConfirm_Click( object sender, RoutedEventArgs e ) {
            try {
                if (isModifyingUsername) {
                    string newUsername = textboxUsername.Text;
                    if (!Utils.IsUsernameValid(newUsername, data)) {
                        textboxUsername.Focus();
                        return;
                    }
                    currentUser = newUsername;
                    mainWindow.LoggedInState(true, newUsername);
                    string newID = newUsername + userData[2];
                    string updateStatement = $"update Users set ID=N'{newID}',Username=N'{newUsername}',Password=N'{userData[2]}' where ID=N'{userData[0]}'";
                    new SqlCommand(updateStatement, connection).ExecuteNonQuery();
                }
                else {
                    string newPassword = passwordboxPassword.Password;
                    if (!Utils.IsPasswordValid(newPassword)) {
                        passwordboxPassword.Focus();
                        return;
                    }
                    string newID = userData[1] + newPassword;
                    string updateStatement = $"update Users set ID=N'{newID}',Username=N'{userData[1]}',Password=N'{newPassword}' where ID=N'{userData[0]}'";
                    new SqlCommand(updateStatement, connection).ExecuteNonQuery();
                }
                GetUserData();
                ModifyMode(false);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonCancel_Click( object sender, RoutedEventArgs e ) {
            ModifyMode(false);
        }

        private void buttonDelete_Click( object sender, RoutedEventArgs e ) {
            if (MessageBox.Show("Delete this account?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try {
                string deleteStatement = $"delete from Users where ID=N'{userData[0]}'";
                new SqlCommand(deleteStatement, connection).ExecuteNonQuery();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            mainWindow.LoggedInState(false, "Not logged in");
            Close();
        }
        private void textboxUsername_GotFocus( object sender, RoutedEventArgs e ) {
            if (textboxUsername.Text == "Username") {
                textboxUsername.Foreground = new SolidColorBrush(Colors.Black);
                textboxUsername.Text = string.Empty;
            }
            textboxHelp.Text = "Username has less than 75 characters.";
        }

        private void textboxUsername_LostFocus( object sender, RoutedEventArgs e ) {
            if (textboxUsername.Text == string.Empty) {
                textboxUsername.Foreground = new SolidColorBrush(Colors.Gray);
                textboxUsername.Text = "Username";
            }
        }

        private void passwordboxPassword_GotFocus( object sender, RoutedEventArgs e ) {
            textboxHelp.Text = "Password has between 8 and 30 characters.\n" +
                               "Password contains atleast:\n" +
                               @" - A special character: ~`!@#$%^&*()_-+={[}]|\:;'<,>.?/" + $"{'"'}\n" +
                               " - A number: 0-9";
        }

    }
}
