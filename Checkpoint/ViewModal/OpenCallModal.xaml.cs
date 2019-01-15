using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Checkpoint.ViewModal
{

    public partial class OpenCallModal : System.Windows.Controls.UserControl
    {
        private Email email;
        private MailControl mailControl;
        private OpenCallManagerControl openCallManagerControl;
        private CompanyControl companyControl;

        private List<Company> allCompanies = new List<Company>();

        public OpenCallModal()
        {
            InitializeComponent();
            DataContext = new OpenCallViewControl();

            email = new Email();
            openCallManagerControl = new OpenCallManagerControl();
            companyControl = new CompanyControl();

            mailControl = new MailControl(openCallManagerControl.getOpenCallManager());

            fillCBCompany();
        }

        private void fillCBCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllCompanies();
            CBCompany.ItemsSource = allCompanies;
        }

        private void loadAttachment(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione o Anexo";
            op.Filter = "Todos os Arquivos (*.*)|*.*";

            if (op.ShowDialog() == true)
            {
                email.attachments.Add(op.FileName);
                String[] parts = op.FileName.Split('\\');
                TBAttachments.Text = TBAttachments.Text + parts[parts.Length-1] + ";";
            }
        }

        private void sendCall(object sender, RoutedEventArgs e)
        {
            if (CBCompany.SelectedIndex != -1 && !"".Equals(TBSubject.Text) && !"".Equals(TBContent.Text))
            {
                if(!"".Equals(openCallManagerControl.getUser()) || !"".Equals(openCallManagerControl.getPassword()))
                {

                    Company company = (Company) CBCompany.SelectedItem;
                    DateTime today = DateTime.Today;
                    String callNumber = company.cnpj.Substring(3,3) + company.cnpj.Substring(7,3) + Convert.ToString(today.Ticks);

                    email.subject = "Chamado Número: " + callNumber + " - " + TBSubject.Text;
                    email.content = "Empresa: " + company.companyName + ". \n\n" + TBContent.Text;

                    mailControl.sendMail(email);
                    DialogHost.Show(new SampleMessageDialog("Chamado aberto com Sucesso."), "DHModal");                   
                }
                else
                {
                    DialogHost.Show(new SampleMessageDialog("Abertura de chamado não configurado."), "DHModal");
                }

                cleanControls();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHModal");
            }
        }

        private void cleanControls()
        {
            CBCompany.SelectedIndex = -1;
            TBSubject.Text = "";
            TBContent.Text = "";
            TBAttachments.Text = "";
        }
    }
}
