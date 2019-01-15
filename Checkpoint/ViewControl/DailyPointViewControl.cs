using System;
using Checkpoint.Tools;
using System.ComponentModel;
using Checkpoint.Model;
using System.Windows.Data;
using Checkpoint.Control;

namespace Checkpoint.ViewControl
{
    class DailyPointViewControl : INotifyPropertyChanged
    {
        EmployeeControl employeeControl = new EmployeeControl();
        ScheduleControl scheduleControl = new ScheduleControl();

        private string _TBFilterEmployee;
        private ICollectionView _EmployeeList;
        private string _TBFilterSchedule;
        private ICollectionView _ScheduleList;

        private ICollectionView _MarkingList;

        public DailyPointViewControl()
        {
            fillCBCompany();
            fillCBSchedule();
        }

        public ICollectionView EmployeeList
        {
            get { return _EmployeeList; }
            set
            {
                this.MutateVerbose(ref _EmployeeList, value, RaisePropertyChanged());
            }
        }

        public ICollectionView ScheduleList
        {
            get { return _ScheduleList; }
            set
            {
                this.MutateVerbose(ref _ScheduleList, value, RaisePropertyChanged());
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

        public string TBFilterSchedule
        {
            get { return _TBFilterSchedule; }
            set
            {
                this.MutateVerbose(ref _TBFilterSchedule, value, RaisePropertyChanged());
                FilterScheduleCollection();
            }
        }

        public void fillCBCompany()
        {
            EmployeeList = CollectionViewSource.GetDefaultView(employeeControl.getAllEmployeesControl());
            EmployeeList.Filter = new Predicate<object>(FilterEmployee);
        }

        public void fillCBSchedule()
        {
            ScheduleList = CollectionViewSource.GetDefaultView(scheduleControl.getAllSchedulesControl());
            ScheduleList.Filter = new Predicate<object>(FilterSchedule);
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

        private void FilterScheduleCollection()
        {
            if (_ScheduleList != null)
            {
                _ScheduleList.Refresh();
            }
        }

        public bool FilterSchedule(object obj)
        {
            Schedule data = (Schedule)obj;

            if (data is Schedule)
            {
                if (!string.IsNullOrEmpty(_TBFilterSchedule))
                {
                    return Util.contains(data.description, _TBFilterSchedule);
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
