using System;

namespace Checkpoint.Model
{
    public class Recess
    {
        public Int32 idRecess { get; set; }
        public Company company { get; set; }
        public String description { get; set; }
        public DateTime recessDate { get; set; }
        public Department department { get; set; }

        public Recess()
        {

        }

        public Recess(String description, DateTime recessDate)
        {
            this.description = description;
            this.recessDate = recessDate;
        }

        public override string ToString()
        {
            return description;
        }
    }
}
