using System;
using System.Windows;

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void mnchatlieu_Click( object sender, RoutedEventArgs e ) {
            Window1 window1 = new Window1();
            window1.ShowDialog();
        }

        private void mnhanvien_Click( object sender, RoutedEventArgs e ) {
            nhanvien nv = new nhanvien();
            nv.ShowDialog();
        }

        private void mnhanghoa_Click( object sender, RoutedEventArgs e ) {
            hanghoa hh = new hanghoa();
            hh.ShowDialog();
        }

        private void mnkhachhang_Click( object sender, RoutedEventArgs e ) {
            khachhang kh = new khachhang();
            kh.ShowDialog();
        }

        private void mnhoadonban_Click( object sender, RoutedEventArgs e ) {
            hoadonbanhang hdbd = new hoadonbanhang();
            hdbd.ShowDialog();
        }

        private void mnhientimkiem_Click( object sender, RoutedEventArgs e ) {
            timkiemhoadon tkhd = new timkiemhoadon();
            tkhd.ShowDialog();
        }

        private void mnhang_Click( object sender, RoutedEventArgs e ) {
            timkiemhang tkhd = new timkiemhang();
            tkhd.ShowDialog();
        }

        private void mntimkiemkhachhang_Click( object sender, RoutedEventArgs e ) {
            timkiemkhachang tkhd = new timkiemkhachang();
            tkhd.ShowDialog();
        }

        private void MenuItem_Click( object sender, RoutedEventArgs e ) {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) {
                // Thực hiện hành động khi người dùng chọn Yes
                this.Close();
            }
            else {
                // Thực hiện hành động khi người dùng chọn No
            }
            Environment.Exit(0);
        }

        private void mnhientrogiup_Click( object sender, RoutedEventArgs e ) {

            trogiup tg = new trogiup();
            tg.Show();
            this.Close();
        }
    }
}
