using Checkpoint.Control;
using Checkpoint.Tools;
using Checkpoint.View;
using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Checkpoint.ViewModal
{
    public partial class SearchCNPJModal : UserControl
    {
        private string cnpj;

        private readonly BackgroundWorker backWorkerChargeCaptcha = new BackgroundWorker();

        public SearchCNPJModal(String cnpj)
        {
            InitializeComponent();
            this.cnpj = cnpj;

            backWorkerChargeCaptcha.DoWork += chargeCaptcha;
            backWorkerChargeCaptcha.RunWorkerCompleted += completeChargeCaptcha;

            imgCaptcha.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
            btRecaptcha.IsEnabled = false;
            backWorkerChargeCaptcha.RunWorkerAsync();
        }

        private void chargeCaptcha(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            SearchCNPJ.getCaptcha();
        }

        private void completeChargeCaptcha(object sender, RunWorkerCompletedEventArgs e)
        {
            imgCaptcha.Source.Dispatcher.Invoke(() => imgCaptcha.Source = SearchCNPJ.bitmapImage);
            btRecaptcha.IsEnabled = true;
        }

        private void chargeCaptcha(object sender, RoutedEventArgs e)
        {
            imgCaptcha.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
            btRecaptcha.IsEnabled = false;
            backWorkerChargeCaptcha.RunWorkerAsync();
        }

        private void searchCNPJ(object sender, RoutedEventArgs e)
        {
            searchCNPJ();
        }

        private async void searchCNPJ()
        {
            BTSearchCnpj.IsEnabled = false;
            String captcha = TBCaptcha.Text;

            await Task.Run(() =>
            {
                SearchCNPJ.Consulta(cnpj, captcha);
                CompanyRegisterView.empresa = SearchCNPJ.Empresa;
            });

            BTSearchCnpj.IsEnabled = true;
        }
    }
}
