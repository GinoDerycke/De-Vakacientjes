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

        private void GetFirstNameLastName(string name, out string firstName, out string lastName)
        {
            int p = name.IndexOf(" ", StringComparison.InvariantCultureIgnoreCase);
            if (p != -1)
            {
                firstName = name.Substring(0, p);
                lastName = name.Substring(p + 1, name.Length - p - 1);
            }
            else
            {
                firstName = name;
                lastName = "";
            }
        }

        private bool AnalyzeData(List<FamilyActivity> familyActivityList, int weekNumber)
        {
            int tableNr = xlsxDataSet.Tables.IndexOf($"Week {weekNumber}");

            if (tableNr == -1) return false;

            DataTable table = xlsxDataSet.Tables[tableNr];

            grdTest.ItemsSource = table.AsDataView();

            int colMaandag = -1;
            int colDinsdag = -1;
            int colWoensdag = -1;
            int colDonderdag = -1;
            int colVrijdag = -1;
            bool validDayColumns = false;
            bool abort = false;
            
            foreach (DataRow row in table.Rows)
            {
                bool child = false;
                bool parent = false;
                string name = "";
                string startTime = "";
                string endTime = "";
                bool newDay = false;
                int dayNumber = -1;
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

                                if (i == colMaandag)
                                    dayNumber = 1;
                                if (i == colDinsdag)
                                    dayNumber = 2;
                                if (i == colWoensdag)
                                    dayNumber = 3;
                                if (i == colDonderdag)
                                    dayNumber = 4;
                                if (i == colVrijdag)
                                    dayNumber = 5;
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
                                name = fieldValue.Trim();
                                continue;
                            }

                            if ((name != "") && (startTime == ""))
                            {
                                startTime = fieldValue.Trim();
                                continue;
                            }

                            if ((startTime != "") && (endTime == ""))
                            {
                                endTime = fieldValue.Trim();

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
                                    var child1 = VakacientjesDb.GetChild(name);
                                    if (child1.Id == -1)
                                    {
                                        string firstName;
                                        string lastName;
                                        GetFirstNameLastName(name, out firstName, out lastName);

                                        SelectChildWindow selectChildWindow = new SelectChildWindow(firstName, lastName);
                                        var res = selectChildWindow.ShowDialog();
                                        if (res == true)
                                        {
                                            child1 = VakacientjesDb.GetChild(selectChildWindow.cmbChild.Text);
                                            if (child1.Id == -1)
                                            {
                                                MessageBox.Show($"Onverwachte fout: kind '{selectChildWindow.cmbChild.Text}' toch niet gevonden." +
                                                                "\n\n De bewerking wordt afgebroken.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                                                abort = true;
                                            }
                                        }
                                        else
                                        {
                                            abort = selectChildWindow.abort;
                                        }
                                    }

                                    if (child1.Id != -1)
                                    {
                                        FamilyActivity familyActivity = familyActivityList.FirstOrDefault(a => a.FamilyId == child1.FamilyId);
                                        if (familyActivity == null)
                                        {
                                            familyActivity = new FamilyActivity();
                                            familyActivity.FamilyId = child1.FamilyId;
                                            familyActivityList.Add(familyActivity);
                                        }

                                        if (morning == true)
                                        {
                                            PlayActivity playActivity = new PlayActivity();
                                            playActivity.PersonId = child1.Id;
                                            playActivity.WeekNumber = weekNumber;
                                            playActivity.DayNumber = dayNumber;
                                            playActivity.Morning = true;
                                            familyActivity.ChildPlayActivities.Add(playActivity);
                                        }

                                        if (afterNoon == true)
                                        {
                                            PlayActivity playActivity = new PlayActivity();
                                            playActivity.PersonId = child1.Id;
                                            playActivity.WeekNumber = weekNumber;
                                            playActivity.DayNumber = dayNumber;
                                            playActivity.Morning = false;
                                            familyActivity.ChildPlayActivities.Add(playActivity);
                                        }
                                    }
                                }
                                else
                                {
                                    var parent1 = VakacientjesDb.GetParent(name);
                                    if (parent1.Id == -1)
                                    {
                                        string firstName;
                                        string lastName;
                                        GetFirstNameLastName(name, out firstName, out lastName);

                                        SelectChildWindow selectChildWindow = new SelectChildWindow(firstName, lastName);
                                        selectChildWindow.parentMode = true;
                                        var res = selectChildWindow.ShowDialog();
                                        if (res == true)
                                        {
                                            parent1 = VakacientjesDb.GetParent(selectChildWindow.cmbChild.Text);
                                            if (parent1.Id == -1)
                                            {
                                                MessageBox.Show($"Onverwachte fout: ouder '{selectChildWindow.cmbChild.Text}' toch niet gevonden." +
                                                                "\n\n De bewerking wordt afgebroken.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                                                abort = true;
                                            }
                                        }
                                        else
                                        {
                                            abort = selectChildWindow.abort;
                                        }
                                    }

                                    if (parent1.Id != -1)
                                    {
                                        FamilyActivity familyActivity = familyActivityList.FirstOrDefault(a => a.FamilyId == parent1.FamilyId);
                                        if (familyActivity == null)
                                        {
                                            familyActivity = new FamilyActivity();
                                            familyActivity.FamilyId = parent1.FamilyId;
                                            familyActivityList.Add(familyActivity);
                                        }

                                        if (morning == true)
                                        {
                                            PlayActivity playActivity = new PlayActivity();
                                            playActivity.PersonId = parent1.Id;
                                            playActivity.WeekNumber = weekNumber;
                                            playActivity.DayNumber = dayNumber;
                                            playActivity.Morning = true;
                                            familyActivity.ParentPlayActivities.Add(playActivity);
                                        }

                                        if (afterNoon == true)
                                        {
                                            PlayActivity playActivity = new PlayActivity();
                                            playActivity.PersonId = parent1.Id;
                                            playActivity.WeekNumber = weekNumber;
                                            playActivity.DayNumber = dayNumber;
                                            playActivity.Morning = false;
                                            familyActivity.ParentPlayActivities.Add(playActivity);
                                        }
                                    }
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

                    if (abort == true)
                        break;
                }

                if (abort == true)
                    break;

                if (validDayColumns == false)
                {
                    validDayColumns = ((colMaandag != -1) || (colDinsdag != -1) || (colWoensdag != -1) || (colDonderdag != -1) || (colVrijdag != -1));
                    if (validDayColumns == true)
                    {
                        validDayColumns = ((colMaandag != -1) && (colDinsdag != -1) && (colWoensdag != -1) && (colDonderdag != -1) && (colVrijdag != -1));
                        if (validDayColumns == false)
                        {
                            MessageBox.Show($"Niet alle dagen zijn gevonden in week {weekNumber}.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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

                excelReader.Close();

                List<FamilyActivity> familyActivityList = new List<FamilyActivity>();

                AnalyzeData(familyActivityList, 2);
            }
        }
    }
}
