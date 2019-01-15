using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class ResignationReasonViewControl : INotifyPropertyChanged
    {
        ResignationReasonControl resignationReasonControl = new ResignationReasonControl();

        private string _TBDescription;
        private string _TBFilter;
        private ICollectionView _ResignationReasonList;

        public ResignationReasonViewControl()
        {
            fillGridResignationReason();
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public ICollectionView ResignationReasonList
        {
            get { return _ResignationReasonList; }
            set
            {
                this.MutateVerbose(ref _ResignationReasonList, value, RaisePropertyChanged());
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

        public void fillGridResignationReason()
        {
            ResignationReasonList = CollectionViewSource.GetDefaultView(resignationReasonControl.getAllResignationReasons());
            ResignationReasonList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_ResignationReasonList != null)
            {
                _ResignationReasonList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            ResignationReason data = (ResignationReason) obj;

            if (obj is ResignationReason)
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
