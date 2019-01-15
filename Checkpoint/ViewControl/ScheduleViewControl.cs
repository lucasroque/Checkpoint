using Checkpoint.Tools;
using System;
using System.ComponentModel;
using System.Windows.Data;
using Checkpoint.Control;
using Checkpoint.Model;
using System.Collections.Generic;

namespace Checkpoint.ViewControl
{
    class ScheduleViewControl : INotifyPropertyChanged
    {
        ScheduleControl scheduleControl = new ScheduleControl();

        private string _TBDescription;
        private string _EntryOne;

        private ICollectionView _ScheduleList;

        public ScheduleViewControl() {
            fillScheduleList();
        }

        public string TBDescription
        {
            get { return _TBDescription; }
            set
            {
                this.MutateVerbose(ref _TBDescription, value, RaisePropertyChanged());
            }
        }

        public ICollectionView ScheduleList
        {
            get { return _ScheduleList; }
            set
            {
                this.MutateVerbose(ref _ScheduleList, value, RaisePropertyChanged());
            }
        }

        public string EntryOne
        {
            get { return _EntryOne; }
            set
            {
                this.MutateVerbose(ref _EntryOne, value, RaisePropertyChanged());
            }
        }

        public void fillScheduleList()
        {
            ScheduleList = CollectionViewSource.GetDefaultView(scheduleControl.getNewScheduleDays());
        }
        
        public void setScheduleList(List<ScheduleDay> scheduleList)
        {
            ScheduleList = CollectionViewSource.GetDefaultView(scheduleList);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

    }
}
