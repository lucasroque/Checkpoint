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
using Checkpoint.ViewModal;
using Checkpoint.ViewControl;

namespace Checkpoint.View
{
    public partial class MarkingCardView : System.Windows.Controls.UserControl
    {
        private MarkingControl markingControl;
        private CompanyControl companyControl;
        private DepartmentControl departmentControl;
        private OfficeControl officeControl;
        private EmployeeControl employeeControl;
        private MarkingCardViewControl markingCardViewControl;

        List<Company> allCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Office> allOffices = new List<Office>();

        public MarkingCardView()
        {
            InitializeComponent();

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DPStartDate.SelectedDate = firstDayOfMonth;
            DPEndDate.SelectedDate = lastDayOfMonth;

            markingControl = new MarkingControl();
            companyControl = new CompanyControl();
            departmentControl = new DepartmentControl();
            officeControl = new OfficeControl();
            employeeControl = new EmployeeControl();
            markingCardViewControl = new MarkingCardViewControl();

            fillCBCompany();
            fillCBDepartment();
            fillCBOffice();

            DataContext = markingCardViewControl;
        }

        private void fillGriddDailyMarking()
        {
            if (CBEmployee.SelectedIndex == -1)
            {
                DialogHost.Show(new SampleMessageDialog("Selecione um Funcionário!"), "DHMain");
            }
            else
            {
                DateTime selecteddate = (DateTime)DPStartDate.SelectedDate;
                ObservableCollection<DailyMarking> dailyMarkingList = new ObservableCollection<DailyMarking>(markingControl.getMarkingCard((Employee)CBEmployee.SelectedItem, (DateTime)DPStartDate.SelectedDate, (DateTime)DPEndDate.SelectedDate));
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

        /*private void fillCBEmployee()
        {

            int idDepartment = 0;
            if(CBDepartment.SelectedIndex != -1)
            {
                Department dp = (Department)CBDepartment.SelectedItem;
                idDepartment = dp.idDepartment;
            }

            int idOffice = 0;
            if (CBOffice.SelectedIndex != -1)
            {
                Office of = (Office)CBOffice.SelectedItem;
                idOffice = of.idOffice;
            }

            allEmployees.Clear();
            allEmployees = employeeControl.getAllEmployeesFromDepartment(idDepartment, idOffice);
            CBEmployee.ItemsSource = allEmployees;
        }*/

        private void loadDailyMarking(object sender, RoutedEventArgs e)
        {
            fillGriddDailyMarking();
        }

        private void CBDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idDepartment = 0;
            if (CBDepartment.SelectedIndex != -1)
            {
                Department dp = (Department)CBDepartment.SelectedItem;
                idDepartment = dp.idDepartment;
            }

            int idOffice = 0;
            if (CBOffice.SelectedIndex != -1)
            {
                Office of = (Office)CBOffice.SelectedItem;
                idOffice = of.idOffice;
            }

            markingCardViewControl.fillCBCompanyDepartment(idDepartment, idOffice);
        }

        private void CBOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idDepartment = 0;
            if (CBDepartment.SelectedIndex != -1)
            {
                Department dp = (Department)CBDepartment.SelectedItem;
                idDepartment = dp.idDepartment;
            }

            int idOffice = 0;
            if (CBOffice.SelectedIndex != -1)
            {
                Office of = (Office)CBOffice.SelectedItem;
                idOffice = of.idOffice;
            }

            markingCardViewControl.fillCBCompanyDepartment(idDepartment, idOffice);
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
                                success = markingControl.updateMarking(marking);
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

        private async void showNewAbsenceModal(object sender, RoutedEventArgs e)
        {
            if (CBEmployee.SelectedIndex == -1)
            {
                await DialogHost.Show(new SampleMessageDialog("Selecione um Funcionário!"), "DHMain");
            }
            else
            {
                NewAbsenceModal newAbsenceModal = new NewAbsenceModal((Employee)CBEmployee.SelectedItem);
                Boolean result = (Boolean)await DialogHost.Show(newAbsenceModal, "DHMain");
            }
        }
        
        private void cellEntryOne_TextChanged(object sender, TextChangedEventArgs e)
        {
           DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.entryOne != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.entryOne);

                if ((!dailyMarking.entryOne.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.entryOne = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.entryOne = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            }
        }

        private void cellExitOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.exitOne != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.exitOne);

                if ((!dailyMarking.exitOne.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.exitOne = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.exitOne = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            }
        }

        private void cellEntryTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.entryTwo != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.entryTwo);

                if ((!dailyMarking.entryTwo.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.entryTwo = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.entryTwo = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            }
        }

        private void cellExitTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.exitTwo != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.exitTwo);

                if ((!dailyMarking.exitTwo.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.exitTwo = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.exitTwo = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            }
        }

        private void cellEntryThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.entryThree != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.entryThree);

                if ((!dailyMarking.entryThree.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.entryThree = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.entryThree = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            }
        }

        private void cellExitThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            DailyMarking dailyMarking = ((FrameworkElement)sender).DataContext as DailyMarking;

            if (dailyMarking.exitThree != null)
            {
                String formated = Formatter.getInstance.formatHour(dailyMarking.exitThree);

                if ((!dailyMarking.exitThree.Equals(formated) && formated.Contains(":")) || "".Equals(formated))
                {
                    TimeSpan result;
                    if (TimeSpan.TryParse(formated, out result))
                    {
                        dailyMarking.exitThree = formated;
                        resetDayOff(dailyMarking);
                        GDScheduleDays.Items.Refresh();
                    }
                    else
                    {
                        dailyMarking.exitThree = "";
                        GDScheduleDays.Items.Refresh();
                    }
                }
            } 
        }

        private int compareHours(String hrOne, String hrTwo)
        {
            TimeSpan hrOneTS = TimeSpan.Parse(hrOne);
            TimeSpan hrTwoTS = TimeSpan.Parse(hrTwo);

            return hrOneTS.CompareTo(hrTwoTS);
        }

        private void resetDayOff(DailyMarking dm)
        {
            if ("FOLGA".Equals(dm.entryOne))
            {
                dm.entryOne = "";
            }
            if ("FOLGA".Equals(dm.exitOne))
            {
                dm.exitOne = "";
            }
            if ("FOLGA".Equals(dm.entryTwo))
            {
                dm.entryTwo = "";
            }
            if ("FOLGA".Equals(dm.exitTwo))
            {
                dm.exitTwo = "";
            }
            if ("FOLGA".Equals(dm.entryThree))
            {
                dm.entryThree = "";
            }
            if ("FOLGA".Equals(dm.exitThree))
            {
                dm.exitThree = "";
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
