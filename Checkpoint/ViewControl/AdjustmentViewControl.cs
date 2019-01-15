using Checkpoint.Tools;
using System;
using System.ComponentModel;

namespace Checkpoint.ViewControl
{
    class AdjustmentViewControl : INotifyPropertyChanged
    {

        private string _TBLastMarkingNsr;

        public string TBLastMarkingNsr
        {
            get { return _TBLastMarkingNsr; }
            set
            {
                this.MutateVerbose(ref _TBLastMarkingNsr, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
