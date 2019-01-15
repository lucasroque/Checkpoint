using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.View
{
    public partial class AbsenceRegisterView : UserControl
    {
        private AbsenceControl absenceControl;
        private EmployeeControl employeeControl;
        private JustificationControl justificationControl;
        private AbsenceViewControl absenceViewControl;

        List<Employee> allCompanies = new List<Employee>();
        List<Justification> allJustifications = new List<Justification>();

        private int idAbsenceEditing = 0;

        public AbsenceRegisterView()
        {
            InitializeComponent();

            absenceControl = new AbsenceControl();
            employeeControl = new EmployeeControl();
            justificationControl = new JustificationControl();
            absenceViewControl = new AbsenceViewControl();

            DataContext = absenceViewControl;
            
            fillCBEmployee();
            fillCBJustification();
        }

        private void fillCBEmployee()
        {
            allCompanies.Clear();
            allCompanies = employeeControl.getAllEmployees();
            CBEmployee.ItemsSource = allCompanies;
        }

        private void fillCBJustification()
        {
            allJustifications.Clear();
            allJustifications = justificationControl.getAllJustifications();
            CBJustification.ItemsSource = allJustifications;
        }

        private void upsertAbsence(object sender, RoutedEventArgs e)
        {
            if (CBEmployee.SelectedIndex != -1 && !"".Equals(DPStartDate.Text) && !"".Equals(DPEndDate.Text) && CBJustification.SelectedIndex != -1)
            {
                upsertAbsence();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadAbsence(object sender, RoutedEventArgs e)
        {
            Absence absence = ((FrameworkElement)sender).DataContext as Absence;
            loadAbsence(absence);
        }

        private async void deleteAbsence(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Afastamento?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Absence absence = ((FrameworkElement)sender).DataContext as Absence;
                deleteAbsence(absence);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertAbsence()
        {
            Absence absence = getAbsenceFromControls();
            Boolean success;

            if (idAbsenceEditing != 0)
            {
                absence.idAbsence = idAbsenceEditing;
                success = absenceControl.updateAbsence(absence);
                saveModeControls();
            }
            else
            {
                success = absenceControl.saveAbsence(absence);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Afastamento salvo com sucesso."), "DHMain");
                absenceViewControl.fillGridAbsence();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Afastamento."), "DHMain");
            }
        }

        private void loadAbsence(Absence absence)
        {
            int indexEmployee = -1;
            int indexJustification = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Employee cp = (Employee)allCompanies[i];
                if (absence.employee.idEmployee == cp.idEmployee)
                {
                    indexEmployee = i;
                    break;
                }
            }

            for (int i = 0; i < allJustifications.Count; i++)
            {
                Justification dp = (Justification)allJustifications[i];
                if (absence.justification.idJustification == dp.idJustification)
                {
                    indexJustification = i;
                    break;
                }
            }

            CBEmployee.SelectedIndex = indexEmployee;
            DPStartDate.SelectedDate = absence.startDate;
            DPEndDate.SelectedDate = absence.endDate;
            CBJustification.SelectedIndex = indexJustification;

            idAbsenceEditing = absence.idAbsence;

            editModeControls();
        }

        private void deleteAbsence(Absence absence)
        {
            Boolean success = absenceControl.deleteAbsence(absence);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Afastamento excluído com sucesso."), "DHMain");
                absenceViewControl.fillGridAbsence();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Afastamento."), "DHMain");
            }
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

        private void cleanControls()
        {
            CBEmployee.SelectedIndex = -1;
            DPStartDate.Text = null;
            DPEndDate.SelectedDate = null;
            CBJustification.SelectedIndex = -1;

            idAbsenceEditing = 0;
            saveModeControls();
        }

        private void saveModeControls()
        {
            BTSave.IsEnabled = true;
            BTEdit.IsEnabled = false;
        }

        private void editModeControls()
        {
            BTSave.IsEnabled = false;
            BTEdit.IsEnabled = true;
        }
    }
}
