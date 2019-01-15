using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Checkpoint.View
{
    public partial class DepartmentRegisterView : UserControl
    {

        private DepartmentControl departmentControl;
        private CompanyControl companyControl;
        private DepartmentViewControl departmentViewControl;

        private int idDepartmentEditing = 0;

        public DepartmentRegisterView()
        {
            InitializeComponent();

            departmentControl = new DepartmentControl();
            companyControl = new CompanyControl();
            departmentViewControl = new DepartmentViewControl();

            DataContext = departmentViewControl;

        }

        private void upsertDepartment(object sender, RoutedEventArgs e)
        {

            if (!departmentControl.validateDescription(TBDescription.Text) && idDepartmentEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Descrição já cadastrado."), "DHMain");
                return;
            }

            if (!"".Equals(TBDescription.Text))
            {
                upsertDepartment();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadDepartment(object sender, RoutedEventArgs e)
        {
            Department department = ((FrameworkElement)sender).DataContext as Department;
            loadDepartment(department);
        }

        private async void deleteDepartment(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Departamento?");
            Boolean result = (Boolean) await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Department department = ((FrameworkElement)sender).DataContext as Department;
                deleteDepartment(department);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertDepartment()
        {
            Department department = getDepartmentFromControls();
            Boolean success;

            if (idDepartmentEditing != 0)
            {
                department.idDepartment = idDepartmentEditing;
                success = departmentControl.updateDepartment(department);
                saveModeControls();
            }
            else
            {
                success = departmentControl.saveDepartment(department);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Departamento salvo com sucesso."), "DHMain");
                departmentViewControl.fillGridDepartment();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Departamento."), "DHMain");
            }
        }

        private void loadDepartment(Department department)
        {
            TBDescription.Text = department.description;

            idDepartmentEditing = department.idDepartment;

            editModeControls();
        }

        private void deleteDepartment(Department department)
        {
            Boolean success = departmentControl.deleteDepartment(department);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Departamento excluído com sucesso."), "DHMain");
                departmentViewControl.fillGridDepartment();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Departamento."), "DHMain");
            }
        }

        private Department getDepartmentFromControls()
        {
            Department department = new Department();
            department.description = TBDescription.Text;

            return department;
        }

        private void cleanControls()
        {
            TBDescription.Text = "";

            idDepartmentEditing = 0;
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
