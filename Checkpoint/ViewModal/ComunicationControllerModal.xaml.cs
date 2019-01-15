using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{
    public partial class ComunicationControllerModal : System.Windows.Controls.UserControl
    {
        private ComunicationControl comunicationControl;
        private HardwareControl hardwareControl;

        private List<Hardware> allHardwares = new List<Hardware>();
        private int idComunication = 0;

        public ComunicationControllerModal()
        {
            InitializeComponent();

            DataContext = new ComunicationViewControl();

            comunicationControl = new ComunicationControl();
            hardwareControl = new HardwareControl();

            fillCBHardware();
            
        }

        private void fillCBHardware()
        {
            allHardwares.Clear();
            allHardwares = hardwareControl.getAllHardwares();
            CBHardware.ItemsSource = allHardwares;
        }

        private void insertSyncController(object sender, RoutedEventArgs e)
        {
            
        }

        private void loadComunication()
        {
            Comunication comunication = comunicationControl.getComunication();

            if (comunication != null)
            {
                int indexHardware = -1;

                for (int i = 0; i < allHardwares.Count; i++)
                {
                    Hardware hd = (Hardware)allHardwares[i];
                    if (comunication.hardware.idHardware == hd.idHardware)
                    {
                        indexHardware = i;
                        break;
                    }
                }

                CBHardware.SelectedIndex = indexHardware;
                TBIp.Text = comunication.ip;
                TBPort.Text = comunication.port;
                TBEndDev.Text = comunication.endDev;

                idComunication = comunication.idComunication;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadComunication();
        }
    } 
}
