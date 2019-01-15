using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System.Windows;

namespace Checkpoint.View
{
    public partial class NewResignationReasonModal : System.Windows.Controls.UserControl
    {
        private ResignationReasonControl resignationReasonControl;

        public NewResignationReasonModal()
        {
            InitializeComponent();

            DataContext = new ResignationReasonViewControl();

            resignationReasonControl = new ResignationReasonControl();
        }


        private void insertResignationReason(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBDescription.Text))
            {
                insertResignationReason();
            }
        }

        private void insertResignationReason()
        {
            ResignationReason resignationReason = getResignationReasonFromControls();
            resignationReasonControl.saveResignationReason(resignationReason);
        }

        private ResignationReason getResignationReasonFromControls()
        {
            ResignationReason resignationReason = new ResignationReason();
            resignationReason.description = TBDescription.Text;

            return resignationReason;
        }
    }
}
