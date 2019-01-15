using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class OfficeViewControl : INotifyPropertyChanged
    {
        OfficeControl officeControl = new OfficeControl();

        private string _TBDescription;
        private string _TBFilter;
        private ICollectionView _OfficeList;

        public OfficeViewControl()
        {
            fillGridOffice();
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public ICollectionView OfficeList
        {
            get { return _OfficeList; }
            set
            {
                this.MutateVerbose(ref _OfficeList, value, RaisePropertyChanged());
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

        public void fillGridOffice()
        {
            OfficeList = CollectionViewSource.GetDefaultView(officeControl.getAllOffices());
            OfficeList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_OfficeList != null)
            {
                _OfficeList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Office data = (Office) obj;

            if (obj is Office)
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
