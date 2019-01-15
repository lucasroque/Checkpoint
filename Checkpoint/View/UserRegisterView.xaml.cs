using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Checkpoint.View
{

    public partial class UserRegisterView : System.Windows.Controls.UserControl
    {

        private UsersControl userControl;
        private UserProfilesControl userProfilesControl;
        private UserViewControl userViewControl;


        List<UserProfile> allUserProfiles = new List<UserProfile>();

        private int idUserEditing = 0;

        public UserRegisterView()
        {
            InitializeComponent();

            DataContext = new UserViewControl();

            userControl = new UsersControl();
            userProfilesControl = new UserProfilesControl();
            userViewControl = new UserViewControl();
            
        }

        private void upsertUser(object sender, RoutedEventArgs e)
        {

            if (!userControl.validateDescription(TBLogin.Text) && idUserEditing == 0)
            {
                DialogHost.Show(new SampleMessageDialog("Login já cadastrado."), "DHMain");
                return;
            }

            if (CBUserProfile.SelectedIndex != -1 && !"".Equals(TBLogin.Text) && !"".Equals(TBPassword.Password))
            {
                upsertUser();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void loadUser(object sender, RoutedEventArgs e)
        {
            User user = ((FrameworkElement)sender).DataContext as User;
            loadUser(user);
        }

        private async void deleteUser(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Usuário?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                User user = ((FrameworkElement)sender).DataContext as User;
                deleteUser(user);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertUser()
        {
            User user = getUserFromControls();
            Boolean success;

            if (idUserEditing != 0)
            {
                user.idUser = idUserEditing;
                success = userControl.updateUser(user);
                saveModeControls();
            }
            else
            {
                success = userControl.saveUser(user);
            }

            cleanControls();
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Usuário salvo com sucesso."), "DHMain");
                userViewControl.fillGridUser();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Usuário."), "DHMain");
            }
        }

        private void loadUser(User user)
        {
            int index = -1;

            for (int i = 0; i < allUserProfiles.Count; i++)
            {
                UserProfile up = (UserProfile)allUserProfiles[i];

                if (user.userProfile.idUserProfile == up.idUserProfile)
                {
                    index = i;
                    break;
                }
            }

            CBUserProfile.SelectedIndex = index;
            TBLogin.Text = user.login;
            TBPassword.Password = user.password;
            DPValidity.SelectedDate = user.validity;
            CXActive.IsChecked = user.active;

            idUserEditing = user.idUser;

            editModeControls();
        }

        private void deleteUser(User user)
        {
            Boolean success = userControl.deleteUser(user);

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Usuário excluído com sucesso."), "DHMain");
                userViewControl.fillGridUser();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Usuário."), "DHMain");
            }
        }

        private User getUserFromControls()
        {
            User user = new User();
            user.userProfile = (UserProfile) CBUserProfile.SelectedItem;
            user.login = TBLogin.Text;
            user.password = TBPassword.Password;
            if (DPValidity.SelectedDate != null)
                user.validity = (DateTime) DPValidity.SelectedDate;
            user.active = (bool) CXActive.IsChecked;

            return user;
        }

        private void cleanControls()
        {
            CBUserProfile.SelectedIndex = -1;
            TBLogin.Text = "";
            TBPassword.Password = "";
            DPValidity.SelectedDate = null;
            CXActive.IsChecked = false;

            idUserEditing = 0;
            saveModeControls();
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
