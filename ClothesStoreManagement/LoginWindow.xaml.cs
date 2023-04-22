using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Media;

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
                    Close();
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
            string id = username + Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            foreach (var item in data) {
                if (username == item[1].ToString()) {
                    if (id == item[0].ToString())
                        return true;
                    else {
                        MessageBox.Show("Nhập sai mật khẩu", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
            }
            MessageBox.Show("Tài khoản này không tồn tại.\nHãy đăng ký.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return false;
        }
        private void buttonLogin_Click( object sender, RoutedEventArgs e ) {
            textboxUsername.Text = textboxUsername.Text.Trim();
            if (textboxUsername.Text == string.Empty) {
                MessageBox.Show("Tên đăng nhập không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (passwordboxPassword.Password == string.Empty) {
                MessageBox.Show("Mật khẩu không được để trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (CheckLogIn(textboxUsername.Text, passwordboxPassword.Password)) {
                mainWindow.LoggedInState(true, textboxUsername.Text);
                this.Close();
            }
        }
        private void EnableLogInMode() {
            buttonLogin.IsEnabled = true;
            buttonSignUp.Margin = new Thickness(0, 290, 0, 0);
            buttonSignUp.IsDefault = false;
            buttonLogin.IsDefault = true;
            buttonBack.Visibility = Visibility.Collapsed;
            textboxUsername.Text = string.Empty;
            passwordboxPassword.Password = string.Empty;
            this.Title = "Đăng nhập";
            labelTitle.Content = "ĐĂNG NHẬP";
            textboxHelp.Visibility = Visibility.Collapsed;
            textboxUsername.Focus();
        }
        private void EnableSignUpMode() {
            buttonLogin.IsEnabled = false;
            buttonSignUp.Margin = new Thickness(0, 255, 0, 0);
            buttonSignUp.IsDefault = true;
            buttonLogin.IsDefault = false;
            buttonBack.Visibility = Visibility.Visible;
            textboxUsername.Text = string.Empty;
            passwordboxPassword.Password = string.Empty;
            this.Title = "Đăng ký";
            labelTitle.Content = "ĐĂNG KÝ";
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
            try {
                if (buttonLogin.IsEnabled) { // redirect to sign up
                    EnableSignUpMode();
                }
                else { // is signing up
                    textboxUsername.Text = textboxUsername.Text.Trim();
                    if (!Utils.IsUsernameValid(textboxUsername.Text, data)) {
                        textboxUsername.Focus();
                        return;
                    }
                    if (!Utils.IsPasswordValid(passwordboxPassword.Password)) {
                        passwordboxPassword.Focus();
                        return;
                    }
                    // base64 to base string length ratio: 4*[base.length / 3]
                    string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(passwordboxPassword.Password));
                    string id = textboxUsername.Text + encodedPassword;
                    string insertStatement = "insert into Users (ID,Username,Password) values " +
                        "(N'" + id + "'," +
                        "N'" + textboxUsername.Text + "'," +
                        "N'" + encodedPassword + "')";
                    new SqlCommand(insertStatement, connection).ExecuteNonQuery();
                    EnableLogInMode();
                }
            }
            catch (Exception) {
                MessageBox.Show("Đã xảy ra lỗi khi đăng ký.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void textboxUsername_GotFocus( object sender, RoutedEventArgs e ) {
            textboxHelp.Text = "Tên đăng nhập không chứa quá 75 ký tự";
        }
        private void passwordboxPassword_GotFocus( object sender, RoutedEventArgs e ) {
            textboxHelp.Text = "Mật khẩu phải chứa từ 8 đến 30 ký tự.\n" +
                               "Mật khẩu phải chứa:\n" +
                               @" - 1 ký tự đặc biệt: ~`!@#$%^&*()_-+={[}]|\:;'<,>.?/" + $"{'"'}\n" +
                               " - 1 chữ số: 0-9";
        }
        private void loginWindow_Closed( object sender, EventArgs e ) {
            mainWindow.loginWindow = null;
        }
    }
}
