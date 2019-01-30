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
    /// Interaction logic for AddChildWindow.xaml
    /// </summary>
    public partial class AddChildWindow : Window
    {
        private void GetFamilies()
        {
            cmbFamily.Items.Clear();

            MySqlConnection conn = new MySqlConnection(Application.Current.Resources["MySQLConn"].ToString());
            conn.Open();

            string sql = "select * from family";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                cmbFamily.Items.Add(reader["name"].ToString());

            conn.Close();
        }

        public AddChildWindow(string firstName, string lastName)
        {
            InitializeComponent();

            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;

            GetFamilies();
        }

        private void BtnAddFamily_Click(object sender, RoutedEventArgs e)
        {
            AddFamilyWindow addFamilyWindow = new AddFamilyWindow(txtLastName.Text);
            var res = addFamilyWindow.ShowDialog();

            GetFamilies();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Application.Current.Resources["MySQLConn"].ToString());
            conn.Open();

            int familyId = -1;
            string sql = $"select * from family where name = '{cmbFamily.Text}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                familyId = Convert.ToInt32(reader["id"]);
            reader.Close();

            sql = $"insert into child(family_id, first_name, last_name) values ({familyId}, '{txtFirstName.Text}', '{txtLastName}')";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            conn.Close();

            Close();
        }
    }
}
