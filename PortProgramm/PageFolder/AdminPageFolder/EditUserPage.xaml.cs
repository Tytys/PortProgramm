using Google.Apis.PeopleService.v1.Data;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortProgramm.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для EditUserPage.xaml
    /// </summary>
    public partial class EditUserPage : Page
    {
        User user = new User();
        public EditUserPage(User user)
        {
            InitializeComponent();
            DataContext = user;

            this.user.UserId = user.UserId;

            RoleCb.ItemsSource = DBEntities.GetContext()
                .Role.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = RoleCb.SelectedIndex + 1;
                user = DBEntities.GetContext().User
                    .FirstOrDefault(u => u.UserId == user.UserId);
                user.UserName = LoginTb.textBox.Text;
                user.UserPassword = PasswordTb.textBox.Text;
                user.RoleId = index;  
                user.UserEmail = EmailTb.textBox.Text;
                user.UserFIO = FIOTb.textBox.Text;
                DBEntities.GetContext().SaveChanges();
                MBClass.InfoMB("Данные успешно отредактированы");
                NavigationService.Navigate(new ListUserPage());
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTb.textBox.Text = LoginTb.Hint;
            PasswordTb.textBox.Text = PasswordTb.Hint;
        }
    }
}
