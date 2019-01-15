using Checkpoint.Control;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Checkpoint.Tools;
using Checkpoint.ViewModal;

namespace Checkpoint.View
{
    public partial class CompanyRegisterView : System.Windows.Controls.UserControl
    {

        CompanyControl companyControl;
        List<Company> allCompanies = new List<Company>();
        CompanyViewControl companyViewControl;

        private int idCompanyEditing = 0;
        public static Empresa empresa;

        public CompanyRegisterView()
        {
            InitializeComponent();
            companyControl = new CompanyControl();
            companyViewControl = new CompanyViewControl();

            fillCBState();
            fillCBFatherCompany();
            fillImageControl();

            DataContext = companyViewControl;
        }

        private void fillCBState()
        {
            ObservableCollection<String> stateList = new ObservableCollection<String>(Util.getStates());
            CBState.ItemsSource = stateList;
        }

        private void fillCBFatherCompany()
        {
            allCompanies.Clear();
            allCompanies = companyControl.getAllFatherCompanies();
            CBFatherCompany.ItemsSource = allCompanies;
        }

        private void fillImageControl()
        {
            imgCompanyPhoto.Source = new BitmapImage(new Uri(ConfigControl.Instance.getNoImageFile()));
        }

        private void loadPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selecione uma Imagem";
            op.Filter = "Todas as Imagens Suportadas|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                imgCompanyPhoto.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void upsertCompany(object sender, RoutedEventArgs e)
        {

            if (!companyControl.validateCnpj(TBCnpj.Text) && idCompanyEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("CNPJ já cadastrado."), "DHMain");
                return;
            }

            if (!Inspector.getInstance.validateCnpj(TBCnpj.Text))
            {
                DialogHost.Show(new SampleMessageDialog("CNPJ inválido."), "DHMain");
                return;
            }

            if (!"".Equals(TBResponsibeEmail.Text) && !Inspector.getInstance.validateEmail(TBResponsibeEmail.Text))
            {
                DialogHost.Show(new SampleMessageDialog("Email inválido."), "DHMain");
                return;
            }

            if (RBBranch.IsChecked == true)
            {
                if (CBFatherCompany.SelectedIndex == -1)
                {
                    DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
                    return;
                }

                Company fatherCompany = (Company) CBFatherCompany.SelectedItem;

                if (fatherCompany.idCompany == idCompanyEditing)
                {
                    DialogHost.Show(new SampleMessageDialog("Matriz inválida."), "DHMain");
                    return;
                }

            }

            if (!"".Equals(TBName.Text) && !"".Equals(TBInscription.Text) && !"".Equals(TBCnpj.Text) && !"".Equals(TBResponsibleName.Text))
            {
                upsertCompany();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadCompany(object sender, RoutedEventArgs e)
        {
            Company company = ((FrameworkElement)sender).DataContext as Company;
            loadCompany(company);
        }

        private async void deleteCompany(object sender, RoutedEventArgs e)
        {

            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir esta Empresa?");
            Boolean result = (Boolean) await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                Company company = ((FrameworkElement)sender).DataContext as Company;
                deleteCompany(company);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void RBHeadOffice_Checked(object sender, RoutedEventArgs e)
        {
            if (CBFatherCompany != null)
            {
                CBFatherCompany.IsEnabled = false;
                CBFatherCompany.SelectedIndex = -1;
            }
        }

        private void RBBranch_Checked(object sender, RoutedEventArgs e)
        {
            if (CBFatherCompany != null)
            {
                CBFatherCompany.IsEnabled = true;
            }
        }

        private void searchAddress(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!"".Equals(TBZipCode.Text))
                {
                    var ws = new WSCorreios.AtendeClienteClient();
                    var address = ws.consultaCEP(TBZipCode.Text);
                    TBAddress.Text = address.end;
                    TBNeighborhood.Text = address.bairro;
                    TBCity.Text = address.cidade;
                    CBState.SelectedItem = address.uf;
                }
                else
                {
                    DialogHost.Show(new SampleMessageDialog("CEP inválido."), "DHMain");
                }
            }
            catch (Exception ex)
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao buscar CEP."), "DHMain");
            }
        }

        private async void searchCnpj(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Inspector.getInstance.validateCnpj(TBCnpj.Text))
                {
                    await DialogHost.Show(new SampleMessageDialog("CNPJ inválido."), "DHMain");
                    return;
                }

                SearchCNPJModal searchCNPJModal = new SearchCNPJModal(TBCnpj.Text);
                await DialogHost.Show(searchCNPJModal, "DHMain");

                TBName.Text = empresa.NomeFantasia;
                TBAddress.Text = empresa.Endereco;
                TBNeighborhood.Text = empresa.Bairro;
                TBCity.Text = empresa.Cidade;
                CBState.SelectedItem = empresa.UF;
                TBZipCode.Text = empresa.CEP.Replace(".","").Replace("-", "");
            }
            catch (Exception ex)
            {
                await DialogHost.Show(new SampleMessageDialog("Erro ao buscar CNPJ."), "DHMain");
            }
        }

        private void upsertCompany()
        {
            Company company = getCompanyFromControls();
            Boolean success;

            if (idCompanyEditing != 0)
            {
                company.idCompany = idCompanyEditing;
                success = companyControl.updateCompany(company);
                saveModeControls();
            } else
            {
                success = companyControl.saveCompany(company);
            }

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Empresa salva com sucesso."), "DHMain");
                fillCBFatherCompany();
            } else {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Empresa."), "DHMain");
            }

            cleanControls();
        }

        private void loadCompany(Company company)
        {
            int index = -1;

            for (int i = 0; i < allCompanies.Count; i++)
            {
                Company cp = (Company)allCompanies[i];

                if (company.fatherCompany.idCompany == cp.idCompany)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                RBBranch.IsChecked = true;
            }
            else
            {
                RBHeadOffice.IsChecked = true;
            }

            CBFatherCompany.SelectedIndex = index;
            imgCompanyPhoto.Source = company.companyImage;
            TBName.Text = company.companyName;
            TBCnpj.Text = company.cnpj;
            TBInscription.Text = company.inscription;
            TBResponsibleName.Text = company.responsibleName;
            TBResponsibleOffice.Text = company.responsibleOffice;
            TBResponsibeEmail.Text = company.responsibleEmail;
            TBAddress.Text = company.address;
            TBNeighborhood.Text = company.neighborhood;
            TBCity.Text = company.city;
            CBState.Text = company.state;
            TBZipCode.Text = company.zipCode;
            TBPhone.Text = company.phone;
            TBCei.Text = company.cei;
            TBLeefNumber.Text = company.leefNumber;

            idCompanyEditing = company.idCompany;

            editModeControls();
        }

        private void deleteCompany(Company company)
        {
            Boolean success = companyControl.deleteCompany(company);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Empresa excluída com sucesso."), "DHMain");
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Empresa."), "DHMain");
            }

            cleanControls();
        }

        private Company getCompanyFromControls()
        {
            Company company = new Company();
            company.companyImage = (BitmapImage) imgCompanyPhoto.Source;
            company.fatherCompany = RBBranch.IsChecked == true ? (Company)CBFatherCompany.SelectedItem : null;
            company.companyName = TBName.Text;
            company.cnpj = TBCnpj.Text;
            company.inscription = TBInscription.Text;
            company.responsibleName = TBResponsibleName.Text;
            company.responsibleOffice = TBResponsibleOffice.Text;
            company.responsibleEmail = TBResponsibeEmail.Text;
            company.address = TBAddress.Text;
            company.neighborhood = TBNeighborhood.Text;
            company.city = TBCity.Text;
            company.state = CBState.Text;
            company.zipCode = TBZipCode.Text;
            company.phone = TBPhone.Text;
            company.cei = TBCei.Text;
            company.leefNumber = TBLeefNumber.Text;

            return company;
        }

        private void cleanControls()
        {
            CBFatherCompany.SelectedIndex = -1;
            TBName.Text = "";
            TBCnpj.Text = "";
            TBInscription.Text = "";
            TBResponsibleName.Text = "";
            TBResponsibleOffice.Text = "";
            TBResponsibeEmail.Text = "";
            TBAddress.Text = "";
            TBNeighborhood.Text = "";
            TBCity.Text = "";
            CBState.SelectedIndex = -1;
            TBZipCode.Text = "";
            TBPhone.Text = "";
            TBCei.Text = "";
            TBLeefNumber.Text = "";

            idCompanyEditing = 0;
            fillImageControl();
            saveModeControls();
            companyViewControl.fillGridCompany();
        }

        private void saveModeControls()
        {
            BTSave.IsEnabled = true;
            BTEdit.IsEnabled = false;
        }

        private void editModeControls()
        {
            BTSave.IsEnabled = false;
            BTEdit.IsEnabled = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
