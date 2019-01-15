using System;
using System.Windows.Controls;

namespace Checkpoint.Message
{
    public partial class SampleMessageDialog : UserControl
    {
        public SampleMessageDialog(String message)
        {
            InitializeComponent();
            TBMessage.Text = message;
        }
    }
}
