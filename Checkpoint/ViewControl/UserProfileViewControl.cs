using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Control;
using System.Windows.Data;
using Checkpoint.Model;

namespace Checkpoint.ViewControl
{
    class UserProfileViewControl : INotifyPropertyChanged
    {
        UserProfilesControl userProfilesControl = new UserProfilesControl();

        private string _CBSecurityLevel;
        private string _TBDescription;
        private string _TBFilter;
        private ICollectionView _UserProfilesList;

        public UserProfileViewControl()
        {
            fillGridUserProfiles();
        }

        public string CBSecurityLevel
        {
            get { return _CBSecurityLevel; }
            set
            {
                this.MutateVerbose(ref _CBSecurityLevel, value, RaisePropertyChanged());
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

        public ICollectionView UserProfilesList
        {
            get { return _UserProfilesList; }
            set
            {
                this.MutateVerbose(ref _UserProfilesList, value, RaisePropertyChanged());
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

        public void fillGridUserProfiles()
        {
            UserProfilesList = CollectionViewSource.GetDefaultView(userProfilesControl.getAllUserProfile());
            UserProfilesList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_UserProfilesList != null)
            {
                _UserProfilesList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            UserProfile data = (UserProfile) obj;

            if (obj is UserProfile)
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
