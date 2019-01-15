using System;
using System.Windows.Controls;

namespace Checkpoint.Message
{
    public partial class OptionMessageDialog : UserControl
    {
        public OptionMessageDialog(String message)
        {
            InitializeComponent();
            TBMessage.Text = message;
        }
    }
}
