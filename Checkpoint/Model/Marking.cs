using System;

namespace Checkpoint.Model
{
    public class Marking
    {
        public Int32 idMarking { get; set; }
        public Int64 nsr { get; set; }
        public Int64 cont { get; set; }
        public string pisPasep { get; set; }
        public Int16 day { get; set; }
        public Int16 month { get; set; }
        public Int16 year { get; set; }
        public Int16 hour { get; set; }
        public Int16 minute { get; set; }
        public Int32 idJustification { get; set; }

        public Marking()
        {

        }

        public Marking (Int64 nsr, Int64 cont, string pisPasep, Int16 day, Int16 month, Int16 year, Int16 hour, Int16 minute)
        {
            this.nsr = nsr;
            this.cont = cont;
            this.pisPasep = pisPasep;
            this.day = day;
            this.month = month;
            this.year = year;
            this.hour = hour;
            this.minute = minute;
        }

    }
}
