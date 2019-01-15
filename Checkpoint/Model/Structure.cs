using System;

namespace Checkpoint.Model
{
    public class Structure
    {
        public Int32 idStructure { get; set; }
        public Company company { get; set; }
        public String description { get; set; }
        public String inside { get; set; }
        public Employee responsible { get; set; }

        public override string ToString()
        {
            return description;
        }
    }
}
