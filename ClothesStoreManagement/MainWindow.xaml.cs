using System;
using System.Windows;

namespace ClothesStoreManagement {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public bool isLoggedIn = false;
        public DatabaseWindow databaseWindow = null;
        public LoginWindow loginWindow = null;
        public string currentUsername = string.Empty;

        private void mainWindow_Loaded( object sender, RoutedEventArgs e ) {
            LoggedInState(false, "Chưa đăng nhập");
            buttonLogin.IsEnabled = true;
            buttonLogin.Visibility = Visibility.Visible;
        }
        public void LoggedInState( bool _isLoggedIn, string username ) {
            isLoggedIn = _isLoggedIn;
            buttonToDatabase.IsEnabled = _isLoggedIn;
            buttonLogOut.IsEnabled = _isLoggedIn;
            buttonLogOut.Visibility = _isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            buttonManageAccount.IsEnabled = _isLoggedIn;
            buttonManageAccount.Visibility = _isLoggedIn ? Visibility.Visible : Visibility.Collapsed;
            buttonLogin.IsEnabled = !_isLoggedIn;
            currentUsername = username;
            labelAccountInfo.Content = username;
        }
        private void buttonLogOut_Click( object sender, RoutedEventArgs e ) {
            LoggedInState(false, "Chưa đăng nhập");
        }
        private void buttonManageAccount_Click( object sender, RoutedEventArgs e ) {
            if (!isLoggedIn)
                return;
            try {
                AccountManageWindow accountManageWindow = new AccountManageWindow {
                    Owner = this
                };
                accountManageWindow.ShowDialog();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonToDatabase_Click( object sender, RoutedEventArgs e ) {
            try {
                Utils.databaseWindow = databaseWindow = new DatabaseWindow();
                databaseWindow.Owner = this;
                databaseWindow.ShowDialog();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        private void buttonLogin_Click( object sender, RoutedEventArgs e ) {
            try {
                loginWindow = new LoginWindow {
                    Owner = this
                };
                loginWindow.ShowDialog();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
        private void mainWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            foreach (Window window in Application.Current.Windows)
                if (window != Application.Current.MainWindow)
                    window.Close();
        }
    }
}
