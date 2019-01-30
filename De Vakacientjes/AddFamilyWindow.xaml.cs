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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace De_Vakacientjes
{
    /// <summary>
    /// Interaction logic for AddFamilyWindow.xaml
    /// </summary>
    public partial class AddFamilyWindow : Window
    {
        public AddFamilyWindow(string lastName)
        {
            InitializeComponent();

            txtName.Text = lastName;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Application.Current.Resources["MySQLConn"].ToString());
            conn.Open();

            string sql = $"insert into family(name, email) values ('{txtName.Text}', '{txtEmail.Text}')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            conn.Close();

            Close();
        }
    }
}
