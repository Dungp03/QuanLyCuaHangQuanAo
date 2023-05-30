using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SqlClient;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for chonhang.xaml
    /// </summary>
    public partial class chonhang : Window
    {
        SqlConnection conn = new SqlConnection();
        string ConnectionStrin = "";
        string selectedID = "";
        DataTable dataTable = null;
        
        public chonhang()
        {

            InitializeComponent();
        }

      

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
              
        }

        private void mahang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dongia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sl.Text != "")
            {
                thanhtien.Text = (int.Parse(dongia.Text) * int.Parse(sl.Text)).ToString();
            }
        }

        private void sl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dongia.Text != "" && sl.Text != "")
            {
                thanhtien.Text = (int.Parse(sl.Text) * int.Parse(dongia.Text)).ToString();

            }
            ComboBoxItem item = mahang.SelectedItem as ComboBoxItem;
            if (mahang.SelectedItem != null)
            {
                string sqlStr = "select SoLuong from tblhang where MaHang = '" + item.Content.ToString() + "'";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) <= 0)
                    {
                        lbtb.Content = "Hết hàng";
                        them.IsEnabled = false;
                    }
                    else if (sl.Text != "")
                    {
                        if (reader.GetInt32(0) < int.Parse(sl.Text))
                        {
                            lbtb.Content = "Trong kho chỉ còn " + reader.GetInt32(0).ToString() + " sản phẩm";
                            them.IsEnabled = false;
                        }
                    }
                }
                reader.Close();
            }
            if (sl.Text == "")
            {
                lbtb.Content = "";
            }
        }
    }
}
