using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class EmployeeViewControl : INotifyPropertyChanged
    {
        EmployeeControl employeeControl = new EmployeeControl();

        private string _TBName;
        private string _TBPispasep;
        private string _TBLeefNumber;
        private string _CBCompany;
        private string _CBSchedule;
        private string _DPAdmission;
        private string _TBPhone;
        private string _TBZipCode;
        private string _TBFilter;
        private ICollectionView _EmployeeList;

        public EmployeeViewControl()
        {
            fillGridEmployee();
        }

        public string TBName
        {
            get { return _TBName; }
            set
            {
                this.MutateVerbose(ref _TBName, value, RaisePropertyChanged());
            }
        }

        public string TBPispasep
        {
            get { return _TBPispasep; }
            set
            {
                string lastPisPasep = _TBPispasep == null ? "" : _TBPispasep;

                this.MutateVerbose(ref _TBPispasep, value, RaisePropertyChanged());
                _TBPispasep = Formatter.getInstance.formatPisPasep(value, lastPisPasep);
            }
        }

        public string TBLeefNumber
        {
            get { return _TBLeefNumber; }
            set
            {
                this.MutateVerbose(ref _TBLeefNumber, value, RaisePropertyChanged());
            }
        }

        public string CBCompany
        {
            get { return _CBCompany; }
            set
            {
                this.MutateVerbose(ref _CBCompany, value, RaisePropertyChanged());
            }
        }

        public string CBSchedule
        {
            get { return _CBSchedule; }
            set
            {
                this.MutateVerbose(ref _CBSchedule, value, RaisePropertyChanged());
            }
        }

        public string DPAdmission
        {
            get { return _DPAdmission; }
            set
            {
                this.MutateVerbose(ref _DPAdmission, value, RaisePropertyChanged());
            }
        }

        public string TBPhone
        {
            get { return _TBPhone; }
            set
            {
                _TBPhone = Formatter.getInstance.formatPhoneNumber(value, _TBPhone == null ? "" : _TBPhone);
            }
        }

        public string TBZipCode
        {
            get { return _TBZipCode; }
            set
            {
                _TBZipCode = Formatter.getInstance.formatZipCode(value, _TBZipCode == null ? "" : _TBZipCode);
            }
        }

        public ICollectionView EmployeeList
        {
            get { return _EmployeeList; }
            set
            {
                this.MutateVerbose(ref _EmployeeList, value, RaisePropertyChanged());
            }
        }

        public string TBFilter
        {
            get { return _TBFilter; }
            set
            {
                this.MutateVerbose(ref _TBFilter, value, RaisePropertyChanged());
                FilterCollection();
            }
        }

        public void fillGridEmployee()
        {
            EmployeeList = CollectionViewSource.GetDefaultView(employeeControl.getAllEmployees());
            EmployeeList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_EmployeeList != null)
            {
                _EmployeeList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Employee data = (Employee)obj;

            if (data is Employee)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.employeeName, _TBFilter);
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
