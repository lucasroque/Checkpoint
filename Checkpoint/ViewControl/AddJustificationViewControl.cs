using Checkpoint.Tools;
using System;
using System.ComponentModel;
namespace Checkpoint.ViewControl
{
    class AddJustificationViewControl : INotifyPropertyChanged
    {

        private string _CBJustification;

        public string CBJustification
        {
            get { return _CBJustification; }
            set
            {
                this.MutateVerbose(ref _CBJustification, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
