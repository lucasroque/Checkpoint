using Checkpoint.Tools;
using System;
using System.ComponentModel;

namespace Checkpoint.ViewControl
{
    class ImportEmployeesViewControl : INotifyPropertyChanged
    {

        private string _CBCompany;
        private string _CBSchedule;

        public string CBCompany
        {
            get { return _CBCompany; }
            set
            {
                this.MutateVerbose(ref _CBCompany, value, RaisePropertyChanged());
            }
        }

        public string CBSchedule
        {
            get { return _CBSchedule; }
            set
            {
                this.MutateVerbose(ref _CBSchedule, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

    }
}
