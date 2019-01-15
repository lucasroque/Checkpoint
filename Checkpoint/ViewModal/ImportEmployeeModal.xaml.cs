using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{

    public partial class ImportEmployeeModal : UserControl
    {
        private ImportControl importControl;
        private CompanyControl companyControl;
        private ScheduleControl scheduleControl;

        private readonly BackgroundWorker backWorkerImportEmployee = new BackgroundWorker();
        
        private String afdFile = "";
        private Boolean success = false;
        private List<Company> allCompanies = new List<Company>();
        private List<Schedule> allSchedules = new List<Schedule>();

        public ImportEmployeeModal()
        {
            InitializeComponent();

            importControl = new ImportControl();
            companyControl = new CompanyControl();
            scheduleControl = new ScheduleControl();

            DataContext = new ImportEmployeesViewControl();

            backWorkerImportEmployee.DoWork += importEmployeeBackground;
            backWorkerImportEmployee.RunWorkerCompleted += backWorkerImportEmployeeResponse;

            fillCBCompany();
            fillCBSchedule();
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

        private void loadEmployeeFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione o Arquivo AFD";
            op.Filter = "CSV (*.csv)|*.csv";

            if (op.ShowDialog() == true)
            {
                afdFile = op.FileName;
                TBPath.Text = afdFile;
            }
        }

        public void startProgress()
        {
            BTImport.IsEnabled = false;
            BTSearch.IsEnabled = false;
            PBImport.IsIndeterminate = true;
        }

        public void stopProgress()
        {
            BTImport.IsEnabled = true;
            BTSearch.IsEnabled = true;
            PBImport.IsIndeterminate = false;
        }

        private void importEmployee(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(afdFile))
            {
                startProgress();

                List<object> arguments = new List<object>();

                Company company = (Company) CBCompany.SelectedItem;
                Schedule schedule = (Schedule) CBSchedule.SelectedItem;

                arguments.Add(afdFile);
                arguments.Add(company.idCompany);
                arguments.Add(schedule.idSchedule);

                backWorkerImportEmployee.RunWorkerAsync(arguments);

            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Selecione um arquivo para importação."), "DHModal");
            }

        }

        private void importEmployeeBackground(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            List<object> genericlist = doWorkEventArgs.Argument as List<object>;

            success = importControl.importEmployee((String)genericlist[0], (int)genericlist[1], (int)genericlist[2]);
        }

        private void backWorkerImportEmployeeResponse(object sender, RunWorkerCompletedEventArgs e)
        {
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Arquivo importado com sucesso."), "DHModal");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Falha na importação do arquivo."), "DHModal");
            }

            cleanControls();
            stopProgress();
        }

        private void cleanControls()
        {
            CBCompany.SelectedIndex = -1;
            CBSchedule.SelectedIndex = -1;
            TBPath.Text = "";
            afdFile = "";
            success = false;
        }
    }
}
