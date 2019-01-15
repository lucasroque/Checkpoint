using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.Tools;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Checkpoint.ViewModal
{
    public partial class AdjustmentModal : System.Windows.Controls.UserControl
    {

        private AdjustmentControl adjustmentControl;
        private int idAdjustment = 0;

        public AdjustmentModal()
        {
            InitializeComponent();

            DataContext = new AdjustmentViewControl();

            adjustmentControl = new AdjustmentControl();
        }

        private void loadAdjustment()
        {
            Adjustment adjustment = adjustmentControl.getAdjustment();

            if (adjustment != null)
            {
                TBLastMarkingNsr.Text = Convert.ToString(adjustment.adjLastMarkingNsr);

                idAdjustment = adjustment.idAdjustment;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadAdjustment();
        }

        private void saveAdjustment(object sender, RoutedEventArgs e)
        {

            if (!"".Equals(TBLastMarkingNsr.Text))
            {

                if (!TBLastMarkingNsr.Text.All(char.IsNumber))
                {
                    DialogHost.Show(new SampleMessageDialog("Última Marcação Inválida."), "DHModal");
                    return;
                }

                Adjustment adjustment = new Adjustment();
                adjustment.adjLastMarkingNsr = Convert.ToInt32(TBLastMarkingNsr.Text);

                if (idAdjustment == 0)
                {
                    adjustmentControl.saveAdjustment(adjustment);
                }
                else
                {
                    adjustment.idAdjustment = idAdjustment;
                    adjustmentControl.updateAdjustment(adjustment);
                }
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHModal");
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
