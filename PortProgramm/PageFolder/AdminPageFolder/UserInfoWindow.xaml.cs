using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;

namespace PortProgramm.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        User user = new User();
        public UserInfoWindow(User user)
        {
            InitializeComponent();
            DataContext = user;
            this.user.UserId = user.UserId;
            PasspotImg.Source = ImageClass.ConvertByteArrayToImage(DBEntities.GetContext().User.Where(x => x.UserId == this.user.UserId).First().PassportPhoto);
            RoleCb.ItemsSource = DBEntities.GetContext()
               .Role.ToList();
            AdressCb.ItemsSource = DBEntities.GetContext()
               .Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext()
               .Gender.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
