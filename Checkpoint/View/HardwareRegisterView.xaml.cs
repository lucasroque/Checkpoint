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

namespace Checkpoint.View
{

    public partial class HardwareRegisterView : System.Windows.Controls.UserControl
    {
        private HardwareControl hardwareControl;
        private MakerControl makerControl;
        private HardwareViewControl hardwareViewControl;


        List<Maker> allCompanies = new List<Maker>();

        private int idHardwareEditing = 0;

        public HardwareRegisterView()
        {
            InitializeComponent();
            
            hardwareControl = new HardwareControl();
            makerControl = new MakerControl();
            hardwareViewControl = new HardwareViewControl();

            DataContext = hardwareViewControl;
            
            fillCBMaker();
            fillImageControl();
        }

        private void fillCBMaker()
        {
            allCompanies.Clear();
            allCompanies = makerControl.getAllMakers();
            CBMaker.ItemsSource = allCompanies;
        }

        private void fillImageControl()
        {
            imgHardwarePhoto.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
        }

        private void loadPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Imagem";
            op.Filter = "Todas as Imagens Suportadas|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                imgHardwarePhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void upsertHardware(object sender, RoutedEventArgs e)
        {
            if (CBMaker.SelectedIndex != -1 && !"".Equals(TBDescription.Text))
            {
                upsertHardware();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadHardware(object sender, RoutedEventArgs e)
        {
            Hardware hardware = ((FrameworkElement)sender).DataContext as Hardware;
            loadHardware(hardware);
        }

        private async void deleteHardware(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Equipamento?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Hardware hardware = ((FrameworkElement)sender).DataContext as Hardware;
                deleteHardware(hardware);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertHardware()
        {
            Hardware hardware = getHardwareFromControls();
            Boolean success;

            if (idHardwareEditing != 0)
            {
                hardware.idHardware = idHardwareEditing;
                success = hardwareControl.updateHardware(hardware);
                saveModeControls();
            }
            else
            {
                success = hardwareControl.saveHardware(hardware);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Equipamento salvo com sucesso."), "DHMain");
                hardwareViewControl.fillGridHardware();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Equipamento."), "DHMain");
            }
        }

        private void loadHardware(Hardware hardware)
        {
            int index = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Maker cp = (Maker)allCompanies[i];

                if (hardware.maker.idMaker == cp.idMaker)
                {
                    index = i;
                    break;
                }
            }

            imgHardwarePhoto.Source = hardware.hardwareImage;
            CBMaker.SelectedIndex = index;
            TBDescription.Text = hardware.description;

            idHardwareEditing = hardware.idHardware;

            editModeControls();
        }

        private void deleteHardware(Hardware hardware)
        {
            Boolean success = hardwareControl.deleteHardware(hardware);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Equipamento excluído com sucesso."), "DHMain");
                hardwareViewControl.fillGridHardware();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Equipamento."), "DHMain");
            }
        }

        private Hardware getHardwareFromControls()
        {
            Hardware hardware = new Hardware();
            hardware.hardwareImage = (BitmapImage)imgHardwarePhoto.Source;
            hardware.maker = (Maker)CBMaker.SelectedItem;
            hardware.description = TBDescription.Text;

            return hardware;
        }

        private void cleanControls()
        {
            CBMaker.SelectedIndex = -1;
            TBDescription.Text = "";

            idHardwareEditing = 0;
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
