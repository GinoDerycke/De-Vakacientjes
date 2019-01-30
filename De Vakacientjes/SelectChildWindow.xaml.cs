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
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class SelectChildWindow : Window
    {
        private string _firstName;
        private string _lastName;

        public SelectChildWindow(string firstName, string lastName)
        {
            InitializeComponent();

            _firstName = firstName;
            _lastName = lastName;

            lblChild.Content = "Kind "+ $"{firstName + " " + lastName}" + " niet gevonden.";

            MySqlConnection conn = new MySqlConnection(Application.Current.Resources["MySQLConn"].ToString());
            conn.Open();

            string sql = "select * from child";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                cmbChild.Items.Add(reader["first_name"] + " " + reader["last_name"]);

            conn.Close();
        }

        private void BtnChild_Click(object sender, RoutedEventArgs e)
        {
            AddChildWindow addChildWindow = new AddChildWindow(_firstName, _lastName);
            var res = addChildWindow.ShowDialog();
        }
    }
}
