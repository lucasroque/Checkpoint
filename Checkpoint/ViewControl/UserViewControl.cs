using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class UserViewControl : INotifyPropertyChanged
    {
        UsersControl usersControl = new UsersControl();

        private string _CBUserProfile;
        private string _TBLogin;
        private string _TBPassword;
        private string _TBFilter;
        private ICollectionView _UserList;

        public UserViewControl()
        {
            fillGridUser();
        }

        public string CBUserProfile
        {
            get { return _CBUserProfile; }
            set
            {
                this.MutateVerbose(ref _CBUserProfile, value, RaisePropertyChanged());
            }
        }

        public string TBLogin
        {
            get { return _TBLogin; }
            set
            {
                this.MutateVerbose(ref _TBLogin, value, RaisePropertyChanged());
            }
        }

        public string TBPassword
        {
            get { return _TBPassword; }
            set
            {
                this.MutateVerbose(ref _TBPassword, value, RaisePropertyChanged());
            }
        }

        public ICollectionView UserList
        {
            get { return _UserList; }
            set
            {
                this.MutateVerbose(ref _UserList, value, RaisePropertyChanged());
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

        public void fillGridUser()
        {
            UserList = CollectionViewSource.GetDefaultView(usersControl.getAllUser());
            UserList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_UserList != null)
            {
                _UserList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            User data = (User) obj;

            if (obj is User)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.login, _TBFilter);
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
