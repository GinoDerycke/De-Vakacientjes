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
    /// Interaction logic for AddChildWindow.xaml
    /// </summary>
    public partial class AddChildWindow : Window
    {
        public AddChildWindow(string firstName, string lastName)
        {
            InitializeComponent();

            txtFirstName.Text = firstName;
            txtLastName.Text = lastName;

            var familyList = VakacientjesDb.GetFamilies();
            foreach (Family family in familyList)
                cmbFamily.Items.Add(family.Name);
        }

        private void BtnAddFamily_Click(object sender, RoutedEventArgs e)
        {
            AddFamilyWindow addFamilyWindow = new AddFamilyWindow(txtLastName.Text);
            var res = addFamilyWindow.ShowDialog();

            if (res == true)
            {
                var familyList = VakacientjesDb.GetFamilies();
                int maxId = -1;
                int selectedIndex = -1;
                cmbFamily.Items.Clear();
                foreach (Family family in familyList)
                {
                    if (family.Id > maxId)
                    {
                        maxId = family.Id;
                        selectedIndex = familyList.IndexOf(family);
                    }
                    cmbFamily.Items.Add(family.Name);
                }
                cmbFamily.SelectedIndex = selectedIndex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtFirstName.Text.Trim() == "")
            {
                MessageBox.Show("Je hebt nog geen voornaam ingevuld.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("Je hebt nog geen familienaam ingevuld.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbFamily.Text == "")
            {
                MessageBox.Show("Je hebt nog geen familie geselecteerd.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Family family = VakacientjesDb.GetFamily(cmbFamily.Text);

            if (family.Id == -1)
            {
                MessageBox.Show($"Familie '{cmbFamily.Text}' niet gevonden.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (VakacientjesDb.AddChild(family.Id, txtFirstName.Text.Trim(), txtLastName.Text.Trim()) == true)
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

        private void BtnSwitchNames_Click(object sender, RoutedEventArgs e)
        {
            string temp = txtLastName.Text;
            txtLastName.Text = txtFirstName.Text;
            txtFirstName.Text = temp;
        }
    }
}
