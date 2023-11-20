using Google.Apis.PeopleService.v1.Data;
using Microsoft.Win32;
using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
        byte[] data = null;
        public EditUserPage(User user)
        {
            InitializeComponent();
            DataContext = user;

            this.user.UserId = user.UserId;
            

            RoleCb.ItemsSource = DBEntities.GetContext()
               .Role.ToList();
            AdressCb.ItemsSource = DBEntities.GetContext()
               .Adress.ToList();
            GenderCb.ItemsSource = DBEntities.GetContext()
               .Gender.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = RoleCb.SelectedIndex + 1;
                LoginTb.textBox.Text = LoginTb.Hint;
                PasswordTb.textBox.Text = PasswordTb.Hint;
                EmailTb.textBox.Text = EmailTb.Hint;
                FIOTb.textBox.Text = FIOTb.Hint;
                PhoneNumberTb.textBox.Text = PhoneNumberTb.Hint;
                user = DBEntities.GetContext().User
                    .FirstOrDefault(u => u.UserId == user.UserId);
                user.UserName = LoginTb.textBox.Text;
                user.UserPassword = PasswordTb.textBox.Text;
                user.RoleId = index;  
                user.UserEmail = EmailTb.textBox.Text;
                user.UserFIO = FIOTb.textBox.Text;
                user.PhoneNumber = PhoneNumberTb.textBox.Text;
                user.AdressId = int.Parse(AdressCb.SelectedValue.ToString());
                user.GenderId = int.Parse(GenderCb.SelectedValue.ToString());
                user.PassportPhoto = data;
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
            EmailTb.textBox.Text = EmailTb.Hint;
            FIOTb.textBox.Text = FIOTb.Hint;
            PhoneNumberTb.textBox.Text = PhoneNumberTb.Hint;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (.jpg,.jpeg, .png) |.jpg; .jpeg;.png";
            if (openFileDialog.ShowDialog() == true)
            {

                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = new Uri(openFileDialog.FileName);
                imageSource.EndInit();


                byte[] imageData;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageSource));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    imageData = ms.ToArray();
                }


                //using (var context = DBEntities.GetContext())
                //{
                //    User reader = context.User.FirstOrDefault(r => r.IdUser == VariableClass.ReaderId);
                //    if (reader != null)
                //    {
                //        reader.PhotoReader = imageData;
                //        context.SaveChanges();
                //    }
                //}

                data = imageData;

                //PhotoIm.Source = imageSource;
            }
        }
    }
}
