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
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class SelectChildWindow : Window
    {
        private string _firstName;
        private string _lastName;
        public bool abort = false;
               
        public SelectChildWindow(string firstName, string lastName)
        {
            InitializeComponent();

            _firstName = firstName;
            _lastName = lastName;

            lblChild.Content = "Kind "+ $"'{firstName + " " + lastName}'" + " niet gevonden.";

            var childList = VakacientjesDb.GetChildren();
            foreach (Child child in childList)
                cmbChild.Items.Add(child.FirstName + " " + child.LastName);
        }

        private void BtnAddChild_Click(object sender, RoutedEventArgs e)
        {
            AddChildWindow addChildWindow = new AddChildWindow(_firstName, _lastName);
            var res = addChildWindow.ShowDialog();

            if (res == true)
            {
                var childList = VakacientjesDb.GetChildren();
                int maxId = -1;
                int selectedIndex = -1;
                cmbChild.Items.Clear();
                foreach (Child child in childList)
                {
                    if (child.Id > maxId)
                    {
                        maxId = child.Id;
                        selectedIndex = childList.IndexOf(child);
                    }
                    cmbChild.Items.Add(child.FirstName + " " + child.LastName);
                }
                cmbChild.SelectedIndex = selectedIndex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (cmbChild.Text == "")
            {
                MessageBox.Show("Je hebt nog geen kind geselecteerd.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnIgnore_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnAbor_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            abort = true;
            Close();
        }
    }
}
