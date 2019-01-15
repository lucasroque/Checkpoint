using System;

namespace Checkpoint.Model
{
    public class ResignationReason
    {
        public Int32 idResignationReason { get; set; }
        public String description { get; set; }

        public ResignationReason(int idResignationReason)
        {
            this.idResignationReason = idResignationReason;
        }

        public ResignationReason()
        {
        }

        public override string ToString()
        {
            return description;
        }
    }
}
