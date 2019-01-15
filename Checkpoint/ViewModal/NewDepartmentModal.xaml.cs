using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{
    public partial class NewDepartmentModal : System.Windows.Controls.UserControl
    {

        private DepartmentControl departmentControl;
        private CompanyControl companyControl;

        public NewDepartmentModal()
        {
            InitializeComponent();

            DataContext = new DepartmentViewControl();

            departmentControl = new DepartmentControl();
            companyControl = new CompanyControl();

        }

        private void insertDepartment(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBDescription.Text))
            {
                insertDepartment();
            }
        }

        private void insertDepartment()
        {
            Department department = getDepartmentFromControls();
            departmentControl.saveDepartment(department);
        }

        private Department getDepartmentFromControls()
        {
            Department department = new Department();
            department.description = TBDescription.Text;

            return department;
        }

    }
}
