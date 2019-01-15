using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;


namespace Checkpoint.ViewModal
{
    public partial class ExportTaxFilesModal : System.Windows.Controls.UserControl
    {
        private CompanyControl companyControl;
        private DepartmentControl departmentControl;
        private ExportTaxFileControl exportTaxFileControl;

        private List<Company> allCompanies = new List<Company>();
        private List<Department> allDepartments = new List<Department>();
        private Boolean fdtFile = true;

        public ExportTaxFilesModal()
        {
            InitializeComponent();

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            DPStartDate.SelectedDate = firstDayOfMonth;
            DPEndDate.SelectedDate = lastDayOfMonth;

            companyControl = new CompanyControl();
            departmentControl = new DepartmentControl();
            exportTaxFileControl = new ExportTaxFileControl();

            fillCBCompany();
            fillCBDepartment();
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

        private void loadFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();

            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TBPathFolder.Text = fb.SelectedPath;
            }

        }

        private void RBFDTChecked(object sender, RoutedEventArgs e)
        {
            fdtFile = true;
        }

        private void RBCJEF_Checked(object sender, RoutedEventArgs e)
        {
            fdtFile = false;
        }

        private void exportTaxFiles(object sender, RoutedEventArgs e)
        {

            if (!"".Equals(TBPathFolder.Text.Trim()))
            {
                if (DPStartDate.SelectedDate != null || DPEndDate.SelectedDate != null)
                {

                    Boolean success = true;

                    DateTime startDate = (DateTime)DPStartDate.SelectedDate;
                    DateTime endDate = (DateTime)DPEndDate.SelectedDate;
                    Company company = (Company)CBCompany.SelectedItem;
                    Department department = (Department)CBDepartment.SelectedItem;

                    if (fdtFile)
                    {
                        success = exportTaxFileControl.exportFDTFile(TBPathFolder.Text.Trim(), startDate, endDate, company, department);
                    }
                    else
                    {
                        success = exportTaxFileControl.exportCJEFFile(TBPathFolder.Text.Trim(), startDate, endDate, company, department);
                    }

                    if(success)
                    {
                        DialogHost.Show(new SampleMessageDialog("Arquivos exportados com Sucesso."), "DHModal");
                    }
                    else
                    {
                        DialogHost.Show(new SampleMessageDialog("Erros ao exportar Arquivos."), "DHModal");
                    }

                }
                else
                {
                    DialogHost.Show(new SampleMessageDialog("Datas Inválidas."), "DHModal");
                }
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Selecione local para Exportação."), "DHModal");
            }
            
        }
    }
}
