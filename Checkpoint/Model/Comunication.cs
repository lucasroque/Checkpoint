using System;

namespace Checkpoint.Model
{
    public class Comunication
    {
        public Int32 idComunication { get; set; }
        public Hardware hardware { get; set; }
        public String ip { get; set; }
        public String port { get; set; }
        public String endDev { get; set; }
    }
}
