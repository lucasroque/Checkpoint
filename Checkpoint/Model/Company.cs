using System;
using System.Windows.Media.Imaging;

namespace Checkpoint.Model
{
    public class Company
    {
        public Int32 idCompany { get; set; }
        public BitmapImage companyImage { get; set; }
        public String companyName { get; set; }
        public Company fatherCompany { get; set; }
        public String cnpj { get; set; }
        public String inscription { get; set; }
        public String responsibleName { get; set; }
        public String responsibleOffice { get; set; }
        public String responsibleEmail { get; set; }
        public String address { get; set; }
        public String neighborhood { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zipCode { get; set; }
        public String phone { get; set; }
        public String cei { get; set; }
        public String leefNumber { get; set; }

        public Company() { }

        public Company(Int32 idCompany)
        {
            this.idCompany = idCompany;
        }

        public Company(Int32 idCompany, String companyName)
        {
            this.idCompany = idCompany;
            this.companyName = companyName;
        }

        public override string ToString()
        {
            return companyName;
        }

        public static explicit operator Company(int v)
        {
            throw new NotImplementedException();
        }
    }
}
