using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace De_Vakacientjes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DataSet xlsxDataSet;

        public object MessageBoxButtons { get; private set; }

        private bool AnalyzeData(int weekNr)
        {
            int tableNr = xlsxDataSet.Tables.IndexOf($"Week {weekNr}");

            if (tableNr == -1) return false;

            DataTable table = xlsxDataSet.Tables[tableNr];

            grdTest.ItemsSource = table.AsDataView();

            int colMaandag = -1;
            int colDinsdag = -1;
            int colWoensdag = -1;
            int colDonderdag = -1;
            int colVrijdag = -1;
            bool validDayColumns = false;

            foreach (DataRow row in table.Rows)
            {
                bool child = false;
                bool parent = false;
                string name = "";
                string startTime = "";
                string endTime = "";
                bool newDay = false;
                bool morning = false;
                bool afterNoon = false;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    string fieldValue = row.Field<String>(i);

                    if (String.IsNullOrWhiteSpace(fieldValue) == false)
                    {
                        if (validDayColumns == true)
                        {
                            if ((i == colMaandag) || (i == colDinsdag) || (i == colWoensdag) || (i == colDonderdag) || (i == colVrijdag))
                                newDay = true;

                            if (newDay)
                            {
                                child = false;
                                parent = false;
                                name = "";
                                startTime = "";
                                endTime = "";
                                newDay = false;
                                morning = false;
                                afterNoon = false;
                            }

                            if (fieldValue.StartsWith("Kind", StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                child = true;
                                continue;
                            }

                            if (fieldValue.StartsWith("Ouder", StringComparison.InvariantCultureIgnoreCase) == true)
                            {
                                parent = true;
                                continue;
                            }

                            if (((child == true) || (parent == true)) && (name == ""))
                            {
                                name = fieldValue;
                                continue;
                            }

                            if ((name != "") && (startTime == ""))
                            {
                                startTime = fieldValue;
                                continue;
                            }

                            if ((startTime != "") && (endTime == ""))
                            {
                                endTime = fieldValue;

                                //Determine morning and afternoon
                                string hourString = "";
                                int p = startTime.IndexOf("u", StringComparison.InvariantCultureIgnoreCase);
                                if (p == -1)
                                    p = startTime.IndexOf("h", StringComparison.InvariantCultureIgnoreCase);
                                if (p == -1)
                                    p = startTime.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                                hourString = startTime.Substring(0, p);
                                
                                int hour = 0;
                                if (String.IsNullOrWhiteSpace(hourString) == false)
                                    hour = Convert.ToInt32(hourString);

                                if (hour < 12)
                                    morning = true;

                                p = endTime.IndexOf("u", StringComparison.InvariantCultureIgnoreCase);
                                if (p == -1)
                                    p = endTime.IndexOf("h", StringComparison.InvariantCultureIgnoreCase);
                                if (p == -1)
                                    p = endTime.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                                hourString = endTime.Substring(0, p);
                                if (String.IsNullOrWhiteSpace(hourString) == false)
                                    hour = Convert.ToInt32(hourString);
                                else
                                    hour = 0;

                                if (hour > 13)
                                    afterNoon = true;

                                //
                                if (child)
                                {
                                    MessageBox.Show($"Kind: \n{name}\n{startTime}\n{endTime}\n{morning}\n{afterNoon}");
                                }
                                else
                                {
                                    MessageBox.Show($"Ouder: \n{name}\n{startTime}\n{endTime}\n{morning}\n{afterNoon}");
                                }
                            }
                        }
                        else
                        {
                            //Find column with days
                            if (fieldValue.Equals("MAANDAG", StringComparison.InvariantCultureIgnoreCase) == true)
                                colMaandag = i;
                            if (fieldValue.Equals("DINSDAG", StringComparison.InvariantCultureIgnoreCase) == true)
                                colDinsdag = i;
                            if (fieldValue.Equals("WOENSDAG", StringComparison.InvariantCultureIgnoreCase) == true)
                                colWoensdag = i;
                            if (fieldValue.Equals("DONDERDAG", StringComparison.InvariantCultureIgnoreCase) == true)
                                colDonderdag = i;
                            if (fieldValue.Equals("VRIJDAG", StringComparison.InvariantCultureIgnoreCase) == true)
                                colVrijdag = i;
                        }

                    }
                }

                if (validDayColumns == false)
                {
                    validDayColumns = ((colMaandag != -1) || (colDinsdag != -1) || (colWoensdag != -1) || (colDonderdag != -1) || (colVrijdag != -1));
                    if (validDayColumns == true)
                    {
                        validDayColumns = ((colMaandag != -1) && (colDinsdag != -1) && (colWoensdag != -1) && (colDonderdag != -1) && (colVrijdag != -1));
                        if (validDayColumns == false)
                        {
                            MessageBox.Show($"Niet alle dagen zijn gevonden in week {weekNr}.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                xlsxDataSet = excelReader.AsDataSet();

                //foreach (DataTable dt in xlsxDataSet.Tables)
                //    MessageBox.Show(dt.TableName);

                excelReader.Close();

                AnalyzeData(2);
            }
        }
    }
}
