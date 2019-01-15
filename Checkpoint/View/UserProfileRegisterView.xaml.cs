using Checkpoint.Control;
using Checkpoint.Message;
using Checkpoint.Model;
using Checkpoint.ViewControl;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Checkpoint.View
{

    public partial class UserProfileRegisterView : System.Windows.Controls.UserControl
    {
        private UserProfilesControl userProfileControl;
        private UserProfileViewControl userProfileViewControl;

        private int idUserProfileEditing = 0;

        public UserProfileRegisterView()
        {
            InitializeComponent();

            userProfileControl = new UserProfilesControl();
            userProfileViewControl = new UserProfileViewControl();

            DataContext = userProfileViewControl;
            
            fillCBSecurityLevels();
        }

        private void upsertUserProfile(object sender, RoutedEventArgs e)
        {
            if (CBSecurityLevel.SelectedIndex != -1 && !"".Equals(TBDescription.Text))
            {
                upsertUserProfile();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Preencher campos obrigatórios."), "DHMain");
            }
        }

        private void fillCBSecurityLevels()
        {
            CBSecurityLevel.ItemsSource = userProfileControl.getSecurityLevels();
        }

        private void loadUserProfile(object sender, RoutedEventArgs e)
        {
            UserProfile userProfile = ((FrameworkElement)sender).DataContext as UserProfile;
            loadUserProfile(userProfile);
        }

        private async void deleteUserProfile(object sender, RoutedEventArgs e)
        {
            OptionMessageDialog optionMessageDialog = new OptionMessageDialog("Deseja realmente excluir este Perfil de Usuário?");
            Boolean result = (Boolean)await DialogHost.Show(optionMessageDialog, "DHMain");

            if (result == true)
            {
                UserProfile userProfile = ((FrameworkElement)sender).DataContext as UserProfile;
                deleteUserProfile(userProfile);
            }
        }

        private void cleanControls(object sender, RoutedEventArgs e)
        {
            cleanControls();
        }

        private void upsertUserProfile()
        {
            UserProfile userProfile = getUserProfileFromControls();
            Boolean success;

            if (idUserProfileEditing != 0)
            {
                userProfile.idUserProfile = idUserProfileEditing;
                success = userProfileControl.updateUserProfile(userProfile);
                saveModeControls();
            }
            else
            {
                success = userProfileControl.saveUserProfile(userProfile);
            }

            cleanControls();

            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Perfil de Usuário salvo com sucesso."), "DHMain");
                userProfileViewControl.fillGridUserProfiles();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao salvar Perfil de Usuário."), "DHMain");
            }
        }

        private void loadUserProfile(UserProfile userProfile)
        {
            int index = -1;

            for (int i = 0; i < userProfileControl.getSecurityLevels().Count; i++)
            {
                Int32 securityLevel = userProfileControl.getSecurityLevels()[i];

                if (userProfile.securityLevel == securityLevel)
                {
                    index = i;
                    break;
                }
            }

            CBSecurityLevel.SelectedIndex = index;
            TBDescription.Text = userProfile.description;

            idUserProfileEditing = userProfile.idUserProfile;

            editModeControls();
        }

        private void deleteUserProfile(UserProfile userProfile)
        {
            Boolean success = userProfileControl.deleteUserProfile(userProfile);
            
            if (success)
            {
                DialogHost.Show(new SampleMessageDialog("Perfil de Usuário excluído com sucesso."), "DHMain");
                userProfileViewControl.fillGridUserProfiles();
            }
            else
            {
                DialogHost.Show(new SampleMessageDialog("Erro ao excluir Perfil de Usuário."), "DHMain");
            }
        }

        private UserProfile getUserProfileFromControls()
        {
            UserProfile userProfile = new UserProfile();
            userProfile.securityLevel = (Int32) CBSecurityLevel.SelectedItem;
            userProfile.description = TBDescription.Text;

            return userProfile;
        }

        private void cleanControls()
        {
            CBSecurityLevel.SelectedIndex = -1;
            TBDescription.Text = "";

            idUserProfileEditing = 0;
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
