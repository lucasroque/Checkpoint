using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.Model
{
    class RejectedMarking
    {
        public Int32 idRejectedMarking { get; set; }
        public Int64 nsr { get; set; }
        public Int64 cont { get; set; }
        public string pisPasep { get; set; }
        public Int16 day { get; set; }
        public Int16 month { get; set; }
        public Int16 year { get; set; }
        public Int16 hour { get; set; }
        public Int16 minute { get; set; }
        public string description { get; set; }

        public RejectedMarking()
        {

        }

    }
}
