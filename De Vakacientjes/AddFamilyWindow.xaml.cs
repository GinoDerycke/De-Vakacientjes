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
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Je hebt nog geen naam ingevuld.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (VakacientjesDb.AddFamily(txtName.Text, txtEmail.Text) == true)
            {
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
