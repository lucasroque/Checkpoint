using Checkpoint.Control;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{
    public partial class AboutModal : UserControl
    {
        public AboutModal()
        {
            InitializeComponent();
            TBVersion.Text = "Versão: " + ConfigControl.SYSTEM_VERSION;
        }
    }
}
