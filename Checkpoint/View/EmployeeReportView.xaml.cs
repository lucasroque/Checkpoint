using Checkpoint.Control;
using Checkpoint.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.View
{

    public partial class EmployeeReportView : System.Windows.Controls.UserControl
    {
        private EmployeeControl employeeControl;
        private CompanyControl companyControl;
        private DepartmentControl departmentControl;
        private OfficeControl officeControl;

        List<Company> allCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Office> allOffices = new List<Office>();

        public EmployeeReportView()
        {
            InitializeComponent();
            
            employeeControl = new EmployeeControl();
            companyControl = new CompanyControl();
            departmentControl = new DepartmentControl();
            officeControl = new OfficeControl();

            fillGriddEmployee();
            fillCBCompany();
            fillCBDepartment();
            fillCBOffice();
        }

        private void fillGriddEmployee()
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


            ObservableCollection<Employee> employeeList = new ObservableCollection<Employee>(employeeControl.getAllEmployeesFromDepartment(idDepartment, idOffice));
            GDEmployee.ItemsSource = employeeList;
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

        private void loadDailyMarking(object sender, RoutedEventArgs e)
        {
            fillGriddEmployee();
        }
    }
}
