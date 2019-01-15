using Checkpoint.Control;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Checkpoint.ViewModal;
using Checkpoint.Tools;

namespace Checkpoint.View
{
    public partial class EmployeeRegisterView : System.Windows.Controls.UserControl
    {
        private EmployeeControl employeeControl;
        private CompanyControl companyControl;
        private ScheduleControl scheduleControl;
        private DepartmentControl departmentControl;
        private OfficeControl officeControl;
        private ResignationReasonControl resignationReasonControl;
        private EmployeeViewControl employeeViewControl;

        List<Company> allCompanies = new List<Company>();
        List<Schedule> allSchedules = new List<Schedule>();
        List<Department> allDepartments = new List<Department>();
        List<Office> allOffices = new List<Office>();
        List<ResignationReason> allRReasons = new List<ResignationReason>();

        private int idEmployeeEditing = 0;

        public EmployeeRegisterView()
        {
            InitializeComponent();

            employeeControl = new EmployeeControl();
            companyControl = new CompanyControl();
            scheduleControl = new ScheduleControl();
            departmentControl = new DepartmentControl();
            officeControl = new OfficeControl();
            resignationReasonControl = new ResignationReasonControl();
            employeeViewControl = new EmployeeViewControl();

            DataContext = employeeViewControl;

            fillCBCompany();
            fillCBSchedule();
            fillCBDepartment();
            fillCBOffice();
            fillCBRReasons();
            fillImageControl();
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            CBCompany.ItemsSource = allCompanies;
        }

        private void fillCBSchedule()
        {
            allSchedules.Clear();
            allSchedules = scheduleControl.getAllSchedules();
            CBSchedule.ItemsSource = allSchedules;
        }

        private void fillCBDepartment()
        {
            allDepartments.Clear();
            allDepartments = departmentControl.getAllDepartments();
            CBDepartment.ItemsSource = allDepartments;
        }

        private void fillCBOffice()
        {
            allOffices.Clear();
            allOffices = officeControl.getAllOffices();
            CBOffice.ItemsSource = allOffices;
        }

        private void fillCBRReasons()
        {
            allRReasons.Clear();
            allRReasons = resignationReasonControl.getAllResignationReasons();
            CBResignationReason.ItemsSource = allRReasons;
        }

        private void fillImageControl()
        {
            imgEmployeePhoto.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
        }

        private void loadPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Imagem";
            op.Filter = "Todas as Imagens Suportadas|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                imgEmployeePhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void upsertEmployee(object sender, RoutedEventArgs e)
        {
            if (!Inspector.getInstance.validatePis(TBPispasep.Text))
            {
                DialogHost.Show(new SampleMessageDialog("PIS inválido."), "DHMain");
                return;
            }

            if (!employeeControl.validadePisPasep(TBPispasep.Text) && idEmployeeEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Pis/Pasep já cadastrado."), "DHMain");
                return;
            }

            if (!employeeControl.validadeLeefNumber(TBLeefNumber.Text) && idEmployeeEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Número folha já cadastrado."), "DHMain");
                return;
            }

            if (!"".Equals(TBName.Text) && !"".Equals(TBPispasep.Text) && !"".Equals(TBLeefNumber.Text) && CBCompany.SelectedIndex != -1 && CBSchedule.SelectedIndex != -1 && !"".Equals(DPAdmission.Text))
            {
                upsertEmployee();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }

        }

        private void loadEmployee(object sender, RoutedEventArgs e)
        {
            Employee employee = ((FrameworkElement)sender).DataContext as Employee;
            loadEmployee(employee);
        }

        private async void deleteEmployee(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Funcionário?");
            Boolean result = (Boolean) await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Employee employee = ((FrameworkElement)sender).DataContext as Employee;
                deleteEmployee(employee);
            }
        }

        private async void showNewDepartmentModal(object sender, RoutedEventArgs e)
        {
            NewDepartmentModal newDepartmentModal = new NewDepartmentModal();
            Boolean result = (Boolean) await DialogHost.Show(newDepartmentModal, "DHMain");

            if (result)
            {
                fillCBDepartment();
                CBDepartment.SelectedIndex = allDepartments.Count - 1;
            }

        }

        private async void showNewOfficeModal(object sender, RoutedEventArgs e)
        {
            NewOfficeModal newOfficeModal = new NewOfficeModal();
            Boolean result = (Boolean)await DialogHost.Show(newOfficeModal, "DHMain");

            if (result)
            {
                fillCBOffice();
                CBOffice.SelectedIndex = allOffices.Count - 1;
            }

        }

        private async void showNewRReasonModal(object sender, RoutedEventArgs e)
        {
            NewResignationReasonModal newResignationReasonModal = new NewResignationReasonModal();
            Boolean result = (Boolean)await DialogHost.Show(newResignationReasonModal, "DHMain");

            if (result)
            {
                fillCBRReasons();
                CBResignationReason.SelectedIndex = allRReasons.Count - 1;
            }

        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertEmployee()
        {
            Employee employee = getEmployeeFromControls();
            Boolean success;

            if (idEmployeeEditing != 0)
            {
                employee.idEmployee = idEmployeeEditing;
                success = employeeControl.updateEmployee(employee);
                saveModeControls();
            }
            else
            {
                success = employeeControl.saveEmployee(employee);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Funcionário salvo com sucesso."), "DHMain");
                employeeViewControl.fillGridEmployee();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Funcionário."), "DHMain");
            }
        }

        private void loadEmployee(Employee employee)
        {

            int indexCompany = -1;
            int indexSchedule = -1;
            int indexDepartment = -1;
            int indexOffice = -1;
            int indexRReason = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Company cp = (Company)allCompanies[i];
                if (employee.company.idCompany == cp.idCompany)
                {
                    indexCompany = i;
                    break;
                }
            }

            for (int i = 0; i < allSchedules.Count; i++)
            {
                Schedule sc = (Schedule)allSchedules[i];
                if (employee.schedule.idSchedule == sc.idSchedule)
                {
                    indexSchedule = i;
                    break;
                }
            }

            for (int i = 0; i < allDepartments.Count; i++)
            {
                Department dp = (Department)allDepartments[i];
                if (employee.department.idDepartment == dp.idDepartment)
                {
                    indexDepartment = i;
                    break;
                }
            }

            for (int i = 0; i < allOffices.Count; i++)
            {
                Office o = (Office)allOffices[i];
                if (employee.office.idOffice == o.idOffice)
                {
                    indexOffice = i;
                    break;
                }
            }

            for (int i = 0; i < allRReasons.Count; i++)
            {
                ResignationReason rr = (ResignationReason)allRReasons[i];
                if (employee.resignationReason.idResignationReason == rr.idResignationReason)
                {
                    indexRReason = i;
                    break;
                }
            }

            imgEmployeePhoto.Source = employee.employeeImage;
            TBName.Text = employee.employeeName;
            TBPispasep.Text = employee.pisPasep;
            CBCompany.SelectedIndex = indexCompany;
            CBSchedule.SelectedIndex = indexSchedule;

            TBLeefNumber.Text = employee.leefNumber;
            TBCtps.Text = employee.ctps;
            CBDepartment.SelectedIndex = indexDepartment;
            CBOffice.SelectedIndex = indexOffice;
            DPAdmission.SelectedDate = employee.admission;
            DPResignation.SelectedDate = employee.resignation;
            CBResignationReason.SelectedIndex = indexRReason;

            TBPhone.Text = employee.phone;
            TBEmail.Text = employee.email;
            TBGenegalRegister.Text = employee.generalRegistry;
            TBRegisterEntity.Text = employee.registryEntity;
            TBFather.Text = employee.father;
            TBMother.Text = employee.mother;
            DPBirth.SelectedDate = employee.birth;
            TBGender.Text = employee.gender;
            TBCivilStatus.Text = employee.civilStatus;
            TBNacionality.Text = employee.nationality;
            TBNaturalness.Text = employee.naturalness;
            TBAddress.Text = employee.address;
            TBNeighborhood.Text = employee.neighborhood;
            TBCity.Text = employee.city;
            TBState.Text = employee.state;
            TBZipCode.Text = employee.zipCode;

            idEmployeeEditing = employee.idEmployee;

            editModeControls();
        }

        private void deleteEmployee(Employee employee)
        {
            Boolean success = employeeControl.deleteEmployee(employee);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Funcionário excluído com sucesso."), "DHMain");
                employeeViewControl.fillGridEmployee();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Funcionário."), "DHMain");
            }
        }

        private Employee getEmployeeFromControls()
        {

            Employee employee = new Employee();

            employee.employeeImage = (BitmapImage)imgEmployeePhoto.Source;
            employee.employeeName = TBName.Text;
            employee.pisPasep = TBPispasep.Text;
            employee.company = (Company) CBCompany.SelectedItem;
            employee.schedule = (Schedule)CBSchedule.SelectedItem;

            employee.leefNumber = TBLeefNumber.Text;
            employee.ctps = TBCtps.Text;
            employee.department = (Department) CBDepartment.SelectedItem;
            employee.office = (Office) CBOffice.SelectedItem;
            employee.resignationReason = (ResignationReason)CBResignationReason.SelectedItem;
            if (DPAdmission.SelectedDate != null)
                employee.admission =  (DateTime) DPAdmission.SelectedDate;
            if (DPResignation.SelectedDate != null)
                employee.resignation = (DateTime) DPResignation.SelectedDate;
            

            employee.phone = TBPhone.Text;
            employee.email = TBEmail.Text;
            employee.generalRegistry = TBGenegalRegister.Text;
            employee.registryEntity = TBRegisterEntity.Text;
            employee.father = TBFather.Text;
            employee.mother = TBMother.Text;
            if (DPBirth.SelectedDate != null)
                employee.birth = (DateTime) DPBirth.SelectedDate;
            employee.gender = TBGender.Text;
            employee.civilStatus = TBCivilStatus.Text;
            employee.nationality = TBNacionality.Text;
            employee.naturalness = TBNaturalness.Text;
            employee.address = TBAddress.Text;
            employee.neighborhood = TBNeighborhood.Text;
            employee.city = TBCity.Text;
            employee.state = TBState.Text;
            employee.zipCode = TBZipCode.Text;

            return employee;
        }

        private void cleanControls()
        {
            TBName.Text = "";
            TBPispasep.Text = "";
            CBCompany.SelectedIndex = -1;
            CBSchedule.SelectedIndex = -1;

            TBLeefNumber.Text = "";
            TBCtps.Text = "";
            CBDepartment.SelectedIndex = -1;
            CBOffice.SelectedIndex = -1;
            DPAdmission.SelectedDate = null;
            DPResignation.SelectedDate = null;
            CBResignationReason.SelectedIndex = -1;

            TBPhone.Text = "";
            TBEmail.Text = "";
            TBGenegalRegister.Text = "";
            TBRegisterEntity.Text = "";
            TBFather.Text = "";
            TBMother.Text = "";
            DPBirth.SelectedDate = null;
            TBGender.Text = "";
            TBCivilStatus.Text = "";
            TBNacionality.Text = "";
            TBNaturalness.Text = "";
            TBAddress.Text = "";
            TBNeighborhood.Text = "";
            TBCity.Text = "";
            TBZipCode.Text = "";

            idEmployeeEditing = 0;
            fillImageControl();
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
