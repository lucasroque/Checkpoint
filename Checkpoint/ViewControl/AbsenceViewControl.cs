using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class AbsenceViewControl : INotifyPropertyChanged
    {
        private string _CBEmployee;
        private string _DPStartDate;
        private string _DPEndDate;
        private string _CBJustification;
        private string _TBFilter;
        private ICollectionView _AbsenceList;

        private AbsenceControl absenceControl = new AbsenceControl();

        public AbsenceViewControl()
        {
            fillGridAbsence();
        }

        public string CBEmployee
        {
            get { return _CBEmployee; }
            set
            {
                this.MutateVerbose(ref _CBEmployee, value, RaisePropertyChanged());
            }
        }

        public string DPStartDate
        {
            get { return _DPStartDate; }
            set
            {
                string lastStartDate = _DPStartDate == null ? "" : _DPStartDate;

                this.MutateVerbose(ref _DPStartDate, value, RaisePropertyChanged());
                _DPStartDate = Formatter.getInstance.formatDate(value, lastStartDate);
            }
        }

        public string DPEndDate
        {
            get { return _DPEndDate; }
            set
            {
                string lastEndDate = _DPEndDate == null ? "" : _DPEndDate;

                this.MutateVerbose(ref _DPEndDate, value, RaisePropertyChanged());
                _DPEndDate = Formatter.getInstance.formatDate(value, lastEndDate);
            }
        }

        public string CBJustification
        {
            get { return _CBJustification; }
            set
            {
                this.MutateVerbose(ref _CBJustification, value, RaisePropertyChanged());
            }
        }

        public ICollectionView AbsenceList
        {
            get { return _AbsenceList; }
            set
            {
                this.MutateVerbose(ref _AbsenceList, value, RaisePropertyChanged());
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

        public void fillGridAbsence()
        {
            AbsenceList = CollectionViewSource.GetDefaultView(absenceControl.getAllAbsence());
            AbsenceList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_AbsenceList != null)
            {
                _AbsenceList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Absence data = (Absence) obj;

            if (data is Absence)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.employee.employeeName, _TBFilter);
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
