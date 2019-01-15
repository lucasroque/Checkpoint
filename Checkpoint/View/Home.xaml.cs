using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewModal;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.View
{
    public partial class Home : UserControl
    {
        private Email email;
        private BobbinRequestManagerControl bobbinRequestManagerControl;
        private BadgeRequestManagerControl badgeRequestManagerControl;
        private MailControl bobbinMailControl;
        private MailControl badgeMailControl;

        public Home()
        {
            InitializeComponent();

            email = new Email();
            bobbinRequestManagerControl = new BobbinRequestManagerControl();
            badgeRequestManagerControl = new BadgeRequestManagerControl();
            bobbinMailControl = new MailControl(bobbinRequestManagerControl.getBobbinRequestManager());
            badgeMailControl = new MailControl(badgeRequestManagerControl.getBadgeRequestManager());

        }

        private void menuItemRegisterCompany_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainControl.Content = new CompanyRegisterView();
        }

        private void menuItemRegisterEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainControl.Content = new EmployeeRegisterView();
        }

        private void menuItemRegisterSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainControl.Content = new ScheduleRegisterView();
        }

        private void menuItemDailyPointOnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainControl.Content = new DailyPointView();
        }

        private void menuItemMarkingCardOnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainControl.Content = new MarkingCardView();
        }

        private async void showSyncModal(object sender, RoutedEventArgs e)
        {
            try
            {
                SyncModal newSyncModal = new SyncModal();
                await DialogHost.Show(newSyncModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        private async void showAFDImportModal(object sender, RoutedEventArgs e)
        {
            try
            {
                AFDImportModal newAFDImportModal = new AFDImportModal();
                await DialogHost.Show(newAFDImportModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        private async void menuItemConfigurationBackup_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BackupModal newBackupModal = new BackupModal();
                await DialogHost.Show(newBackupModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        private async void OpenCall_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenCallModal openCallModal = new OpenCallModal();
                await DialogHost.Show(openCallModal, "DHMain");
            }
            catch (Exception ee)
            {
                
            }
        }

        private async void BobbinRequest_OnClick(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(bobbinRequestManagerControl.getGetUser()) || !"".Equals(bobbinRequestManagerControl.getGetPassword()))
            {
                
                DateTime today = DateTime.Today;
                String callNumber = Convert.ToString(today.Ticks);

                email.subject = "Solicitação de Bobina Número: " + Convert.ToString(today.Ticks);
                email.content = "Bobina próxima do fim.";

                bobbinMailControl.sendMail(email);
                await DialogHost.Show(new SampleMessageDialog("Solicitação de Bobina efetuada sucesso."));
            }
            else
            {
                await DialogHost.Show(new SampleMessageDialog("Solicitação de Bobina não configurado."));
            }
        }

        private async void BadgeRequest_OnClick(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(badgeRequestManagerControl.getGetUser()) || !"".Equals(badgeRequestManagerControl.getGetPassword()))
            {

                DateTime today = DateTime.Today;
                String callNumber = Convert.ToString(today.Ticks);

                email.subject = "Solicitação de Crachá Número: " + Convert.ToString(today.Ticks);
                email.content = "Empresa necessita de crachá.";

                bobbinMailControl.sendMail(email);
                await DialogHost.Show(new SampleMessageDialog("Solicitação de Crachá efetuada sucesso."));
            }
            else
            {
                await DialogHost.Show(new SampleMessageDialog("Solicitação de Crachá não configurado."));
            }
        }

        private async void InfoButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutModal aboutModal = new AboutModal();
            Boolean result = (Boolean)await DialogHost.Show(aboutModal);
        }
    }
}
