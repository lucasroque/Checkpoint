using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Checkpoint.Message
{
    public partial class AddJustificationMessageDialog : System.Windows.Controls.UserControl
    {
        private JustificationControl justificationControl;
        private List<Justification> allJustifications = new List<Justification>();

        public AddJustificationMessageDialog()
        {
            InitializeComponent();

            DataContext = new AddJustificationViewControl();

            justificationControl = new JustificationControl();

            fillCBJustification();
        }

        private void fillCBJustification()
        {
            allJustifications.Clear();
            allJustifications = justificationControl.getAllJustifications();
            CBJustification.ItemsSource = allJustifications;
        }

    }
}
