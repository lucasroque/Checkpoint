using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class RecessViewControl : INotifyPropertyChanged
    {
        RecessControl recessControl = new RecessControl();

        private string _CBCompany;
        private string _TBDescription;
        private string _DPRecessDate;
        private string _CBDepartment;
        private string _TBFilter;
        private ICollectionView _RecessList;

        public RecessViewControl()
        {
            fillGridRecess();
        }

        public string CBCompany
        {
            get { return _CBCompany; }
            set
            {
                this.MutateVerbose(ref _CBCompany, value, RaisePropertyChanged());
            }
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public string DPRecessDate
        {
            get { return _DPRecessDate; }
            set
            {
                string lastRecessDate = _DPRecessDate == null ? "" : _DPRecessDate;

                this.MutateVerbose(ref _DPRecessDate, value, RaisePropertyChanged());
                _DPRecessDate = Formatter.getInstance.formatDate(value, lastRecessDate);
            }
        }

        public string CBDepartment
        {
            get { return _CBDepartment; }
            set
            {
                this.MutateVerbose(ref _CBDepartment, value, RaisePropertyChanged());
            }
        }

        public ICollectionView RecessList
        {
            get { return _RecessList; }
            set
            {
                this.MutateVerbose(ref _RecessList, value, RaisePropertyChanged());
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

        public void fillGridRecess()
        {
            RecessList = CollectionViewSource.GetDefaultView(recessControl.getAllRecess());
            RecessList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_RecessList != null)
            {
                _RecessList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Recess data = (Recess) obj;

            if (obj is Recess)
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
