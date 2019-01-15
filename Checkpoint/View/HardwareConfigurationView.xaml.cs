using Checkpoint.Control;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Checkpoint.View
{

    public partial class HardwareConfigurationView : System.Windows.Controls.UserControl
    {
        private HardwareConfigurationControl hardwareConfigurationControl;
        private CompanyControl companyControl;
        private HardwareControl hardwareControl;

        List<Company> allCompanies = new List<Company>();
        List<Hardware> allHardwares = new List<Hardware>();

        private int idHardwareConfigurationEditing = 0;

        public HardwareConfigurationView()
        {
            InitializeComponent();

            DataContext = new HardwareConfigurationViewControl();

            hardwareConfigurationControl = new HardwareConfigurationControl();
            companyControl = new CompanyControl();
            hardwareControl = new HardwareControl();

            fillGridHardwareConfiguration();
            fillCBCompany();
            fillCBHardware();
            fillImageControl();
        }

        private void fillGridHardwareConfiguration()
        {
            ObservableCollection<HardwareConfiguration> hardwareConfigurationList = new ObservableCollection<HardwareConfiguration>(hardwareConfigurationControl.getAllHardwareConfigurations());
            GDHardwareConfiguration.ItemsSource = hardwareConfigurationList;
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            CBCompany.ItemsSource = allCompanies;
        }

        private void fillCBHardware()
        {
            allHardwares.Clear();
            allHardwares = hardwareControl.getAllHardwares();
            CBHardware.ItemsSource = allHardwares;
        }

        private void fillImageControl()
        {
            imgHardwareConfigurationPhoto.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
        }

        private void upsertHardwareConfiguration(object sender, RoutedEventArgs e)
        {
            if (CBCompany.SelectedIndex != -1 && CBHardware.SelectedIndex != -1 && !"".Equals(TBCryptographicKey.Text) && !"".Equals(TBSerialNumber.Text) && !"".Equals(TBPort.Text) && !"".Equals(TBIp.Text))
            {
                upsertHardwareConfiguration();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadHardwareConfiguration(object sender, RoutedEventArgs e)
        {
            HardwareConfiguration hardwareConfiguration = ((FrameworkElement)sender).DataContext as HardwareConfiguration;
            loadHardwareConfiguration(hardwareConfiguration);
        }

        private async void deleteHardwareConfiguration(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Equipamento?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                HardwareConfiguration hardwareConfiguration = ((FrameworkElement)sender).DataContext as HardwareConfiguration;
                deleteHardwareConfiguration(hardwareConfiguration);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertHardwareConfiguration()
        {
            HardwareConfiguration hardwareConfiguration = getHardwareConfigurationFromControls();
            Boolean success;

            if (idHardwareConfigurationEditing != 0)
            {
                hardwareConfiguration.idHardwareConfiguration = idHardwareConfigurationEditing;
                success = hardwareConfigurationControl.updateHardwareConfiguration(hardwareConfiguration);
                saveModeControls();
            }
            else
            {
                success = hardwareConfigurationControl.saveHardwareConfiguration(hardwareConfiguration);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Equipamento salvo com sucesso."), "DHMain");
                fillGridHardwareConfiguration();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Equipamento."), "DHMain");
            }
        }

        private void loadHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            int index = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Company cp = (Company)allCompanies[i];

                if (hardwareConfiguration.company.idCompany == cp.idCompany)
                {
                    index = i;
                    break;
                }
            }

            int indexHardware = -1;

            for (int i = 0; i < allHardwares.Count; i++)
            {
                Hardware hd = (Hardware)allHardwares[i];

                if (hardwareConfiguration.hardware.idHardware == hd.idHardware)
                {
                    indexHardware = i;
                    break;
                }
            }

            CBCompany.SelectedIndex = index;
            CBHardware.SelectedIndex = indexHardware;
            TBCryptographicKey.Text = hardwareConfiguration.cryptographicKey;
            TBSerialNumber.Text = hardwareConfiguration.serialNumber;
            TBModel.Text = hardwareConfiguration.model;
            TBVersion.Text = hardwareConfiguration.version;
            TBPort.Text = hardwareConfiguration.port;
            TBIp.Text = hardwareConfiguration.ip;
            TBCpf.Text = hardwareConfiguration.cpf;

            idHardwareConfigurationEditing = hardwareConfiguration.idHardwareConfiguration;

            editModeControls();
        }

        private void deleteHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            Boolean success = hardwareConfigurationControl.deleteHardwareConfiguration(hardwareConfiguration);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Equipamento excluído com sucesso."), "DHMain");
                fillGridHardwareConfiguration();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Equipamento."), "DHMain");
            }
        }

        private HardwareConfiguration getHardwareConfigurationFromControls()
        {
            HardwareConfiguration hardwareConfiguration = new HardwareConfiguration();
            hardwareConfiguration.company = (Company)CBCompany.SelectedItem;
            hardwareConfiguration.hardware = (Hardware)CBHardware.SelectedItem;
            hardwareConfiguration.cryptographicKey = TBCryptographicKey.Text;
            hardwareConfiguration.serialNumber = TBSerialNumber.Text;
            hardwareConfiguration.model = TBModel.Text;
            hardwareConfiguration.version = TBVersion.Text;
            hardwareConfiguration.port = TBPort.Text;
            hardwareConfiguration.ip = TBIp.Text;
            hardwareConfiguration.cpf = TBCpf.Text;

            return hardwareConfiguration;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CBHardware.SelectedIndex >= 0)
            {
                Hardware hd = (Hardware)CBHardware.SelectedItem;

                imgHardwareConfigurationPhoto.Source = hd.hardwareImage;

            } else
            {
                imgHardwareConfigurationPhoto.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
            }
            
        }

        private void cleanControls()
        {
            CBCompany.SelectedIndex = -1;
            CBHardware.SelectedIndex = -1;
            TBCryptographicKey.Text = "";
            TBSerialNumber.Text = "";
            TBModel.Text = "";
            TBVersion.Text = "";
            TBPort.Text = "";
            TBIp.Text = "";

            idHardwareConfigurationEditing = 0;
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
