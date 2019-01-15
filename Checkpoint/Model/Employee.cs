using System;
using System.Windows.Media.Imaging;

namespace Checkpoint.Model
{
    public class Employee
    {
        public Int32 idEmployee { get; set; }
        public BitmapImage employeeImage { get; set; }
        public String employeeName { get; set; }
        public String pisPasep { get; set; }
        public String leefNumber { get; set; }
        public Company company { get; set; }
        public Schedule schedule { get; set; }

        public String ctps { get; set; }
        public Department department { get; set; }
        public Office office { get; set; }
        public DateTime admission { get; set; }
        public DateTime resignation { get; set; }
        public ResignationReason resignationReason { get; set; }

        public String phone { get; set; }
        public String email { get; set; }
        public String generalRegistry { get; set; }
        public String registryEntity { get; set; }
        public String father { get; set; }
        public String mother { get; set; }
        public DateTime birth { get; set; }
        public String gender { get; set; }
        public String civilStatus { get; set; }
        public String nationality { get; set; }
        public String naturalness { get; set; }
        public String address { get; set; }
        public String neighborhood { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zipCode { get; set; }

        public Employee()
        {

        }

        public Employee(Int32 idEmployee, String employeeName)
        {
            this.idEmployee = idEmployee;
            this.employeeName = employeeName;
        }

        public override string ToString()
        {
            return employeeName;
        }
    }
}
