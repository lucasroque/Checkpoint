using MaterialDesignThemes.Wpf;
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Checkpoint.View
{
    public partial class ResignationReasonRegisterView : System.Windows.Controls.UserControl
    {
        private ResignationReasonControl resignationReasonControl;
        private ResignationReasonViewControl resignationReasonViewControl = new ResignationReasonViewControl();

        private int idResignationReasonEditing = 0;

        public ResignationReasonRegisterView()
        {
            InitializeComponent();
            
            resignationReasonControl = new ResignationReasonControl();
            resignationReasonViewControl = new ResignationReasonViewControl();

            DataContext = resignationReasonViewControl;
            
        }

        private void upsertResignationReason(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBDescription.Text))
            {
                upsertResignationReason();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadResignationReason(object sender, RoutedEventArgs e)
        {
            ResignationReason resignationReason = ((FrameworkElement)sender).DataContext as ResignationReason;
            loadResignationReason(resignationReason);
        }

        private async void deleteResignationReason(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Motivo de Demissão?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                ResignationReason resignationReason = ((FrameworkElement)sender).DataContext as ResignationReason;
                deleteResignationReason(resignationReason);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertResignationReason()
        {
            ResignationReason resignationReason = getResignationReasonFromControls();
            Boolean success;

            if (idResignationReasonEditing != 0)
            {
                resignationReason.idResignationReason = idResignationReasonEditing;
                success = resignationReasonControl.updateResignationReason(resignationReason);
                saveModeControls();
            }
            else
            {
                success = resignationReasonControl.saveResignationReason(resignationReason);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Motivo de Demissão salvo com sucesso."), "DHMain");
                resignationReasonViewControl.fillGridResignationReason();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Motivo de Demissão."), "DHMain");
            }
        }

        private void loadResignationReason(ResignationReason resignationReason)
        {
            TBDescription.Text = resignationReason.description;

            idResignationReasonEditing = resignationReason.idResignationReason;

            editModeControls();
        }

        private void deleteResignationReason(ResignationReason resignationReason)
        {
            Boolean success = resignationReasonControl.deleteResignationReason(resignationReason);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Motivo de Demissão excluído com sucesso."), "DHMain");
                resignationReasonViewControl.fillGridResignationReason();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Movivo de Demissão."), "DHMain");
            }
        }

        private ResignationReason getResignationReasonFromControls()
        {
            ResignationReason resignationReason = new ResignationReason();
            resignationReason.description = TBDescription.Text;

            return resignationReason;
        }

        private void cleanControls()
        {
            TBDescription.Text = "";

            idResignationReasonEditing = 0;
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
