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
    public partial class JustificationRegisterView : System.Windows.Controls.UserControl
    {
        private JustificationControl justificationControl;
        private JustificationViewControl justificationViewControl;

        private int idJustificationEditing = 0;

        public JustificationRegisterView()
        {
            InitializeComponent();
            
            justificationControl = new JustificationControl();
            justificationViewControl = new JustificationViewControl();

            DataContext = justificationViewControl;
            
        }

        private void upsertJustification(object sender, RoutedEventArgs e)
        {

            if (!justificationControl.validateDescription(TBDescription.Text) && idJustificationEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Descrição já cadastrado."), "DHMain");
                return;
            }

            if (!"".Equals(TBDescription.Text))
            {
                upsertJustification();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadJustification(object sender, RoutedEventArgs e)
        {
            Justification justification = ((FrameworkElement)sender).DataContext as Justification;
            loadJustification(justification);
        }

        private async void deleteJustification(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir esta Justificativa?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Justification justification = ((FrameworkElement)sender).DataContext as Justification;
                deleteJustification(justification);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertJustification()
        {
            Justification justification = getJustificationFromControls();
            Boolean success;

            if (idJustificationEditing != 0)
            {
                justification.idJustification = idJustificationEditing;
                success = justificationControl.updateJustification(justification);
                saveModeControls();
            }
            else
            {
                success = justificationControl.saveJustification(justification);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Justificativa salva com sucesso."), "DHMain");
                justificationViewControl.fillGridJustification();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Justificativa."), "DHMain");
            }
        }

        private void loadJustification(Justification justification)
        {
            TBDescription.Text = justification.description;

            idJustificationEditing = justification.idJustification;

            editModeControls();
        }

        private void deleteJustification(Justification justification)
        {
            Boolean success = justificationControl.deleteJustification(justification);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Justificativa excluída com sucesso."), "DHMain");
                justificationViewControl.fillGridJustification();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Justificativa."), "DHMain");
            }
        }

        private Justification getJustificationFromControls()
        {
            Justification justification = new Justification();
            justification.description = TBDescription.Text;

            return justification;
        }

        private void cleanControls()
        {
            TBDescription.Text = "";

            idJustificationEditing = 0;
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