using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using Checkpoint.Model;
using System.Windows.Data;

namespace Checkpoint.ViewControl
{
    class DepartmentViewControl : INotifyPropertyChanged
    {
        DepartmentControl departmentControl = new DepartmentControl();

        private string _TBDescription;
        private string _TBFilter;
        private ICollectionView _DepartmentList;

        public DepartmentViewControl()
        {
            fillGridDepartment();
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public ICollectionView DepartmentList
        {
            get { return _DepartmentList; }
            set
            {
                this.MutateVerbose(ref _DepartmentList, value, RaisePropertyChanged());
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

        public void fillGridDepartment()
        {
            DepartmentList = CollectionViewSource.GetDefaultView(departmentControl.getAllDepartments());
            DepartmentList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_DepartmentList != null)
            {
                _DepartmentList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Department data = (Department) obj;

            if (data is Department)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.description, _TBFilter);
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
