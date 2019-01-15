using System;

namespace Checkpoint.Model
{
    public class Justification
    {
        public Int32 idJustification { get; set; }
        public String description { get; set; }

        public Justification(int idJustification)
        {
            this.idJustification = idJustification;
        }

        public Justification()
        {
        }

        public override string ToString()
        {
            return description;
        }
    }
}
