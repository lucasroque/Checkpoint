
using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Tools;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Checkpoint.ViewModal
{

    public partial class BackupModal : UserControl
    {
        public BackupModal()
        {
            InitializeComponent();
            TBPathFolder.Text = ConfigControl.Instance.getBackupDirectory();
        }

        private void loadBackupFolder(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();

            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TBPathFolder.Text = fb.SelectedPath;
            }

        }

        private void makeBackup(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBPathFolder.Text))
            {
                BackupManager.getInstance.DoBackup(TBPathFolder.Text);
                DialogHost.Show(new SampleMessageDialog("Backup efetuado com sucesso."), "DHModal");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Diretório Inválido."), "DHModal");
            }
        }

        private void loadBackupFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione o Arquivo de Backup";
            op.Filter = "MDB (*.mdb)|*.mdb";

            if (op.ShowDialog() == true)
            {
                TBPathFile.Text = op.FileName;
            }
        }

        private void loadBackup(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(TBPathFile.Text))
            {
                BackupManager.getInstance.loadBackup(TBPathFile.Text);
                DialogHost.Show(new SampleMessageDialog("Backup carregado com sucesso."), "DHModal");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Arquivo Inválido."), "DHModal");
            }
        }

        private void reset()
        {
            TBPathFolder.Text = ConfigControl.Instance.getBackupDirectory();
            TBPathFile.Text = "";
        }
    }
}
