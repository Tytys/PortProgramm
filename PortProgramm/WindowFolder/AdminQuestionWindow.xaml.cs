using PortProgramm.ClassFolder;
using PortProgramm.DataFolder;
using PortProgramm.PageFolder;
using PortProgramm.PageFolder.AdminPageFolder;
using PortProgramm.WindowFolder.AdminWindowFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PortProgramm.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AdminQuestionWindow.xaml
    /// </summary>
    public partial class AdminQuestionWindow : Window
    {
        string _CaptchaCode;
        public AdminQuestionWindow()
        {
            InitializeComponent();
        }

        private void PasswordText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPb.Focus();
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int w = (int)CaptchaIMG.Width;
            int h = (int)CaptchaIMG.Height;
            var CaptchaCode = Captcha.GenerateCaptcha();
            _CaptchaCode = CaptchaCode;

            var res = Captcha.GenerateImage(w, h, CaptchaCode);

            Stream stream = new MemoryStream(res.CaptchaByteData);

            CaptchaIMG.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = false
            };
            MainBorder.BeginAnimation(Border.OpacityProperty, fadeOutAnimation);         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = DBEntities.GetContext()
                    .User.FirstOrDefault(u => u.UserName == VariableClass.AdminLogin);

            if (CaphaImput.Text == _CaptchaCode && user.UserPassword == PasswordPb.Password)
            {
                AdminWindow adminWindow = App.Current.Windows[0] as AdminWindow;
                
                ListUserPage page = adminWindow.MainFrame.Content as ListUserPage;
                Type pagetype = typeof(ListUserPage);
                
                MethodInfo info = pagetype.GetMethod("ShowPassword");
                if (info != null)
                {
                    info.Invoke(page, null);
                }

                DoubleAnimation fadeOutAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(1),
                    AutoReverse = false
                };
                fadeOutAnimation.Completed += (S, _) =>
                {
                    
                    Close();
                };
                MainBorder.BeginAnimation(Border.OpacityProperty, fadeOutAnimation);
            }
            else
            {
                int w = (int)CaptchaIMG.Width;
                int h = (int)CaptchaIMG.Height;
                var CaptchaCode = Captcha.GenerateCaptcha();
                _CaptchaCode = CaptchaCode;

                var res = Captcha.GenerateImage(w, h, CaptchaCode);

                Stream stream = new MemoryStream(res.CaptchaByteData);

                CaptchaIMG.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private void CaphaText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CaphaImput.Focus();
        }

        private void CaphaImput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CaphaImput.Text) && CaphaImput.Text.Length > 0)
                CaphaText.Visibility = Visibility.Collapsed;
            else
                CaphaText.Visibility = Visibility.Visible;
        }

        private void PasswordPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswordPb.Password) && PasswordPb.Password.Length > 0)
                PasswordText.Visibility = Visibility.Collapsed;
            else
                PasswordText.Visibility = Visibility.Visible;
        }
    }
}
