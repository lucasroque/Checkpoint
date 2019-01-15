using System;

namespace Checkpoint.Model
{
    public class Maker
    {
        public Int32 idMaker { get; set; }
        public String description { get; set; }

        public override string ToString()
        {
            return description;
        }
    }
}
