using System;
using System.Collections.Generic;

namespace Checkpoint.Model
{
    public class Schedule
    {
        public Int32 idSchedule { get; set; }
        public String description { get; set; }
        public String nightlyStart { get; set; }
        public String overtimePercentage { get; set; }
        public List<ScheduleDay> scheduleDays = new List<ScheduleDay>();

        public Schedule()
        {

        }

        public Schedule(Int32 idSchedule)
        {
            this.idSchedule = idSchedule;
        }

        public Schedule(Int32 idSchedule, String description)
        {
            this.idSchedule = idSchedule;
            this.description = description;
        }

        public override string ToString()
        {
            return description;
        }

    }
}
