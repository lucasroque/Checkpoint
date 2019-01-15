using Checkpoint.Tools;
using System;
using System.ComponentModel;

namespace Checkpoint.ViewControl
{
    class OpenCallViewControl : INotifyPropertyChanged
    {
        private string _CBCompany;
        private string _TBSubject;
        private string _TBContent;

        public string CBCompany
        {
            get { return _CBCompany; }
            set
            {
                this.MutateVerbose(ref _CBCompany, value, RaisePropertyChanged());
            }
        }

        public string TBSubject
        {
            get { return _TBSubject; }
            set
            {
                this.MutateVerbose(ref _TBSubject, value, RaisePropertyChanged());
            }
        }

        public string TBContent
        {
            get { return _TBContent; }
            set
            {
                this.MutateVerbose(ref _TBContent, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
