using System;
using System.Windows.Media.Imaging;

namespace Checkpoint.Model
{
    public class HardwareConfiguration
    {
        public Int32 idHardwareConfiguration { get; set; }
        public BitmapImage hardwareImage { get; set; }
        public Company company { get; set; }
        public Hardware hardware { get; set; }
        public String serialNumber { get; set; }
        public String cryptographicKey { get; set; }
        public String model { get; set; }
        public String version { get; set; }
        public String port { get; set; }
        public String ip { get; set; }
        public String cpf { get; set; }

        public override string ToString()
        {
            return hardware.description;
        }
    }
}
