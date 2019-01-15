using System;
using Checkpoint.Tools;
using System.ComponentModel;
using Checkpoint.Model;
using System.Windows.Data;
using Checkpoint.Control;

namespace Checkpoint.ViewControl
{
    class MarkingCardViewControl : INotifyPropertyChanged
    {
        EmployeeControl employeeControl = new EmployeeControl();

        private string _TBFilterEmployee;
        private ICollectionView _EmployeeList;

        public MarkingCardViewControl()
        {
            fillCBCompany();
        }

        public ICollectionView EmployeeList
        {
            get { return _EmployeeList; }
            set
            {
                this.MutateVerbose(ref _EmployeeList, value, RaisePropertyChanged());
            }
        }

        public string TBFilterEmployee
        {
            get { return _TBFilterEmployee; }
            set
            {
                this.MutateVerbose(ref _TBFilterEmployee, value, RaisePropertyChanged());
                FilterEmployeeCollection();
            }
        }

        public void fillCBCompany()
        {
            EmployeeList = CollectionViewSource.GetDefaultView(employeeControl.getAllEmployees());
            EmployeeList.Filter = new Predicate<object>(FilterEmployee);
        }

        public void fillCBCompanyDepartment(int idDepartment, int idOffice)
        {
            EmployeeList = CollectionViewSource.GetDefaultView(employeeControl.getAllEmployeesFromDepartment(idDepartment, idOffice));
            EmployeeList.Filter = new Predicate<object>(FilterEmployee);
        }

        private void FilterEmployeeCollection()
        {
            if (_EmployeeList != null)
            {
                _EmployeeList.Refresh();
            }
        }

        public bool FilterEmployee(object obj)
        {
            Employee data = (Employee)obj;

            if (data is Employee)
            {
                if (!string.IsNullOrEmpty(_TBFilterEmployee))
                {
                    return Util.contains(data.employeeName, _TBFilterEmployee);
                }
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
