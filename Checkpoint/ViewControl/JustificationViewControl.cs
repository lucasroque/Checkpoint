using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class JustificationViewControl : INotifyPropertyChanged
    {
        JustificationControl justificationControl = new JustificationControl();

        private string _TBDescription;
        private string _TBFilter;
        private ICollectionView _JustificationList;

        public JustificationViewControl()
        {
            fillGridJustification();
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public ICollectionView JustificationList
        {
            get { return _JustificationList; }
            set
            {
                this.MutateVerbose(ref _JustificationList, value, RaisePropertyChanged());
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

        public void fillGridJustification()
        {
            JustificationList = CollectionViewSource.GetDefaultView(justificationControl.getAllJustifications());
            JustificationList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_JustificationList != null)
            {
                _JustificationList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Justification data = (Justification) obj;

            if (obj is Justification)
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
