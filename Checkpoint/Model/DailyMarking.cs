using System;
using System.Collections.Generic;

namespace Checkpoint.Model
{
    public class DailyMarking
    {
        public Employee employee { get; set; }
        public DateTime date { get; set; }
        public List<Marking> markings { get; set; }
        public String entryOne { get; set; }
        public String exitOne { get; set; }
        public String entryTwo { get; set; }
        public String exitTwo { get; set; }
        public String entryThree { get; set; }
        public String exitThree { get; set; }
        public String standartHours { get; set; }
        public String faultHours { get; set; }
        public String extraHours { get; set; }
        public String workload { get; set; }

        public DailyMarking(){}

        public DailyMarking(Employee employee, DateTime date)
        {
            this.employee = employee;
            this.date = date;
            markings = new List<Marking>();
        }
    }
}
