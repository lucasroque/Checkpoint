using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.View
{

    public partial class OfficeRegisterView : System.Windows.Controls.UserControl
    {
        private OfficeControl officeControl;
        private OfficeViewControl officeViewControl;

        private int idOfficeEditing = 0;

        public OfficeRegisterView()
        {
            InitializeComponent();

            officeControl = new OfficeControl();
            officeViewControl = new OfficeViewControl();

            DataContext = officeViewControl;
        }

        private void upsertOffice(object sender, RoutedEventArgs e)
        {

            if (!officeControl.validateDescription(TBDescription.Text) && idOfficeEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Descrição já cadastrado."), "DHMain");
                return;
            }

            if (!"".Equals(TBDescription.Text))
            {
                upsertOffice();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadOffice(object sender, RoutedEventArgs e)
        {
            Office office = ((FrameworkElement)sender).DataContext as Office;
            loadOffice(office);
        }

        private async void deleteOffice(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Cargo?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Office office = ((FrameworkElement)sender).DataContext as Office;
                deleteOffice(office);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertOffice()
        {
            Office office = getOfficeFromControls();
            Boolean success;

            if (idOfficeEditing != 0)
            {
                office.idOffice = idOfficeEditing;
                success = officeControl.updateOffice(office);
                saveModeControls();
            }
            else
            {
                success = officeControl.saveOffice(office);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Cargo salvo com sucesso."), "DHMain");
                officeViewControl.fillGridOffice();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Cargo."), "DHMain");
            }
        }

        private void loadOffice(Office office)
        {
            TBDescription.Text = office.description;

            idOfficeEditing = office.idOffice;

            editModeControls();
        }

        private void deleteOffice(Office office)
        {
            Boolean success = officeControl.deleteOffice(office);
            officeViewControl.fillGridOffice();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Cargo excluído com sucesso."), "DHMain");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Cargo."), "DHMain");
            }
        }

        private Office getOfficeFromControls()
        {
            Office office = new Office();
            office.description = TBDescription.Text;

            return office;
        }

        private void cleanControls()
        {
            TBDescription.Text = "";

            idOfficeEditing = 0;
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
