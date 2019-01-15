using Checkpoint.Control;
using MaterialDesignThemes.Wpf;
using Checkpoint.View;
using System;
using System.Windows;
using Checkpoint.ViewModal;

namespace Checkpoint
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConfigControl.Instance.createImageDirectories();

            lblCursorPosition.Text = ConfigControl.SYSTEM_VERSION;

            this.mainControl.Content = new Home();
        }

        //---------------------------------------CADASTROS---------------------------------------

        private void menuItemHome_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new Home();
        }

        private void menuItemRegisterCompany_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new CompanyRegisterView();
        }
         
        private void menuItemRegisterEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new EmployeeRegisterView();
        }

        private void menuItemRegisterSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new ScheduleRegisterView();
        }

        private void menuItemRegisterDepartment_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new DepartmentRegisterView();
        }

        private void menuItemRegisterOffice_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new OfficeRegisterView();
        }

        private void menuItemRegisterStructure_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new StructureRegisterView();
        }

        private void menuItemRegisterRecess_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new RecessRegisterView();
        }

        private void menuItemRegisterJustification_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new JustificationRegisterView();
        }

        private void menuItemRegisterAbsenceOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new AbsenceRegisterView();
        }

        private void menuItemRegisterResignationReason_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new ResignationReasonRegisterView();
        }

        //---------------------------------------RELATORIOS---------------------------------------

        private void menuItemDailyPointOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new DailyPointView();
        }

        private void menuItemMarkingCardOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new MarkingCardView();
        }

        private void menuItemReportEmployeeOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new EmployeeReportView();
        }

        private async void showExportTaxFilesModal(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportTaxFilesModal exportTaxFilesModal = new ExportTaxFilesModal();
                await DialogHost.Show(exportTaxFilesModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        //---------------------------------------SERVICOS---------------------------------------

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

        private async void showImportEmployeeModal(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportEmployeeModal newImportEmployeeModal = new ImportEmployeeModal();
                await DialogHost.Show(newImportEmployeeModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        //---------------------------------------CONFIGURACOES---------------------------------------

        private void menuItemUserRegisterView_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new UserRegisterView();
        }

        private void menuItemConfigurationHardwareOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new HardwareConfigurationView();
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

        private async void showAdjustmentModal(object sender, RoutedEventArgs e)
        {
            try
            {
                AdjustmentModal adjustmentModal = new AdjustmentModal();
                await DialogHost.Show(adjustmentModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        //---------------------------------------ROOT---------------------------------------

        private void menuItemUserProfileRegisterView_OnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new UserProfileRegisterView();
        }

        private void menuItemRegisterHardwareOnClick(object sender, RoutedEventArgs e)
        {
            this.mainControl.Content = new HardwareRegisterView();
        }

        private async void showMailsManagerModal(object sender, RoutedEventArgs e)
        {
            try
            {
                MailsManagerModal mailsManagerModal = new MailsManagerModal();
                await DialogHost.Show(mailsManagerModal, "DHMain");
            }
            catch (Exception ee)
            {

            }
        }

        public void setMainControl(Object ob)
        {
            this.mainControl.Content = ob;
        }
    }
}
