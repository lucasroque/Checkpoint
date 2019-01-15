using System;

namespace Checkpoint.Model
{
    public class Absence
    {
        public Int32 idAbsence { get; set; }
        public Employee employee { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Justification justification { get; set; }

        public Absence(int idAbsence)
        {
            this.idAbsence = idAbsence;
        }

        public Absence()
        {
        }

    }
}
