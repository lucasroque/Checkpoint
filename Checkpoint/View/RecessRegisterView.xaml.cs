using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Checkpoint.Tools;

namespace Checkpoint.View
{
    public partial class RecessRegisterView : System.Windows.Controls.UserControl
    {
        private RecessControl recessControl;
        private CompanyControl companyControl;
        private DepartmentControl departmentControl;
        private RecessViewControl recessViewControl = new RecessViewControl();

        List<Company> allCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();

        private int idRecessEditing = 0;

        public RecessRegisterView()
        {
            InitializeComponent();

            recessControl = new RecessControl();
            companyControl = new CompanyControl();
            departmentControl = new DepartmentControl();
            recessViewControl = new RecessViewControl();

            DataContext = recessViewControl;
            
            fillCBCompany();
            fillCBDepartment();
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            CBCompany.ItemsSource = allCompanies;
        }

        private void fillCBDepartment()
        {
            allDepartments.Clear();
            allDepartments = departmentControl.getAllDepartments();
            CBDepartment.ItemsSource = allDepartments;
        }

        private void upsertRecess(object sender, RoutedEventArgs e)
        {

            if (!recessControl.validateRecess((Company)CBCompany.SelectedItem, (DateTime) DPRecessDate.SelectedDate) && idRecessEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Feriado já cadastrado."), "DHMain");
                return;
            }

            if (CBCompany.SelectedIndex != -1 && !"".Equals(TBDescription.Text) && !"".Equals(DPRecessDate.Text))
            {
                upsertRecess();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void insertStandardRecess(object sender, RoutedEventArgs e)
        {
            Boolean success = false;

            foreach (Company company in allCompanies)
            {
                foreach (Recess recess in Util.getStandardRecess())
                {
                    recess.company = company;
                    success = recessControl.saveRecess(recess);

                    if (!success)
                    {
                        break;
                    }
                }
            }

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Feriados padrões salvo com sucesso."), "DHMain");
                recessViewControl.fillGridRecess();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Feriados padrões."), "DHMain");
            }
        }

        private void loadRecess(object sender, RoutedEventArgs e)
        {
            Recess recess = ((FrameworkElement)sender).DataContext as Recess;
            loadRecess(recess);
        }

        private async void deleteRecess(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Feriado?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Recess recess = ((FrameworkElement)sender).DataContext as Recess;
                deleteRecess(recess);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertRecess()
        {
            Recess recess = getRecessFromControls();
            Boolean success;

            if (idRecessEditing != 0)
            {
                recess.idRecess = idRecessEditing;
                success = recessControl.updateRecess(recess);
                saveModeControls();
            }
            else
            {
                success = recessControl.saveRecess(recess);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Feriado salvo com sucesso."), "DHMain");
                recessViewControl.fillGridRecess();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Feriado."), "DHMain");
            }
        }

        private void loadRecess(Recess recess)
        {
            int indexCompany = -1;
            int indexDepartment = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Company cp = (Company)allCompanies[i];
                if (recess.company.idCompany == cp.idCompany)
                {
                    indexCompany = i;
                    break;
                }
            }

            for (int i = 0; i < allDepartments.Count; i++)
            {
                Department dp = (Department)allDepartments[i];
                if (recess.department.idDepartment == dp.idDepartment)
                {
                    indexDepartment = i;
                    break;
                }
            }

            CBCompany.SelectedIndex = indexCompany;
            TBDescription.Text = recess.description;
            DPRecessDate.SelectedDate = recess.recessDate;
            CBDepartment.SelectedIndex = indexDepartment;

            idRecessEditing = recess.idRecess;

            editModeControls();
        }

        private void deleteRecess(Recess recess)
        {
            Boolean success = recessControl.deleteRecess(recess);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Feriado excluído com sucesso."), "DHMain");
                recessViewControl.fillGridRecess();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Feriado."), "DHMain");
            }
        }

        private Recess getRecessFromControls()
        {
            Recess recess = new Recess();
            recess.company = (Company) CBCompany.SelectedItem;
            recess.description = TBDescription.Text;
            recess.recessDate = (DateTime) DPRecessDate.SelectedDate;
            recess.department = (Department)CBDepartment.SelectedItem;

            return recess;
        }

        private void cleanControls()
        {
            CBCompany.SelectedIndex = -1;
            TBDescription.Text = "";
            DPRecessDate.SelectedDate = null;
            CBDepartment.SelectedIndex = -1;

            idRecessEditing = 0;
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
