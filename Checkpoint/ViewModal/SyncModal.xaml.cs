using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.RWIntegration;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{

    public partial class SyncModal : UserControl
    {
        private CompanyControl companyControl;
        private HardwareConfigurationControl hardwareConfigurationControl;

        private List<Company> allCompanies = new List<Company>();
        private List<HardwareConfiguration> allHardwaresConfiguration = new List<HardwareConfiguration>();

        public SyncModal()
        {
            InitializeComponent();

            companyControl = new CompanyControl();
            hardwareConfigurationControl = new HardwareConfigurationControl();

            fillCBCompany();
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            CBCompanySend.ItemsSource = allCompanies;
            CBCompanyReceive.ItemsSource = allCompanies;
        }

        private void CBCompanySend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillCBHardwareSend();
        }

        private void CBCompanyRead_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillCBHardwareReceive();
        }

        private void fillCBHardwareSend()
        {
            Company company = (Company)CBCompanySend.SelectedItem;

            allHardwaresConfiguration.Clear();
            allHardwaresConfiguration = hardwareConfigurationControl.getAllHardwareConfigurations(company.idCompany);
            CBHardwareSend.ItemsSource = allHardwaresConfiguration;
        }

        private void fillCBHardwareReceive()
        {
            Company company = (Company)CBCompanyReceive.SelectedItem;

            allHardwaresConfiguration.Clear();
            allHardwaresConfiguration = hardwareConfigurationControl.getAllHardwareConfigurations(company.idCompany);
            CBHardwareReceive.ItemsSource = allHardwaresConfiguration;
        }

        private void sendEmployer(object sender, RoutedEventArgs e)
        {
        }

        private void sendEmployees(object sender, RoutedEventArgs e)
        {
        }

        private void sendMarkings(object sender, RoutedEventArgs e)
        {
        }

        private void readEmployer(object sender, RoutedEventArgs e)
        {
            CommandReadEmployer commandReadEmployer = new CommandReadEmployer();
            HardwareConfiguration hardwareConfiguration = (HardwareConfiguration) CBCompanyReceive.SelectedItem;

            ErrorCommand ec = commandReadEmployer.execute(hardwareConfiguration.ip, hardwareConfiguration.port, "05021923327", "");
            Company company = commandReadEmployer.getCompany();
        }

        private void readEmployees(object sender, RoutedEventArgs e)
        {
        }

        private void readMarkings(object sender, RoutedEventArgs e)
        {
        }

    }
}
