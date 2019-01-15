using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class HardwareConfigurationViewControl : INotifyPropertyChanged
    {
        HardwareConfigurationControl hardwareConfigurationControl = new HardwareConfigurationControl();

        private string _CBHardwareConfiguration;
        private string _CBCompany;
        private string _CBHardware;
        private string _TBSerialNumber;
        private string _TBCryptographicKey;
        private string _TBPort;
        private string _TBIp;
        private string _TBCpf;
        private string _TBFilter;
        private ICollectionView _HardwareConfigurationList;

        public HardwareConfigurationViewControl()
        {
            fillGridHardwareConfiguration();
        }

        public string CBHardwareConfiguration
        {
            get { return _CBHardwareConfiguration; }
            set
            {
                this.MutateVerbose(ref _CBHardwareConfiguration, value, RaisePropertyChanged());
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

        public string CBHardware
        {
            get { return _CBHardware; }
            set
            {
                this.MutateVerbose(ref _CBHardware, value, RaisePropertyChanged());
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

        public string TBCryptographicKey
        {
            get { return _TBCryptographicKey; }
            set
            {
                this.MutateVerbose(ref _TBCryptographicKey, value, RaisePropertyChanged());
            }
        }

        public string TBPort
        {
            get { return _TBPort; }
            set
            {
                this.MutateVerbose(ref _TBPort, value, RaisePropertyChanged());
            }
        }

        public string TBIp
        {
            get { return _TBIp; }
            set
            {
                this.MutateVerbose(ref _TBIp, value, RaisePropertyChanged());
            }
        }

        public string TBCpf
        {
            get { return _TBCpf; }
            set
            {
                this.MutateVerbose(ref _TBCpf, value, RaisePropertyChanged());
            }
        }

        public ICollectionView HardwareConfigurationList
        {
            get { return _HardwareConfigurationList; }
            set
            {
                this.MutateVerbose(ref _HardwareConfigurationList, value, RaisePropertyChanged());
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

        public void fillGridHardwareConfiguration()
        {
            HardwareConfigurationList = CollectionViewSource.GetDefaultView(hardwareConfigurationControl.getAllHardwareConfigurations());
            HardwareConfigurationList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_HardwareConfigurationList != null)
            {
                _HardwareConfigurationList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            HardwareConfiguration data = (HardwareConfiguration) obj;

            if (obj is HardwareConfiguration)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.hardware.description, _TBFilter);
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