using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotNetOpenAuth.OAuth2;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using PortProgramm.WindowFolder.AdminWindowFolder;
using PortProgramm.WindowFolder.ManagerWindowFolder;
using PortProgramm.WindowFolder.EmployeeWindowFolder;
using PortProgramm.WindowFolder.UserWindowFolder;

namespace PortProgramm.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
            Loaded += AuthorizationPage_Loaded;

            
        }

        private void AuthorizationPage_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1) // Продолжительность анимации в секундах
            };

            // Запускаем анимацию
            MainBorder.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
                textPassword.Visibility = Visibility.Collapsed;
            else
                textPassword.Visibility = Visibility.Visible;
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = DBEntities.GetContext()
                    .User.FirstOrDefault(u => u.UserName == txtEmail.Text);

                if (user == null)
                {
                    MBClass.ErrorMB("Введен не верный логин");
                    txtEmail.Focus();
                    return;
                }
                if (user.UserPassword != passwordBox.Password)
                {
                    MBClass.ErrorMB("Введен не верный пароль");
                    passwordBox.Focus();
                    return;
                }
                else
                {
                    switch (user.RoleId)
                    {
                        case 1:                                                 
                            new AdminWindow().ShowDialog();
                            break;
                        case 2:
                            new ManagerWindow().ShowDialog();
                            break;
                        case 3:
                            new EmployeeWindow().ShowDialog();
                            break;
                        case 4:
                            new UserWindow().ShowDialog();
                            break;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void txtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
                textEmail.Visibility = Visibility.Collapsed;
            else
                textEmail.Visibility = Visibility.Visible;
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
