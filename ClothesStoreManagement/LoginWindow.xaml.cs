using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
        }

        MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
        SqlConnection connection = new SqlConnection();
        DataRow[] data = null;
        private void accountWindow_ContentRendered( object sender, EventArgs e ) {
            ConnectToDatabase();
        }
        private void ConnectToDatabase() {
            connection.ConnectionString = @"Data Source=.;Initial Catalog=DatabaseUsers;Integrated Security=True;";
            try {
                connection.Open();
                if (connection.State == ConnectionState.Open) {
                    UpdateData();
                    EnableLogInMode();
                }
            }
            catch (Exception) {
                if (connection.State != ConnectionState.Open) {
                    MessageBox.Show("Không thể kết nối với Cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
        }

        private void UpdateData() {
            string GetTableQuery = "select * from Users";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(GetTableQuery, connection);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            DataTable dataTable = dataSet.Tables[0];
            data = new DataRow[dataTable.Rows.Count];
            dataTable.Rows.CopyTo(data, 0);
        }

        private bool CheckLogIn( string username, string password ) {
            if (data == null)
                return false;
            UpdateData(); // get latest data
            string id = username + password;
            foreach (var item in data) {
                if (id == item[0].ToString())
                    return true;
            }
            return false;
        }

        private void buttonLogin_Click( object sender, RoutedEventArgs e ) {
            if (textboxUsername.Text == string.Empty || textboxUsername.Text == "Username" || passwordboxPassword.Password == string.Empty) {
                MessageBox.Show("username or password is empty");
                return;
            }
            if (CheckLogIn(textboxUsername.Text, passwordboxPassword.Password)) {
                mainWindow.LoggedInState(true, textboxUsername.Text);
                this.Close();
            }
            else {
                MessageBox.Show("Account doesn't exist.");
            }
        }

        private void EnableLogInMode() {
            buttonLogin.IsEnabled = true;
            buttonSignUp.Margin = new Thickness(0, 290, 0, 0);
            buttonBack.Visibility = Visibility.Collapsed;
            textboxUsername.Text = "Username";
            passwordboxPassword.Password = string.Empty;
            this.Title = "Login";
            labelTitle.Content = "USER LOGIN";
            textboxHelp.Visibility = Visibility.Collapsed;
            textboxUsername.Focus();
        }

        private void EnableSignUpMode() {
            buttonLogin.IsEnabled = false;
            buttonSignUp.Margin = new Thickness(0, 255, 0, 0);
            buttonBack.Visibility = Visibility.Visible;
            textboxUsername.Text = "Username";
            passwordboxPassword.Password = string.Empty;
            this.Title = "Sign Up";
            labelTitle.Content = "SIGN UP";
            textboxHelp.Text = string.Empty;
            textboxHelp.Visibility = Visibility.Visible;
            textboxUsername.Focus();
        }

        private void buttonBack_Click( object sender, RoutedEventArgs e ) {
            if (!buttonLogin.IsEnabled) {
                EnableLogInMode();
            }
        }

        private void buttonSignUp_Click( object sender, RoutedEventArgs e ) {
            // before: Margin = "0,290,0,0"
            // after: Margin="0,255,0,0"
            if (buttonLogin.IsEnabled) { // redirect to sign up
                EnableSignUpMode();
            }
            else { // is signing up
                if (!Utils.IsUsernameValid(textboxUsername.Text, data)) {
                    textboxUsername.Focus();
                    return;
                }
                if (!Utils.IsPasswordValid(passwordboxPassword.Password)) {
                    passwordboxPassword.Focus();
                    return;
                }
                string id = textboxUsername.Text + passwordboxPassword.Password;
                string insertStatement = "insert into Users (ID,Username,Password) values " +
                    "(N'" + id + "'," +
                    "N'" + textboxUsername.Text + "'," +
                    "N'" + passwordboxPassword.Password + "')";
                new SqlCommand(insertStatement, connection).ExecuteNonQuery();
                EnableLogInMode();
            }
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
