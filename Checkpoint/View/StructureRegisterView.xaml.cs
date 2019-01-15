using Checkpoint.Control;
using Checkpoint.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.View
{
    public partial class StructureRegisterView : System.Windows.Controls.UserControl
    {
        private StructureControl structureControl;
        private CompanyControl companyControl;

        private int idStructureEditing = 0;

        public StructureRegisterView()
        {
            InitializeComponent();

            structureControl = new StructureControl();
            companyControl = new CompanyControl();

            fillGridStructure();
            fillCBCompany();

        }

        private void fillGridStructure()
        {
            ObservableCollection<Structure> structureList = new ObservableCollection<Structure>(structureControl.getAllStructures());
            GDStructure.ItemsSource = structureList;
        }

        private void fillCBCompany()
        {
            CBCompany.ItemsSource = companyControl.getAllCompanies();
        }

        private void upsertStructure(object sender, RoutedEventArgs e)
        {
            upsertStructure();
        }

        private void loadStructure(object sender, RoutedEventArgs e)
        {
            Structure structure = ((FrameworkElement)sender).DataContext as Structure;
            loadStructure(structure);
        }

        private void deleteStructure(object sender, RoutedEventArgs e)
        {
            Structure structure = ((FrameworkElement)sender).DataContext as Structure;
            deleteStructure(structure);
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertStructure()
        {
            Structure structure = getStructureFromControls();

            if (idStructureEditing != 0)
            {
                structure.idStructure = idStructureEditing;
                structureControl.updateStructure(structure);
                saveModeControls();
            }
            else
            {
                structureControl.saveStructure(structure);
            }

            cleanControls();
            fillGridStructure();
        }

        private void loadStructure(Structure structure)
        {
            CBCompany.SelectedItem = structure.company;
            TBDescription.Text = structure.description;
            TBInside.Text = structure.inside;
            CBResponsible.SelectedItem = structure.responsible;

            idStructureEditing = structure.idStructure;

            editModeControls();
        }

        private void deleteStructure(Structure structure)
        {
            structureControl.deleteStructure(structure);
            fillGridStructure();
        }

        private Structure getStructureFromControls()
        {
            Structure structure = new Structure();
            structure.company = (Company)CBCompany.SelectedItem;
            structure.description = TBDescription.Text;
            structure.inside = TBInside.Text;
            structure.responsible = (Employee)CBResponsible.SelectedItem;

            return structure;
        }

        private void cleanControls()
        {
            CBCompany.SelectedIndex = -1;
            TBDescription.Text = "";
            TBInside.Text = "";
            CBResponsible.SelectedIndex = -1;

            idStructureEditing = 0;
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
