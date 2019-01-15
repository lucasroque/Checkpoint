using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class HardwareViewControl : INotifyPropertyChanged
    {
        HardwareControl hardwareControl = new HardwareControl();

        private string _CBHardware;
        private string _TBDescription;
        private string _TBSerialNumber;
        private string _TBFilter;
        private ICollectionView _HardwareList;

        public HardwareViewControl()
        {
            fillGridHardware();
        }

        public string CBHardware
        {
            get { return _CBHardware; }
            set
            {
                this.MutateVerbose(ref _CBHardware, value, RaisePropertyChanged());
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

        public string TBSerialNumber
        {
            get { return _TBSerialNumber; }
            set
            {
                this.MutateVerbose(ref _TBSerialNumber, value, RaisePropertyChanged());
            }
        }

        public ICollectionView HardwareList
        {
            get { return _HardwareList; }
            set
            {
                this.MutateVerbose(ref _HardwareList, value, RaisePropertyChanged());
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

        public void fillGridHardware()
        {
            HardwareList = CollectionViewSource.GetDefaultView(hardwareControl.getAllHardwares());
            HardwareList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_HardwareList != null)
            {
                _HardwareList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Hardware data = (Hardware) obj;

            if (obj is Hardware)
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