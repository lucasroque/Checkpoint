using System;

namespace Checkpoint.Model
{
    public class Office
    {
        public Int32 idOffice { get; set; }
        public String description { get; set; }

        public Office(int idOffice)
        {
            this.idOffice = idOffice;
        }

        public Office()
        {
        }

        public Office(int idOffice, String description)
        {
            this.idOffice = idOffice;
            this.description = description;
        }

        public override string ToString()
        {
            return description;
        }
    }
}
