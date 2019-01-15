using Checkpoint.Control;
using Checkpoint.Message;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Checkpoint.ViewModal
{
    public partial class AFDImportModal : System.Windows.Controls.UserControl
    {
        private readonly BackgroundWorker backWorkerImportAFD = new BackgroundWorker();
        private ImportControl importControl = new ImportControl();
        private String afdFile = "";
        private Boolean success;

        public AFDImportModal()
        {
            InitializeComponent();

            backWorkerImportAFD.DoWork += importAFDBackground;
            backWorkerImportAFD.RunWorkerCompleted += backWorkerImportADFResponse;
        }

        private void loadAFD(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione o Arquivo AFD";
            op.Filter = "TXT (*.txt)|*.txt";

            if (op.ShowDialog() == true)
            {
                afdFile = op.FileName;
                TBAFDPath.Text = afdFile;
            }
        }

        public void startProgress()
        {
            BTImport.IsEnabled = false;
            BTSearch.IsEnabled = false;
            PBImport.IsIndeterminate = true;
        }

        public void stopProgress()
        {
            BTImport.IsEnabled = true;
            BTSearch.IsEnabled = true;
            PBImport.IsIndeterminate = false;
        }

        private void importAFD(object sender, RoutedEventArgs e)
        {
            if (!"".Equals(afdFile))
            {
                startProgress();

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MinValue;

                if (DPStartDate.SelectedDate != null)
                    startDate = (DateTime)DPStartDate.SelectedDate;

                if (DPEndDate.SelectedDate != null)
                    endDate = (DateTime)DPEndDate.SelectedDate;

                List<object> arguments = new List<object>();
                arguments.Add(afdFile);
                arguments.Add(startDate);
                arguments.Add(endDate);
                arguments.Add(TBPispasep.Text);

                backWorkerImportAFD.RunWorkerAsync(arguments);

            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Selecione um arquivo para importação."), "DHModal");
            }
            
        }

        private void importAFDBackground(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            List<object> genericlist = doWorkEventArgs.Argument as List<object>;

            success = importControl.importAFD((String) genericlist[0], (DateTime) genericlist[1], (DateTime) genericlist[2], (String) genericlist[3]);

            

        }

        private void backWorkerImportADFResponse(object sender, RunWorkerCompletedEventArgs e)
        {
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Arquivo importado com sucesso."), "DHModal");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Falha na importação do arquivo."), "DHModal");
            }

            cleanControls();
            stopProgress();
        }

        private void cleanControls()
        {
            TBAFDPath.Text = "";
            DPStartDate.SelectedDate = null;
            DPEndDate.SelectedDate = null;
            TBPispasep.Text = "";
            afdFile = "";
            success = false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
