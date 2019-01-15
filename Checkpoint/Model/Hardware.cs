using System;
using System.Windows.Media.Imaging;

namespace Checkpoint.Model
{
    public class Hardware
    {
        public Int32 idHardware { get; set; }
        public BitmapImage hardwareImage { get; set; }
        public Maker maker { get; set; }
        public String description { get; set; }

        public override string ToString()
        {
            return description;
        }
    }
}
