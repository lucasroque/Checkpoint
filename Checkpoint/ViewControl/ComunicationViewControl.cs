using Checkpoint.Tools;
using System;
using System.ComponentModel;

namespace Checkpoint.ViewControl
{
    class ComunicationViewControl : INotifyPropertyChanged
    {
        private string _CBHardware;
        private string _TBIp;
        private string _TBPort;
        private string _TBEndDev;


        public string CBHardware
        {
            get { return _CBHardware; }
            set
            {
                this.MutateVerbose(ref _CBHardware, value, RaisePropertyChanged());
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

        public string TBPort
        {
            get { return _TBPort; }
            set
            {
                this.MutateVerbose(ref _TBPort, value, RaisePropertyChanged());
            }
        }

        public string TBEndDev
        {
            get { return _TBEndDev; }
            set
            {
                this.MutateVerbose(ref _TBEndDev, value, RaisePropertyChanged());
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
