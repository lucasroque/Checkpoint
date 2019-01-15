using Checkpoint.Tools;
using System;
using System.ComponentModel;

namespace Checkpoint.ViewControl
{
    class MailsManagerViewControl : INotifyPropertyChanged
    {
        private string _TBCallOpenUser;
        private string _TBCallOpenPassword;
        private string _TBCallOpenHost;
        private string _TBCallOpenPort;

        private string _TBBobbinRequestUser;
        private string _TBBobbinRequestPassword;
        private string _TBBobbinRequestHost;
        private string _TBBobbinRequestPort;

        private string _TBBadgeRequestUser;
        private string _TBBadgeRequestPassword;
        private string _TBBadgeRequestHost;
        private string _TBBadgeRequestPort;

        public string TBCallOpenUser
        {
            get { return _TBCallOpenUser; }
            set
            {
                this.MutateVerbose(ref _TBCallOpenUser, value, RaisePropertyChanged());
            }
        }

        public string TBCallOpenPassword
        {
            get { return _TBCallOpenPassword; }
            set
            {
                this.MutateVerbose(ref _TBCallOpenPassword, value, RaisePropertyChanged());
            }
        }

        public string TBCallOpenHost
        {
            get { return _TBCallOpenHost; }
            set
            {
                this.MutateVerbose(ref _TBCallOpenHost, value, RaisePropertyChanged());
            }
        }

        public string TBCallOpenPort
        {
            get { return _TBCallOpenPort; }
            set
            {
                this.MutateVerbose(ref _TBCallOpenPort, value, RaisePropertyChanged());
            }
        }

        public string TBBobbinRequestUser
        {
            get { return _TBBobbinRequestUser; }
            set
            {
                this.MutateVerbose(ref _TBBobbinRequestUser, value, RaisePropertyChanged());
            }
        }

        public string TBBobbinRequestPassword
        {
            get { return _TBBobbinRequestPassword; }
            set
            {
                this.MutateVerbose(ref _TBBobbinRequestPassword, value, RaisePropertyChanged());
            }
        }

        public string TBBobbinRequestHost
        {
            get { return _TBBobbinRequestHost; }
            set
            {
                this.MutateVerbose(ref _TBBobbinRequestHost, value, RaisePropertyChanged());
            }
        }

        public string TBBobbinRequestPort
        {
            get { return _TBBobbinRequestPort; }
            set
            {
                this.MutateVerbose(ref _TBBobbinRequestPort, value, RaisePropertyChanged());
            }
        }

        public string TBBadgeRequestUser
        {
            get { return _TBBadgeRequestUser; }
            set
            {
                this.MutateVerbose(ref _TBBadgeRequestUser, value, RaisePropertyChanged());
            }
        }

        public string TBBadgeRequestPassword
        {
            get { return _TBBadgeRequestPassword; }
            set
            {
                this.MutateVerbose(ref _TBBadgeRequestPassword, value, RaisePropertyChanged());
            }
        }

        public string TBBadgeRequestHost
        {
            get { return _TBBadgeRequestHost; }
            set
            {
                this.MutateVerbose(ref _TBBadgeRequestHost, value, RaisePropertyChanged());
            }
        }

        public string TBBadgeRequestPort
        {
            get { return _TBBadgeRequestPort; }
            set
            {
                this.MutateVerbose(ref _TBBadgeRequestPort, value, RaisePropertyChanged());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
