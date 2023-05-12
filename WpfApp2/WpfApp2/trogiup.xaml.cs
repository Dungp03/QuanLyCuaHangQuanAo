using System.Windows;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for trogiup.xaml
    /// </summary>
    public partial class trogiup : Window {
        public trogiup() {
            InitializeComponent();
        }

        private void Button_Click( object sender, RoutedEventArgs e ) {

            MainWindow main = new MainWindow();
            main.Show();
            this.Close();


        }
    }
}
