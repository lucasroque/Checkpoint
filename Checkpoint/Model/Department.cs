using System;

namespace Checkpoint.Model
{
    public class Department
    {
        public Int32 idDepartment { get; set; }
        public String description { get; set; }

        public Department(int idDepartment)
        {
            this.idDepartment = idDepartment;
        }

        public Department(int idDepartment, String description)
        {
            this.idDepartment = idDepartment;
            this.description = description;
        }

        public Department()
        {
        }

        public override string ToString()
        {
            return description;
        }
    }
}
