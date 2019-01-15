using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Checkpoint.ViewControl;

namespace Checkpoint.View
{
    public partial class DailyPointView : System.Windows.Controls.UserControl
    {
        private MarkingControl markingControl;
        private CompanyControl companyControl;
        private DepartmentControl departmentControl;
        private OfficeControl officeControl;
        private DailyPointViewControl dailyPointViewControl;

        List<Company> allCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Office> allOffices = new List<Office>();

        public DailyPointView()
        {
            InitializeComponent();

            DPDate.SelectedDate = DateTime.Now.Date;

            markingControl = new MarkingControl();
            companyControl = new CompanyControl();
            departmentControl = new DepartmentControl();
            officeControl = new OfficeControl();
            dailyPointViewControl = new DailyPointViewControl();

            fillGriddDailyMarking();
            fillCBCompany();
            fillCBDepartment();
            fillCBOffice();

            DataContext = dailyPointViewControl;
        }

        private void fillGriddDailyMarking()
        {
            if (DPDate.SelectedDate != null)
            {
                DateTime selecteddate = (DateTime)DPDate.SelectedDate;
                ObservableCollection<DailyMarking> dailyMarkingList = new ObservableCollection<DailyMarking>(markingControl.getDailyMarkings((Company)CBCompany.SelectedItem, (Department)CBDepartment.SelectedItem, (Office)CBOffice.SelectedItem, (Employee)CBEmployee.SelectedItem, (Schedule)CBSchedule.SelectedItem, selecteddate));
                GDScheduleDays.ItemsSource = dailyMarkingList;
            }  
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            allCompanies.Insert(0, new Company(0, "Todos"));
            CBCompany.ItemsSource = allCompanies;
        }

        private void fillCBDepartment()
        {
            allDepartments.Clear();
            allDepartments = departmentControl.getAllDepartments();
            allDepartments.Insert(0, new Department(0, "Todos"));
            CBDepartment.ItemsSource = allDepartments;
        }

        private void fillCBOffice()
        {
            allOffices.Clear();
            allOffices = officeControl.getAllOffices();
            allOffices.Insert(0, new Office(0, "Todos"));
            CBOffice.ItemsSource = allOffices;
        }

        private void nextDate(object sender, RoutedEventArgs e)
        {
            DateTime selecteddate = (DateTime)DPDate.SelectedDate;
            selecteddate = selecteddate.AddDays(1);

            DPDate.SelectedDate = selecteddate;
            fillGriddDailyMarking();         
        }

        private void backDate(object sender, RoutedEventArgs e)
        {
            DateTime selecteddate = (DateTime)DPDate.SelectedDate;
            selecteddate = selecteddate.AddDays(-1);

            DPDate.SelectedDate = selecteddate;
            fillGriddDailyMarking();
        }

        private void loadDailyMarking(object sender, RoutedEventArgs e)
        {
            fillGriddDailyMarking();
        }

        private void upsertMarking(object sender, RoutedEventArgs e)
        {
            Boolean success = true;

            foreach (DailyMarking dm in GDScheduleDays.ItemsSource)
            {
                String validation = markingControl.dailyMarkingValidate(dm);

                if ("".Equals(validation))
                {
                    if (dm.markings.Count > 0)
                    {
                        markingControl.markingsUpdate(dm);

                        foreach (Marking marking in dm.markings)
                        {
                            if (marking.idMarking == 0)
                            {
                                success = markingControl.saveMarking(marking);
                            }
                            else
                            {
                                if (marking.idJustification > 0)
                                {
                                    success = markingControl.updateJustificationMarking(marking);
                                    marking.nsr = -1;
                                    marking.cont = -1;
                                    success = markingControl.saveMarking(marking);
                                }
                            }
                        }
                    }
                }
                else
                {
                    DialogHost.Show(new SampleMessageDialog(validation), "DHMain");
                }
            }

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Marcação salva com sucesso."), "DHMain");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Marcação."), "DHMain");
            }

            fillGriddDailyMarking();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void cellEntryOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.entryOne);

            if ((!dailyMarking.entryOne.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {
                    dailyMarking.entryOne = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification)await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[0].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.entryOne = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private async void cellExitOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.exitOne);

            if ((!dailyMarking.exitOne.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {
                    dailyMarking.exitOne = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification) await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[1].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.exitOne = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private async void cellEntryTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.entryTwo);

            if ((!dailyMarking.entryTwo.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {

                    dailyMarking.entryTwo = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification) await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[2].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.entryTwo = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private async void cellExitTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.exitTwo);

            if ((!dailyMarking.exitTwo.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {
                    dailyMarking.exitTwo = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification) await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[3].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.exitTwo = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private async void cellEntryThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.entryThree);

            if ((!dailyMarking.entryThree.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {
                    dailyMarking.entryThree = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification) await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[4].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.entryThree = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private async void cellExitThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;
            String formated = Formatter.getInstance.formatHour(dailyMarking.exitThree);

            if ((!dailyMarking.exitThree.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
            {
                TimeSpan result;
                if (TimeSpan.TryParse(formated, out result))
                {
                    dailyMarking.exitThree = formated;

                    AddJustificationMessageDialog addJustificationMessageDialog = new AddJustificationMessageDialog();
                    Justification justification = (Justification) await DialogHost.Show(addJustificationMessageDialog, "DHMain");

                    if (justification != null && dailyMarking.markings.Count > 0)
                    {
                        dailyMarking.markings[5].idJustification = justification.idJustification;
                    }

                    GDScheduleDays.Items.Refresh();
                }
                else
                {
                    dailyMarking.exitThree = "";
                    GDScheduleDays.Items.Refresh();
                }
            }
        }

        private int compareHours(String hrOne, String hrTwo)
        {
            TimeSpan hrOneTS = TimeSpan.Parse(hrOne);
            TimeSpan hrTwoTS = TimeSpan.Parse(hrTwo);

            return hrOneTS.CompareTo(hrTwoTS);
        }
    }
}
