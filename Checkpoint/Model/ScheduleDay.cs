using System;

namespace Checkpoint.Model
{
    public class ScheduleDay
    {
        public Int32 idSchedule { get; set; }
        public Int32 idScheduleDay { get; set; }
        public Int32 day_key { get; set; }
        public String descriptionDay { get; set; }
        public String EntryOne { get; set; }
        public String exitOne { get; set; }
        public String entryTwo { get; set; }
        public String exitTwo { get; set; }
        public String entryThree { get; set; }
        public String exitThree { get; set; }
        public String entryTolerance { get; set; }
        public String exitTolerance { get; set; }
        public String workload { get; set; }
        public String endOfficeHour { get; set; }
        public Boolean dayOff { get; set; }
        public Boolean compensation { get; set; }
        public Boolean neutral { get; set; }
        public Boolean automaticDayOff { get; set; }

        public ScheduleDay()
        {

        }

        public ScheduleDay(Int16 day_key, String descriptionDay, String entryTolerance, String exitTolerance, Boolean dayOff)
        {
            this.day_key = day_key;
            this.descriptionDay = descriptionDay;
            this.entryTolerance = entryTolerance;
            this.exitTolerance = exitTolerance;
            this.dayOff = dayOff;
        }

    }
}
