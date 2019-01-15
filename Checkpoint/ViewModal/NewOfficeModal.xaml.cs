using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{
    public partial class NewOfficeModal : System.Windows.Controls.UserControl
    {

        private OfficeControl officeControl;

        public NewOfficeModal()
        {
            InitializeComponent();

            DataContext = new OfficeViewControl();

            officeControl = new OfficeControl();
        }


        private void insertOffice(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBDescription.Text))
            {
                insertOffice();
            }
        }

        private void insertOffice()
        {
            Office office = getOfficeFromControls();
            officeControl.saveOffice(office);
        }

        private Office getOfficeFromControls()
        {
            Office office = new Office();
            office.description = TBDescription.Text;

            return office;
        }

    }
}
