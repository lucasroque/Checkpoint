using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.Tools;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

namespace Checkpoint.ViewModal
{
    public partial class MailsManagerModal : System.Windows.Controls.UserControl
    {

        private OpenCallManagerControl openCallManagerControl;
        private BobbinRequestManagerControl bobbinRequestManagerControl;
        private BadgeRequestManagerControl badgeRequestManagerControl;

        private int idOpenCallManager = 0;
        private int idBobbinRequestManager = 0;
        private int idBadgeRequestManager = 0;

        public MailsManagerModal()
        {
            InitializeComponent();

            DataContext = new MailsManagerViewControl();

            openCallManagerControl = new OpenCallManagerControl();
            bobbinRequestManagerControl = new BobbinRequestManagerControl();
            badgeRequestManagerControl = new BadgeRequestManagerControl();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadFields();
        }

        private void loadFields()
        {
            OpenCallManager openCallManager = openCallManagerControl.getOpenCallManager();
            BobbinRequestManager bobbinRequestManager = bobbinRequestManagerControl.getBobbinRequestManager();
            BadgeRequestManager badgeRequestManager = badgeRequestManagerControl.getBadgeRequestManager();

            if (openCallManager != null)
            {
                TBCallOpenUser.Text = openCallManager.user;
                TBCallOpenPassword.Password = openCallManager.password;
                TBCallOpenHost.Text = openCallManager.host;
                TBCallOpenPort.Text = Convert.ToString(openCallManager.port);

                idOpenCallManager = openCallManager.idOpenCallManager;
            }

            if (bobbinRequestManager != null)
            {
                TBBobbinRequestUser.Text = bobbinRequestManager.user;
                TBBobbinRequestPassword.Password = bobbinRequestManager.password;
                TBBobbinRequestHost.Text = bobbinRequestManager.host;
                TBBobbinRequestPort.Text = Convert.ToString(bobbinRequestManager.port);

                idBobbinRequestManager = bobbinRequestManager.idBobbinRequest;
            }

            if (badgeRequestManager != null)
            {
                TBBadgeRequestUser.Text = badgeRequestManager.user;
                TBBadgeRequestPassword.Password = badgeRequestManager.password;
                TBBadgeRequestHost.Text = badgeRequestManager.host;
                TBBadgeRequestPort.Text = Convert.ToString(badgeRequestManager.port);

                idBadgeRequestManager = badgeRequestManager.idBadgeRequest;
            }
        }



        private void saveManagers(object sender, RoutedEventArgs e)
        {
            if (!Inspector.getInstance.validateEmail(TBCallOpenUser.Text))
            {
                DialogHost.Show(new SampleMessageDialog("Email Abertura de Chamado Inválido."), "DHModal");
                return;
            }
            else if (!Inspector.getInstance.validateEmail(TBBobbinRequestUser.Text))
            {
                DialogHost.Show(new SampleMessageDialog("Email Solicitação de Bobina inválido."), "DHModal");
                return;
            }
            else if (!Inspector.getInstance.validateEmail(TBBadgeRequestUser.Text))
            {
                DialogHost.Show(new SampleMessageDialog("Email Solicitação de Crachá inválido."), "DHModal");
                return;
            }

            if (!"".Equals(TBCallOpenUser.Text) && !"".Equals(TBCallOpenPassword.Password) && !"".Equals(TBCallOpenHost.Text) && !"".Equals(TBCallOpenPort.Text)
                && !"".Equals(TBBobbinRequestUser.Text) && !"".Equals(TBBobbinRequestPassword.Password) && !"".Equals(TBBobbinRequestHost.Text) && !"".Equals(TBBobbinRequestPort.Text)
                && !"".Equals(TBBadgeRequestUser.Text) && !"".Equals(TBBadgeRequestPassword.Password) && !"".Equals(TBBadgeRequestHost.Text) && !"".Equals(TBBadgeRequestPort.Text))
            {
                saveOpenCallManager();
                saveBobbinRequestManager();
                saveBadgeRequestManager();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHModal");
            }
        }

        private void saveOpenCallManager()
        {
            OpenCallManager openCallManager = new OpenCallManager();
            openCallManager.user = TBCallOpenUser.Text;
            openCallManager.password = TBCallOpenPassword.Password;
            openCallManager.host = TBCallOpenHost.Text;
            openCallManager.port = Convert.ToInt32(TBCallOpenPort.Text);

            if (idOpenCallManager == 0)
            {
                openCallManagerControl.saveOpenCallManager(openCallManager);
            }
            else
            {
                openCallManager.idOpenCallManager = idOpenCallManager;
                openCallManagerControl.updateOpenCallManager(openCallManager);
            }
        }

        private void saveBobbinRequestManager()
        {
            BobbinRequestManager bobbinRequestManager = new BobbinRequestManager();
            bobbinRequestManager.user = TBBobbinRequestUser.Text;
            bobbinRequestManager.password = TBBobbinRequestPassword.Password;
            bobbinRequestManager.host = TBBobbinRequestHost.Text;
            bobbinRequestManager.port = Convert.ToInt32(TBBobbinRequestPort.Text);

            if (idBobbinRequestManager == 0)
            {
                bobbinRequestManagerControl.saveBobbinRequestManager(bobbinRequestManager);
            }
            else
            {
                bobbinRequestManager.idBobbinRequest = idBobbinRequestManager;
                bobbinRequestManagerControl.updateBobbinRequestManager(bobbinRequestManager);
            }
        }

        private void saveBadgeRequestManager()
        {
            BadgeRequestManager badgeRequestManager = new BadgeRequestManager();
            badgeRequestManager.user = TBBadgeRequestUser.Text;
            badgeRequestManager.password = TBBadgeRequestPassword.Password;
            badgeRequestManager.host = TBBadgeRequestHost.Text;
            badgeRequestManager.port = Convert.ToInt32(TBBadgeRequestPort.Text);

            if (idBadgeRequestManager == 0)
            {
                badgeRequestManagerControl.saveBadgeRequestManager(badgeRequestManager);
            }
            else
            {
                badgeRequestManager.idBadgeRequest = idBadgeRequestManager;
                badgeRequestManagerControl.updateBadgeRequestManager(badgeRequestManager);
            }
        }
    }
}
