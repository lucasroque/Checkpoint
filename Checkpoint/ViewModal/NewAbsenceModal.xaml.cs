using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{
    public partial class NewAbsenceModal : System.Windows.Controls.UserControl
    {
        private AbsenceControl absenceControl;
        private EmployeeControl employeeControl;
        private JustificationControl justificationControl;

        private List<Employee> allEmployees = new List<Employee>();
        private List<Justification> allJustifications = new List<Justification>();

        private Employee employee;

        public NewAbsenceModal(Employee employee)
        {
            InitializeComponent();

            DataContext = new AbsenceViewControl();

            absenceControl = new AbsenceControl();
            employeeControl = new EmployeeControl();
            justificationControl = new JustificationControl();

            this.employee = employee;

            fillCBEmployee();
            fillCBJustification();
        }

        private void fillCBEmployee()
        {
            allEmployees.Clear();
            allEmployees.Add(employee);
            CBEmployee.ItemsSource = allEmployees;
            CBEmployee.SelectedIndex = 0;
        }

        private void fillCBJustification()
        {
            allJustifications.Clear();
            allJustifications = justificationControl.getAllJustifications();
            CBJustification.ItemsSource = allJustifications;
        }

        private void insertAbsence(object sender, RoutedEventArgs e)
        {
            if (CBEmployee.SelectedIndex != -1 && !"".Equals(DPStartDate.Text) && !"".Equals(DPEndDate.Text) && CBEmployee.SelectedIndex != -1)
            {
                insertAbsence();
            }
        }

        private void insertAbsence()
        {
            Absence absence = getAbsenceFromControls();
            absenceControl.saveAbsence(absence);
        }

        private Absence getAbsenceFromControls()
        {
            Absence absence = new Absence();
            absence.employee = (Employee)CBEmployee.SelectedItem;
            absence.startDate = (DateTime)DPStartDate.SelectedDate;
            absence.endDate = (DateTime)DPEndDate.SelectedDate;
            absence.justification = (Justification)CBJustification.SelectedItem;

            return absence;
        }
    }
}
